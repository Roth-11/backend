using System.ComponentModel.DataAnnotations;

namespace NotesApi.Dto.Auth
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
