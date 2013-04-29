using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        // Number of concurrent operations to create syndication items
        private readonly int MAX_PARALLEL_TASKS_TO_CREATE_SYNDICATION_ITEMS = Convert.ToInt32(ConfigurationManager.AppSettings["MAX_PARALLEL_TASKS_TO_CREATE_SYNDICATION_ITEMS"] ?? "-1");

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

            //LeiH: change to multi-tasking for performnace gain
            //var _cancellation = new CancellationTokenSource(); //Todo: in case abort is needed
            var po = new ParallelOptions
            {
                //CancellationToken = _cancellation.Token,
                MaxDegreeOfParallelism = MAX_PARALLEL_TASKS_TO_CREATE_SYNDICATION_ITEMS
            };
            var cdResponse = new ConcurrentDictionary<int, CreateSyndicationItemExResponse>();
            var count = lstRssItem.Count();
            Parallel.For(0, count, po, (i, loopState)=>
                                                    {
                                                        var rssItem = lstRssItem.ElementAt(i);
                                                     //if (_cancellation.IsCancellationRequested)
                                                     //    loopState.Stop();

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
                                                        var res = SyndicationAggregationService.CreateSyndicationItemEx(ControlData, request);
                                                     cdResponse.AddOrUpdate(i, res, (key, oldvalue) => res);
                                                 });
            if (!cdResponse.IsEmpty)
            {
                var lstResponse = new List<CreateSyndicationItemExResponse>(count);
                for (var i = 0; i < count; i++)
                {
                    lstResponse.Add(cdResponse[i]);
                }
                return lstResponse;
            }
            return null;
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
