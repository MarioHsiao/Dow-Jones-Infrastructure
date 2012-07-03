using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.DiscoveryGraph;

namespace DowJones.Web.Showcase.Controllers
{
    public class DiscoveryGraphController : Controller
    {
        //
        // GET: /DiscoveryGraph/

        public ActionResult Index(string master = "_Layout")
        {
            var discoveryGraph = new DiscoveryGraphModel();
            return View("Index", master, discoveryGraph);
        }

		public ActionResult ComponentExplorerDemo(bool interact = false)
		{
			ViewBag.IsInteractive = interact;
			return Index("_Layout_ComponentExplorer");
		}
    }
}
