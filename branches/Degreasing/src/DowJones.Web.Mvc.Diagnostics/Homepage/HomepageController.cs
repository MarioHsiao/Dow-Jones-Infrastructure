using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.Homepage
{
    public class HomepageController : Controller
    {
        public ActionResult Index()
        {
            return new DiagnosticsViewAction<Homepage>();
        }
    }
}
