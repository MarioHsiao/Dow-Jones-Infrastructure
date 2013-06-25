using DowJones.Preferences;

namespace DowJones.Prod.X.Models.Site
{
    public interface IBasicSiteModel
    {
        IPreferences Preferences { get; }
        IActionProperties Properties { get; }
        IGenericSiteUrls GenericSiteUrls { get; }
        IUsageTrackingProperties UsageTrackingProperties { get; }
    }
}