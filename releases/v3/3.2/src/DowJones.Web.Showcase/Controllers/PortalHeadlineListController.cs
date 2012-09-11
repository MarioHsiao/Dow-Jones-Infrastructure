using System;
using System.Linq;
using System.Web.Mvc;
using DowJones.Ajax.HeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Extensions;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Web.Mvc.ModelBinders;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
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

        public ActionResult Picture(string query="obama", int count = 10)
        {
            return View("Index", GetPortalHeadlineListPictureSection(query, count));
        }

        public ActionResult Index(string query = "obama and sc=j", int count = 10)
        {
            var model = GetPortalHeadlineListSection(query, count);
            return View("Index", model);          
        }

        public ActionResult ComponentExplorerDemo(string query = "obama and sc=j", int count = 10)
		{
			return View("Index", "_Layout_ComponentExplorer", new PortalHeadlineListModel
			{
				MaxNumHeadlinesToShow = 5,
				ShowAuthor = true,
				ShowSource = true,
				ShowPublicationDateTime = true,
				ShowTruncatedTitle = false,
				AuthorClickable = true,
				SourceClickable = true,
				DisplaySnippets = SnippetDisplayType.Hover,
				Layout = PortalHeadlineListLayout.HeadlineLayout,
				PagePrevSelector = ".prev",
				PageNextSelector = ".next"

			});
		}

        public ActionResult AccessionNumSearch([ModelBinder(typeof(CommaStringSplitModelBinder))]string[] accessionNums)
        {
            if (accessionNums.IsNullOrEmpty())
            {
                accessionNums = new[] { 
					//"J000000020120330e83u0003i", 
					//"J000000020120330e83u0001v", 
					//"J000000020120330e83u0003x", 
					//"J000000020120330e83u0003o", 
					//"APPIC00020120330e83u003ec", 
					"DJFDBR0020120315e83f00001",
					"DJFVW00020120406e846mueo8"
					
				};
                //BALO000020090710e57a00023 BALO000020090710e57a00022 
                //EDIFIN0020031215dzbr000nd EDIFIN0020031215dzbr000x3 
                //WC93409020090709e5780000j WC93409020090709e5780000g 
                //BOARDB0020090710e57a011xf BOARDB0020090710e57a011n7 
                //BOARDR0020090710e57a08zb7 BOARDR0020090710e57a08z6f 
                //X900550020090710e57a0005l X900550020090710e57a0002t 
                //MMSAUX0020090710e57a0005l MMSAUX0020090710e57a0002u 
                //MMSAJZ0020090710e57a00001 MMSAJZ0020090709e5790002t

                // 
            }

            var request = new AccessionNumberSearchRequestDTO
                              {
                                     SortBy = SortBy.FIFO,
                                     MetaDataController =
                                         {
                                             Mode = CodeNavigatorMode.None
                                         },
                                     DescriptorControl =
                                         {
                                             Mode = DescriptorControlMode.None,
                                             Language = "en"
                                         },
                                     AccessionNumbers = accessionNums.ToArray()
                                 };
            request.MetaDataController.ReturnCollectionCounts = false;
            request.MetaDataController.ReturnKeywordsSet = false;
            request.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.None;  
            request.SearchCollectionCollection.Clear();
            request.SearchCollectionCollection.AddRange(Enum.GetValues(typeof(SearchCollection)).Cast<SearchCollection>());
            var results = _searchManager.PerformAccessionNumberSearch<PerformContentSearchRequest, PerformContentSearchResponse>(request);

            var headlineListDataResult = _headlineListManager.Process(results);
            var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert(headlineListDataResult);

            var model =  new PortalHeadlineListModel
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

            return View("Index", model);
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

        private  PortalHeadlineListModel GetPortalHeadlineListPictureSection(string query, int count)
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
            request.StructuredSearch.Query.SearchCollectionCollection.Add(SearchCollection.Pictures);
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


        private PortalHeadlineListModel GetPortalHeadlineListSection(string url = null)
        {
            url = url ?? "http://feeds.haacked.com/haacked";
            var feed = _headlineListManager.ProcessFeed(url);

            HeadlineListDataResult headlineListDataResult = feed.result;
            var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert(headlineListDataResult);

            return new PortalHeadlineListModel
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
