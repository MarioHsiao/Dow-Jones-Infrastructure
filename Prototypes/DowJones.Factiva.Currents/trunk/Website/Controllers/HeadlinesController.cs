using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Factiva.Currents.Website.Controllers
{
    public class HeadlinesController : Controller
    {
        //
        // GET: /Headline/

        public ActionResult Index(int year, int month, int day, string name, string an)
        {
	        var pubDate = new DateTime(year, month, day);

	        ViewBag.Title = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.Replace('-', ' '));
	        ViewBag.PublicationDate = pubDate;
	        ViewBag.AccessionNumber = an;

			return View();
        }

    }
}
