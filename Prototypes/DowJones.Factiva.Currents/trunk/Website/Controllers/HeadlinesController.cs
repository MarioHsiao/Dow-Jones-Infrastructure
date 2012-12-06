using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Factiva.Currents.Website.Contracts;

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
		    var headlines = _contentProvider.GetHeadlines(sc);

			return View(headlines);
        }

		[OutputCache(CacheProfile = "HeadlineCache")]
	    public ActionResult ViewAll(string sc)
	    {
		    ViewBag.SearchContext = sc;
		    return View();
	    }
    }
}
