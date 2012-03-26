// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionalMapNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.RegionalMap;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.PCM.RegionalMap.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using EntityType = DowJones.Tools.Ajax.HeadlineList.EntityType;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreePerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using NewsEntityNewsVolumeVariation = DowJones.Web.Mvc.UI.Models.Common.NewsEntityNewsVolumeVariation;
using RegionalMapNewsVolumePackage = DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.RegionalMapNewsVolumePackage;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;
using TimeFrame = DowJones.Utilities.Managers.Search.TimeFrame;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    /// <summary>
    /// The regional map news page module service result.
    /// </summary>
    [DataContract(Name = "regionalMapNewsPageModuleServiceResult", Namespace = "")]
    public class RegionalMapNewsPageModuleServiceResult : Generic.AbstractServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>, RegionalMapNewsVolumePackage>, IPopulate<RegionalMapNewsPageModuleDataRequest>
    {
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration && Settings.Default.CacheRegionalMapNewsPageModuleService;
        
        public void Populate(ControlData controlData, RegionalMapNewsPageModuleDataRequest request, IPreferences preferences)
        {
            Populate(controlData, request, preferences, Settings.Default.UsePcm);
        }

        public void Populate(ControlData controlData, RegionalMapNewsPageModuleDataRequest request, IPreferences preferences, bool usePcm)
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

                    var module = GetModule(request, controlData, preferences);

                    MaxPartsAvailable = 1;
                    PartResults = GetParts(module, controlData, preferences, request, usePcm);
                },
                preferences);
        }


        internal RegionalMapNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<RegionalMapNewspageModule>(request, controlData, preferences);
            if (module.QueryEntityCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);
            }

            return module;
        }

        protected internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(RegionalMapNewspageModule module, string regionFcode, TimeFrame timeFrame, int firstResult, int maxResults, IPreferences preferences)
            where TSearch : IPerformContentSearchRequest, new()
        {
            return (from queryEntity in module.QueryEntityCollection
                    where queryEntity.RegionFcode == regionFcode
                    select CreateSearchRequest<TSearch>(queryEntity.QueryCollection, firstResult, maxResults, timeFrame.ConvertToCurrentDateRange(), preferences)).FirstOrDefault();
        }

        protected internal List<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>> GetParts(RegionalMapNewspageModule module, ControlData controlData, IPreferences preferences, RegionalMapNewsPageModuleDataRequest request, bool usePcm)
        {
            var results = new List<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>>();
            var partResult = new RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>();

            // fire version that uses PCM
            var localResult = partResult;
            if (usePcm)
            {
                ProcessServicePartResult<RegionalMapNewsVolumePackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    localResult.Package = GetRegionalMapInitialData(preferences, request);
                    results.Add(localResult);
                },
                preferences);
            }

            // if this version fails fall back to old logic
            if (!usePcm || partResult.ReturnCode != 0)
            {
                partResult = new RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>();
                results.Clear();
                ProcessServicePartResult<RegionalMapNewsVolumePackage>(
                    MethodBase.GetCurrentMethod(),
                    partResult,
                    () =>
                    {
                        partResult.Package = GetRegionalMapInitialData(module, preferences, request);
                        results.Add(partResult);
                    },
                    preferences);
            }

            return results;
        }

        /// <summary>
        /// The get part results.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="controlData">The control data.</param>
        /// <param name="preferences">The preferences.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A <see cref="AbstractRegionalMapPackage">RegionalMapPackage</see> object.&gt;
        /// </returns>
        protected internal List<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>> GetCachedParts(RegionalMapNewspageModule module, ControlData controlData, IPreferences preferences, RegionalMapNewsPageModuleDataRequest request)
        {
            var results = new List<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>>();
            var partResult = new RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>();
            results.Add(partResult);

            ProcessServicePartResult<RegionalMapNewsVolumePackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        partResult.Package = GetRegionalMapInitialData(module, preferences, request);
                    },
                    preferences);
            return results;
        }

        /// <summary>
        /// The get news volume variations for region query.
        /// </summary>
        /// <param name="queryEntity">The query collection.</param>
        /// <param name="request">The request.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A <see cref="UI.Models.Common.NewsEntityNewsVolumeVariation">EntityNewsVolumeVariation</see> object.
        /// </returns>
        protected internal NewsEntityNewsVolumeVariation GetRegionalVolumeVariationPerRegion(RegionalMapQueryEntity queryEntity, RegionalMapNewsPageModuleDataRequest request, IPreferences preferences)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);

            var currentPeriod = TaskFactory.StartNew(() => GetSearchResponseForSpecifiedPeriod(request.TimeFrame.ConvertToCurrentDateRange(), queryEntity, request, preferences));
            var previousPeriod = TaskFactory.StartNew(() => GetSearchResponseForSpecifiedPeriod(request.TimeFrame.ConvertToPreviousDateRange(), queryEntity, request, preferences, 0, 0, CacheTimePeriod.Previous));

            Task.WaitAll(currentPeriod, previousPeriod);

            var currentPeriodResponse = currentPeriod.Result;
            var previousPeriodResponse = previousPeriod.Result;

            var currentHitCount = currentPeriodResponse.ContentSearchResult.HitCount;
            var previousHitCount = previousPeriodResponse.ContentSearchResult.HitCount;

            var entityNewsVolumeVariation = new NewsEntityNewsVolumeVariation
            {
                Code = queryEntity.RegionFcode,
                //// Not a real Factiva code but an identifier.
                Type = EntityType.Region,
                NewEntrant = (previousHitCount == 0) ? true : false,
                CurrentTimeFrameNewsVolume = new WholeNumber(currentHitCount),
                PreviousTimeFrameNewsVolume = new WholeNumber(previousHitCount),
                SearchContextRef = GenerateSearchContextRef(request, queryEntity.RegionFcode),
            };

            if (!entityNewsVolumeVariation.NewEntrant)
            {
                entityNewsVolumeVariation.PercentVolumeChange = new Percent((((double)currentHitCount / previousHitCount) - 1) * 100);
            }

            return entityNewsVolumeVariation;
        }

        /// <summary>
        /// The get regional map initial data.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="preferences">The preferences.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A Regional Map Package
        /// </returns>
        protected internal RegionalMapNewsVolumePackage GetRegionalMapInitialData(RegionalMapNewspageModule module, IPreferences preferences, RegionalMapNewsPageModuleDataRequest request)
        {
            var regionalMapNewsVolumePackage = new RegionalMapNewsVolumePackage
            {
                RegionNewsVolume = GetRegionalVolumeVariations(module.QueryEntityCollection, request, preferences),
            };

            return regionalMapNewsVolumePackage;
        }

        /// <summary>
        /// The get regional map initial data.
        /// </summary>
        /// <param name="preferences">The preferences.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A Regional Map Package
        /// </returns>
        protected internal RegionalMapNewsVolumePackage GetRegionalMapInitialData(IPreferences preferences, RegionalMapNewsPageModuleDataRequest request)
        {
            var regionalMapNewsVolumePackage = new RegionalMapNewsVolumePackage
            {
                RegionNewsVolume = GetRegionalVolumeVariations(request, preferences),
            };

            return regionalMapNewsVolumePackage;
        }
        
        /// <summary>
        /// Generates the search context ref.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="code">The FII code.</param>
        /// <returns>A search context reference string.</returns>
        private static string GenerateSearchContextRef(RegionalMapNewsPageModuleDataRequest request, string code)
        {
            // RegionalMap::{pageId}::{moduleId}::{timeFrame}::{entity}::{code}
            // return string.Format("RegionalMap::{0}::{1}::{2}::{3}::{4}", request.PageId, request.ModuleId, request.TimeFrame, EntityType.Region, code);
            var searchContext = new RegionalMapBubbleSearchContext
            {
                Code = code,
                TimeFrame = request.TimeFrame
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(RegionalMapBubbleSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        /// <summary>
        /// Creates the search request.
        /// </summary>
        /// <typeparam name="TSearch">The type of the search.</typeparam>
        /// <param name="queryCollection">The query collection.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <param name="datePeriod">The date period.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A IPerformContentSearchRequest object.
        /// </returns>
        private static IPerformContentSearchRequest CreateSearchRequest<TSearch>(QueryCollection queryCollection, int firstResult, int maxResults, Dates datePeriod, IPreferences preferences)
            where TSearch : IPerformContentSearchRequest, new()
        {
            var request = ModuleSearchUtility.GetSearchRequest<TSearch>(
                queryCollection,
                firstResult,
                maxResults,
                ResultSortOrder.PublicationDateReverseChronological,
                new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites });

            //// APPLY SPECIFIC MODIFICATION/CHANGES
            request.StructuredSearch.Query.Dates = datePeriod;
            request.DescriptorControl.Language = preferences.InterfaceLanguage;
            return request;
        }

        private static Factiva.Gateway.Messages.PCM.RegionalMap.V1_0.TimeFrame MapTimeFrame(TimeFrame timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrame.LastMonth:
                    return Factiva.Gateway.Messages.PCM.RegionalMap.V1_0.TimeFrame.LastMonth;
                case TimeFrame.LastSixMonths:
                    return Factiva.Gateway.Messages.PCM.RegionalMap.V1_0.TimeFrame.LastSixMonths;
                case TimeFrame.LastTwoYear:
                    return Factiva.Gateway.Messages.PCM.RegionalMap.V1_0.TimeFrame.LastTwoYear;
                case TimeFrame.LastWeek:
                    return Factiva.Gateway.Messages.PCM.RegionalMap.V1_0.TimeFrame.LastWeek;
                case TimeFrame.LastYear:
                    return Factiva.Gateway.Messages.PCM.RegionalMap.V1_0.TimeFrame.LastYear;
                default:
                    return Factiva.Gateway.Messages.PCM.RegionalMap.V1_0.TimeFrame.ThreeMonths;
            }
        }

        private IPerformContentSearchResponse GetSearchResponseForSpecifiedPeriod(
            Dates datePeriod,
            RegionalMapQueryEntity queryEntity,
            RegionalMapNewsPageModuleDataRequest request,
            IPreferences preferences,
            int firstResult = 0,
            int maxResults = 0,
            CacheTimePeriod timePeriod = CacheTimePeriod.Current)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);
            //// GET ROOT QUERY
            var searchRequest = CreateSearchRequest<FreePerformContentSearchRequest>(queryEntity.QueryCollection, firstResult, maxResults, datePeriod, preferences);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new RegionalMapItemCacheKeyGenerator(
                                    request.ModuleId,
                                    queryEntity.RegionFcode,
                                    request.TimeFrame, 
                                    timePeriod)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            FreePerformContentSearchResponse response = null;
            RecordTransaction(
                string.Concat("type: ", typeof(FreePerformContentSearchResponse).Name), 
                MethodBase.GetCurrentMethod().Name, 
                manager =>
                {
                    response = manager.Invoke<FreePerformContentSearchResponse>(searchRequest).ObjectResponse;
                },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));
            return response;
        }

        private List<NewsEntityNewsVolumeVariation> GetRegionalVolumeVariations(RegionalMapNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var regionalMapRequest = new PCMRegionalMapRequest();
            int moduleIdInt;

            if (int.TryParse(request.ModuleId, out moduleIdInt))
            {
                regionalMapRequest.ModuleId = moduleIdInt;
            }

            regionalMapRequest.TimeFrame = MapTimeFrame(request.TimeFrame);
            regionalMapRequest.Version = string.Empty;

            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new RegionalMapCacheKeyGenerator(
                                    request.ModuleId,
                                    request.TimeFrame)
                {
                    CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                };

                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            PCMRegionalMapResponse response = null;
            RecordTransaction(
                string.Concat("type: ", typeof(PCMRegionalMapResponse).Name),
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<PCMRegionalMapResponse>(regionalMapRequest).ObjectResponse;
                },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));
            return new RegionalMapAssembler().Convert(response);
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
        private List<NewsEntityNewsVolumeVariation> GetRegionalVolumeVariations(IEnumerable<RegionalMapQueryEntity> queryEntities, RegionalMapNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var tasks = queryEntities.Select(aQueryEntity => TaskFactory.StartNew(() => GetRegionalVolumeVariationPerRegion(aQueryEntity, request, preferences))).ToList();
            Task.WaitAll(tasks.ToArray());
            tasks.OrderBy(task => task.Result.Code);
            var results = tasks.Select(task => task.Result).ToList();
            return results;
        }
    }
}