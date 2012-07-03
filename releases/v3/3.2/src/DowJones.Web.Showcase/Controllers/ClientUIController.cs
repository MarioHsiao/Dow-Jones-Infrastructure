using System.Web.Mvc;

namespace DowJones.Web.Showcase.Controllers
{
    public class ClientUIController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowLoading( )
        {
            return View("ShowLoading");
        }
    }
}
