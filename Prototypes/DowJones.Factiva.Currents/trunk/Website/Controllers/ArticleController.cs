using System;
using System.Globalization;
using System.Web.Mvc;
using DowJones.Factiva.Currents.Website.Contracts;

namespace DowJones.Factiva.Currents.Website.Controllers
{
	public class ArticleController : Controller
	{
        private readonly IContentProvider _contentProvider;

        public ArticleController(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

		[OutputCache(CacheProfile = "ArticleCache")]
		public ActionResult Index(int year, int month, int day, string name, string an)
		{
			var pubDate = new DateTime(year, month, day);

			ViewBag.Title = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.Replace('-', ' '));
			ViewBag.PublicationDate = pubDate;
			ViewBag.AccessionNumber = an;

            var headlines = _contentProvider.GetHeadlinesByAccessionNumber(an);
            headlines.CurrentsHeadline.ShowSnippet = true;
            return View(headlines);
		}

	}
}
