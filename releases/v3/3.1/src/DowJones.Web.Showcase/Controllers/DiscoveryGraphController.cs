using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.DiscoveryGraph;

namespace DowJones.Web.Showcase.Controllers
{
    public class DiscoveryGraphController : Controller
    {
        //
        // GET: /DiscoveryGraph/

        public ActionResult Index()
        {
            var discoveryGraph = new DiscoveryGraphModel();
            return View("Index", discoveryGraph);
        }

    }
}
