using System.Web.Mvc;
using DowJones.Web.Mvc.Infrastructure;
using DowJones.Web.Mvc.Threading;

namespace DowJones.Web.Showcase.Controllers
{
    public class StaDemoController : AspCompatControllerBase
    {
        public ActionResult Index()
        {
            return View("ApartmentState", new { ActionName = "Index" });
        }

        public ActionResult Test1()
        {
            return View("ApartmentState", new { ActionName = "Test1" });
        }

        public ActionResult Test2()
        {
            return View("ApartmentState", new { ActionName = "Test2" });
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewData["ApartmentState"] = System.Threading.Thread.CurrentThread.GetApartmentState();
        }
    }
}
