using System.Collections.Generic;
using DowJones.Globalization;
using DowJones.Search;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.UI.Components.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.SearchBuilder
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SearchBuilderData
    {
        [JsonProperty("freeText")]
        public string FreeText { get; set; }

        [JsonProperty("topicId")]
        public string TopicId { get; set; }

        [JsonProperty("alertId")]
        public string AlertId { get; set; }

        [JsonProperty("filters")]
        public SearchChannelFilters Filters { get; set; }

        [JsonProperty("newsFilters")]
        public SearchNewsFilters NewsFilters { get; set; }

        [JsonModelBinder]
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

        /// <summary>
        /// Gets or Sets the Exclusions list
        /// </summary>
        [JsonProperty("exclusionsList")]
        public List<CodeDesc> ExclusionsList { get; set; }

        /// <summary>
        /// Gets or Sets the SourceGroup
        /// </summary>
        [JsonProperty("sourceGroup")]
        public List<SourceGroupItem> SourceGroup { get; set; }

        /// <summary>
        /// Gets or Sets the LookUp Addition Footer Notes
        /// </summary>
        [JsonProperty("lookUpAdditionalFooterNotes")]
        public Dictionary<DowJones.Utilities.Search.Core.FilterType, string> LookUpAdditionalFooterNotes { get; set; }

        /// <summary>
        /// Gets or Sets the Source Filters
        /// </summary>
        [JsonProperty("additionalSourceFilters")]
        public List<CodeDesc> AdditionalSourceFilters { get; set; }
    
        public SearchBuilderData(IResourceTextManager resources = null)
        {
            resources = resources ?? DowJones.DependencyInjection.ServiceLocator.Resolve<IResourceTextManager>();
            ExclusionsList = SearchBuilderModel.GetCodeValueFromEnum<ExclusionFilter>(resources);
        }
    }
}