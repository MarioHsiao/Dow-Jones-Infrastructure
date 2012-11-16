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
	    private readonly ISearchContext _searchContextProvider;

	    public HeadlinesController(ISearchContext searchContextProvider)
	    {
		    _searchContextProvider = searchContextProvider;
	    }

	    public ActionResult Index(string sc)
	    {
		    var headlines = _searchContextProvider.GetHeadlines(sc);

			return View(headlines);
        }

	    public ActionResult ViewAll(string sc)
	    {
		    ViewBag.SearchContext = sc;
		    return View();
	    }
    }
}
