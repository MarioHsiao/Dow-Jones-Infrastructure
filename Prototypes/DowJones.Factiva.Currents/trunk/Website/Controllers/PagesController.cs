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

		[Route("{name}")]
		public ActionResult Index(string name)
		{
			var canonicalName = name.Replace("-", " ");
			var modules = _pageAssetProvider.GetPageByName(canonicalName);

			return View(modules);
		}
	}
}
