using System.Web.Mvc;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.MvcShowcase.Controllers
{
    public class BaseController : ControllerBase
    {
		[OutputCache(Duration = 12*60*60, VaryByParam = "mode")]
		public virtual ActionResult Data(string mode = "js")
		{
			var view = mode == "cs" ? "_cSharpData" : "_jsData";
			return PartialView(view);
		}       
	}
}
