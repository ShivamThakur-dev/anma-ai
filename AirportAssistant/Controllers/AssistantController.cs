using Microsoft.AspNetCore.Mvc;
using AirportAssistant.DTO;
using AirportAssistant.Data;

namespace AirportAssistant.Controllers
{
    [ApiController]
    [Route("api/assistant")]
    public class AssistantController : ControllerBase
    {
        [HttpPost]
        public IActionResult AskAssistant([FromBody] UserQuery query)
        {
            if (query.Message.ToLower().Contains("baggage"))
            {
                if (MockFlightData.BaggageBelts.ContainsKey(query.FlightNumber))
                {
                    int belt = MockFlightData.BaggageBelts[query.FlightNumber];

                    return Ok(new
                    {
                        response = $"Your baggage for flight {query.FlightNumber} will arrive on belt {belt}."
                    });
                }
            }

            return Ok(new
            {
                response = "Sorry, I could not find baggage information."
            });
        }
    }
}