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
            var apiKey = "NmFhOTNhNDMtOTZkMy00OWM5LTlkYWYtYWZmZjViYzU3NmExOjFxV1R1NVdwUXAxWEVHOUZ6ZlBxVE9oYzNWcFFKMnB0YVU2eXVIdzJ3OFk9"; // keep this secret
            var personaConfig = new
            {
                name = "Airport Assistant",
                avatarId = "6cc28442-cccd-42a8-b6e4-24b7210a09c5",
                voiceId = "aeb61fbb-acde-40f5-898a-88350aaa513b",
                llmId = "85906141-db1c-4927-b74d-3c82ebe2436e",
                systemPrompt = "You are an airport assistant."
            };

            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anam.ai/v1/auth/session-token")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { personaConfig }), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, responseContent);
            }

            var json = JsonDocument.Parse(responseContent);
            var sessionToken = json.RootElement.GetProperty("sessionToken").GetString();

            return Ok(new { sessionToken });
        }
    }
}