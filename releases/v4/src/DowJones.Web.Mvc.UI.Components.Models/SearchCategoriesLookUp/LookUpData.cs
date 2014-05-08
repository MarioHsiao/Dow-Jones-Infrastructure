using System.Collections.Generic;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.UI.Components.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.SearchCategoriesLookUp
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LookUpData
    {
        private List<CodeDesc> _languageList;

        /// <summary>
        /// Gets the Language list.
        /// </summary>
        [JsonProperty("languageList")]
        public List<CodeDesc> LanguageList
        {
            get
            {
                return _languageList;
            }
            set { _languageList = value; }
        }

        /// <summary>
        /// Gets or sets the Filters.
        /// </summary>
        [JsonProperty("filters")]
        public FilterList<IFilterItem> Filters { get; set; }

        /// <summary>
        /// Gets or Sets the SourceGroup
        /// </summary>
        [JsonProperty("sourceGroup")]
        public List<SourceGroupItem> SourceGroup { get; set; }

        /// <summary>
        /// Gets or Sets the Source Filters
        /// </summary>
        [JsonProperty("additionalSourceFilters")]
        public List<CodeDesc> AdditionalSourceFilters { get; set; }

        /// <summary>
        /// Gets or Sets the Additional Footer Note
        /// </summary>
        [JsonProperty("additionalFooterNote")]
        public List<CodeDesc> AdditionalFooterNote { get; set; }
    }
}