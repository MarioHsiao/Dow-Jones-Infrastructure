// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyndicationNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.Converters.HeadlineList;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Syndication;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.PCM.Syndication.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using SortOrder = Factiva.Gateway.Messages.PCM.Syndication.V1_0.SortOrder;
using SyndicationIdCollection = Factiva.Gateway.Messages.PCM.Syndication.V1_0.SyndicationIdCollection;
using SyndicationItem = Factiva.Gateway.Messages.Assets.Item.V1_0.SyndicationItem;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "syndicationNewsPageModuleServiceResult", Namespace = "")]
    public class SyndicationNewsPageModuleServiceResult : Generic.AbstractServiceResult<SyndicationNewsPageServicePartResult<SyndicationPackage>, SyndicationPackage>, 
        IPopulate<NewsPageHeadlineModuleDataRequest>,
        IUpdateDefinitionAndPopulate<SyndicationNewsPageModuleUpdateRequest, NewsPageHeadlineModuleDataRequest>
    {
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration &&
                                                   Settings.Default.CacheSyndicationNewsPageModuleService;

        public void Populate(ControlData controlData, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            UpdateDefinitionAndPopulate(controlData, null, request, preferences);
        }

        public void UpdateDefinitionAndPopulate(ControlData controlData, SyndicationNewsPageModuleUpdateRequest updateRequest, NewsPageHeadlineModuleDataRequest getRequest, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        if (updateRequest != null && updateRequest.IsValid())
                        {
                            var updateResult = new SyndicationNewsPageModuleUpdateServiceResult();
                            updateResult.Update(controlData, updateRequest, preferences);
                            if (updateResult.ReturnCode != 0)
                            {
                                ReturnCode = updateResult.ReturnCode;
                                ElapsedTime = updateResult.ElapsedTime;
                                StatusMessage = updateResult.StatusMessage;
                                return;
                            }
                        }

                        if (getRequest == null || !getRequest.IsValid())
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                        }

                        if (preferences == null)
                        {
                            preferences = GetPreferences(controlData);
                        }

                        hasCacheBeenEnabled = hasCacheBeenEnabled && getRequest.CacheState != CacheState.Off;
                        GetModuleData(getRequest, preferences, controlData);
                    },
                    preferences);
        }

        protected internal void GetModuleData(NewsPageHeadlineModuleDataRequest request, IPreferences preferences, ControlData controlData)
        {
            var module = GetModule(request, controlData, preferences);

            MaxPartsAvailable = module.SyndicationFeedIDCollection.Count;
            PartResults = GetFeeds(GetPage(module.SyndicationFeedIDCollection, request), request, preferences, controlData);
        }

        protected internal SyndicationNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<SyndicationNewspageModule>(request, controlData, preferences);
            if (module.SyndicationFeedIDCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptySyndicationFeedIDCollection);
            }
            return module;
        }

        protected internal List<SyndicationNewsPageServicePartResult<SyndicationPackage>> GetFeeds(IEnumerable<string> feedIds, NewsPageHeadlineModuleDataRequest request, IPreferences preferences, ControlData controlData)
        {
            if (feedIds == null)
            {
                return null;
            }

            if (feedIds.Count() == 1)
            {
                return new List<SyndicationNewsPageServicePartResult<SyndicationPackage>> { ProcessFeed(request.FirstPartToReturn.ToString(), feedIds.First(), request, preferences) };
            }

            var index = request.FirstPartToReturn - 1;
            var tasks = (from uri in feedIds
                         let identifier = Interlocked.Increment(ref index).ToString()
                         select TaskFactory.StartNew(
                            () => ProcessFeed(identifier, uri, request, preferences), TaskCreationOptions.None))
                            .ToList();

            Task.WaitAll(tasks.ToArray());
            var orderedTasks = tasks.OrderBy(task => Int32.Parse(task.Result.Identifier));
            return orderedTasks.Select(task => task.Result).ToList();
        }

        protected internal SyndicationNewsPageServicePartResult<SyndicationPackage> ProcessFeed(string id, string feed, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            var temp = ProcessFeed(feed, request, preferences);
            temp.Identifier = id;
            return temp;
        }

        protected internal SyndicationNewsPageServicePartResult<SyndicationPackage> ProcessFeed(string syndicationFeedId, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            var partResult = new SyndicationNewsPageServicePartResult<SyndicationPackage>();
            ProcessServicePartResult<SyndicationPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    var datetimeFormatter = new DateTimeFormatter(preferences);
                    var syndicationPackage = new SyndicationPackage
                    {
                        FeedId = syndicationFeedId,
                    };

                    //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(controlData, Settings.Default.DataAccessProxyUser);
                    var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

                    if (hasCacheBeenEnabled)
                    {
                        var generator = new SyndicationGetItemCacheKeyGenerator(request.ModuleId, syndicationFeedId)
                        {
                            CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                        };
                        lightweightProxy = generator.GetCacheControlData(lightweightProxy);
                    }

                    var dataRetrievalManager = new ModuleDataRetrievalManager(lightweightProxy, preferences);
                    long itemId;
                    if (Int64.TryParse(syndicationFeedId, out itemId))
                    {
                        var itemRequest = new GetItemByIDRequest
                                              {
                                                  Id = itemId,
                                              };

                        GetItemByIDResponse itemResponse = null;
                       
                        try
                        {
                            var proxy = lightweightProxy;
                            RecordTransaction(
                                "Asset.GetSyndicationHeadlines",
                                syndicationPackage.FeedId,
                                manager =>
                                    {
                                        itemResponse = manager.Invoke<GetItemByIDResponse>(itemRequest, proxy).ObjectResponse;
                                    },
                                dataRetrievalManager);

                            if (itemResponse != null)
                            {
                                var syndicationItem = itemResponse.Item as SyndicationItem;
                                if (syndicationItem != null)
                                {
                                    syndicationPackage.HtmlPageForFeedUri = syndicationItem.Properties.Value;
                                    syndicationPackage.FaviconUri = new FaviconUrlBuilder(FaviconUrlBuilder.IconSize.Small, syndicationItem.Properties.Value).ToString();
                                }
                            }
                        }
                        catch (Exception)
                        {
                            syndicationPackage.FaviconUri = string.Empty;
                            syndicationPackage.HtmlPageForFeedUri = string.Empty;
                        }
                    }
                    
                    var getSyndicationHeadlinesRequest = GetSyndicationRequest(syndicationFeedId, request);
                    lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

                    if (hasCacheBeenEnabled &&
                        request.FirstResultToReturn == 0)
                    {
                        var generator = new SyndicationCacheKeyGetHeadlinesGenerator(request.ModuleId, syndicationFeedId, request.MaxResultsToReturn)
                        {
                            CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                        };
                        lightweightProxy = generator.GetCacheControlData(lightweightProxy);
                    }

                    GetSyndicationHeadlinesResponse getSyndicationHeadlinesResponse = null;
                    RecordTransaction(
                        "SyndicationService.GetSyndicationHeadlines",
                        syndicationPackage.FeedId,
                        manager =>
                            {
                                getSyndicationHeadlinesResponse = manager.Invoke<GetSyndicationHeadlinesResponse>(getSyndicationHeadlinesRequest, lightweightProxy).ObjectResponse;
                            },
                        dataRetrievalManager);

                    if (getSyndicationHeadlinesResponse != null)
                    {
                        IListDataResultConverter converter = new SyndicationHeadlineResponseConverter(getSyndicationHeadlinesResponse, datetimeFormatter,getSyndicationHeadlinesRequest);
                        syndicationPackage.FeedTitle = SyndicationHeadlineResponseConverter.StripHTML(getSyndicationHeadlinesResponse.SyndicationItemCollection.First().Channel.Title);
                        syndicationPackage.FeedType = FeedType.RSS;
                        syndicationPackage.Result = (PortalHeadlineListDataResult)converter.Process(); ////.Convert(request.TruncationType);
                    }

                    partResult.Package = syndicationPackage;
                },
                preferences);
            return partResult;
        }

        private static GetSyndicationHeadlinesRequest GetSyndicationRequest(string syndicationFeedId, NewsPageHeadlineModuleDataRequest request)
        {
            return new GetSyndicationHeadlinesRequest
            {
                FirstResultToReturn = request.FirstResultToReturn,
                MaxResultsToReturn = request.MaxResultsToReturn,
                SortBy = SortColumn.PublicationDate,
                SortOrder = SortOrder.Descending,
                FeedFormat = FeedFormatType.FactivaHeadlines,
                SyndicationIdCollection = new SyndicationIdCollection { syndicationFeedId },
            };
        }
    }
}
