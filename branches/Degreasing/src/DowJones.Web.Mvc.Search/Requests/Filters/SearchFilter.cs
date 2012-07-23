using DowJones.Search.Core;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.Search.Requests.Filters
{
    [JsonModelBinder]
    public class SearchFilter
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QueryFilters Include { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QueryFilters Exclude { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual SearchOperator? Operator { get; set; }

        public SearchFilter()
        {
            Exclude = new QueryFilters();
            Include = new QueryFilters();
        }
    }
}