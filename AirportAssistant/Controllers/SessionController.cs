using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AirportAssistant.Controllers
{
    [ApiController]
    [Route("api/session-token")]
    public class SessionController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SessionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSessionToken()
        {
            // Your Anam AI API key (keep secret) 
            var apiKey = "Y2RjYzlmMzQtZTM1MC00NmMxLTg5NTEtY2FkNjg0NDliNDIzOkg2L1VpZ0hudE5NeFZDcXJUR0hxaWlZak1SdFJhOEo3Y2U3Qm0yNUlCNFU9";

            // Use the persona ID that has webhook enabled
            var personaId = "da4e96aa-16ca-4aca-a608-9d7687602e0e";

            var httpClient = _httpClientFactory.CreateClient();
           var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anam.ai/v1/auth/session-token")
{
    Content = new StringContent(
        JsonSerializer.Serialize(new {
            personaConfig = new {
                personaId = personaId,   
                languageCode = "auto"
            }
        }),
        Encoding.UTF8,
        "application/json"
    )
};

            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, responseContent);

            var json = JsonDocument.Parse(responseContent);
            var sessionToken = json.RootElement.GetProperty("sessionToken").GetString();

            return Ok(new { sessionToken });
        }
    }
}
