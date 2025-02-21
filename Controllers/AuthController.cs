using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Data;
using NotesApi.Dto.Auth;
using NotesApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly NotesContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AuthController(NotesContext context, IConfiguration config, HttpClient httpClient)
        {
            _context = context;
            _config = config;
            _httpClient = httpClient;
        }

        // ✅ REGISTER API
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("Username already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // ✅ LOGIN API
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            string token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // ✅ Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ✅ PROTECTED ENDPOINT (Requires Authentication)
        [HttpGet("protected-endpoint")]
        [Authorize]
        public IActionResult ProtectedEndpoint()
        {
            return Ok(new { message = "Access granted to protected resource." });
        }

        // ✅ CALL EXTERNAL PROTECTED ENDPOINT
        [HttpGet("call-protected-api")]
        [Authorize]
        public async Task<IActionResult> CallExternalApi()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is required.");
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("https://localhost:7013/api/auth/protected-endpoint");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(new { message = "Success", data = content });
                }

                return Unauthorized("Access to external API denied.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calling external API: {ex.Message}");
            }
        }
    }
}
