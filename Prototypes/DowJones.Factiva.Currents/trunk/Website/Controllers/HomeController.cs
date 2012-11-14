using System.Web.Mvc;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Factiva.Currents.Website.Controllers
{
	using Contracts;

	public class HomeController : ControllerBase
	{
		private readonly IPageAssetProvider _pageAssetProvider;

		public HomeController(IPageAssetProvider pageAssetProvider)
		{
			_pageAssetProvider = pageAssetProvider;
		}

		public ActionResult Index(string type, string name)
		{
			var pages = _pageAssetProvider.GetPages();
			return View(pages);
		}
	}
}
