using DowJones.Preferences;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Web.Models.Interfaces;
using DowJones.Security.Interfaces;

namespace DowJones.Prod.X.Web.Models
{
    public abstract class AbstractBasicSiteViewModel<T> : IBasicSiteModel
    {
        protected AbstractBasicSiteViewModel(IBasicSiteRequestDto basicSiteRequest)
        {
            RequestDto = basicSiteRequest;
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
        
        public IPrinciple Principle
        {
            get { return RequestDto.Principle; }
        }
    }
}