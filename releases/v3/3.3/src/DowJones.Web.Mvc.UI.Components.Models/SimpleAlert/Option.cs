using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.SimpleAlert
{
    public class Option
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("optGroup")]
        public bool OptGroup { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }
    }
}