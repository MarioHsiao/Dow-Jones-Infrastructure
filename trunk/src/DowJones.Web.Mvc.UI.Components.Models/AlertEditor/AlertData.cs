using DowJones.AlertEditor;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AlertData : AlertProperties
    {
        [JsonProperty("properties")]
        public AlertProperties Properties { get; set; }

        [JsonProperty("searchQuery")]
        public AlertSearchQuery SearchQuery { get; set; }
    }
}