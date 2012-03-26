using System.Collections.Generic;
using DowJones.Search;
using DowJones.Utilities.Search.Core;
using Newtonsoft.Json;
using SortOrder = Factiva.Gateway.Messages.Track.V1_0.SortOrder;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class AlertSearchQuery
    {
        [JsonProperty("freeText")]
        public string FreeText { get; set; }

        [JsonProperty("filters")]
        public SearchChannelFilters Filters { get; set; }

        [JsonProperty("newsFilters")]
        public SearchNewsFilters NewsFilters { get; set; }

        [JsonProperty("contentLanguages")]
        public IEnumerable<string> ContentLanguages { get; set; }

        [JsonProperty("searchIn")]
        public SearchFreeTextArea SearchIn { get; set; }

        [JsonProperty("sortBy")]
        public SortOrder SortBy { get; set; }

        [JsonProperty("dateRange")]
        public SearchDateRange DateRange { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("exclusionFilter")]
        public IEnumerable<ExclusionFilter> ExclusionFilter { get; set; }

        [JsonProperty("duplicates")]
        public bool Duplicates { get; set; }

        [JsonProperty("socialMedia")]
        public bool IncludeSocialMedia { get; set; }
    }
}