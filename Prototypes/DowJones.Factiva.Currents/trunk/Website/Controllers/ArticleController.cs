using System;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using DowJones.Factiva.Currents.Website.Contracts;

namespace DowJones.Factiva.Currents.Website.Controllers
{
	public class ArticleController : Controller
	{
        private readonly IContentProvider _contentProvider;
        private readonly string _loginUrl;
        private readonly string _signUpUrl;

        public ArticleController(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
            _loginUrl =  ConfigurationManager.AppSettings.Get("LoginUrl").Trim('/');
            _signUpUrl = ConfigurationManager.AppSettings.Get("SignupUrl").Trim('/');
        }

		[OutputCache(CacheProfile = "ArticleCache")]
		public ActionResult Index(int year, int month, int day, string name, string an)
		{
			var pubDate = new DateTime(year, month, day);

			ViewBag.Title = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.Replace('-', ' '));
			ViewBag.PublicationDate = pubDate;
			ViewBag.AccessionNumber = an;

            var headlines = _contentProvider.GetHeadlinesByAccessionNumber(an);
            if (headlines.CurrentsHeadline != null)
            {
                headlines.CurrentsHeadline.ShowLanguage = true;
                headlines.CurrentsHeadline.ShowWordCount = true;
                headlines.CurrentsHeadline.ShowSnippet = true;
                headlines.CurrentsHeadline.ShowSourceCode = true;
            }

            headlines.LogInUrl =_loginUrl;
            headlines.SignUpUrl = _signUpUrl;
            return View(headlines);
		}

	}
}
