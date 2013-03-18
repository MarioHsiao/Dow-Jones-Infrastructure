using DowJones.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.Search.Requests.Filters
{
    public class QueryFilter
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("codeType", NullValueHandling = NullValueHandling.Ignore)]
        public string CodeType { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public NewsFilterCategory Category { get; set; }


        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
}
