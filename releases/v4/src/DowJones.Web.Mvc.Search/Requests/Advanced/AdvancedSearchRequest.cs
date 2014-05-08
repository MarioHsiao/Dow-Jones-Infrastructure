using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests.Filters;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.Search.Requests
{
    public abstract class AdvancedSearchRequest : SearchRequest
    {
        internal IEnumerable<SearchFilter> Channels
        {
            get
            {
                return new [] {
                    Author,
                    Company,
                    Executive,
                    Industry,
                    Subject,
                    Region,
                }.WhereNotNull();
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchFilter Author { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchFilter Company { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ExclusionFilter> Exclusions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchFilter Executive { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchFilter Industry { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchFilter Subject { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SearchFilter Region { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SourceSearchFilter Source { get; set; }
    }

    [JsonModelBinder]
    public class SourceSearchFilter
    {
        // NOTE: Must be List<QueryFilters> for deserialization purposes - DO NOT CHANGE!
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<QueryFilters> Include { get; set; }

        // NOTE: Must be List<QueryFilters> for deserialization purposes - DO NOT CHANGE!
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<QueryFilters> Exclude { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ListId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ListName { get; set; }

        public SourceSearchFilter()
        {
            Exclude = new List<QueryFilters>();
            Include = new List<QueryFilters>();
        }
    }
}