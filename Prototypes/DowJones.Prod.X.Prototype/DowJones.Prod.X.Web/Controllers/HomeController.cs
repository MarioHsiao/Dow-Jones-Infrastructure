using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Ajax;
using DowJones.Assemblers.Charting.MarketData;
using DowJones.Assemblers.Headlines;
using DowJones.DTO.Web.Request;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Loggers;
using DowJones.Managers.Charting.MarketData;
using DowJones.Prod.X.Common;
using DowJones.Prod.X.Core.DataTransferObjects;
using DowJones.Prod.X.Core.Services.Search;
using DowJones.Prod.X.Models.Search;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Models.Site.Home;
using DowJones.Prod.X.Web.Controllers.Base;
using DowJones.Prod.X.Web.Models;
using DowJones.Url;
using DowJones.Utilities;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.StockKiosk;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using SearchMode = DowJones.Prod.X.Models.Search.SearchMode;

namespace DowJones.Prod.X.Web.Controllers
{
    public enum SearchType
    {
        Company,
        Region,
        NewsSubject,
        Industry,
        Author,
        Executive,
        Keyword,
    }

    //[RequireAuthentication(Order = 0)]
    public class HomeController : BaseController
    {
        private readonly HeadlineListConversionManager _headlineListManager;
        private readonly SearchService _searchService;
        private readonly UrlHelper _urlHelper;
        public const string DefaultFileHandlerUrl = "~/DowJones.Web.Handlers.Article.ContentHandler.ashx";
        private readonly MarketDataInstrumentIntradayResultSetAssembler _marketDataInstrumentIntradayResultSetAssembler;
        private readonly ILog _log = LogManager.GetLogger(typeof (HomeController));

        public HomeController(HeadlineListConversionManager headlineListManager, SearchService searchService, UrlHelper urlHelper, MarketDataInstrumentIntradayResultSetAssembler marketDataInstrumentIntradayResultSetAssembler)
        {
            _headlineListManager = headlineListManager;
            _searchService = searchService;
            _urlHelper = urlHelper;
            _marketDataInstrumentIntradayResultSetAssembler = marketDataInstrumentIntradayResultSetAssembler;
        }

        public ActionResult Index()
        {
            var model = new HomeIndexViewModel(BasicSiteRequestDto, ControlData, MainNavigationCategory.Home)
                            {
                                BaseActionModel = new IndexModel()
                            };

            return View("Index", model);
        }

        [Route("/{searchType}/Search")]
        public ActionResult Search(string query = "", SearchType searchType = SearchType.Keyword, int firstResult = 0, int maxResults = 10)
        {
            var model = new HomeSearchViewModel(BasicSiteRequestDto, ControlData, MainNavigationCategory.Search)
                            {
                                BaseActionModel = GetNewsletterInformation(query, firstResult, maxResults)
                            };

            switch (searchType)
            {
                case SearchType.Company:
                    return View("Search-Company", model);
                default:
                    return View("Search", model);
            }

            
        }
        
        public ActionResult Hud()
        {
            var model = new HomeHudViewModel(BasicSiteRequestDto, ControlData, MainNavigationCategory.Search);

            return View("Hud", model);
        }


        private StockKioskModel GetStockKioskModel(IEnumerable<string> syms, SymbolType symbolType = SymbolType.FCode, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
        {
            var kioskModel = new StockKioskModel();
            var response = MarketDataChartingManager.GetMarketChartData(syms.ToArray(), symbolType, TimePeriod.OneDay, frequency);
            if (response.PartResults == null || !response.PartResults.Any())
            {
                return null;
            }
            var data = _marketDataInstrumentIntradayResultSetAssembler.Convert(response.PartResults);
            kioskModel.Data = data;
            kioskModel.PageSize = pageSize;
            if (kioskModel.PageSize > 8) kioskModel.PageSize = 8; //min as per the design to fit in

            return kioskModel;
        }

        private StockKioskModel GetStockKioskModel()
        {
            var syms = new List<string>(new[] { "ibm", "mcrost", "goog", "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" });
            return GetStockKioskModel(syms);
        }

        private IPerformContentSearchResponse GetPortalHeadlineListSection(string query, int firstResult, int maxResults)
        {
            var dto = new SearchServiceDTO
                          {
                              Query = query,
                              FirstResult = firstResult,
                              MaxResults = maxResults,
                              SearchMode = SearchMode.Simple,
                              SortOrder = SortOrder.RelevanceHighFreshness,
                          };

            var request = _searchService.BuildPerformContentSearchRequest<PerformContentSearchRequest>(dto);
            return _searchService.PerformSearch<PerformContentSearchRequest, PerformContentSearchResponse>(request);
        }

        private PortalHeadlineListModel GetPortalHeadlinesListModel(IPerformContentSearchResponse response, PortalHeadlineListLayout layout = PortalHeadlineListLayout.HeadlineLayout)
        {
            var headlineListDataResult = _headlineListManager.Process(response, GenerateUrl);
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
                Layout = layout,
                AllowPagination = false,
                PagePrevSelector = ".prev",
                PageNextSelector = ".next"
            };
        }

        private string GenerateUrl(ContentHeadline contentHeadline, bool isDuplicate)
        {
            // link to an action and send parameters 
            if (contentHeadline == null || contentHeadline.ContentItems == null)
            {
                return null;
            }

            var contentType = contentHeadline.ContentItems.ContentType.ToLower();
            var contentCategory = GetContentCategory(contentType);

            // look through based on type and provide the correct Uri
            switch (contentCategory)
            {
                case ContentCategory.Website:
                    foreach (
                        var item in
                            contentHeadline.ContentItems.ItemCollection.Where(
                                item =>
                                (item.Type.IsNotEmpty() &&
                                 item.Type.Trim().IsNotEmpty() &&
                                 item.Type.ToLower() == "webpage")))
                    {
                        return item.Ref; //_urlHelper.Action("Index", "Archive", GenerateEditionArticleRouteValueDictionary(contentHeadline));
                    }
                    break;
                case ContentCategory.Blog:
                case ContentCategory.Board:
                case ContentCategory.CustomerDoc:
                case ContentCategory.Internal:
                    foreach (var item in contentHeadline.ContentItems.ItemCollection.Where(item => item.Type.ToLower() == "webpage"))
                    {
                        return item.Ref;
                    }
                    break;
                case ContentCategory.Multimedia:
                    return _urlHelper.Action("Index", "Archive", GenerateEditionArticleRouteValueDictionary(contentHeadline));
                case ContentCategory.Publication:
                    if (contentType == "html" || contentType == "file" || contentType == "pdf")
                    {
                        var tItems = contentHeadline.ContentItems.ItemCollection
                                                    .Where(tItem => !string.IsNullOrEmpty(tItem.Mimetype)).ToList();

                        foreach (
                            var strHref in
                                tItems.Select(
                                    contentItem =>
                                    GetHandlerUrl(ImageType.Display, contentHeadline.AccessionNo, contentItem))
                                      .Where(strHref => contentType == "pdf"))
                        {
                            return strHref;
                        }
                    }
                    break;
            }
            return _urlHelper.Action("Index", "Archive", GenerateEditionArticleRouteValueDictionary(contentHeadline));
        }

        private static ContentCategory GetContentCategory(string contentType)
        {
            switch (contentType)
            {
                case "webpage":
                    return ContentCategory.Website;
                case "file":
                case "article":
                case "pdf":
                case "analyst":
                case "html":
                case "articlewithgraphics":
                    return ContentCategory.Publication;
                case "picture":
                    return ContentCategory.Picture;
                case "multimedia":
                    return ContentCategory.Multimedia;
                case "internal":
                    return ContentCategory.Internal;
                case "summary":
                    return ContentCategory.Summary;
                case "board":
                    return ContentCategory.Board;
                case "blog":
                    return ContentCategory.Blog;
                case "customerdoc":
                    return ContentCategory.CustomerDoc;
                default:
                    return ContentCategory.External;
            }
        }

        private RouteValueDictionary GenerateEditionArticleRouteValueDictionary(ContentHeadline contentHeadline)
        {
            var dictionary = new RouteValueDictionary
                                 {
                                     {
                                         CommonRequestParameterNames.AccessionNumberParamName,
                                         contentHeadline.AccessionNo
                                     },
                                     {
                                         CommonRequestParameterNames.AccessPointCodeParamName,
                                         ControlData.AccessPointCode
                                     }
                                 };

            return new RouteValueDictionary(dictionary);
        }
        
        /// <summary>
        /// The get handler url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="contentItem">The content item.</param>
        /// <param name="isBlob">if set to <c>true</c> [is BLOB].</param>
        /// <returns>
        /// A string representing the Handerl Url
        /// </returns>
        private string GetHandlerUrl(ImageType imageType, string accessionNo, ContentItem contentItem, bool isBlob = false)
        {
            var reference = contentItem.Ref;
            var mimeType = contentItem.Mimetype;

            if (DefaultFileHandlerUrl.HasValue())
            {
                var ub = new UrlBuilder(DefaultFileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "ImageType"), Map(imageType));
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "IsBlob"), (isBlob) ? "y" : "");

                if (!ControlData.AccessPointCode.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), ControlData.AccessPointCode);
                }

                if (!Preferences.InterfaceLanguage.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), Preferences.InterfaceLanguage);
                }

                if (!ControlData.ProductID.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), ControlData.ProductID);
                }

                if (!ControlData.AccessPointCodeUsage.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), ControlData.AccessPointCodeUsage);
                }

                if (!ControlData.CacheKey.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "CacheKey"), ControlData.CacheKey);
                }

                if (!ControlData.ClientCode.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), ControlData.ClientCode);
                }

                if (!ControlData.SessionID.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), ControlData.SessionID);
                }
                else if (!ControlData.EncryptedToken.IsNullOrEmpty()) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), ControlData.EncryptedToken);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), ControlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), ControlData.UserPassword);
                }

                return ub.ToString();
            }
            return null;
        }

        private static string Map(ImageType imageType)
        {
            switch (imageType)
            {
                default:
                    return "dispix";
                case ImageType.Fingernail:
                    return "fnail";
                case ImageType.Thumbnail:
                    return "tnail";
            }
        }

        private SearchModel GetNewsletterInformation(string query, int firstResult = 0, int maxResult = 10)
        {
            try
            {
                using (new TransactionLogger(_log, MethodBase.GetCurrentMethod()))
                {
                    var taskFactory = TaskFactoryManager.Instance.GetDefaultTaskFactory();
                    var getRealtimeHeadlinesTask = taskFactory.StartNew(() => GetPortalHeadlineListSection(query + " rst:SFDJNW", firstResult, 5));
                    var getMainHeadlinesTask = taskFactory.StartNew(() => GetPortalHeadlineListSection(query, firstResult, 20));

                    Task.WaitAll(getRealtimeHeadlinesTask, getMainHeadlinesTask);

                    return new SearchModel
                               {
                                   Realtime = GetPortalHeadlinesListModel(getRealtimeHeadlinesTask.Result, PortalHeadlineListLayout.TimelineLayout),
                                   Headlines =  GetPortalHeadlinesListModel(getMainHeadlinesTask.Result)
                               };
                }
            }
            catch (AggregateException exception)
            {
                foreach (var ex in exception.InnerExceptions)
                {
                    throw ex;
                }
            }

            throw new DowJonesUtilitiesException();
        }
        
    }
}