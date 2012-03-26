using System.Web.Mvc;
using DowJones.Web.Mvc;

namespace DowJones.Web.Showcase.Controllers
{
    public class HomeController : DowJonesControllerBase
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
