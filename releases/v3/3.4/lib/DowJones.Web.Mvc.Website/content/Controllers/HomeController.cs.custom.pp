﻿using System.Web.Mvc;
using DowJones.Assemblers.Headlines;
using DowJones.Managers.Search;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using Factiva.Gateway.Messages.Search.V2_0;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace $rootnamespace$.Controllers
{
    public class HomeController : DowJones.Web.Mvc.ControllerBase
    {

		private readonly HeadlineListConversionManager _headlineListManager;
        private readonly SearchManager _searchManager;

        public HomeController(HeadlineListConversionManager headlineListManager, SearchManager searchManger)
        {
            _headlineListManager = headlineListManager;
            _searchManager = searchManger;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Search(string query = "obama and sc=j", int count = 10)
        {
            var model = GetPortalHeadlineListSection(query, count);
            return View("Search", model);
        }

        private PortalHeadlineListModel GetPortalHeadlineListSection(string query, int count)
        {
            var request = new PerformContentSearchRequest();
            request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString
            {
                Mode = SearchMode.Traditional,
                Value = query,
            });
            request.StructuredSearch.Formatting.ExtendedFields = true;
            request.StructuredSearch.Formatting.MarkupType = MarkupType.Highlight;
            request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.PublicationDateReverseChronological;
            request.StructuredSearch.Formatting.SnippetType = SnippetType.Fixed;
            request.StructuredSearch.Formatting.FeaturedContentControl.ReturnFeaturedHeadlines = false;
            request.StructuredSearch.Version = "2.10";
            request.NavigationControl.ReturnHeadlineCoding = true;
            request.MaxResults = count;
            request.FirstResult = 0;

            var results = _searchManager.PerformContentSearch<PerformContentSearchResponse>(request);
            var headlineListDataResult = _headlineListManager.Process(results);
            var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert(headlineListDataResult);

            return new PortalHeadlineListModel
            {
                MaxNumHeadlinesToShow = 5,
                Result = portalHeadlineListDataResult,
                ShowAuthor = true,
                ShowSource = true,
                ShowPublicationDateTime = true,
                ShowTruncatedTitle = false,
                AuthorClickable = true,
                SourceClickable = true,
                DisplaySnippets = SnippetDisplayType.Hover,
                Layout = PortalHeadlineListLayout.HeadlineLayout,
                AllowPagination = false,
                PagePrevSelector = ".prev",
                PageNextSelector = ".next"
            };
        }
    }
}
