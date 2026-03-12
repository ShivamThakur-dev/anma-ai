using System.Collections.Generic;

namespace AirportAssistant.Data
{
    public static class MockFlightData
    {
        public static Dictionary<string, int> BaggageBelts = new()
        {
            { "AI203", 5 },
            { "AI404", 3 },
            { "UK220", 7 },
            { "6E112", 2 }
        };
    }
}