// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrendingNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Search;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Trending;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using DowJones.Web.Mvc.UI.Models.Common;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreePerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    /// <summary>
    /// The trending news page module service result.
    /// </summary>
    [DataContract(Name = "trendingNewsPageModuleServiceResult", Namespace = "")]
    public class TrendingNewsPageModuleServiceResult : Generic.AbstractServiceResult<TrendingNewsPageServicePartResult<AbstractTrendingPackage>, AbstractTrendingPackage>, IPopulate<TrendingNewsPageModuleRequest>
    {
        private static readonly int NumberOfEntitiesToRequest = Settings.Default.TrendingNumberOfEntitiesToRequest;
        private const int NumberOfEntitiesToShow = 5;
        private const int DefaultMinimumHitCount = 5;
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration && Settings.Default.CacheTrendingNewsPageModuleService;

        #region IPopulate<TrendingNewsPageModuleRequest> Members

        public void Populate(ControlData controlData, TrendingNewsPageModuleRequest request, IPreferences preferences)
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

                    var module = GetModule(request, controlData, preferences);

                    // get the feeds from the definition
                    MaxPartsAvailable = 3;
                    hasCacheBeenEnabled = hasCacheBeenEnabled && request.CacheState != CacheState.Off;
                    PartResults = GetPartResults(controlData, request, preferences, module);
                },
                preferences);

            return;
        }

        #endregion

        protected internal static SearchStringCollection GetSearchStringCollection(QueryEntity queryEntity)
        {
            var searchStringCollection = new SearchStringCollection();
            if (queryEntity != null && queryEntity.QueryCollection.Count > 0)
            {
                foreach (var searchstring in queryEntity.QueryCollection.Select(query => new SearchString
                {
                    Id = "phrase",
                    Value = query.Text,
                    Type = SearchType.Free,
                    Mode = SearchManager.MapSearchMode(query.SearchMode),
                    Combine = false,
                    Filter = false,
                    Validate = true
                }))
                {
                    searchStringCollection.Add(searchstring);
                }
            }

            return searchStringCollection;
        }

        protected internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(SearchStringCollection searchStringCollection, Dates datePeriod, EntityType entityType, int firstResult, int maxResults, IPreferences preferences)
            where TSearch : IPerformContentSearchRequest, new()
        {
            var navigationControlID = GetNavigationControlId(entityType);

            // GET ROOT QUERY
            var request = ModuleSearchUtility.GetSearchRequest<TSearch>(
                searchStringCollection,
                firstResult,
                maxResults,
                ResultSortOrder.PublicationDateReverseChronological,
                new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites },
                DeduplicationMode.Off);

            //// APPLY SPECIFIC MODIFICATION/CHANGES
            request.StructuredSearch.Query.Dates = datePeriod;
            request.DescriptorControl.Language = preferences.InterfaceLanguage;
            request.DescriptorControl.Mode = DescriptorControlMode.All;
            request.NavigationControl.CodeNavigatorControl.CustomCollection = new NavigatorControlCollection
                                                                                  {
                                                                                      new NavigatorControl
                                                                                          {
                                                                                              Id = navigationControlID,
                                                                                              MaxBuckets = NumberOfEntitiesToRequest // MaxBuckets should be a constant,

                                                                                          }
                                                                                  };
            return request;
        }

        protected internal static string GetNavigationControlId(EntityType entityType)
        {
            var fi = entityType.GetType().GetField(entityType.ToString());
            var attribute = fi.GetCustomAttributes(typeof(NavigationControlIdAttribute), false).First() as NavigationControlIdAttribute;

            if (attribute != null)
            {
                return attribute.Id;
            }

            throw new DowJonesUtilitiesException("Invalid Trending Request Type ");
        }

        protected internal static Tools.Ajax.HeadlineList.EntityType MapToTargetEntityType(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Companies:
                    return Tools.Ajax.HeadlineList.EntityType.Company;
                case EntityType.People:
                    return Tools.Ajax.HeadlineList.EntityType.Person;
                case EntityType.Subjects:
                    return Tools.Ajax.HeadlineList.EntityType.NewsSubject;

                default:
                    throw new DowJonesUtilitiesException("Cannot convert Requests.EntityType to Tools.Ajax.HeadlineList.EntityType");
            }
        }

        protected internal TrendingNewsPageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<TrendingNewsPageModule>(request, controlData, preferences);
            if (module.QueryEntityCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);
            }

            return module;
        }

        protected internal List<TrendingNewsPageServicePartResult<AbstractTrendingPackage>> GetPartResults(ControlData controlData, TrendingNewsPageModuleRequest request, IPreferences preferences, TrendingNewsPageModule module)
        {
            // At least one Trend type
            // Guard.IsNotNullOrEmpty<TrendType>(request.TrendTypes, "Trending Type");
            var result = new List<TrendingNewsPageServicePartResult<AbstractTrendingPackage>>();

            request.Parts = (List<TrendType>)request.Parts.GetUniques();
            
            if (module.QueryEntityCollection.Count <= 0)
            {
                return null;
            }

            var currentEntity = module.QueryEntityCollection.FirstOrDefault();

            var currentDateRange = request.TimeFrame.ConvertToCurrentDateRange();
            //// If "Trending Up" or "Trending Down" is requested we would need to sort to get either one of them
            var previousDateRange = request.TimeFrame.ConvertToPreviousDateRange();

            var containsTrending = request.Parts.Contains(TrendType.TrendingUp) || request.Parts.Contains(TrendType.TrendingDown);

            Task<IPerformContentSearchResponse> previousPeriodTask = null;
            var currentPeriodTask = TaskFactory.StartNew(() => GetSearchResponseForSpecificPeriod(request, currentEntity, preferences, request.EntityType, currentDateRange, CacheTimePeriod.Current));

            if (containsTrending)
            {
                previousPeriodTask = TaskFactory.StartNew(() => GetSearchResponseForSpecificPeriod(request, currentEntity, preferences, request.EntityType, previousDateRange, CacheTimePeriod.Previous));
                Task.WaitAll(currentPeriodTask, previousPeriodTask);
            }
            else
            {
                Task.WaitAll(currentPeriodTask);
            }

            var currentPeriodResponse = currentPeriodTask.Result;
            if (containsTrending)
            {
                var previousPeriodResponse = previousPeriodTask.Result;
                RecordTransaction(
                    "Trending Post-Processing",
                    "Sorting trending data",
                    () => result.AddRange(GetTrendingInformation(request, currentEntity, currentPeriodResponse, previousPeriodResponse, controlData, preferences)));
            }

            if (request.Parts.Contains(TrendType.TopEntities))
            {
                result.Add(GetTopRequested(currentPeriodResponse, request));
            }

            return result;
        }

        protected internal TrendingNewsPageServicePartResult<AbstractTrendingPackage> GetTopRequested(IPerformContentSearchResponse currentPeriodResponse, TrendingNewsPageModuleRequest request)
        {
            var topEntitiesPartResult = new TrendingNewsPageServicePartResult<AbstractTrendingPackage>
            {
                Package = GetTopFiveEntities(currentPeriodResponse, request)
            };

            return topEntitiesPartResult;
        }

        protected internal List<TrendingNewsPageServicePartResult<AbstractTrendingPackage>> GetTrendingInformation(
            TrendingNewsPageModuleRequest request,
            QueryEntity queryEntity,
            IPerformContentSearchResponse currentPeriodResponse,
            IPerformContentSearchResponse previousPeriodResponse,
            ControlData controlData,
            IPreferences preferences)
        {
            // NN: Logic
            // Get unique entities, by code, from both previous and current time frames
            // For each entity, keep previous and current count on the same row
            // For each entity, rankScore is calculated with the following formula:
            //      rankScore = (Current - Previous)(ln(Current+0.5) * ln(Current+0.5))
            // Sort entities by rankScore
            var result = new List<TrendingNewsPageServicePartResult<AbstractTrendingPackage>>();

            var targetEntityType = MapToTargetEntityType(request.EntityType);

            // Get raw
            var currentEntitiesRaw = currentPeriodResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection.First().BucketCollection;
            var previousEntitiesRaw = previousPeriodResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection.First().BucketCollection;

            var entitiesDictionary = new Dictionary<string, Change>();
            foreach (var entity in currentEntitiesRaw)
            {
                if (!entitiesDictionary.ContainsKey(entity.Id))
                {
                    entitiesDictionary.Add(entity.Id, new Change { Code = entity.Id, Descriptor = entity.Value });
                }

                entitiesDictionary[entity.Id].Current = entity.HitCount;
            }

            foreach (var entity in previousEntitiesRaw)
            {
                if (!entitiesDictionary.ContainsKey(entity.Id))
                {
                    entitiesDictionary.Add(entity.Id, new Change { Code = entity.Id, Descriptor = entity.Value });
                }

                entitiesDictionary[entity.Id].Previous = entity.HitCount;
            }

            // drop entities with both current and previous < DefaultMinimumHitCount
            var filteredEntities = from kvp in entitiesDictionary
                                   where kvp.Value.Previous >= DefaultMinimumHitCount || kvp.Value.Current >= DefaultMinimumHitCount
                                   select kvp;

            if (request.Parts.Contains(TrendType.TrendingUp))
            {
                var trendingUpEntities = (from kvp in filteredEntities
                                          where kvp.Value.RankScore >= 0
                                          orderby kvp.Value descending
                                          select kvp.Value).Take(5).OrderByDescending(changeEntity => changeEntity.ChangePercentage);
                result.Add(CreateTrendingPartResult<TrendingUpPackage>(request, trendingUpEntities, TrendType.TrendingUp, targetEntityType));
            }

            if (request.Parts.Contains(TrendType.TrendingDown))
            {
                var trendingDownEntities = (from kvp in filteredEntities
                                            where kvp.Value.RankScore <= 0
                                            orderby kvp.Value ascending
                                            select kvp.Value).Take(5).OrderBy(changeEntity => changeEntity.ChangePercentage);

                var trendingDownWithUpdatedCurrentHitCount = GetCurrentHitCount(trendingDownEntities, request, queryEntity, preferences);
                result.Add(CreateTrendingPartResult<TrendingDownPackage>(request, trendingDownWithUpdatedCurrentHitCount, TrendType.TrendingDown, targetEntityType));
            }

            return result;
        }

        private IEnumerable<Change> GetCurrentHitCount(IEnumerable<Change> trendingDownEntities,
                                                       TrendingNewsPageModuleRequest request,
                                                       QueryEntity queryEntity,
                                                       IPreferences preferences)
        {
            if (trendingDownEntities == null || trendingDownEntities.Count() == 0)
                return new List<Change>();

            // get entities with 0 in current hit count
            var codes = (from entity in trendingDownEntities
                         where entity.Current == 0
                         select entity.Code).ToArray();

            if (codes.Length == 0)
                return trendingDownEntities;

            var searchRequest = CreateSearchRequest<PerformContentSearchRequest>(
                                    GetSearchStringCollection(queryEntity),
                                    request.TimeFrame.ConvertToCurrentDateRange(),
                                    request.EntityType,
                                    0,
                                    0,
                                    preferences);

            SearchUtility.ScopeType scopeType;
            switch (request.EntityType)
            {
                case EntityType.Companies:
                    scopeType = SearchUtility.ScopeType.AnyFDS;
                    break;
                case EntityType.People:
                    scopeType = SearchUtility.ScopeType.AnyPeople;
                    break;
                case EntityType.Subjects:
                    scopeType = SearchUtility.ScopeType.AnyNewsSubject;
                    break;
                default:
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.TrendingEntityTypeNotSupported);
            }

            searchRequest.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(scopeType, codes));

            IPerformContentSearchResponse response = null;
            var searchManager = new SearchManager(ControlDataManager.GetLightWeightUserControlData(ProxyUser), preferences.InterfaceLanguage);
            RecordTransaction(
                typeof(PerformContentSearchResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<PerformContentSearchResponse>(searchRequest).ObjectResponse;
                },
                searchManager);

            if (response.ContentSearchResult == null ||
                response.ContentSearchResult.CodeNavigatorSet == null ||
                response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection == null ||
                response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection.Count == 0)
                return trendingDownEntities;

            var currentEntitiesRaw = response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection.First().BucketCollection;

            if (currentEntitiesRaw == null)
                return trendingDownEntities;

            foreach (var entityRaw in currentEntitiesRaw)
            {
                foreach (var entity in trendingDownEntities)
                {
                    if (entity.Code == entityRaw.Id)
                    {
                        entity.Current = entityRaw.HitCount;
                        break;
                    }
                }
            }

            return trendingDownEntities.OrderBy(changeEntity => changeEntity.ChangePercentage);
        }

        private static TrendingNewsPageServicePartResult<AbstractTrendingPackage> CreateTrendingPartResult<TPackage>(
            TrendingNewsPageModuleRequest request, IEnumerable<Change> trendingEntities, TrendType trendType, Tools.Ajax.HeadlineList.EntityType entityType)
            where TPackage : AbstractTrendingPackage, new()
        {
            var trendingServicePartResult = new TrendingNewsPageServicePartResult<AbstractTrendingPackage>
            {
                Package = new TPackage { TrendingEntities = new List<NewsEntityNewsVolumeVariation>() }
            };

            foreach (var changeEntity in trendingEntities)
            {
                var percent = changeEntity.ChangePercentage == decimal.MaxValue ? null : new Percent(changeEntity.ChangePercentage);
                trendingServicePartResult.Package.TrendingEntities.Add(new NewsEntityNewsVolumeVariation
                {
                    NewEntrant = changeEntity.Previous == 0,
                    CurrentTimeFrameNewsVolume = new WholeNumber(changeEntity.Current),
                    PreviousTimeFrameNewsVolume = new WholeNumber(changeEntity.Previous),
                    PercentVolumeChange = percent,
                    Type = entityType,
                    Descriptor = changeEntity.Descriptor,
                    Code = changeEntity.Code,
                    SearchContextRef = GenerateSearchContextRef(request, changeEntity.Code, trendType)
                });
            }
            return trendingServicePartResult;
        }

        private static List<NewsEntity> ConvertGatewayBucketToTopNewsVolumeEntities(TrendingNewsPageModuleRequest request, IEnumerable<Bucket> buckets)
        {
            var targetType = MapToTargetEntityType(request.EntityType);
            return buckets.Select(bucket => new NewsEntity
            {
                CurrentTimeFrameNewsVolume = new WholeNumber(bucket.HitCount),
                Code = bucket.Id,
                Descriptor = bucket.Value,
                Type = targetType,
                SearchContextRef = GenerateSearchContextRef(request, bucket.Id, TrendType.TopEntities)
            }).ToList();
        }

        private static TrendingTopEntitiesPackage GetTopFiveEntities(IPerformContentSearchResponse searchResponse, TrendingNewsPageModuleRequest request)
        {
            var trendingPackage = new TrendingTopEntitiesPackage();
            var currentCompanies = searchResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection.First().BucketCollection;
            var topNCurrent = currentCompanies.OrderByDescending(c => c.HitCount).Take(NumberOfEntitiesToShow).ToList();
            trendingPackage.TopNewsVolumeEntities = ConvertGatewayBucketToTopNewsVolumeEntities(request, topNCurrent);
            return trendingPackage;
        }

        private static string GenerateSearchContextRef(TrendingNewsPageModuleRequest request, string code, TrendType trendType)
        {
            var searchContext = new TrendingItemSearchContext
            {
                EntityType = request.EntityType,
                TimeFrame = request.TimeFrame,
                Code = code,
                TrendType = trendType
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                SearchContextType = typeof(TrendingItemSearchContext).Name,
                Json = searchContext.ToString(),
                PageId = request.PageId,
                ModuleId = request.ModuleId
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        private IPerformContentSearchResponse GetSearchResponseForSpecificPeriod(TrendingNewsPageModuleRequest request, QueryEntity searchStringCollection, IPreferences preferences, EntityType entityType, Dates datePeriod, CacheTimePeriod cacheTimePeriod)
        {
            var searchRequest = CreateSearchRequest<FreePerformContentSearchRequest>(GetSearchStringCollection(searchStringCollection), datePeriod, entityType, 0, 0, preferences);

            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(controlData, Settings.Default.DataAccessProxyUser);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new TrendingCacheKeyGenerator(
                    request.ModuleId,
                    preferences.InterfaceLanguage,
                    entityType,
                    request.TimeFrame,
                    cacheTimePeriod)
                {
                    CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            IPerformContentSearchResponse response = null;
            var searchManager = new SearchManager(lightweightProxy, preferences.InterfaceLanguage);
            RecordTransaction(
                typeof(FreePerformContentSearchResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<FreePerformContentSearchResponse>(searchRequest).ObjectResponse;
                },
                searchManager);
            return response;
        }
    }

    internal class Change : IComparable<Change>
    {
        public string Code;
        public string Descriptor;
        private int previous;
        private int current;

        private double? rankScore;
        private decimal? changePercentage;

        public decimal ChangePercentage
        {
            get
            {
                if (!changePercentage.HasValue)
                {
                    changePercentage = previous == 0 ?
                                           decimal.MaxValue :
                                           decimal.Round((Convert.ToDecimal(current) - Convert.ToDecimal(previous)) / Convert.ToDecimal(previous) * 100, 2);
                }

                return changePercentage.Value;
            }
        }

        public double RankScore
        {
            get
            {
                if (!rankScore.HasValue)
                {
                    var log = Math.Log(current + 3);
                    rankScore = (current - previous) / (log * log);
                }

                return rankScore.Value;
            }
        }

        public int Previous
        {
            get { return previous; }
            set
            {
                rankScore = null;
                changePercentage = null;
                previous = value;
            }
        }

        public int Current
        {
            get { return current; }
            set
            {
                rankScore = null;
                changePercentage = null;
                current = value;
            }
        }

        public int CompareTo(Change other)
        {
            return other == null ? 1 : RankScore.CompareTo(other.RankScore);
        }
    }
}
