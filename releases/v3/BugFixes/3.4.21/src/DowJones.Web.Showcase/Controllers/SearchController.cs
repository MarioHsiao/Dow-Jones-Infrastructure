using System;
using System.Web.Mvc;
using DowJones.Managers.Sparkline;
using DowJones.Search;
using DowJones.Search.Navigation;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.Search.Controllers;
using DowJones.Web.Mvc.Search.Requests;
using DowJones.Web.Mvc.Search.Results;
using DowJones.Web.Mvc.Search.UI.Components.Results.Outlets;
using System.Collections.Generic;

namespace DowJones.Web.Showcase.Controllers
{
	public class SearchController : SearchControllerBase
	{
		public ActionResult Index()
		{
			return RedirectToAction("Results", new { keywords = "google" });
		}

        [Route("alert/results/{alertId}", Defaults = "{ kind: 'alert' }")]
        [Route("search/results/{kind}", Defaults = "{ kind: 'simple' }")]
		public override ActionResult Results(SearchRequest request, SearchResultsOptions options)
		{
            return base.Results(request, options);
		}

        protected override Mvc.Search.UI.Components.Results.SearchResults GetSearchResultsModel(Managers.Search.SearchResponse searchResponse, SearchRequest request, SearchResultsOptions options)
        {
            var navId = GetAuthorOrOutletId(request.PrimaryGroup);
            if (!String.IsNullOrEmpty(navId))
            {
                Navigator navigator = new Navigator();
                if (navId == "au")
                {
                    navigator = searchResponse.Navigators.Author;
                }
                else if (navId == "sc")
                {
                    navigator = searchResponse.Navigators.Source;
                }
                
                return new OutletsSearchResults();

            }
            return base.GetSearchResultsModel(searchResponse, request, options);
        }

        protected override Managers.Search.SearchResponse PerformSearch(Search.AbstractBaseSearchQuery query)
        {
            var navId = GetAuthorOrOutletId(query.PrimarySourceGroupId);
            if (!String.IsNullOrEmpty(navId))
            {
                query.CustomNavigation = new CustomNavigationSetting() { CustomNavigationId = new List<string>() { navId}, MaxBuckets = 1000};
                query.Pagination = new SearchQueryPagination() { MaxResults = 1, StartIndex = 0 };
            }
            return base.PerformSearch(query);
        }

        private static string GetAuthorOrOutletId(string primarySourceGroupId)
        {
            var channelId = (primarySourceGroupId != null) ? primarySourceGroupId.ToLower() : String.Empty;
            string navId = String.Empty;
            switch (channelId)
            {
                case "outlet":
                    navId = "sc";
                    break;
                case "author":
                    navId = "au";
                    break;
            }
            return navId;
        }

		[HttpGet]
		public ActionResult Sparklines([Bind(Prefix = "c")] string companies, [Bind(Prefix = "np")]uint dataPoints = 5u)
		{
			var companyCodes = companies.Split(new[] { ',' });
			var request = new SparklineServiceRequest { CompanyCodes = companyCodes, DataPoints = dataPoints };
			return base.Sparklines(request);
		}

		[HttpGet]
		public override ActionResult Related([Bind(Prefix = "t")] string text, [Bind(Prefix = "mt")] int maxTerms = 10)
		{
			return base.Related(text, maxTerms);
		}

		[HttpGet]
		public ActionResult Test(SearchRequest request)
		{
			return View("Test");
		}

		[HttpPost]
		[ActionName("Test")]
		public ActionResult TestPost(SearchRequest request)
		{
			return Json(request, JsonRequestBehavior.AllowGet);
		}
	}
}
