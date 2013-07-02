using System.Collections.Generic;
using DowJones.Preferences;
using DowJones.Web.Mvc.UI.Components.AutoSuggest;
using DowJones.Web.Mvc.UI.Components.Menu;

namespace DowJones.Prod.X.Models.Site
{
    public enum MainNavigationCategory
    {
        Unspecified,
        Home,
        Dashboard,
        Search,
        Archive,
        News,
        RealTime,
        Preferences,
        Labs
    }

    public class MenuItem<T>
    {
        public T NavigationCategory { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public string IconClass { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }

        public bool SetActive(T category)
        {
            if (NavigationCategory.Equals(category))
            {
                Active = true;
                return true;
            }

            return false;
        }

    }

    public interface IBasicSiteModel
    {
        IPreferences Preferences { get; }
        IActionProperties Properties { get; }
        IGenericSiteUrls GenericSiteUrls { get; }
        IUsageTrackingProperties UsageTrackingProperties { get; }
        AutoSuggestModel AutoSuggest { get; }
        List<MenuItem<MainNavigationCategory>> MainMenuItems { get; }
    }

}