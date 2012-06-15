using DowJones.Caching;
using DowJones.Infrastructure.Common;
using DowJones.Managers.Abstract;
using DowJones.Managers.Topics.Caching;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Assets.Sharing.V2_0;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using GWCacheScope = Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope;
using log4net;

namespace DowJones.Managers.Topics
{
    public class TopicsManager : AbstractAggregationManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TopicsManager));
        //private readonly IPreferences preferences;
        private readonly Product product;

        public TopicsManager(IControlData controlData, Product product = null)//, IPreferences preferences, Product product)
            : base(controlData)
        {
            //this.preferences = preferences;
            this.product = product;
        }

        protected override ILog Log { get { return Logger; } }

        public QueryPropertiesItemCollection GetCommunicatorSubscribableTopics(bool forceCacheRefresh = false)
        {
            return GetSubscribableTopics(QueryType.CommunicatorTopicQuery, new ShareScopeCollection{ShareScope.Account}, forceCacheRefresh);
        }

        public GetSubscribableQueriesResponse GetCommunicatorSubscribableTopicsWithPaging(
            int firstResultToReturn = 1,
            int maxResultsToReturn = 100,
            QuerySortBy sortBy = QuerySortBy.Name,
            SortOrder sortOrder = SortOrder.Ascending,
            FilterCollection filters = null)
        {
            return GetSubscribableTopicsWithPaging(QueryType.CommunicatorTopicQuery, new ShareScopeCollection { ShareScope.Account },
                firstResultToReturn,
                maxResultsToReturn,
                sortBy,
                sortOrder,
                filters);
        }

        public QueryPropertiesItemCollection GetCommunicatorUserTopics()
        {
            return GetUserTopics(new QueryTypeCollection{QueryType.CommunicatorTopicQuery});
        }

        public GetQueriesPropertiesListResponse GetCommunicatorUserTopicsWithPaging(
            int firstResultToReturn = 1,
            int maxResultsToReturn = 100,
            QuerySortBy sortBy = QuerySortBy.Name,
            SortOrder sortOrder = SortOrder.Ascending,
            FilterCollection filters = null)
        {
            return GetUserTopicsWithPaging(new QueryTypeCollection { QueryType.CommunicatorTopicQuery },
                firstResultToReturn,
                maxResultsToReturn,
                sortBy,
                sortOrder,
                filters);
        }

        public QueryPropertiesItemCollection GetSubscribableTopics(QueryType queryType, ShareScopeCollection queriesShareScopeCollection, bool forceCacheRefresh = false)
        {
            var tempControlData = ControlData;

            if (SubscribableTopicsCacheKey.CachingEnabled)
            {
                //var cacheKey = new SubscribableTopicsCacheKey(product)
                var cacheKey = new SubscribableTopicsCacheKey(product, queriesShareScopeCollection)
                {
                    CacheForceCacheRefresh = forceCacheRefresh
                };
                tempControlData = cacheKey.GetCacheControlData(ControlData, product);
            }

            var getSubscribableQueriesRequest = new GetSubscribableQueriesRequest
            {
                QueryType = queryType,
                ShareScopeCollection = queriesShareScopeCollection
            };
            var response = Invoke<GetSubscribableQueriesResponse>(getSubscribableQueriesRequest, tempControlData).ObjectResponse;
            return response.QueryPropertiesItemCollection;
        }

        public GetSubscribableQueriesResponse GetSubscribableTopicsWithPaging(
            QueryType queryType,
            ShareScopeCollection queriesShareScopeCollection,
            int firstResultToReturn = 1,
            int maxResultsToReturn = 100,
            QuerySortBy sortBy = QuerySortBy.Name,
            SortOrder sortOrder = SortOrder.Ascending,
            FilterCollection filters = null)
        {
            var getSubscribableQueriesRequest = new GetSubscribableQueriesRequest
            {
                QueryType = queryType,
                ShareScopeCollection = queriesShareScopeCollection,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Filters = filters,
                Paging = new Paging
                {
                    StartIndex = firstResultToReturn,
                    MaxResultsToReturn = maxResultsToReturn
                }
            };
            var response = Invoke<GetSubscribableQueriesResponse>(getSubscribableQueriesRequest).ObjectResponse;
            //return response.QueryPropertiesItemCollection;
            return response;
        }

        public QueryPropertiesItemCollection GetUserTopics(
            QueryTypeCollection queryTypeCollection,
            int maxResultsToReturn = 100,
            QuerySortBy sortBy = QuerySortBy.Name,
            SortOrder sortOrder = SortOrder.Ascending,
            FilterCollection filters = null)
        {
            var request = new GetQueriesPropertiesListRequest
            {
                MaxResultsToReturn = maxResultsToReturn,
                QueryTypes = queryTypeCollection,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Filters = filters,
                //Paging = new Paging
                //             {
                //                 StartIndex = firstResultToReturn,
                //                 MaxResultsToReturn = maxResultsToReturn
                //             }
            };

            var response = Invoke<GetQueriesPropertiesListResponse>(request).ObjectResponse;
            return response.QueryPropertiesItems;
        }

        public GetQueriesPropertiesListResponse GetUserTopicsWithPaging(
            QueryTypeCollection queryTypeCollection,
            int firstResultToReturn = 1,
            int maxResultsToReturn = 100,
            QuerySortBy sortBy = QuerySortBy.Name,
            SortOrder sortOrder = SortOrder.Ascending,
            FilterCollection filters = null)
        {
            var request = new GetQueriesPropertiesListRequest
            {
                //MaxResultsToReturn = maxResultsToReturn,  //That's the result from the 1st hit, not for the paging
                QueryTypes = queryTypeCollection,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Filters = filters,
                Paging = new Paging
                             {
                                 StartIndex = firstResultToReturn,
                                 MaxResultsToReturn = maxResultsToReturn
                             }
            };

            var response = Invoke<GetQueriesPropertiesListResponse>(request).ObjectResponse;
            //return response.QueryPropertiesItems;
            return response;
        }

        public void DeleteItemFromSessionCache(ICacheKey cacheKey)
        {
            if (cacheKey == null)
                return;

            var request = new DeleteItemRequest
            {
                Key = cacheKey.Serialize(),
                Scope = Mapper.Map<GWCacheScope>(cacheKey.CacheScope)
            };
            Process<DeleteItemResponse>(request);
        }
    }
}
