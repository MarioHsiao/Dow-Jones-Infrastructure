// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SummaryNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Thunderball.Library.Charting;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Ajax.TagCloud;
using DowJones.Utilities.Ajax.TagCloud.Converters;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Summary;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Charting;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.PCM.SummaryRegionalMap.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using EntityType = DowJones.Tools.Ajax.HeadlineList.EntityType;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreePerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using NewsEntity = DowJones.Web.Mvc.UI.Models.Common.NewsEntity;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;
using SummaryRegionalMapPackage = DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.SummaryRegionalMapPackage;
using Tag = DowJones.Web.Mvc.Models.News.Tag;
using TagCollection = DowJones.Web.Mvc.Models.News.TagCollection;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "summaryNewsPageModuleServiceResult", Namespace = "")]
    public class SummaryNewsPageModuleServiceResult : Generic.AbstractServiceResult<SummaryNewsPageServicePartResult<AbstractSummaryPackage>, AbstractSummaryPackage>, IPopulate<SummaryNewsPageModuleDataRequest>
    {
        private const int PartResultTimeout = 2500;
        private const string TailQueryPart = " and (md from -1 to +0 and date after -2)";
        private const float DefaultMinWeightKeywords = 0.0f;
        private DateTimeFormatter dateTimeFormatter;
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration && Settings.Default.CacheSummaryNewsPageModuleService;

        [DataMember(Name = "hasMarketDataIndex")]
        public bool HasMarketDataIndex { get; set; }

        public static IPerformContentSearchRequest CreateSearchRequest<T>(SummaryNewspageModule module, int firstResult, int maxResults, IPreferences preferences)
            where T : IPerformContentSearchRequest, new()
        {
            return CreateTrendingSearchRequest<T>(GetQueryEntity(module.QueryEntityCollection, QueryEntityType.SummaryTrending), 0, firstResult, maxResults, false, preferences);
        }

        public static IPerformContentSearchRequest CreateSearchRequest<T>(SummaryNewspageModule module, string regionFcode, int firstResult, int maxResults, IPreferences preferences)
            where T : IPerformContentSearchRequest, new()
        {
            return (from queryEntity in module.RegionalMapQueryEntityCollection where queryEntity.RegionFcode == regionFcode select CreateSearchRequest<T>(queryEntity.QueryCollection, firstResult, maxResults)).FirstOrDefault();
        }

        #region Implementation of IPopulate<in SummaryNewsPageModuleDataRequest>

        public void Populate(ControlData controlData, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            Populate(controlData, request, preferences, Settings.Default.UsePcm);
        }

        #endregion

        // This one will be called by PCM, so they can set usePcm to false
        public void Populate(ControlData controlData, SummaryNewsPageModuleDataRequest request, IPreferences preferences, bool usePcm)
        {
            Process(controlData, request, preferences, usePcm);
        }

        protected internal static QueryEntity GetQueryEntity(QueryEntityCollection collection, QueryEntityType type)
        {
            var temp = collection.Where(queryEntity => queryEntity.QueryEntityType == type);
            return temp.Count() > 0 ? temp.First() : null;
        }

        protected internal void Process(ControlData controlData, SummaryNewsPageModuleDataRequest request, IPreferences preferences, bool usePcm)
        {
            ProcessServiceResult(
              MethodBase.GetCurrentMethod(),
              () =>
              {
                  if (request == null || !request.IsValid())
                  {
                      throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                  }

                  if (preferences == null)
                  {
                      preferences = GetPreferences(controlData);
                  }

                  hasCacheBeenEnabled = hasCacheBeenEnabled && request.CacheState != CacheState.Off;
                  dateTimeFormatter = new DateTimeFormatter(preferences);
                  GetModuleData(request, controlData, preferences, usePcm);
              },
              preferences);
        }

        protected internal void GetModuleData(SummaryNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences, bool usePcm)
        {
            var module = GetModule<SummaryNewspageModule>(request, controlData, preferences);
            if (module.QueryEntityCollection == null || module.QueryEntityCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);
            }

            HasMarketDataIndex = module.MarketIndex.IsNotEmpty();
            PartResults = GetParts(module, request, controlData, preferences, usePcm);
            MaxPartsAvailable = 5;
        }

        protected internal List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>> GetParts(SummaryNewspageModule module, SummaryNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences, bool callPcm)
        {
            var uniqueParts = request.Parts.GetUniques();

            // if only one part keep it on the same thread););
            if (uniqueParts.Count == 1)
            {
                switch (uniqueParts.First())
                {
                    case SummaryParts.Chart:
                        return new List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>>
                                   {
                                       GetSummaryChartPackage(module.MarketIndex, request, preferences)
                                   };
                    case SummaryParts.RecentArticles:
                        return new List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>>
                                   {
                                       GetSummaryRecentArticlesPackage(GetQueryEntity(module.QueryEntityCollection, QueryEntityType.SummaryRecentNews), request, controlData, preferences)
                                   };
                    case SummaryParts.RecentVideos:
                        return new List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>>
                                   {
                                       GetSummaryVideosPackage(GetQueryEntity(module.QueryEntityCollection, QueryEntityType.SummaryVideo), request, controlData, preferences)
                                   };
                    case SummaryParts.RegionalMap:
                        if (callPcm)
                        {
                            return new List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>>
                                       {
                                           GetSummaryRegionalMapPackageWithPcm(module.RegionalMapQueryEntityCollection, request, preferences)
                                       };
                        }

                        return new List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>>
                                   {
                                       GetSummaryRegionalMapPackage(module.RegionalMapQueryEntityCollection, request, preferences)
                                   };
                    case SummaryParts.Trending:
                        return new List<SummaryNewsPageServicePartResult<AbstractSummaryPackage>>
                                   {
                                       GetSummaryTrendingPackage(GetQueryEntity(module.QueryEntityCollection, QueryEntityType.SummaryTrending), request, preferences)
                                   };
                }
            }
            else
            {
                var tasks = new List<Task<SummaryNewsPageServicePartResult<AbstractSummaryPackage>>>();
                foreach (var part in uniqueParts)
                {
                    switch (part)
                    {
                        case SummaryParts.Chart:
                            tasks.Add(TaskFactory.StartNew(() => GetSummaryChartPackage(module.MarketIndex, request, preferences)));
                            break;
                        case SummaryParts.RecentArticles:
                            tasks.Add(TaskFactory.StartNew(() => GetSummaryRecentArticlesPackage(GetQueryEntity(module.QueryEntityCollection, QueryEntityType.SummaryRecentNews), request, controlData, preferences)));
                            break;
                        case SummaryParts.RecentVideos:
                            tasks.Add(TaskFactory.StartNew(() => GetSummaryVideosPackage(GetQueryEntity(module.QueryEntityCollection, QueryEntityType.SummaryVideo), request, controlData, preferences)));
                            break;
                        case SummaryParts.RegionalMap:
                            tasks.Add(callPcm ?
                                TaskFactory.StartNew(() => GetSummaryRegionalMapPackageWithPcm(module.RegionalMapQueryEntityCollection, request, preferences)) : 
                                TaskFactory.StartNew(() => GetSummaryRegionalMapPackage(module.RegionalMapQueryEntityCollection, request, preferences)));
                            break;
                        case SummaryParts.Trending:
                            tasks.Add(TaskFactory.StartNew(() => GetSummaryTrendingPackage(GetQueryEntity(module.QueryEntityCollection, QueryEntityType.SummaryTrending), request, preferences)));
                            break;
                    }
                }

                Task.WaitAll(tasks.ToArray(), PartResultTimeout);
                return tasks.Select(task => task.Result).ToList();
            }

            return null;
        }

        protected internal SummaryNewsPageServicePartResult<AbstractSummaryPackage> GetSummaryRecentArticlesPackage(QueryEntity recentHeadlinesQueryEntity, SummaryNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
            var tempPackage = new SummaryRecentArticlesPackage();
            partResult.Package = tempPackage;
            ProcessServicePartResult<SummaryRecentArticlesPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        var searchRequest = ModuleSearchUtility.GetSearchRequest<FreePerformContentSearchRequest>(
                            recentHeadlinesQueryEntity.QueryCollection,
                            request.FirstResultToReturn,
                            request.MaxResultsToReturn,
                            ModuleSearchUtility.MapSortOrder(recentHeadlinesQueryEntity.ResultSortOrder),
                            GetSearchCollection(SummaryParts.RecentArticles),
                            preferences,
                            ModuleSearchUtility.MapDuplicationType(recentHeadlinesQueryEntity.DeduplicationType));

                        searchRequest.StructuredSearch.Query.SearchStringCollection.First().Value += TailQueryPart;

                        // Note: Commented this out due to possibility it should not cache recent headlines.
                        /* if (hasCacheBeenEnabled &&
                            request.FirstResultToReturn == 0)
                        {
                            var generator = new SummaryRecentHeadlinesCacheKeyGenerator(request.ModuleId, request.MaxResultsToReturn, preferences.ContentLanguages.ToArray());
                            controlData = generator.GetCacheControlData(controlData);
                        }*/

                        var response = GetPerformContentSearchResponse(searchRequest, controlData, preferences);
                        var conversionManager = new HeadlineListConversionManager(dateTimeFormatter);
                        tempPackage.Result = conversionManager.Process(response).Convert(TruncationType.Small);
                    },
                    preferences);

            return partResult;
        }

        protected internal SummaryNewsPageServicePartResult<AbstractSummaryPackage> GetSummaryChartPackage(string marketIndex, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var partResult = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
            var tempPackage = new SummaryChartPackage();
            partResult.Package = tempPackage;
            ProcessServicePartResult<SummaryChartPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        Dictionary<string, ChartDataResponse> response = null;
                        RecordTransaction(
                            //typeof(Dictionary<string, ChartDataResponse>).FullName,
                            "StockDataService.GetChart",
                            MethodBase.GetCurrentMethod().Name,
                            () =>
                                {
                                    response = StockDataService.GetChart(new[] { marketIndex });
                                });
                        
                        if (response != null && response.Count > 0)
                        {
                            tempPackage.MarketIndexIntradayResult = new MarketIndexIntradayResultAssembler(new DateTimeFormatter(preferences)).Convert(response["temp"]);
                        }
                    },
                    preferences);

            return partResult;
        }

        protected internal SummaryNewsPageServicePartResult<AbstractSummaryPackage> GetSummaryVideosPackage(QueryEntity videosQueryEntity, SummaryNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
            var tempPackage = new SummaryVideosPackage();
            partResult.Package = tempPackage;
            ProcessServicePartResult<SummaryVideosPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    var searchRequest = ModuleSearchUtility.GetSearchRequest<FreePerformContentSearchRequest>(
                           videosQueryEntity.QueryCollection,
                           request.FirstResultToReturn,
                           request.MaxResultsToReturn,
                           ModuleSearchUtility.MapSortOrder(videosQueryEntity.ResultSortOrder),
                           GetSearchCollection(SummaryParts.RecentVideos),
                           preferences,
                           ModuleSearchUtility.MapDuplicationType(videosQueryEntity.DeduplicationType));

                    //// searchRequest.StructuredSearch.Query.SearchStringCollection.First().Value += TailQueryPart;

                    if (hasCacheBeenEnabled && 
                        request.FirstResultToReturn == 0)
                    {
                        var generator = new SummaryRecentVideosCacheKeyGenerator(
                            request.ModuleId, 
                            request.MaxResultsToReturn, 
                            preferences.ContentLanguages.ToArray())
                                            {
                                                CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                            };
                        controlData = generator.GetCacheControlData(controlData);
                    }

                    var response = GetPerformContentSearchResponse(searchRequest, controlData, preferences);
                    var conversionManager = new HeadlineListConversionManager(dateTimeFormatter);
                    tempPackage.Result = conversionManager.Process(response).Convert(request.TruncationType);
                },
                preferences);

            return partResult;
        }
        
        protected internal SummaryNewsPageServicePartResult<AbstractSummaryPackage> GetSummaryTrendingPackage(QueryEntity trendingQueryEntity, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var partResult = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
            var tempPackage = new SummaryTrendingPackage();
            partResult.Package = tempPackage;
            ProcessServicePartResult<SummaryTrendingPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    // run the search and then dissect it.
                    var response = GetTrendingSearchResponse<FreePerformContentSearchRequest, FreePerformContentSearchResponse>(trendingQueryEntity, request, preferences);

                    if (response == null || response.ContentSearchResult == null)
                    {
                        return;
                    }

                    if (response.ContentSearchResult.KeywordSet != null && response.ContentSearchResult.KeywordSet.Count > 0)
                    {
                        tempPackage.KeywordsTagCollection = AssembleTagCollection(response);
                        
                        //// Add search context ref
                        foreach (var tag in tempPackage.KeywordsTagCollection)
                        {
                            tag.SearchContextRef = GenerateSearchContextRef(request, tag.Text, EntityType.Textual);
                        }
                    }

                    if (response.ContentSearchResult.CodeNavigatorSet == null || response.ContentSearchResult.CodeNavigatorSet.Count <= 0)
                    {
                        return;
                    }

                    tempPackage.CompanyNewsEntities =
                        AssembleNewsEntities(
                            request, 
                            response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection
                                                 .Where(i => i.Id.ToLowerInvariant() == "co"));

                    tempPackage.IndustriesNewsEntities =
                        AssembleNewsEntities(
                            request, 
                            response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection
                                                 .Where(i => i.Id.ToLowerInvariant() == "in"));

                    tempPackage.NewsSubjectsNewsEntities =
                        AssembleNewsEntities(
                            request, 
                            response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection
                                                 .Where(i => i.Id.ToLowerInvariant() == "ns"));

                    tempPackage.ExecutivesNewsEntities =
                        AssembleNewsEntities(
                            request, 
                            response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection
                                                 .Where(i => i.Id.ToLowerInvariant() == "pe"));
                },
                preferences);

            return partResult;
        }

        protected internal IPerformContentSearchResponse GetVideosSearchResponse<TRequest, TResponse>(QueryEntity trendingQueryEntity, SummaryNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new()
        {
            var freeSearchRequest = CreateTrendingSearchRequest<TRequest>(trendingQueryEntity, request.MaxEntitiesToReturn, 0, 0, true, preferences);

            // Record the Transaction
            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(TResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<TResponse>(freeSearchRequest).ObjectResponse;
                },
                new ModuleDataRetrievalManager(controlData, preferences));

            return response;
        }

        protected internal IPerformContentSearchResponse GetTrendingSearchResponse<TRequest, TResponse>(QueryEntity trendingQueryEntity, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new()
        {
            var freeSearchRequest = CreateTrendingSearchRequest<TRequest>(trendingQueryEntity, request.MaxEntitiesToReturn, 0, 0, true, preferences);
            
            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, Settings.Default.DataAccessProxyUser);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new SummaryTrendingDataCacheKeyGenerator(request.ModuleId, preferences.ContentLanguages.ToArray(), preferences.InterfaceLanguage)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

             // Record the Transaction
            IPerformContentSearchResponse response = null;

            RecordTransaction(
                typeof(TResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                    {
                        response = manager.Invoke<TResponse>(freeSearchRequest).ObjectResponse;
                    },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));

            return response;
        }

        /// <summary>
        /// The get news volume variations for region query.
        /// </summary>
        /// <param name="queryEntity">The query collection.</param>
        /// <param name="request">The request.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A <see cref="NewsEntity">NewsEntity</see> object.
        /// </returns>
        protected internal NewsEntity GetRegionalVolumePerRegion(RegionalMapQueryEntity queryEntity, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            //// GET ROOT QUERY
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);
            var searchRequest = CreateSearchRequest<FreePerformContentSearchRequest>(queryEntity.QueryCollection, 0, 0);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new SummaryRegionalMapItemCacheKeyGenerator(request.ModuleId, queryEntity.RegionFcode)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            IPerformContentSearchResponse currentPeriodResponse = null;

            RecordTransaction(
                string.Concat("type: ", typeof(FreePerformContentSearchResponse).Name, " cache key: ", "NA"), 
                MethodBase.GetCurrentMethod().Name, 
                manager =>
                {
                    currentPeriodResponse = manager.Invoke<FreePerformContentSearchResponse>(searchRequest).ObjectResponse;
                },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));
            
            var currentHitCount = currentPeriodResponse.ContentSearchResult.HitCount;

            var entityNewsVolumeVariation = new NewsEntity
            {
                Code = queryEntity.RegionFcode,
                //// Not a real Factiva code but an identifier.
                Type = EntityType.Region,
                CurrentTimeFrameNewsVolume = new WholeNumber(currentHitCount),
                SearchContextRef = GenerateSearchContextRef(request, queryEntity.RegionFcode)
            };

            return entityNewsVolumeVariation;
        }

        protected internal SummaryNewsPageServicePartResult<AbstractSummaryPackage> GetSummaryRegionalMapPackage(RegionalMapQueryEntityCollection queryEntityCollection, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var partResult = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
            var tempPackage = new SummaryRegionalMapPackage();
            partResult.Package = tempPackage;
            ProcessServicePartResult<SummaryRegionalMapPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    tempPackage.RegionNewsVolume = GetRegionalVolumeVariations(queryEntityCollection, request, preferences);
                },
                preferences);

            return partResult;
        }

        protected internal SummaryNewsPageServicePartResult<AbstractSummaryPackage> GetSummaryRegionalMapPackageWithPcm(RegionalMapQueryEntityCollection queryEntityCollection, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var partResult = new SummaryNewsPageServicePartResult<AbstractSummaryPackage>();
            var tempPackage = new SummaryRegionalMapPackage();
            partResult.Package = tempPackage;
            var package = tempPackage;
            ProcessServicePartResult<SummaryRegionalMapPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    package.RegionNewsVolume = GetRegionalVolumeVariations(request, preferences);
                },
                preferences);

            if (partResult.ReturnCode != 0)
            {
                tempPackage = new SummaryRegionalMapPackage();
                partResult.Package = tempPackage;
                ProcessServicePartResult<SummaryRegionalMapPackage>(
                    MethodBase.GetCurrentMethod(),
                    partResult,
                    () =>
                    {
                        tempPackage.RegionNewsVolume = GetRegionalVolumeVariations(queryEntityCollection, request, preferences);
                    },
                    preferences);
            }

            return partResult;
        }

        /// <summary>
        /// The get regional volume variations.
        /// </summary>
        /// <param name="queryEntities">The query entities.</param>
        /// <param name="request">The request.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A list of EntityNewsVolumeVariation objects.
        /// </returns>
        protected internal List<NewsEntity> GetRegionalVolumeVariations(IEnumerable<RegionalMapQueryEntity> queryEntities, SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var tasks = queryEntities.Select(aQueryEntity => TaskFactory.StartNew(() => GetRegionalVolumePerRegion(aQueryEntity, request, preferences))).ToList();
            Task.WaitAll(tasks.ToArray());
            tasks.OrderBy(task => task.Result.Code);
            var results = tasks.Select(task => task.Result).ToList();
            return results;
        }

        /// <summary>
        /// The get regional volume variations.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A list of EntityNewsVolumeVariation objects.
        /// </returns>
        protected internal List<NewsEntity> GetRegionalVolumeVariations(SummaryNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var regionalMapRequest = new PCMSummaryRegionalMapRequest();
            int moduleIdInt;
            if (int.TryParse(request.ModuleId, out moduleIdInt))
            {
                regionalMapRequest.ModuleId = moduleIdInt;
            }

            regionalMapRequest.Version = string.Empty;

            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new SummaryRegionalMapCacheKeyGenerator(request.ModuleId)
                {
                    CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                };

                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            PCMSummaryRegionalMapResponse response = null;

            RecordTransaction(
                string.Concat("type: ", typeof(PCMSummaryRegionalMapResponse).Name),
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<PCMSummaryRegionalMapResponse>(regionalMapRequest).ObjectResponse;
                },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));
            var regionalVolumeVariations = new SummaryRegionalMapAssembler().Convert(response);
            foreach (var entity in regionalVolumeVariations)
            {
                entity.SearchContextRef = GenerateSearchContextRef(request, entity.Code);
            }

            return regionalVolumeVariations;
        }

        private static string GenerateSearchContextRef(IModuleRequest request, string code)
        {
            var searchContext = new SummaryRegionalMapBubbleSearchContext
            {
                Code = code,
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                SearchContextType = typeof(SummaryRegionalMapBubbleSearchContext).Name,
                Json = searchContext.ToString(),
                PageId = request.PageId,
                ModuleId = request.ModuleId
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        /// <summary>
        /// Gets the search collection.
        /// </summary>
        /// <param name="dto">The Data Transfer object [i.e. dto].</param>
        /// <returns>A collection of SearchCollections</returns>
        private static SearchCollectionCollection GetSearchCollection(SummaryParts dto)
        {
            var list = new SearchCollectionCollection();
            switch (dto)
            {
                case SummaryParts.RecentArticles:
                    list.Add(SearchCollection.Publications);
                    list.Add(SearchCollection.WebSites);
                    break;
                case SummaryParts.Trending:
                    list.Add(SearchCollection.Publications);
                    list.Add(SearchCollection.WebSites);
                    break;
                case SummaryParts.RecentVideos:
                    list.Add(SearchCollection.Multimedia);
                    break;
            }

            return list;
        }

        /// <summary>
        /// Creates the search request.
        /// </summary>
        /// <typeparam name="TSearch">The type of the search.</typeparam>
        /// <param name="queryCollection">The query collection.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <returns>
        /// A IPerformContentSearchRequest object.
        /// </returns>
        internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(QueryCollection queryCollection, int firstResult, int maxResults)
            where TSearch : IPerformContentSearchRequest, new()
        {
            // Add the query tail
            queryCollection.First().Text += TailQueryPart;

            var request = ModuleSearchUtility.GetSearchRequest<TSearch>(
                queryCollection,
                firstResult,
                maxResults,
                ResultSortOrder.PublicationDateReverseChronological,
                new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites });
            return request;
        }

        private static TagCollection AssembleTagCollection(IPerformContentSearchResponse response)
        {
            var assembler = new KeywordSetTagConverter(response.ContentSearchResult.KeywordSet);
            var rules = new TagCloudGenerationRules();
            var tagCollection = new TagCollection(assembler.Process<Tag>(rules));
            return tagCollection;
        }

        private static List<NewsEntity> AssembleNewsEntities(IModuleRequest request, IEnumerable<Navigator> navigator)
        {
            if (navigator == null || navigator.Count() == 0)
            {
                return null;
            }

            var tempNavigator = navigator.First();
            var entityType = MapEntityType(tempNavigator.Id);
            return tempNavigator.BucketCollection.Select(bucket => new NewsEntity
                                                                           {
                                                                               Code = bucket.Id, 
                                                                               CurrentTimeFrameNewsVolume = new WholeNumber(bucket.HitCount), 
                                                                               Descriptor = bucket.Value,
                                                                               Type = entityType,
                                                                               SearchContextRef = GenerateSearchContextRef(request, bucket.Id, entityType)
                                                                           }).ToList();
        }

        private static string GenerateSearchContextRef(IModuleRequest request, string code, EntityType entityType)
        {
            var searchContext = new SummaryTrendingSearchContext
            {
                EntityType = entityType,
                Code = code,
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                SearchContextType = typeof(SummaryTrendingSearchContext).Name,
                Json = searchContext.ToString(),
                PageId = request.PageId,
                ModuleId = request.ModuleId
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        private static EntityType MapEntityType(string type)
        {
            switch (type.ToLowerInvariant())
            {
                case "in": 
                    return EntityType.Industry;
                case "ns":
                    return EntityType.NewsSubject;
                case "co":
                    return EntityType.Company;
                case "pe":
                    return EntityType.Person;
            }

            return EntityType.UnSpecified;
        }

        private static IEnumerable<string> GetBlackList()
        {
            return new List<string>
                           {
                               "looking statements", 
                               "united states",
                               "new york",
                               "dow jones",
                               "jones newswires", 
                               "associated press",
                               "street journal", 
                               "jones global", 
                               "stock exchange"
                           };
        }

        private static IPerformContentSearchRequest CreateTrendingSearchRequest<TSearch>(QueryEntity queryEntity, int maxEntitiesToReturn, int firstResult, int maxResults, bool getNavigators, IPreferences preferences)
            where TSearch : IPerformContentSearchRequest, new()
        {
            var searchRequest = ModuleSearchUtility.GetSearchRequest<TSearch>(
                queryEntity.QueryCollection,
                firstResult,
                maxResults,
                ResultSortOrder.PublicationDateReverseChronological,
                new SearchCollectionCollection
                    {
                        SearchCollection.Publications, SearchCollection.WebSites
                    },
                preferences);

            searchRequest.StructuredSearch.Query.SearchStringCollection.First().Value += TailQueryPart;
            searchRequest.StructuredSearch.Formatting.KeywordFilter.BlackListTermCollection.AddRange(GetBlackList());
            if (getNavigators)
            {
                //// APPLY SPECIFIC MODIFICATION/CHANGES
                searchRequest.NavigationControl.KeywordControl.ReturnKeywords = true;
                searchRequest.NavigationControl.KeywordControl.MaxKeywords = maxEntitiesToReturn;
                searchRequest.NavigationControl.KeywordControl.MinWeight = DefaultMinWeightKeywords;
                searchRequest.DescriptorControl.Mode = DescriptorControlMode.All;
                searchRequest.DescriptorControl.Language = preferences.InterfaceLanguage;
                searchRequest.NavigationControl.CodeNavigatorControl.CustomCollection.AddRange(new[]
                                                                                                   {
                                                                                                       new NavigatorControl
                                                                                                           {
                                                                                                               MaxBuckets = maxEntitiesToReturn,
                                                                                                               MinBucketValue = 0,
                                                                                                               Id = "co",
                                                                                                           },
                                                                                                       new NavigatorControl
                                                                                                           {
                                                                                                               MaxBuckets = maxEntitiesToReturn,
                                                                                                               MinBucketValue = 0,
                                                                                                               Id = "ns",
                                                                                                           },
                                                                                                       new NavigatorControl
                                                                                                           {
                                                                                                               MaxBuckets = maxEntitiesToReturn,
                                                                                                               MinBucketValue = 0,
                                                                                                               Id = "in",
                                                                                                           },
                                                                                                       new NavigatorControl
                                                                                                           {
                                                                                                               MaxBuckets = maxEntitiesToReturn,
                                                                                                               MinBucketValue = 0,
                                                                                                               Id = "pe",
                                                                                                           },
                                                                                                   });

                searchRequest.DescriptorControl.Language = preferences.InterfaceLanguage;
            }

            return searchRequest;
        }

        private IPerformContentSearchResponse GetPerformContentSearchResponse(IPerformContentSearchRequest performContentSearchRequest, ControlData controlData, IPreferences preferences)
        {
            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(FreePerformContentSearchRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<FreePerformContentSearchResponse>(performContentSearchRequest, controlData).ObjectResponse;
                },
                new ModuleDataRetrievalManager(controlData, preferences));
            return response;
        }
    }
}
