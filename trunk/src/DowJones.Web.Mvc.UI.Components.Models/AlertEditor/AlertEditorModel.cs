using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Components.Common;
using Factiva.Gateway.Messages.Track.V1_0;
using Newtonsoft.Json;
using DowJones.Globalization;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	public class AlertEditorModel : ViewComponentModel
	{
	    private readonly IResourceTextManager _resources;
	    private Dictionary<string, string> _languageList;

        private List<CodeDesc> _emailFormats;
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

		public AlertEditorModel(IResourceTextManager resources = null)
		{
		    _resources = resources ?? DowJones.DependencyInjection.ServiceLocator.Resolve<IResourceTextManager>();
		}
	}
}