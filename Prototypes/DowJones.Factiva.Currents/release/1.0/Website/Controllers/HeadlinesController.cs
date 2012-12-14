using System.Web.Mvc;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Models;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Website.Controllers
{
    public class HeadlinesController : Controller
    {
	    private readonly IContentProvider _contentProvider;

        public HeadlinesController(IContentProvider contentProvider)
	    {
            _contentProvider = contentProvider;
	    }

		[OutputCache(CacheProfile = "HeadlineCache")]
	    public ActionResult Index(string sc)
		{
			
		    var headlineResult = _contentProvider.GetHeadlines(sc);

			var model = new HeadlineList
				{
					ViewAllSearchContext = headlineResult.Package.ViewAllSearchContextRef,
					Headlines = new CurrentsHeadlineModel(new PortalHeadlineListModel(headlineResult.Package.Result)),
				};
		    model.Headlines.MaxNumHeadlinesToShow = 20;
			return View(model);
        }

		[OutputCache(CacheProfile = "HeadlineCache")]
	    public ActionResult ViewAll(string sc)
	    {
		    ViewBag.SearchContext = sc;
		    return View();
	    }
    }
}
