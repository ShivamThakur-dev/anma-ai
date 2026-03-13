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
            var apiKey = "MjIxYmQ0NzEtZTNlMC00ZWEzLTkyNGMtZjk2MzFhMjRlOTI0OmlXdVJoeVNHT2VSNVhWQlN1RnFtYWZCb3B0S3U1QnptSTc4Snd5OCs1Qkk9";

            // Use the persona ID that has webhook enabled
            var personaId = "4b3fd4a5-230c-419f-a67d-255f7419ec46";

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
