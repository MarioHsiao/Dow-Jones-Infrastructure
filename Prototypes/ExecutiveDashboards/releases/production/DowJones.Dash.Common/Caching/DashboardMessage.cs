using Newtonsoft.Json;

namespace DowJones.Dash.Caching
{
    public class DashboardMessage
    {
        [JsonProperty("data")]
        public object Data { get; set; }
        
        [JsonProperty("eventName")]
        public string EventName { get; private set; }

        [JsonProperty("source")]
        public string Source { get; private set; }

        public DashboardMessage(string source = null, string eventName = null, object data = null)
        {
            Data = data;
            EventName = eventName;
            Source = source;
        }
    }
}