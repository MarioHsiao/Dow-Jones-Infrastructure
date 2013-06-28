using DowJones.DependencyInjection;
using DowJones.Preferences;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Web.Models.Interfaces;
using DowJones.Prod.X.Web.Properties;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Components.AutoSuggest;

namespace DowJones.Prod.X.Web.Models
{
    public abstract class AbstractBasicSiteViewModel<T> : IBasicSiteModel
    {
        protected AbstractBasicSiteViewModel(IBasicSiteRequestDto basicSiteRequest)
        {
            RequestDto = basicSiteRequest;
        }

        protected IBasicSiteRequestDto RequestDto { get; set; }

        [Inject("")]
        protected IControlData ControlData { get; set; }

        public T BaseActionModel { get; set; }

        public IPreferences Preferences
        {
            get { return RequestDto.Preferences; }
        }

        public IActionProperties Properties
        {
            get { return RequestDto.Properties; }
        }

        public IGenericSiteUrls GenericSiteUrls
        {
            get { return RequestDto.GenericSiteUrls; }
        }

        public IUsageTrackingProperties UsageTrackingProperties
        {
            get { return RequestDto.UsageTrackingProperties; }
        }

        public AutoSuggestModel AutoSuggest 
        { 
            get { return new AutoSuggestModel
                                          {
                                              SuggestServiceUrl = Settings.Default.SuggestServiceUrl,
                                              AutocompletionType = AutoCompletionType.Source,
                                              AuthType = AuthType.SessionId,
                                              AuthTypeValue = ControlData.SessionID,
                                              ServiceOptions = "{\"types\":\"blog\"}",
                                              ControlId = "djSourceAutoSuggest"
                                          };
            }
        }

        public IPrinciple Principle
        {
            get { return RequestDto.Principle; }
        }
    }
}