using System.Collections.Generic;
using DowJones.AlertEditor;
using DowJones.Search;
using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Track.V1_0;
using Newtonsoft.Json;
using DowJones.Globalization;
using SortOrder = Factiva.Gateway.Messages.Track.V1_0.SortOrder;
using DowJones.Web.Mvc.UI.Components.Search;

namespace DowJones.Web.Mvc.UI.Components.AlertEditor
{
	public class AlertEditorModel : ViewComponentModel
	{
        private Dictionary<string, string> _languageList;

        private readonly IResourceTextManager _resources;
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
                        Desc = _resources.GetString("html")
                    });
                    _emailFormats.Add(new CodeDesc
                    {
                        Code = ((int)DocumentFormat.TextPlain).ToString(),
                        Desc = _resources.GetString("plainText")
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

        private string GetDeliveryTimeString(DeliveryTimes deliveryTimes)
        {
            switch (deliveryTimes)
            {
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Morning:
                    return _resources.GetString("morning");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Afternoon:
                    return _resources.GetString("afternoon");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Both:
                    return _resources.GetString("both");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Continuous:
                    return _resources.GetString("continuous");
                case Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.None:
                    return _resources.GetString("none");
                default:
                    return deliveryTimes.ToString();
            }
        }

        public AlertEditorModel(IResourceTextManager resources = null)
        {
            _resources = resources ?? DowJones.DependencyInjection.ServiceLocator.Resolve<IResourceTextManager>();
        }
	}
}