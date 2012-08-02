using System.Web.Mvc;

namespace DowJones.DegreasedDashboards.Website.Controllers
{
    public class PagesController : DowJones.Web.Mvc.UI.Canvas.Controllers.PageControllerBase
    {
        public ActionResult Index()
        {
            return RedirectToAction("Page", new {id = "1234"});
        }
    }
}
