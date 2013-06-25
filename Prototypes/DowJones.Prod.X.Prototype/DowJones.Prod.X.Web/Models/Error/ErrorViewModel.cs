using System.Web.Mvc;
using DowJones.Prod.X.Web.Models.Interfaces;

namespace DowJones.Prod.X.Web.Models.Error
{
    public class ErrorModel
    {
        public HandleErrorInfo HandleErrorInfo { get; set; }
    }

    public class ErrorViewModel : AbstractBasicSiteViewModel<HandleErrorInfo>
    {
        public ErrorViewModel(IBasicSiteRequestDto basicSiteRequest)
            : base(basicSiteRequest)
        {

        }
    }
}