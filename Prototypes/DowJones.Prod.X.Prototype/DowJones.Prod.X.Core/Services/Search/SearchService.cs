using System;
using System.Linq;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Loggers;
using DowJones.Managers.Abstract;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Preferences;
using DowJones.Prod.X.Core.DataTransferObjects;
using DowJones.Prod.X.Core.Interfaces;
using DowJones.Prod.X.Models.Search;
using DowJones.Session;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;
using DeduplicationMode = DowJones.Prod.X.Models.Search.DeduplicationMode;
using ISearchService = DowJones.Prod.X.Core.Interfaces.ISearchService;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Prod.X.Core.Services.Search
{
    public class SearchService : AbstractAggregationManager, ISearchService
    {
        private readonly HeadlineListConversionManager _headlineListConversionManager;
        private readonly IHeadlineUtilityService _headlineUtility;
        private readonly ILog _log = LogManager.GetLogger(typeof (SearchService));
        private readonly IPreferences _preferences;
        private readonly SearchManager _searchManager;

        public SearchService(IControlData controlData,
                             HeadlineListConversionManager headlineListConversionManager,
                             SearchManager searchManager,
                             IHeadlineUtilityService headlineUtilityService,
                             ITransactionTimer transactionTimer,
                             IPreferences preferences)
            : base(controlData, transactionTimer)
        {
            _headlineListConversionManager = headlineListConversionManager;
            _searchManager = searchManager;
            _preferences = preferences;
            _headlineUtility = headlineUtilityService;
        }

        protected override ILog Log
        {
            get { return _log; }
        }

        public PortalHeadlineListDataResult GetPortalHeadlineListDataResult<TRequest, TResponse>(IPerformContentSearchResponse response, int startIndex = 0)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new()
        {
            return PortalHeadlineConversionManager.Convert(_headlineListConversionManager.Process(response, startIndex, _headlineUtility.GenerateUrl), TruncationType.None);
        }

        public TRequest BuildPerformContentSearchRequest<TRequest>(SearchServiceDTO searchDto)
            where TRequest : IPerformContentSearchRequest, new()
        {
            var formatting = new ResultFormatting
                                 {
                                     ExtendedFields = true,
                                     MarkupType = MarkupType.Highlight,
                                     SnippetType = SnippetType.Fixed,
                                     SortOrder = searchDto.SortOrder.HasValue ? ((SortOrder)searchDto.SortOrder).MapGatewaySortOrder() :SortOrder.PublicationDateReverseChronological.MapGatewaySortOrder(),
                                     DeduplicationMode = searchDto.DeduplicationMode.HasValue ? ((DeduplicationMode)searchDto.DeduplicationMode).MapGatewayDeduplicationModel() : DeduplicationMode.NearExact.MapGatewayDeduplicationModel(),
                                     FeaturedContentControl = {ReturnFeaturedHeadlines = false}
                                 };

            switch (formatting.SortOrder)
            {
                case ResultSortOrder.Relevance:
                case ResultSortOrder.RelevanceHighFreshness:
                case ResultSortOrder.RelevanceMediumFreshness:
                    formatting.FreshnessDate = DateTime.Today.ToUniversalTime();
                    break;
            }

            formatting.ClusterMode = ClusterMode.Off;

            var structuredSearch = new StructuredSearch
                                       {
                                           Linguistics =
                                               {
                                                   LemmatizationOn = true,
                                                   SpellCheckMode = LinguisticsMode.Off,
                                                   SymbolRecognitionMode = LinguisticsMode.Off
                                               },
                                           Formatting = formatting,
                                           Version = "2.10",
                                          
                                       };
            //structuredSearch.Query.Dates.After = _preferences.SimpleSearchDateRange.Convert();
            structuredSearch.Query.Dates.After = "-33";

            structuredSearch.Query.SearchCollectionCollection = new SearchCollectionCollection
                                                 {
                                                     SearchCollection.Publications,
                                                     SearchCollection.Pictures,
                                                     SearchCollection.Multimedia,
                                                     SearchCollection.WebSites,
                                                     SearchCollection.ABlogs
                                                 };
            structuredSearch.Query.SearchStringCollection.AddRange(
                new[]
                    {
                        new SearchString
                            {
                                Id = "FreeText",
                                Type = SearchType.Free,
                                Mode = searchDto.SearchMode.MapGatewaySearchMode(),
                                Value = searchDto.Query
                            },
                        new SearchString
                            {
                                Id = "BSSSource",
                                Mode = SearchMode.Traditional,
                                Value =
                                    "(fmt=(article or report or file or webpage or blog or multimedia or picture) AND (fmt=((article or report or file) or webpage)))"
                            }
                    });

                    //Add content language preference
                    if (_preferences.ContentLanguages.Any())
                    {

                        structuredSearch.Query.SearchStringCollection.Add(new SearchString
                                                       {
                                                           Id = "la",
                                                           Scope = "la",
                                                           Type = SearchType.Controlled,
                                                           Mode = SearchMode.Any,
                                                           Filter = true,
                                                           Value = string.Join(" ", _preferences.ContentLanguages)
                                                       });
                    }

            return new TRequest
                       {
                           StructuredSearch = structuredSearch,
                           MaxResults = searchDto.MaxResults,
                           FirstResult = searchDto.FirstResult
                       };
        }

        public IPerformContentSearchResponse PerformSearch<TRequest, TResponse>(TRequest request)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new()
        {
            return _searchManager.PerformContentSearch<TResponse>(request);
        }

        public PortalHeadlineListDataResult GetPortalHeadlineListDataResult<TRequest, TResponse>(string[] accessionNumbers, SortOrder sortOrder)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new()
        {
            var requestDto = new AccessionNumberSearchRequestDTO
                                 {
                                     SortBy = Mapper.Map<SortOrder, SortBy>(sortOrder),
                                     MetaDataController =
                                         {
                                             Mode = CodeNavigatorMode.None
                                         },
                                     DescriptorControl =
                                         {
                                             Mode = DescriptorControlMode.None,
                                             Language = "en",
                                         },
                                     AccessionNumbers = accessionNumbers
                                 };

            requestDto.MetaDataController.ReturnCollectionCounts = false;
            requestDto.MetaDataController.ReturnKeywordsSet = false;
            requestDto.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.None;

            // add all the search collections to the search.
            requestDto.SearchCollectionCollection.Clear();
            requestDto.SearchCollectionCollection.AddRange(Enum.GetValues(typeof (SearchCollection)).Cast<SearchCollection>());
            
            var response = _searchManager.PerformAccessionNumberSearch<TRequest, TResponse>(requestDto);
            return PortalHeadlineConversionManager.Convert(_headlineListConversionManager.Process(response, false, _headlineUtility.GenerateUrl), TruncationType.None);
        }

    }
}