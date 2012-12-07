using System;
using System.Linq;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Models;

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
			ViewBag.AccessionNumber = an;
            var headline = _contentProvider.GetHeadlineByAccessionNumber(an);

			if (headline == null)
				return View();



			var model = new HeadlinePreview
				{
					Headline = headline,
				};

            return View(model);
		}

	}
}
