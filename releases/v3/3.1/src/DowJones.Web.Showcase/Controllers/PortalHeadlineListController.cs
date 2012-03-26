using System.Web.Mvc;
using DowJones.Ajax.HeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Managers.Search;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Showcase.Models;
using Factiva.Gateway.Messages.Search.V2_0;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Web.Showcase.Controllers
{
    public class PortalHeadlineListController : ControllerBase
    {
        private readonly HeadlineListConversionManager _headlineListManager;
        private readonly SearchManager _searchManager;

        public PortalHeadlineListController(HeadlineListConversionManager headlineListManager, SearchManager searchManger)
        {
            _headlineListManager = headlineListManager;
            _searchManager = searchManger;
        }

        public ActionResult Index(string query = "obama and sc=j", int count = 10)
        {
            var model = GetPortalHeadlineListSection(query, count);
            return View("Index");          
        }

        public ActionResult Multi(string query = "obama and sc=j", int count = 10)
        {
            var model = new MultiviiewPortalHeadlinesListModel
                            {
                                AuthorView = GetPortalHeadlineListSection(query, count), 
                                HeadlineView = GetPortalHeadlineListSection(query, count),
                                TimelineView = GetPortalHeadlineListSection(query, count),
                            };

            model.AuthorView.Layout = PortalHeadlineListLayout.AuthorLayout;
            model.TimelineView.Layout = PortalHeadlineListLayout.TimelineLayout;

            return View("Multi", model);
        }

        public ActionResult PartialResult(string url, string callback)
        {
            var model = GetPortalHeadlineListSection(url);
            return ViewComponent(model, callback);
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
            request.NavigationControl.ReturnHeadlineCoding = true;
            request.MaxResults = count;
            request.FirstResult = 0;
            
            var results = _searchManager.PerformContentSearch<PerformContentSearchResponse>(request);
            var headlineListDataResult =  _headlineListManager.Process( results );
            var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert( headlineListDataResult );

            return new PortalHeadlineListModel()
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
            };
        }


        private PortalHeadlineListModel GetPortalHeadlineListSection(string url = null)
        {
            url = url ?? "http://feeds.haacked.com/haacked";
            var feed = _headlineListManager.ProcessFeed(url);

            HeadlineListDataResult headlineListDataResult = feed.result;
            var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert(headlineListDataResult);

            return new PortalHeadlineListModel()
                       {
                           MaxNumHeadlinesToShow = 5,
                           Result = portalHeadlineListDataResult,
                           ShowPublicationDateTime = true,
                           ShowAuthor = true,
                           ShowSource = true,
                           AuthorClickable = true,
                           SourceClickable = true
                       };
        }

        public ActionResult SectionSample()
        {
            return View("SectionSample");
        }
    }
}
