using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.DiscoveryGraph
{
    public class DiscoveryGraphData
    {
        [JsonProperty("discovery")]
        public DiscoveryGraphParentNewsEntities Discovery { get; set; }
    }
}
