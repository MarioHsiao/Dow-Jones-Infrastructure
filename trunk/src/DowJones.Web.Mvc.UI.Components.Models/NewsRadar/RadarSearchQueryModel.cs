using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{
    [DataContract(Name = "radarSearchQuery")]
    public class RadarSearchQueryModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "queryType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public QueryType QueryType { get; set; }

        [DataMember(Name = "searchString")]
        public string SearchString { get; set; }

        [DataMember(Name = "scope")]
        public Scope Scope { get; set; }

        [DataMember(Name = "searchMode")]
        public int SearchMode { get; set; }
    }
}
