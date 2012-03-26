using System;
using System.Linq;
using DowJones.Managers.Search;
using DowJones.Web.Mvc.UI.Components.Common;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Newtonsoft.Json;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using Factiva.Gateway.Messages;
using System.Collections.Generic;
using Factiva.Gateway.Messages.Track.V1_0;
using Track = Factiva.Gateway.Messages.Track.V1_0;
using DowJones.Globalization;
using DowJones.Utilities.Search.Core;
using DowJones.Search;
using FilterItem = DowJones.Utilities.Search.Core.FilterItem;

namespace DowJones.Web.Mvc.UI.Components.Models
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
    }

    public class SearchCategoriesLookUpModel : ViewComponentModel
    {
        #region ..:: Private Members ::..

        private ResourceTextManager _rt = ResourceTextManager.Instance;
        private LookUpData _lookUpData = new LookUpData();
        #endregion

        #region ..:: Public Members ::..

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
                    _lookUpData.LanguageList = sortedLangList.Select(lang => new CodeDesc() {Code = lang.Key, Desc = lang.Value}).ToList();
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
        #endregion
        
        #region ..:: Constructor ::..

        public SearchCategoriesLookUpModel()
        {
            
        }

        #endregion
    }
}
