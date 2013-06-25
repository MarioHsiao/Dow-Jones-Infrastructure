using DowJones.Prod.X.Web.Models.Interfaces;

namespace DowJones.Prod.X.Web.Controllers.Base
{
    public interface IContentController
    {
        IBasicSiteRequestDto BasicSiteRequestDto { get; set; }
    }
}