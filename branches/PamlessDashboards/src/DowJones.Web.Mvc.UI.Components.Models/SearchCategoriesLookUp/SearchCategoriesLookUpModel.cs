using System.Linq;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.Search;
using Newtonsoft.Json;
using DowJones.Globalization;
using DowJones.Utilities.Search.Core;

namespace DowJones.Web.Mvc.UI.Components.SearchCategoriesLookUp
{
    public class SearchCategoriesLookUpModel : ViewComponentModel
    {
        private LookUpData _lookUpData = new LookUpData();

        /// <summary>
        /// Gets or sets the FilterType.
        /// </summary>
        [ClientProperty("filterType")]
        public FilterType FilterType { get; set; }

        /// <summary>
        /// Gets or sets the SuggestServiceUrl.
        /// </summary>
        [ClientProperty("suggestServiceUrl")]
        public string SuggestServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the EnableSaveList.
        /// </summary>
        [ClientProperty("enableSaveList")]
        public bool EnableSaveList { get; set; }

        /// <summary>
        /// Gets or sets the EnableBrowse.
        /// </summary>
        [ClientProperty("enableBrowse")]
        public bool EnableBrowse { get; set; }

        /// <summary>
        /// Gets or sets the EnableSourceList.
        /// </summary>
        [ClientProperty("enableSourceList")]
        public bool EnableSourceList { get; set; }

        /// <summary>
        /// Gets the Language list.
        /// </summary>
        [ClientData]
        [JsonProperty("data")]
        public LookUpData LookUpData { 
            get
            {
                if ((FilterType == FilterType.Language || FilterType == FilterType.Source) && _lookUpData.LanguageList == null)
                {
                    var sortedLangList = LanguageUtilityManager.GetSortedLanguageList();
                    _lookUpData.LanguageList = sortedLangList.Select(lang => new CodeDesc {Code = lang.Key, Desc = lang.Value}).ToList();
                }
                return _lookUpData;
            } 
            set { _lookUpData = value; }
        }

        /// <summary>
        /// Gets or sets the InterfaceLanguage.
        /// </summary>
        [ClientProperty("interfaceLanguage")]
        public string InterfaceLanguage { get{ return "en";} }

        /// <summary>
        /// Gets or sets the TaxanomyServiceUrl.
        /// </summary>
        [ClientProperty("taxonomyServiceUrl")]
        public string TaxonomyServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the QueriesServiceUrl.
        /// </summary>
        [ClientProperty("queriesServiceUrl")]
        public string QueriesServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the ProductId.
        /// </summary>
        [ClientProperty("productId")]
        public string ProductId { get; set; }
    }
}
