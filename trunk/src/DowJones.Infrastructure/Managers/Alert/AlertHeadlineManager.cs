using System;
using System.Linq;
using DowJones.Ajax.HeadlineList;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Managers.Search;
using DowJones.Models.Search;
using DowJones.Search;
using DowJones.Search.Navigation;
using DowJones.Session;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using SortOrder = DowJones.Search.SortOrder;

namespace DowJones.Managers.Alert
{
    public interface IAlertSearchService : ISearchService
    {
    }

    /// <summary>
    /// AlertHeadlineManager
    /// </summary>
    // TODO: ****************************************************************
    // TODO: Process Markup, Snippet and date fitler based on user preference
    // TODO: ****************************************************************

    public class AlertHeadlineManager : IAlertSearchService
    {
        private readonly IControlData _controlData;
        private readonly SearchQueryBuilder _queryBuilder;

        public AlertHeadlineManager(IControlData controlData, SearchQueryBuilder queryBuilder)
        {
            _controlData = controlData;
            _queryBuilder = queryBuilder;
        }

        #region IAlertSearchService Members

        public SearchResponse PerformSearch(AbstractBaseSearchQuery request)
        {
            var alertSearchQuery = request as AlertSearchQuery;

            if (alertSearchQuery == null)
            {
                return null;
            }
            var response = new SearchResponse();

            if (alertSearchQuery.IsValid())
            {
                var baseSearchRequest = _queryBuilder.GetRequest<PerformContentSearchRequest>(request);
                GetFolderHeadlines2Request alertHeadlineRequest = BuildAlertHeadlineRequest(baseSearchRequest, alertSearchQuery);
                response = GetResponse(alertHeadlineRequest, alertSearchQuery);
            }
            return response;
        }

        #endregion

        private static GetFolderHeadlines2Request BuildAlertHeadlineRequest(PerformContentSearchRequest contentSearchRequest, AlertSearchQuery alertSearchQuery)
        {
            var headlinesRequest = new GetFolderHeadlines2Request {
                folderID = alertSearchQuery.AlertId, 
                responseFormat = FolderHeadlineResponseFormat.Search20, 
                viewType = MapViewType(alertSearchQuery.ViewType),
                bookMark = alertSearchQuery.Bookmark, 
                sessionMark = alertSearchQuery.Sessionmark, 
                bResetSessionMark = alertSearchQuery.ResetSessionmark, 
                searchQuery = contentSearchRequest
            };

            //Update content search
            SearchQueryPagination pagination = alertSearchQuery.Pagination;
            contentSearchRequest.FirstResult = (pagination.StartIndex.HasValue) ? pagination.StartIndex.Value : 0;
            contentSearchRequest.MaxResults = (pagination.MaxResults.HasValue) ? pagination.MaxResults.Value : 100;

            // SnippetType
            //contentSearchRequest.StructuredSearch.Formatting.SnippetType = SnippetType.Fixed; //TODO: Preference

            // SortOrder
            contentSearchRequest.StructuredSearch.Formatting.SortOrder = MapAlertSortOrder(alertSearchQuery.Sort);

            contentSearchRequest.StructuredSearch.Formatting.ClusterMode = ClusterMode.Off;

            // MarkupType
            contentSearchRequest.StructuredSearch.Formatting.MarkupType = MarkupType.Highlight; //TODO: Preference

            SearchString searchString;
            if (!String.IsNullOrEmpty(alertSearchQuery.Keywords))
            {
                searchString = new SearchString {Id = "FreeText", Type = SearchType.Free, Value = alertSearchQuery.Keywords, Mode = SearchMode.Simple};
                contentSearchRequest.StructuredSearch.Query.SearchStringCollection.Add(searchString);
            }

            contentSearchRequest.NavigationControl.ReturnHeadlineCoding = true;
            return headlinesRequest;
        }

        private static ResultSortOrder MapAlertSortOrder(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.PublicationDateOldestFirst:
                    return ResultSortOrder.PublicationDateChronological;
                case SortOrder.Relevance:
                    return ResultSortOrder.Relevance;
                case SortOrder.ArrivalTime:
                    return ResultSortOrder.ArrivalTime;
                default:
                    return ResultSortOrder.PublicationDateReverseChronological;
            }
        }

        public static HeadlinesViewType MapViewType(AlertHeadlineViewType type)
        {
            switch (type)
            {
                case AlertHeadlineViewType.All:
                    return HeadlinesViewType.ViewAll;
                case AlertHeadlineViewType.New:
                    return HeadlinesViewType.ViewNew;
                case AlertHeadlineViewType.Session:
                    return HeadlinesViewType.ViewSession;
                default:
                    return HeadlinesViewType.ViewAll;
            }
        }

        private SearchResponse GetResponse(GetFolderHeadlines2Request request, AbstractBaseSearchQuery searchQuery)
        {
            ServiceResponse serviceResponse = TrackService.GetFolderHeadlines2(ControlDataManager.Convert(_controlData), request);

            var r = serviceResponse.GetObject<GetFolderHeadlines2Response>();

            if (r.folderHeadlinesResponse == null ||
                r.folderHeadlinesResponse.folderHeadlinesResult == null ||
                r.folderHeadlinesResponse.folderHeadlinesResult.count == 0)
            {
                return null;
            }

            if (r.folderHeadlinesResponse.folderHeadlinesResult.folder[0].status !=  0)
            {
                throw new DowJonesUtilitiesException(r.folderHeadlinesResponse.folderHeadlinesResult.folder[0].status);
            }

            PerformContentSearchResponse d = r.folderHeadlinesResponse.folderHeadlinesResult.folder[0].PerformContentSearchResponse;

            //Always get highlighting string from folder info instead of result which many not be correct one.
            if (d.ContentSearchResult != null)
            {
                d.ContentSearchResult.CanonicalQueryString = r.folderHeadlinesResponse.folderHeadlinesResult.folder[0].highlightString;
            }
            
            var contentSearchResponse = new PerformContentSearchResponse
                                            {
                                                ContentSearchResult = d.ContentSearchResult
                                            };


            var histogram = Mapper.Map<Histogram>(contentSearchResponse.ContentSearchResult);

            var results = Mapper.Map<HeadlineListDataResult>(contentSearchResponse);
            var secondarySourceGroup = searchQuery.Filters.Source.Where(each=> each.SourceType == SourceFilterType.ProductDefineCode).FirstOrDefault();
            if (secondarySourceGroup != null)
            {
                searchQuery.SecondarySourceGroupId = secondarySourceGroup.SourceCode;
            }
            var navigators =
                Mapper.Map<ResultNavigator>(new ProductContentSearchResult(searchQuery.ProductId,
                                                                           searchQuery.PrimarySourceGroupId, searchQuery.SecondarySourceGroupId) {ContentSearchResult = contentSearchResponse.ContentSearchResult});

            ContentSearchResult searchResult = contentSearchResponse.ContentSearchResult;
          
            return new SearchResponse
                       {
                           ContextId = searchResult.SearchContext,
                           Navigators = navigators,
                           Response = contentSearchResponse,
                           Results = results,
                           Query =  searchQuery,
                           AlertInfo = r.folderHeadlinesResponse.folderHeadlinesResult.folder[0],
                           Histogram =  histogram
                           
                       };
        }
    }
}