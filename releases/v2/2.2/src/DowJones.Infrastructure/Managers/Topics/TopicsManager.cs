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

        public TopicsManager(IControlData controlData, Product product)//, IPreferences preferences, Product product)
            : base(controlData)
        {
            //this.preferences = preferences;
            this.product = product;
        }

        protected override ILog Log { get { return Logger; } }

        public QueryPropertiesItemCollection GetCommunicatorSubscribableTopics()
        {
            return GetSubscribableTopics(QueryType.CommunicatorTopicQuery, new ShareScopeCollection{ShareScope.Account});
        }

        public QueryPropertiesItemCollection GetCommunicatorUserTopics()
        {
            return GetUserTopics(new QueryTypeCollection{QueryType.CommunicatorTopicQuery});
        }

        public QueryPropertiesItemCollection GetSubscribableTopics(QueryType queryType, ShareScopeCollection queriesShareScopeCollection, bool forceCacheRefresh = false)
        {
            var tempControlData = ControlData;

            if (SubscribableTopicsCacheKey.CachingEnabled)
            {
                //var cacheKey = new SubscribableTopicsCacheKey(product, queriesShareScopeCollection)
                var cacheKey = new SubscribableTopicsCacheKey(product)
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
