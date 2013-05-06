using System;
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
using log4net;
using CategoryCollection = Factiva.Gateway.Messages.Assets.Item.V1_0.CategoryCollection;

namespace DowJones.Managers.Rss
{
    public class RssManager
    {
        [Inject("Injecting ControlData")]
        private Factiva.Gateway.Utils.V1_0.ControlData ControlData { get; set; }

        [InjectAttribute("Logger")]
        private ILog Logger { get; set; }

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

        public void CreateSyndicationItems(List<RssItem> lstRssItem)
        {
            if (lstRssItem == null || lstRssItem.Count() == 0) return;

            //LeiH: change to multi-tasking for performnace gain
            //var _cancellation = new CancellationTokenSource(); //Todo: in case abort is needed
            var po = new ParallelOptions
            {
                //CancellationToken = _cancellation.Token,
                MaxDegreeOfParallelism = Settings.Default.MAX_PARALLEL_TASKS_TO_CREATE_SYNDICATION_ITEMS
            };

            Parallel.ForEach(lstRssItem, po, (rssItem, loopState)=>
                                                {
                                                     //if (_cancellation.IsCancellationRequested)
                                                     //    loopState.Stop();
                                                    var t = Thread.CurrentThread;
                                                    Logger.Debug(
                                                        String.Format(
                                                            "Running Task..., Thread ID={0}, AptState ={1}, IsPool={2}",
                                                            t.ManagedThreadId,
                                                            t.GetApartmentState(),
                                                            t.IsThreadPoolThread)
                                                    );
                                                    
                                                    var res = CreateSyndicationItem(rssItem);

                                                    //update the rssItem with response code 
                                                    if (res != null)
                                                    {
                                                        rssItem.FeedId = res.ItemId;
                                                        rssItem.SyndicationId = res.SyndicationId;
                                                        rssItem.ErrorCode = res.Rc;
                                                    }
                                                });
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
