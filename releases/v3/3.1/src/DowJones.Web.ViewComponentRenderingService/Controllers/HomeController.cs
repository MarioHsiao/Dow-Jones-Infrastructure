using System.Web.Mvc;

namespace DowJones.Web.Article.Website.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            // Just return that we are alive
            return new HttpStatusCodeResult(200, "Working!");
        }

    }
}
