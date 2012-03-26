using DowJones.Managers.Abstract;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Assets.Sharing.V2_0;
using log4net;

namespace DowJones.Managers.Topics
{
    public class TopicsManager : AbstractAggregationManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TopicsManager));
        //private readonly IPreferences preferences;
        //private readonly Product product;

        public TopicsManager(IControlData controlData)//, IPreferences preferences, Product product)
            : base(controlData)
        {
            //this.preferences = preferences;
            //this.product = product;
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

        public QueryPropertiesItemCollection GetSubscribableTopics(QueryType queryType, ShareScopeCollection queriesShareScopeCollection)
        {
            var getSubscribableQueriesRequest = new GetSubscribableQueriesRequest
            {
                QueryType = QueryType.CommunicatorTopicQuery,
                ShareScopeCollection = queriesShareScopeCollection
            };
            var response = Invoke<GetSubscribableQueriesResponse>(getSubscribableQueriesRequest).ObjectResponse;
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
    }
}
