using System.Web.Mvc;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Web.Models.Interfaces;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Models.Error
{
    public class ErrorModel
    {
        public HandleErrorInfo HandleErrorInfo { get; set; }
    }

    public class ErrorViewModel : AbstractBasicSiteViewModel<HandleErrorInfo>
    {
        public ErrorViewModel(IBasicSiteRequestDto basicSiteRequest, IControlData controlData, MainNavigationCategory category)
            : base(basicSiteRequest, controlData, category)
        {

        }
    }
}