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
            var apiKey = "Mjk2ZTZlZjItM2I3Yy00ZTg4LTk1NTUtNjE2MDA1ZTI3ZDc0OjNhbjNZUVAwdCtDK241bnE4ZlQ4YWxqb2o5c1k5dEVwRmZCWU4yeUFNSmc9";

            // Use the persona ID that has webhook enabled
            var personaId = "480d5041-74ef-4a84-810a-858902c1b016";

            var httpClient = _httpClientFactory.CreateClient();
           var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anam.ai/v1/auth/session-token")
{
    Content = new StringContent(
        JsonSerializer.Serialize(new {
            personaConfig = new {
                personaId = personaId,               
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
