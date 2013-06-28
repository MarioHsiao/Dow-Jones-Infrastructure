using DowJones.Preferences;
using DowJones.Web.Mvc.UI.Components.AutoSuggest;

namespace DowJones.Prod.X.Models.Site
{
    public interface IBasicSiteModel
    {
        IPreferences Preferences { get; }
        IActionProperties Properties { get; }
        IGenericSiteUrls GenericSiteUrls { get; }
        IUsageTrackingProperties UsageTrackingProperties { get; }
        AutoSuggestModel AutoSuggest { get; }
    }
}