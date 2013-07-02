using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Models.Site.Archive;
using DowJones.Prod.X.Web.Models.Interfaces;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Models
{
    public class ArchiveIndexViewModel : AbstractBasicSiteViewModel<ArchiveModel>
    {
        public ArchiveIndexViewModel(IBasicSiteRequestDto basicSiteRequest, IControlData controlData, MainNavigationCategory category)
            : base(basicSiteRequest, controlData, category)
        {
        }
    }
}