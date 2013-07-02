using System.Collections.Generic;
using System.Linq;
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
        private readonly AutoSuggestModel _suggestModel;
        private readonly IControlData _controlData;
        private readonly List<MenuItem<MainNavigationCategory>> _mainNavigationMenuItems;

        protected AbstractBasicSiteViewModel(IBasicSiteRequestDto basicSiteRequest, IControlData controlData, MainNavigationCategory mainNavigationCategory)
        {
            RequestDto = basicSiteRequest;
            _controlData = controlData;

            if (controlData != null)
            {
                _suggestModel = new AutoSuggestModel
                                    {
                                        SuggestServiceUrl = Settings.Default.SuggestServiceUrl,
                                        AutocompletionType = AutoCompletionType.Source,
                                        AuthType = AuthType.SessionId,
                                        AuthTypeValue = _controlData.SessionID,
                                        ServiceOptions = "{\"types\":\"companies\"}",
                                        ControlId = "djSourceAutoSuggest"
                                    };
            }

            _mainNavigationMenuItems = RequestDto.MainNavMenuProvider.GetMenuItems();

            foreach (var menuItem in _mainNavigationMenuItems.Where(menuItem => menuItem.SetActive(mainNavigationCategory)))
            {
                break;
            }
        }

        protected IBasicSiteRequestDto RequestDto { get; set; }

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
            get { return _suggestModel; }
        }

        public List<MenuItem<MainNavigationCategory>> MainMenuItems
        {
            get { return _mainNavigationMenuItems; }
        }

        public IPrinciple Principle
        {
            get { return RequestDto.Principle; }
        }
    }
}