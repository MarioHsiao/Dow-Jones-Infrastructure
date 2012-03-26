using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.AlertEditor;
using DowJones.Search;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.UI.Components.Common;
using Factiva.Gateway.Messages.Track.V1_0;
using Newtonsoft.Json;
using DowJones.Globalization;
using SortOrder = Factiva.Gateway.Messages.Track.V1_0.SortOrder;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	public class AlertEditorModel : ViewComponentModel
	{
        private Dictionary<string, string> _languageList;

        ResourceTextManager _rtM = ResourceTextManager.Instance;
        private List<CodeDesc> _emailFormats;
        //private List<CodeDesc> _deliveryTimes;
        private AlertData _alertData = new AlertData();

        [ClientProperty("searchCriteriaEditable")]
        public bool SearchCriteriaEditable { get; set; }

        [ClientProperty("pressClipsEnabled")]
        public bool PressClipsEnabled { get; set; }

        [ClientProperty("pressClipsOnly")]
        public bool PressClipsOnly { get; set; }

        [ClientProperty("emailFormats")]
        public List<CodeDesc> EmailFormats
        {
            get
            {
                if (_emailFormats == null)
                {
                    _emailFormats = new List<CodeDesc>();
                    // Needs only TextHtml & TextPlain;
                    // The other two values (ApplicationPdf & ApplicationRtf) is not needed;
                    _emailFormats.Add(new CodeDesc
                    {
                        Code = ((int)DocumentFormat.TextHtml).ToString(),
                        Desc = _rtM.GetString("html")
                    });
                    _emailFormats.Add(new CodeDesc
                    {
                        Code = ((int)DocumentFormat.TextPlain).ToString(),
                        Desc = _rtM.GetString("plainText")
                    });
                }

                return _emailFormats;
            }
        }

        //[ClientProperty("deliveryTimes")]
        //public List<CodeDesc> DeliveryTimes
        //{
        //    get
        //    {
        //        if (_deliveryTimes == null)
        //        {
        //            _deliveryTimes = new List<CodeDesc>();
        //            foreach (DeliveryTimes delivery in Enum.GetValues(typeof(DeliveryTimes)))
        //            {
        //                _deliveryTimes.Add(new CodeDesc
        //                {
        //                    Code = ((int)delivery).ToString(),
        //                    Desc = GetDeliveryTimeString(delivery)
        //                });
        //            }

        //        }

        //        return _deliveryTimes;
        //    }
        //}

	    [ClientProperty("languageList")]
	    public Dictionary<string, string> LanguageList
	    {
	        get
	        {
                if(_languageList == null){
                    _languageList = new Dictionary<string, string>();
                    var sortedLangList = LanguageUtilityManager.GetSortedLanguageList();
                    foreach (var lang in sortedLangList)
                    {
                        _languageList.Add(lang.Key, lang.Value);   
                    }
                }
	            return _languageList;
	        }
	    }

		[ClientData]
		[JsonProperty("data")]
		public AlertData Data
		{
			get { return _alertData; }
			set { _alertData = value; }
		}

		public AlertEditorModel()
		{
		}

        private string GetDeliveryTimeString(DeliveryTimes deliveryTimes)
        {
            switch (deliveryTimes)
            {
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Morning:
                    return _rtM.GetString("morning");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Afternoon:
                    return _rtM.GetString("afternoon");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Both:
                    return _rtM.GetString("both");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Continuous:
                    return _rtM.GetString("continuous");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.None:
                    return _rtM.GetString("none");
                default:
                    return deliveryTimes.ToString();
            }
        } 
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AlertData : AlertProperties
	{
        [JsonProperty("properties")]
        public AlertProperties Properties { get; set; }

        [JsonProperty("searchQuery")]
		public AlertSearchQuery SearchQuery { get; set; }
	}

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