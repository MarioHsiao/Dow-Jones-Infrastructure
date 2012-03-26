using System;
using DowJones.Ajax.HeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.DependencyInjection;
using DowJones.Managers.Search;
using DowJones.Web.Showcase.Models;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Web.Showcase.Extensions
{
    public static class HeadlineListConversionManagerExtensions
    {
        private static SearchManager SearchManager
        {
            get { return ServiceLocator.Resolve<SearchManager>(); }
        }

        public static HeadlineListModel PerformSearch(this HeadlineListConversionManager manager, string searchText, ContentSearchMode? searchMode)
        {
            HeadlineListDataResult result;

            switch (searchMode.GetValueOrDefault())
            {
                case ContentSearchMode.ContentServer:
                    result = manager.PerformContentSearch(searchText);
                    break;
                default:
                    result = manager.PerformTwitterSearch(searchText);
                    break;
            }

            return new HeadlineListModel { Query = searchText, Result = result };
        }

        public static HeadlineListDataResult PerformTwitterSearch(this HeadlineListConversionManager manager, string searchText)
        {
            var result = manager.Process("http://search.twitter.com/search.atom?q=" + searchText);
            return result;
        }

        public static HeadlineListDataResult PerformContentSearch(this HeadlineListConversionManager manager, string searchText)
        {
            var searchRequest = GetPerformContentSearchRequest(searchText);
            var searchResponse = GetPerformContentSearchResponse(searchRequest);
                        
            var result = manager.Process(searchResponse);

            return result;
        }
        public static HeadlineListDataResult PerformContentSearch(this HeadlineListConversionManager manager, string searchText, out IPerformContentSearchResponse response)
        {
            var searchRequest = GetPerformContentSearchRequest(searchText);
            var searchResponse = GetPerformContentSearchResponse(searchRequest);

            var result = manager.Process(searchResponse);
            response = searchResponse;

            return result;
        }
        public static HeadlineListDataResult PerformContentSearch(this HeadlineListConversionManager manager, string searchText, DowJones.Session.IControlData controlData, out ContentSearchResult contentSearchResult)
        {
            var searchRequest = GetPerformContentSearchRequest(searchText);
            var searchResponse = GetPerformContentSearchResponse(searchRequest, controlData);

            var result = manager.Process(searchResponse);
            contentSearchResult = searchResponse.ContentSearchResult;
            return result;
        }

        public static HeadlineListDataResult PerformContentSearch(this HeadlineListConversionManager manager, string searchText, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo)
        {
            var searchRequest = GetPerformContentSearchRequest(searchText);
            var searchResponse = GetPerformContentSearchResponse(searchRequest);

            var result = manager.Process(searchResponse, null, generateSnippetThumbnailForHeadlineInfo);

            return result;
        }

        public static HeadlineListDataResult GetHeadlineListDataResult(this HeadlineListConversionManager manager, IPerformContentSearchResponse searchResponse)
        {
            var result = manager.Process(searchResponse);
            return result;
        }


        private static PerformContentSearchRequest GetPerformContentSearchRequest(string searchText)
        {
            var request = new PerformContentSearchRequest();

            var searchstring = new SearchString
            {
                Value = searchText,
                Type = SearchType.Free,
                Mode = SearchMode.Simple,
                Combine = true,
                Filter = false,
                Scope = string.Empty,
                Validate = true
            };

            request.StructuredSearch.Query.SearchStringCollection.Add(searchstring);
            request.DescriptorControl.Mode = DescriptorControlMode.All;
            request.FirstResult = 0;
            request.MaxResults = 40;
            request.NavigationControl.KeywordControl.ReturnKeywords = true;
            request.NavigationControl.TimeNavigatorMode = TimeNavigatorMode.PublicationMonth;
            //request.NavigationControl.ContextualNavigatorControlCollection.AddRange(new[]{ CodeN
            request.NavigationControl.CodeNavigatorControl.Mode = CodeNavigatorMode.All;
            request.NavigationControl.CodeNavigatorControl.MaxBuckets = 5;
            //request.NavigationControl.CodeNavigatorControl.Mode = CodeNavigatorMode.All;
            //request.StructuredSearch.Query.Dates
            
            request.StructuredSearch.Query.SearchCollectionCollection.AddRange(new[] { SearchCollection.Publications, SearchCollection.Summary, SearchCollection.Boards, SearchCollection.CustomerDoc, });
            request.StructuredSearch.Formatting.DeduplicationMode = DeduplicationMode.Similar;
            request.StructuredSearch.Formatting.ClusterMode = ClusterMode.On;
            request.StructuredSearch.Formatting.FreshnessDate = DateTime.Now;
            request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.RelevanceHighFreshness;

            return request;
        }

        private static IPerformContentSearchResponse GetPerformContentSearchResponse(PerformContentSearchRequest performContentSearchRequest)
        {
            if (performContentSearchRequest == null)
                return null;

            var response = SearchManager.PerformContentSearch<PerformContentSearchResponse>(performContentSearchRequest);
            
            return response;
        }

        private static IPerformContentSearchResponse GetPerformContentSearchResponse(PerformContentSearchRequest performContentSearchRequest, DowJones.Session.IControlData sessionCd)
        {
            if (performContentSearchRequest == null)
                return null;

            var response = SearchManager.PerformContentSearch<PerformContentSearchResponse>(performContentSearchRequest);
            
            return response;
        }
    }
}