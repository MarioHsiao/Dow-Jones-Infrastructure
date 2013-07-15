using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Web.Models.Interfaces;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Models
{
    public class HomeHudViewModel : AbstractBasicSiteViewModel<string>
    {
        public HomeHudViewModel(IBasicSiteRequestDto basicSiteRequest, IControlData controlData, MainNavigationCategory mainNavigationCategory) 
            : base(basicSiteRequest, controlData, mainNavigationCategory)
        {
        }
    }
}