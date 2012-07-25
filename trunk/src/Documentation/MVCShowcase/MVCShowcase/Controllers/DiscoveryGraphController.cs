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

		[OutputCache(Duration = 12*60*60, VaryByParam = "mode")]
		public override ActionResult Data(string mode = "js")
		{
			var view = mode == "cs" ? "_cSharpData" : "_jsData";
			return PartialView(view);
		}  
		
    }
}
