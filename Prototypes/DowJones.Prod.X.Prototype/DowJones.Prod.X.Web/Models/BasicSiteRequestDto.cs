using DowJones.Preferences;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Web.Models.Interfaces;
using DowJones.Security.Interfaces;

namespace DowJones.Prod.X.Web.Models
{
    public class BasicSiteRequestDto : IBasicSiteRequestDto
    {
        public BasicSiteRequestDto(
            IPreferences preferences, 
            IPrinciple principle, 
            IGenericSiteUrls genericSiteUrls, 
            IActionProperties properties, 
            IUsageTrackingProperties usageTrackingProperties,
            IMainNavMenuProvider mainNavMenuProvider)
        {
            Preferences = preferences;
            GenericSiteUrls = genericSiteUrls;
            Properties = properties;
            Principle = principle;
            UsageTrackingProperties = usageTrackingProperties;
            MainNavMenuProvider = mainNavMenuProvider;
        }

        public IPreferences Preferences { get; private set; }

        public IGenericSiteUrls GenericSiteUrls { get; private set; }

        public IActionProperties Properties { get; private set; }

        public IPrinciple Principle { get; private set; }

        public IUsageTrackingProperties UsageTrackingProperties { get; private set; }

        public IMainNavMenuProvider MainNavMenuProvider { get; private set; }
    }
}