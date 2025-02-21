using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace NotesApi.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CallProtectedEndpointAsync(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.GetAsync("https://localhost:7013/api/auth/protected-endpoint");
            return response;
        }
    }
}
