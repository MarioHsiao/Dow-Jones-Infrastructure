// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Radar;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using DowJones.Web.Mvc.UI.Models.Common;
using Factiva.Gateway.Messages.Assets.FCP.V1_1;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Symbology.Taxonomy.V1_0;
using Newtonsoft.Json;
using CodeNavigatorControl = Factiva.Gateway.Messages.Search.V2_0.CodeNavigatorControl;
using CodeNavigatorMode = Factiva.Gateway.Messages.Search.V2_0.CodeNavigatorMode;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using DateFormat = Factiva.Gateway.Messages.Search.V2_0.DateFormat;
using Dates = Factiva.Gateway.Messages.Search.V2_0.Dates;
using FreeSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreeSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using NavigationControl = Factiva.Gateway.Messages.Search.V2_0.NavigationControl;
using NavigatorControl = Factiva.Gateway.Messages.Search.V2_0.NavigatorControl;
using RadarNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.RadarNewspageModule;
using SearchCollection = Factiva.Gateway.Messages.Search.V2_0.SearchCollection;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;
using SearchString = Factiva.Gateway.Messages.Search.V2_0.SearchString;
using SearchType = Factiva.Gateway.Messages.Search.V2_0.SearchType;
using StructuredQuery = Factiva.Gateway.Messages.Search.V2_0.StructuredQuery;
using StructuredSearch = Factiva.Gateway.Messages.Search.V2_0.StructuredSearch;
using TimeNavigatorMode = Factiva.Gateway.Messages.Search.V2_0.TimeNavigatorMode;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    /// <summary>
    /// Radar News Page Module Service Result
    /// </summary>
    [DataContract(Name = "radarNewsPageModuleServiceResult", Namespace = "")]
    public class RadarNewsPageModuleServiceResult : Generic.AbstractServiceResult<RadarNewsPageServicePartResult<RadarPackage>, RadarPackage>, IPopulate<RadarNewsPageModuleDataRequest>
    {
        private const int MaxCompanyResultsToReturn = 24;
 
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration &&
                                                    Settings.Default.CacheRadarNewsPageModuleService;
        
        #region IPopulate Members
        
        /// <summary>
        /// Populates the specified control data.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="request">The get request.</param>
        /// <param name="preferences">The preferences.</param>
        public void Populate(ControlData controlData, RadarNewsPageModuleDataRequest request, IPreferences preferences)
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
                        PartResults = GetParts(module, request, controlData, preferences);
                    },
                    preferences);
        }

        #endregion

        internal RadarNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<RadarNewspageModule>(request, controlData, preferences);
            if (module.NewsSubjectCodesCollection == null || module.NewsSubjectCodesCollection.Count < 0 ||
                module.CompanyQueryEntity == null || module.CompanyQueryEntity.QueryCollection.Count < 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleData);
            }

            return module;
        }

        protected internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(RadarNewspageModule module, TimeFrame timeFrame, int firstResult, int maxResults)
           where TSearch : IPerformContentSearchRequest, new()
        {
            var searchText = module.CompanyQueryEntity.QueryCollection.First().Text;
            var searchRequest = new TSearch
            {
                FirstResult = firstResult,
                MaxResults = maxResults,

                // Removed due to getting code names is threw Radar Transactions 
                /*DescriptorControl = new DescriptorControl
                {
                    Mode = DescriptorControlMode.All
                },*/ 
                NavigationControl = new NavigationControl
                {
                    ReturnCollectionCounts = false,
                    ReturnHeadlineCoding = false,
                    CodeNavigatorControl = new CodeNavigatorControl
                    {
                        Mode = CodeNavigatorMode.None,
                        MinBucketValue = 0,
                        MaxBuckets = 0,
                        CustomCollection = new NavigatorControlCollection
                        {
                            new NavigatorControl
                                {
                                    Id = "co", // Id comes from request
                                    MaxBuckets = MaxCompanyResultsToReturn // DataAccessUtilities.NumberofEntitiestoRequest // MaxBuckets should be a constant
                                }
                        }
                    },
                    TimeNavigatorMode = TimeNavigatorMode.None
                },
                StructuredSearch = new StructuredSearch
                {
                    Query = new StructuredQuery
                    {
                        SearchCollectionCollection = new SearchCollectionCollection
                                                         {
                                                             SearchCollection.Publications, 
                                                             SearchCollection.WebSites
                                                         },
                        SearchStringCollection = new SearchStringCollection
                                                     {
                                                         new SearchString
                                                             {
                                                                 Id = "radarFreeText", 
                                                                 Value = searchText, 
                                                                 Type = SearchType.Free, 
                                                                 Mode = SearchMode.Simple, 
                                                                 Filter = false, 
                                                                 Validate = true
                                                             },
                                                        new SearchString
                                                            {
                                                                 Id = "newsubjects", 
                                                                 Value = string.Join(" ", module.NewsSubjectCodesCollection.ToArray()), 
                                                                 Type = SearchType.Controlled, 
                                                                 Mode = SearchMode.Any, 
                                                                 Scope = "ns",
                                                                 Combine = false, 
                                                                 Filter = false, 
                                                                 Validate = true
                                                            }
                                                     },
                        Dates = new Dates
                        {
                            Format = DateFormat.MMDDCCYY,
                            After = SearchManager.GetDate(timeFrame)
                        }
                    }
                }
            };
            return searchRequest;
        }

        /// <summary>
        /// The perform free text search.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="request">The request.</param>
        /// <param name="preferences"></param>
        /// <returns>A list of companies/count </returns>
        protected internal List<Tuple<string, int>> PerformFreeTextSearch(RadarNewspageModule module, RadarNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var response = GetSearchResponse(module, request, preferences);
            return response.ContentSearchResult.CodeNavigatorSet.NavigatorCollection.First().BucketCollection.Select(companyCode => Tuple.Create(companyCode.Id, companyCode.HitCount)).ToList();
        }

        /// <summary>
        /// Gets the package.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="request">The request.</param>
        /// <param name="controlData">The control data.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A <see name="RadarPackage">RadarPackage</see> object.
        /// </returns>
        protected internal List<RadarNewsPageServicePartResult<RadarPackage>> GetParts(RadarNewspageModule module, RadarNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new RadarNewsPageServicePartResult<RadarPackage>();
            var results = new List<RadarNewsPageServicePartResult<RadarPackage>> { partResult };

            ProcessServicePartResult<RadarPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        // Initialize the package
                        /*var package = new RadarPackage
                                          {
                                              ParentNewsEntities = new ParentNewsEntities(),
                                          };
                        */
                        var parentNewsEntities = new ParentNewsEntities();

                        // Perform free text search to get the list of companies
                        var newsSubjects = module.NewsSubjectCodesCollection.ToList();
                        var companiesWithHitCount = PerformFreeTextSearch(module, request, preferences);
                        var newsSubjectsDictionary = GetNewsSubjectsDictionary(newsSubjects, request, controlData, preferences);
                        var screeningRequest = CreateGetCompanyScreeningListExRequest(module, preferences, companiesWithHitCount);

                        //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, Settings.Default.DataAccessProxyUser);
                        var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

                        if (hasCacheBeenEnabled)
                        {
                            var generator = new RadarSearchCompanyScreeningCacheKeyGnerator(request.ModuleId, request.TimeFrame)
                                                {
                                                    CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                                };
                            lightweightProxy = generator.GetCacheControlData(lightweightProxy);
                        }

                        GetCompanyScreeningListExResponse response = null;
                        RecordTransaction(
                            typeof(GetCompanyScreeningListExRequest).FullName,
                            MethodBase.GetCurrentMethod().Name,
                            manager =>
                            {
                                response = manager.Invoke<GetCompanyScreeningListExResponse>(screeningRequest, lightweightProxy).ObjectResponse;
                            },
                            new ModuleDataRetrievalManager(controlData, preferences));

                        ParentNewsEntity entity;
                        foreach (var company in response.CompanyScreeningListResult.Companies.CompanyCollection)
                        {
                            entity = new ParentNewsEntity
                                         {
                                             Code = company.Fcode,
                                             Descriptor = company.CompanyName,
                                             Type = Tools.Ajax.HeadlineList.EntityType.Company, 
                                             NewsEntities = new NewsEntities()
                                         };

                            foreach (var newsdata in company.NewsDataList)
                            {
                                var id = Int32.Parse(newsdata.ID);

                                var code = newsSubjects[id - 1];

                                var temp = new NewsEntity
                                               {
                                                   Type = Tools.Ajax.HeadlineList.EntityType.NewsSubject, 
                                                   SearchContextRef = GenerateSearchContextRef(request, company.Fcode, code.ToLowerInvariant()), 
                                                   Code = code.ToLowerInvariant()
                                               };

                                temp.Descriptor = newsSubjectsDictionary.ContainsKey(temp.Code) ? newsSubjectsDictionary[temp.Code] : temp.Code;
                                temp.CurrentTimeFrameNewsVolume = new WholeNumber(GetHitCountFromNewsData(newsdata, request));
                                entity.NewsEntities.Add(temp);
                            }

                            entity.CurrentTimeFrameNewsVolume = new WholeNumber(entity.NewsEntities.Sum(i => i.CurrentTimeFrameNewsVolume.Value));
                            parentNewsEntities.Add(entity);
                        }

                        // Remove the news entities with zero and reorder the list by size.
                        var tempParentNewsEntities = parentNewsEntities
                            .Where(tempEntity => tempEntity.CurrentTimeFrameNewsVolume.Value > 0)
                            .OrderBy(tempEntity => tempEntity.CurrentTimeFrameNewsVolume.Value).Reverse();

                        var query = from radarEntity in parentNewsEntities
                                    from newsSubject in radarEntity.NewsEntities
                                    select newsSubject.CurrentTimeFrameNewsVolume;

                        var package = new RadarPackage
                                          {
                                              ParentNewsEntities = new ParentNewsEntities(),
                                          };
                        package.ParentNewsEntities.AddRange(tempParentNewsEntities);

                        if (query.Count() > 0)
                        {
                            package.MinHitCount = new WholeNumber(query.Min(n => n.Value));
                            package.MaxHitCount = new WholeNumber(query.Max(n => n.Value));
                        }
                        else
                        {
                            package.MinHitCount = new WholeNumber(0);
                            package.MaxHitCount = new WholeNumber(0);
                        }

                        partResult.Package = package;
                    },
                    preferences);
            return results;
        }

        protected internal Dictionary<string, string> GetNewsSubjectsDictionary(List<string> newsSubjects, RadarNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var dictionary = new Dictionary<string, string>();

            var taxonomyCodesRequest = new GetTaxonomyCodesRequest
                              {
                                  Scheme = TaxonomyScheme.FactivaNewsSubject, 
                                  SchemeToReturn = TaxonomyScheme.FactivaNewsSubject, 
                                  Language = preferences.InterfaceLanguage
                              };
            taxonomyCodesRequest.CodeCollection.AddRange(newsSubjects);
            GetTaxonomyCodesResponse response = null;

            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, Settings.Default.DataAccessProxyUser);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new RadarSearchSymbologyCacheKeyGnerator(request.ModuleId, newsSubjects, preferences.InterfaceLanguage)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            RecordTransaction(
                typeof(GetTaxonomyCodesResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<GetTaxonomyCodesResponse>(taxonomyCodesRequest, lightweightProxy).ObjectResponse;
                },
                new ModuleDataRetrievalManager(controlData, preferences));

            if (response != null)
            {
                foreach (var code in response.TaxonomyCodeResultSet.TaxonomyCodeResultCollection)
                {
                    dictionary.Add(code.RequestedCode.Value.ToLowerInvariant(), code.ResultTaxonomyCodeCollection.First().Descriptor.Value);
                }
            }

            return dictionary;
        }

        private static string GenerateSearchContextRef(RadarNewsPageModuleDataRequest request, string companyCode, string newsSubjectCode)
        {
            var searchContext = new RadarCellSearchContext
            {
                CompanyCode = companyCode,
                NewsSubjectCode = newsSubjectCode,
                TimeFrame = request.TimeFrame
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(RadarCellSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        /// <summary>
        /// The create get company screening list ex request.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="preferences">The preferences.</param>
        /// <param name="companiesWithHitCount">The companies with hit count.</param>
        /// <returns>
        /// Get Company Screening Object.
        /// </returns>
        private static GetCompanyScreeningListExRequest CreateGetCompanyScreeningListExRequest(RadarNewspageModule module, IPreferences preferences, IEnumerable<Tuple<string, int>> companiesWithHitCount)
        {
            var getCompanyScreeningListExRequest = new GetCompanyScreeningListExRequest
            {
                ScreeningContext = null,
                ScreeningCriteriaID = null,
                GetHitCounts = true,
                FirstResultToReturn = 0,
                MaxResultsToReturn = MaxCompanyResultsToReturn,
                SortBy = CompanyProfilesSortBy.NewsMentions,
                SortOrder = CompanyProfilesSortOrder.Descending,
                Language = preferences.InterfaceLanguage,
                DisplayOption = ScreeningListDisplayOptions.CompanyNewsInfo,
                ReturnSeperateNewsDataHits = false,
                IncludeDescriptors = true
            };

            getCompanyScreeningListExRequest.SearchContentSets.SearchContentSetsCollection.AddRange(new[] { SearchContentSet.Publications, SearchContentSet.Web });

            getCompanyScreeningListExRequest.CompanyListOptionalElements.AddRange(new[]
                                                                                      {
                                                                                          CompanyListOptionalElements.None
                                                                                      });
            getCompanyScreeningListExRequest.Currency = "USD";
            getCompanyScreeningListExRequest.RadarViewID = "1";
            getCompanyScreeningListExRequest.SuppressBillingRecords = false;

            getCompanyScreeningListExRequest.CriteriaGroups = new CompanyCriteriaGroups
            {
                CriteriaGroupCollection = new List<CompanyCriteriaGroup>()
            };

            var companyCriteriaGroup = new CompanyCriteriaGroup
            {
                GroupName = "MyTerritory.Fcode",
                CriteriaOperator = CriteriaOperator.OR // GroupID=1,Hits=0
            };

            var companySymbolCriteria = new CompanySymbolCriteria();
            foreach (var symbolCriteria in companiesWithHitCount.Select(symbol => new SymbolCriteria
            {
                SymbolCodeScheme = SymbolCodeScheme.FII,
                Code = symbol.Item1
            }))
            {
                companySymbolCriteria.SymbolCollection.Add(symbolCriteria);
            }

            companyCriteriaGroup.CriteriaCollection.Add(companySymbolCriteria);
            getCompanyScreeningListExRequest.CriteriaGroups.CriteriaGroupCollection.Add(companyCriteriaGroup);
            getCompanyScreeningListExRequest.RadarView = new RadarView_New
            {
                RadarQueryCollection = new List<RadarQuery>()
            };

            var searchText = module.CompanyQueryEntity.QueryCollection.First().Text;
            foreach (var query in module.NewsSubjectCodesCollection.Select(subject => new RadarQuery
                                                                                          {
                                                                                              QueryType = Factiva.Gateway.Messages.Assets.FCP.V1_1.RadarViewQueryType.Custom,
                                                                                              Query =
                                                                                                  {
                                                                                                      searchString = new[]
                                                                                                                         {
                                                                                                                             new Factiva.Gateway.Messages.Search.V1_0.SearchString
                                                                                                                                 {
                                                                                                                                     Value = string.Format("ns:{0} {1}", subject, searchText),
                                                                                                                                 },
                                                                                                                         },
                                                                                                  }
                                                                                          }))
            {
                getCompanyScreeningListExRequest.RadarView.RadarQueryCollection.Add(query);
            }
            
            return getCompanyScreeningListExRequest;
        }

        private static float GetHitCountFromNewsData(NewsData newsdata, RadarNewsPageModuleDataRequest request)
        {
            float hitCount;
            switch (request.TimeFrame)
            {
                case TimeFrame.LastWeek:
                    hitCount = newsdata.Week;
                    break;
                case TimeFrame.LastMonth:
                    hitCount = newsdata.Month;
                    break;
                default:
                    throw new DowJonesUtilitiesException("Invalid Radar Module time frame request");
            }

            return hitCount;
        }

        /// <summary>
        /// The get search response.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="request">The request.</param>
        /// <param name="preferences"></param>
        /// <returns>
        /// A Free Search Response
        /// </returns>
        private FreeSearchResponse GetSearchResponse(RadarNewspageModule module, RadarNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var freeSearchRequest = CreateSearchRequest<FreeSearchRequest>(module, request.TimeFrame, 0, 0);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new RadarSearchCompanyNavigatorCacheKeyGnerator(request.ModuleId, request.TimeFrame)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            FreeSearchResponse response = null;

            RecordTransaction(
                typeof(FreeSearchRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                    {
                        response = manager.Invoke<FreeSearchResponse>(freeSearchRequest).ObjectResponse;
                    },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));
            return response;
        }
    }
}
