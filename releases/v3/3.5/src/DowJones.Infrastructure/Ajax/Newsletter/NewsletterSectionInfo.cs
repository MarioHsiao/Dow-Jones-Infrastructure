using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Ajax.Newsletter
{
    public class NewsletterSectionInfo
    {
        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember(Name = "position")]
        [JsonProperty("position")]
        public int Position { get; set; }

        [DataMember(Name = "subSections")]
        [JsonProperty("subSections")]
        public IEnumerable<NewsletterSectionInfo> SubSections { get; set; }
    }
}
