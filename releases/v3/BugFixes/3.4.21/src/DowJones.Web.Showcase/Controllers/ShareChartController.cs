using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.ShareChart;

namespace DowJones.Web.Showcase.Controllers
{
    public class ShareChartController : Controller
    {
        //
        // GET: /ShareChart/

        public ActionResult Index()
        {
            return View("Index", new ShareChartModel());
        }

    }
}
