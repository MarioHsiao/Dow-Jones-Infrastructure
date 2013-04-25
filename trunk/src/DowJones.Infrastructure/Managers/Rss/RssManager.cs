using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Properties;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using Factiva.Gateway.Messages.PCM.Syndication.V1_0;
using Factiva.Gateway.Services.V1_0;
using CategoryCollection = Factiva.Gateway.Messages.Assets.Item.V1_0.CategoryCollection;

namespace DowJones.Managers.Rss
{
    public class RssManager
    {
        [Inject("Injecting ControlData")]
        private Factiva.Gateway.Utils.V1_0.ControlData ControlData { get; set; }

        // If we ask for more, the service will throw error
        private readonly int MAX_ITEMS_TO_GET = Convert.ToInt32(ConfigurationManager.AppSettings["RSS_FEEDS_MAX_LIMIT"] ?? "240");

        public CreateSyndicationItemExResponse CreateSyndicationItem(RssItem rssItem)
        {
            if (rssItem == null) return null;
            CategoryCollection categoryCollection = null;
            if(!string.IsNullOrEmpty(rssItem.Category))
            {
                categoryCollection = new CategoryCollection {rssItem.Category};
            }
            var itemEx = new SyndicationItemEx()
            {
                Properties = new SyndicationItemProperties()
                {
                    SyndicationItemType = SyndicationItemType.RSS,
                    Name = rssItem.Name,
                    Value = rssItem.Url,
                    Aggregation = GetAggregation(rssItem.AggregationDays),
                    CategoryCollection = categoryCollection
                },
                ShareProperties = new ShareProperties
                {
                    AccessControlScope = AccessControlScope.Personal,
                    AssignedScope = ShareScope.Personal,
                    ListingScope = ShareScope.Personal,
                    SharePromotion = ShareScope.Personal
                }
            };

            var request = new CreateSyndicationItemExRequest()
            {
                SyndicationItemEx = itemEx
            };

            return SyndicationAggregationService.CreateSyndicationItemEx(ControlData, request);
        }

        public List<CreateSyndicationItemExResponse> CreateSyndicationItems(IEnumerable<RssItem> lstRssItem)
        {
            if (lstRssItem == null || lstRssItem.Count() == 0) return null;

            var lstResponse = new List<CreateSyndicationItemExResponse>();
            foreach (var rssItem in lstRssItem)
            {

                //TODO: Add logic to fork multiple tasks

                CategoryCollection categoryCollection = null;
                if (!string.IsNullOrEmpty(rssItem.Category))
                {
                    categoryCollection = new CategoryCollection {rssItem.Category};
                }
                var itemEx = new SyndicationItemEx()
                                 {
                                     Properties = new SyndicationItemProperties()
                                                      {
                                                          SyndicationItemType = SyndicationItemType.RSS,
                                                          Name = rssItem.Name,
                                                          Value = rssItem.Url,
                                                          Aggregation = GetAggregation(rssItem.AggregationDays),
                                                          CategoryCollection = categoryCollection
                                                      },
                                     ShareProperties = new ShareProperties
                                                           {
                                                               AccessControlScope = AccessControlScope.Personal,
                                                               AssignedScope = ShareScope.Personal,
                                                               ListingScope = ShareScope.Personal,
                                                               SharePromotion = ShareScope.Personal
                                                           }
                                 };

                var request = new CreateSyndicationItemExRequest()
                                  {
                                      SyndicationItemEx = itemEx
                                  };

                lstResponse.Add(SyndicationAggregationService.CreateSyndicationItemEx(ControlData, request));
            }
            return lstResponse;
        }

        public GetSyndicationItemExListResponse GetSyndicationItemList()
        {
            var getRequest = new GetSyndicationItemExListRequest {MaxResultsToReturn = Settings.Default.RSS_FEEDS_MAX_LIMIT};

            return SyndicationAggregationService.GetSyndicationItemExList(ControlData, getRequest);
        }

        private Aggregation GetAggregation(ushort aggregationPeriodinDays)
        {
            Aggregation aggregation = null;
            if (aggregationPeriodinDays > 0)
            {
                aggregation = new Aggregation()
                {
                    Type = AggregationType.Day,
                    Value = aggregationPeriodinDays.ToString()
                };
            }

            return aggregation;
        }
    }
}
