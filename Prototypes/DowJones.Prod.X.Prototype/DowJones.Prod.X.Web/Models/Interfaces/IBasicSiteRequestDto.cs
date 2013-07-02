using DowJones.Preferences;
using DowJones.Prod.X.Models.Site;
using DowJones.Security.Interfaces;

namespace DowJones.Prod.X.Web.Models.Interfaces
{
    public interface IBasicSiteRequestDto
    {
        IPreferences Preferences { get; }

        IGenericSiteUrls GenericSiteUrls { get; }

        IActionProperties Properties { get; }

        IPrinciple Principle { get; }

        IUsageTrackingProperties UsageTrackingProperties { get; }

        IMainNavMenuProvider MainNavMenuProvider { get; }
    }
}