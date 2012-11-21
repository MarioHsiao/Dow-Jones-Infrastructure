using System.Globalization;
using System.Web.Mvc;
using AttributeRouting;
using DowJones.Extensions;
using DowJones.Web.Mvc.Routing;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Factiva.Currents.Website.Controllers
{
	using Contracts;

	public class PagesController : ControllerBase
	{
		private readonly IPageAssetProvider _pageAssetProvider;

		public PagesController(IPageAssetProvider pageAssetProvider)
		{
			_pageAssetProvider = pageAssetProvider;
		}

		[OutputCache(CacheProfile = "PageCache")]
		public ActionResult Index(string name)
		{
			var canonicalName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.Replace("-", " "));
			var modules = _pageAssetProvider.GetPageByName(canonicalName);

			ViewBag.Title = "Dow Jones Current:: {0}".FormatWith(canonicalName);
			return View(modules);
		}
	}
}
