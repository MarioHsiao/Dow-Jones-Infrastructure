using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Models.Site.Home;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Models
{
    public class HomeSearchViewModel : AbstractBasicSiteViewModel<SearchModel>
    {
        public HomeSearchViewModel(Interfaces.IBasicSiteRequestDto basicSiteRequest, IControlData controlData, MainNavigationCategory category)
            : base(basicSiteRequest, controlData, category)
        {
        }
    }
}