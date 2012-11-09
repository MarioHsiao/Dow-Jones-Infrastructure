using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using DowJones.Web.Mvc.Routing;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Factiva.Currents.Website.Controllers
{
	using Contracts;

	[RoutePrefix("Pages")]
	public class PagesController : ControllerBase
	{
		private readonly IPageAssetProvider _pageAssetProvider;

		public PagesController(IPageAssetProvider pageAssetProvider)
		{
			_pageAssetProvider = pageAssetProvider;
		}

		[Route("{type}/{name}")]
		public ActionResult Index(string type, string name)
		{
			var modules = _pageAssetProvider.GetPageByName(name);

			return View(modules);
		}
	}
}
