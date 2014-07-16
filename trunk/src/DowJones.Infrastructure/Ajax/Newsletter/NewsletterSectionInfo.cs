using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Ajax.Newsletter
{
    public class NewsletterSectionInfo
    {
        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember(Name = "index")]
        [JsonProperty("index")]
        public int Index { get; set; }

        [DataMember(Name = "positionIndicator")]
        [JsonProperty("positionIndicator")]
        public string PositionIndicator { get; set; }
    }
}
