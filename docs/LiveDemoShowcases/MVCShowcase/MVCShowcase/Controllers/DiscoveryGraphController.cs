using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.DiscoveryGraph;

namespace DowJones.MvcShowcase.Controllers
{
	public class DiscoveryGraphController : BaseController
    {

        public ActionResult Index()
        {
			var model = new DiscoveryGraphModel();
            return View(model);
        }

    }
}
