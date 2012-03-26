// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Ajax.TagCloud;
using DowJones.Utilities.Ajax.TagCloud.Converters;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Core;
using DowJones.Utilities.Managers.Search;
using DowJones.Utilities.Search.Controller;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.Models.News;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.CompanyOverview;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using DowJones.Web.Mvc.UI.Models.Company;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.CodedNews;
using Factiva.Gateway.Messages.CodedNews.Search;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;
using Factiva.Gateway.Messages.MarketData.V1_0;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;
using Newtonsoft.Json;
using Company = Factiva.Gateway.Messages.Symbology.Company.V1_0.Company;
using CompanyActiveStatus = Factiva.Gateway.Messages.Symbology.Company.V1_0.CompanyActiveStatus;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using ControlDataManager = DowJones.Utilities.Managers.ControlDataManager;
using ExecutiveCollection = DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.ExecutiveCollection;
using FreeSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreeSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using ListingStatus = Factiva.Gateway.Messages.Symbology.Util.V1_0.ListingStatus;
using Quote = DowJones.Web.Mvc.UI.Models.Company.Quote;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;
using Tag = DowJones.Web.Mvc.Models.News.Tag;
using TagCollection = DowJones.Web.Mvc.Models.News.TagCollection;
using TimePeriod = Factiva.Gateway.Messages.MarketData.V1_0.TimePeriod;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "companyOverviewNewsPageModuleServiceResult", Namespace = "")]
    [KnownType(typeof(CompanyChartPackage))]
    [KnownType(typeof(CompanyTrendingPackage))]
    [KnownType(typeof(CompanySnapshotPackage))]
    [KnownType(typeof(CompanyRecentArticlesPackage))]
    public class CompanyOverviewNewsPageModuleServiceResult : Generic.AbstractServiceResult<CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>, AbstractCompanyPackage>, IPopulate<CompanyOverviewNewsPageModuleDataRequest>, IUpdateDefinitionAndPopulate<CompanyOverviewNewsPageModuleUpdateRequest, CompanyOverviewNewsPageModuleDataRequest>
    {
        private const int PartResultTimeout = 2500;
        private const int PartResultSubTaskTimeout = 2500;
        private const TaskCreationOptions Options = TaskCreationOptions.None;
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration &&
                                                    Settings.Default.CacheCompanyOverviewNewsPageModuleService;

        private DateTimeFormatter dateTimeFormatter;

        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "fcode")]
        public string FCode { get; set; }

        [DataMember(Name = "hasPublicCompanyInformation")]
        public bool HasPublicCompanyInformation { get; set; }

        #region Implementation of IPopulate

        public void Populate(ControlData controlData, CompanyOverviewNewsPageModuleDataRequest request, IPreferences preferences)
        {
            UpdateDefinitionAndPopulate(controlData, null, request, preferences);
        }

        #endregion

        #region Implementation of IUpateDefinitionAndPopulate

        public void UpdateDefinitionAndPopulate(ControlData controlData, CompanyOverviewNewsPageModuleUpdateRequest updateRequest, CompanyOverviewNewsPageModuleDataRequest getRequest, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        if (updateRequest != null && updateRequest.IsValid())
                        {
                            // fire some type of update transaction
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
                        dateTimeFormatter = new DateTimeFormatter(preferences);
                        GetModuleData(getRequest, controlData, preferences);
                    },
                    preferences);
        }

        #endregion

        protected internal static List<string> GetBlackList()
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

        protected internal static CodedStructuredSearchRequest CreateCodedStructuredSearchRequest(Company company, bool useCustomDate, string startDate, string endDate, int firstResult, int maxResults, IPreferences preferences)
        {
            var request = new CodedStructuredSearchRequest
                              {
                                  Formatting = new ResultFormatting
                                                   {
                                                       ClusterMode = ClusterMode.Off,
                                                       DeduplicationMode = DeduplicationMode.NearExact
                                                   },
                                  PreferenceLanguage = preferences.ContentLanguages.ToList(),
                                  CompanyFilters = new[] { new CompanyFilter { Fcode = company.Code, Status = company.CompanyStatus } },
                                  ReturnNavSet = false,
                                  NewsType = CodedNewsType.LatestNews,
                                  OccurrenceSearch = true,
                                  HeadlineStart = firstResult,
                                  HeadlineCount = maxResults,
                              };

            request.Formatting.SortOrder = ResultSortOrder.PublicationDateReverseChronological;

            if (useCustomDate)
            {
                request.DateController = new DateController
                {
                    DateQualifier = DateQualifier.CustomDateRange,
                    DateFormat = Factiva.Gateway.Messages.Search.V2_0.DateFormat.MMDDCCYY,
                    Range = new Factiva.Gateway.Messages.Search.V2_0.DateRange
                    {
                        From = startDate,
                        To = endDate,
                    }
                };
            }
            else
            {
                request.DateController = new DateController
                {
                    DateQualifier = DateQualifier.ThreeMonths,
                    //// Before = 1,
                };
            }

            return request;
        }
        
        protected internal static CodedStructuredSearchRequest GetBaseCodedStructuredSearchRequest(Company company, int firstResult, int maxResults, IPreferences preferences)
        {
            return new CodedStructuredSearchRequest
                       {
                           Formatting = new ResultFormatting
                                           {
                                               ClusterMode = ClusterMode.Off,
                                               DeduplicationMode = DeduplicationMode.NearExact,
                                           },
                           PreferenceLanguage = preferences.ContentLanguages.ToList(),
                           CompanyFilters = new[]
                                                {
                                                    new CompanyFilter
                                                        {
                                                            Fcode = company.Code, Status = company.CompanyStatus
                                                        }
                                                },
                           ReturnNavSet = false,
                           NewsType = CodedNewsType.LatestNews,
                           OccurrenceSearch = true,
                           HeadlineStart = firstResult,
                           HeadlineCount = maxResults,
                       };
        }

        protected internal void GetModuleData(CompanyOverviewNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule(request, controlData, preferences);

            FCode = module.FCodeCollection.FirstOrDefault();
            var company = GetCompany(FCode, request, preferences);

            // update outside properties
            CompanyName = company.ResultCompany.Name.Value;

            var uniqueParts = request.Parts.GetUniques();

            if (company.ResultCompany != null && company.ResultCompany.CompanyStatus != null)
            {
                HasPublicCompanyInformation = company.ResultCompany.CompanyStatus.ListingStatus == ListingStatus.Listed &&
                                          company.ResultCompany.CompanyStatus.ActiveStatus == CompanyActiveStatus.Active;
            }

            PartResults = GetParts(company, uniqueParts, request, controlData, preferences);
            MaxPartsAvailable = 4;
        }

        protected internal CompanyOverviewNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<CompanyOverviewNewspageModule>(request, controlData, preferences);
            if (module.FCodeCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyFCodeCollection);
            }

            return module;
        }

        protected internal CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage> GetCompanySnapShotPackage(Company company, CompanyOverviewNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>();
            ProcessServicePartResult<CompanySnapshotPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    Task<Quote> quoteTask = null;
                    Task<Additionaldata> additionalDataTask = null;

                    var investextReportsTask = TaskFactory.StartNew(() => GetInvestextReports(company, request, preferences), Options);
                    var zacksReportsTask = TaskFactory.StartNew(() => GetZacksReports(company, request, preferences), Options);
                    var dataMonitorReportsTask = TaskFactory.StartNew(() => GetDataMonitorReports(company, request, preferences), Options);
                    var executivesTask = TaskFactory.StartNew(() => GetExecutives(company, request, preferences), Options);

                    if (company.CompanyStatus.ListingStatus == ListingStatus.Listed && company.CompanyStatus.ActiveStatus == CompanyActiveStatus.Active)
                    {
                        quoteTask = TaskFactory.StartNew(() => GetQuote(company, request, controlData, preferences), Options);
                        additionalDataTask = TaskFactory.StartNew(() => GetAdditionalData(company, request, preferences), Options);
                        Task.WaitAll(investextReportsTask, zacksReportsTask, dataMonitorReportsTask, executivesTask, quoteTask, additionalDataTask);
                    }
                    else
                    {
                        Task.WaitAll(investextReportsTask, zacksReportsTask, dataMonitorReportsTask, executivesTask);
                    }
                        
                    var quote = quoteTask == null ? null : quoteTask.Result;
                    var additionalData = additionalDataTask == null ? null : additionalDataTask.Result;
                        
                    if (quote != null && additionalData != null)
                    {
                        quote.MarketCap = additionalData.MarketCap;
                        quote.ListedExchanges.First().Descriptor = additionalData.ExchangeDescriptor;
                    }

                    partResult.Package = new CompanySnapshotPackage
                                                {
                                                    Quote = quote,
                                                    InvestextReports = investextReportsTask.Result,
                                                    ZacksReports = zacksReportsTask.Result,
                                                    DataMonitorReports = dataMonitorReportsTask.Result,
                                                    Executives = executivesTask.Result,
                                                };
                },
                preferences);

            return partResult;
        }

        protected internal CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage> GetCompanyTrendingPackage(Company company, CompanyOverviewNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var partResult = new CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>();
            ProcessServicePartResult<CompanyTrendingPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    partResult.Package = new CompanyTrendingPackage
                    {
                        Result = GetTrendingData(company, request, preferences),
                    };
                },
                preferences);

            return partResult;
        }

        protected internal CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage> GetCompanyRecentArticlesPackage(Company company, CompanyOverviewNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>();
            ProcessServicePartResult<CompanyRecentArticlesPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    partResult.Package = new CompanyRecentArticlesPackage
                    {
                        Result = GetRecentArticles(company, request, controlData, preferences),
                        ViewAllSearchContextRef = GenerateSearchContextRef(request)
                    };
                },
                preferences);

            return partResult;
        }

        protected internal CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage> GetCompanyChartPackage(Company company, AbstractModuleGetRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>();
            ProcessServicePartResult<CompanyChartPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    var stockTask = TaskFactory.StartNew(() => GetMarketNewsData(company, request, controlData, preferences));
                    var newsTask = TaskFactory.StartNew(() => GetNewsChartData(company, request, preferences));
                    Task.WaitAll(new Task[] { stockTask, newsTask }, PartResultSubTaskTimeout);
                    partResult.Package = new CompanyChartPackage 
                    {
                        StockDataResult = stockTask.Result,
                        NewsDataResult = newsTask.Result
                    };
                },
                preferences);

            return partResult;
        }

        protected ExecutiveCollection GetExecutives(Company company, AbstractModuleGetRequest request, IPreferences preferences)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);

            var getReportListRequest = new GetReportListExRequest
            {
                Fcode = company.Code,
                Category = ReportCategory.Company,
                FirstResultToReturn = 0,
                MaxResultsToReturn = 100,
                SortBy = ReportListSortBy.PublishedDate,
                SortOrder = CompanyProfilesSortOrder.Descending,
                SymbolCodeScheme = SymbolCodeScheme.FII,
                ReturnReplyItem = true,
                ReturnRestrictors = false,
                ReturnExistenceFlags = true,
            };

            // update the adoctype
            getReportListRequest.Adoctype.StringCollection.Add("core");

            // analyst reports
            getReportListRequest.Subtype.StringCollection.Add("cogen");

            ////var proxyControldata = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);
            var dataRetrievalManager = new ModuleDataRetrievalManager(lightweightProxy, preferences);
            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewCogenReportList(request.ModuleId, company.Code)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            GetReportListExResponse response = null;
            
            var proxy = lightweightProxy;
            RecordTransaction(
                typeof(GetReportListExRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<GetReportListExResponse>(getReportListRequest, proxy).ObjectResponse;
                },
                dataRetrievalManager);

            if (response != null &&
                response.GetReportListExResult != null &&
                response.GetReportListExResult.ReportListResult != null &&
                response.GetReportListExResult.ReportListResult.Reports != null &&
                response.GetReportListExResult.ReportListResult.Reports.ReportCollection != null &&
                response.GetReportListExResult.ReportListResult.Reports.ReportCollection.Count > 0)
            {
                var primaryCogenRef = string.Empty;

                foreach (var item in response.GetReportListExResult.ReportListResult.Reports.ReportCollection.First().AdocTOC.ItemCollection
                                     .Where(item => item.Subtype.ToLowerInvariant() == "cogen" && item.Value.ToLowerInvariant() == "primary"))
                {
                    primaryCogenRef = item.@ref;
                    break;
                }

                var assetsRequest = new GetAssetsRequest();
                assetsRequest.AccessionNumberCollection.Add(primaryCogenRef);

                //// proxyControldata = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
                lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

                if (hasCacheBeenEnabled)
                {
                    var generator = new CompanyOverviewCogenCacheKeyGenerator(request.ModuleId, company.Code)
                                        {
                                            CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                        };
                    lightweightProxy = generator.GetCacheControlData(lightweightProxy);
                }

                GetAssetsResponse assetsResponse = null;
                RecordTransaction(
                    typeof(GetAssetsRequest).FullName,
                    MethodBase.GetCurrentMethod().Name,
                    manager =>
                    {
                        assetsResponse = manager.Invoke<GetAssetsResponse>(assetsRequest, lightweightProxy).ObjectResponse;
                    },
                    dataRetrievalManager);

                if (assetsResponse.AssetsCollection.Count > 0)
                {
                    // pull out the asset
                    var cogen = assetsResponse.AssetsCollection[0] as CompanyInformationAsset;
                    if (cogen != null &&
                        cogen.AssetData != null &&
                        cogen.AssetData.CompanyInformation != null &&
                        cogen.AssetData.CompanyInformation.KeyExecutiveCollection != null &&
                        cogen.AssetData.CompanyInformation.KeyExecutiveCollection.Count > 0)
                    {
                        var assembler = new ExecutiveAssembler();
                        return new ExecutiveCollection(assembler.Convert(cogen.AssetData.CompanyInformation.KeyExecutiveCollection));
                    }
                }
            }

            return null;
        }

        private static string GenerateSearchContextRef(CompanyOverviewNewsPageModuleDataRequest request, string keyword)
        {
            var searchContext = new CompanyOverviewTrendingBubbleSearchContext
            {
                Keyword = keyword,
                UseCustomDateRange = request.UseCustomDateRange,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(CompanyOverviewTrendingBubbleSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        private static string GenerateSearchContextRef(CompanyOverviewNewsPageModuleDataRequest request)
        {
            var searchContext = new CompanyOverviewRecentArticlesViewAllSearchContext
            {
                UseCustomDateRange = request.UseCustomDateRange,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(CompanyOverviewRecentArticlesViewAllSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        private static CodedStructuredSearchRequest GetDataMonitorSearchRequest(Company company, IPreferences preferences)
        {
            return new CodedStructuredSearchRequest
                       {
                           Formatting = new ResultFormatting
                                           {
                                               ClusterMode = ClusterMode.Off,
                                               DeduplicationMode = DeduplicationMode.Off
                                           },
                           PreferenceLanguage = preferences.ContentLanguages.ToList(),
                           DateController = new DateController
                                                {
                                                    DateQualifier = DateQualifier.LastYear
                                                },
                           CompanyFilters = new[]
                                                {
                                                    new CompanyFilter { Fcode = company.Code, Status = company.CompanyStatus }
                                                },
                           ReturnNavSet = false,
                           NewsType = CodedNewsType.ReportAll,
                           OccurrenceSearch = true,
                           HeadlineStart = 0,
                           HeadlineCount = 2,
                       };
        }

        private HistoricalNewsDataResult GetNewsChartData(Company company, AbstractModuleGetRequest request, IPreferences preferences)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);

            var codedSearchRequest = CreateCodedStructuredSearchRequest(company, false, string.Empty, string.Empty, 0, 0, preferences);
            codedSearchRequest.ReturnNavSet = true;
            codedSearchRequest.MetaDataController = new MetaDataController
            {
                TimeNavigatorMode = TimeNavigatorMode.PublicationDate,
                ReturnCollectionCounts = false,
                ReturnKeywordsSet = false,
                Mode = CodeNavigatorMode.None
            };
            
            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
            var lightweightUser = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewNewsChartCacheKeyGenerator(request.ModuleId, company.Code)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightUser = generator.GetCacheControlData(lightweightUser);
            }

            var searchManager = new SearchManager(lightweightUser, preferences.InterfaceLanguage);

            // Record the Transaction
            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(FreeSearchRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.PerformCodedStructuredSearch<FreeSearchRequest, FreeSearchResponse>(codedSearchRequest);
                },
                searchManager);

            var assembler = new HistoricalNewsDataResultAssembler(dateTimeFormatter);
            return response.ContentSearchResult.TimeNavigatorSet.Count == 1 ? assembler.Convert(response.ContentSearchResult.TimeNavigatorSet.NavigatorCollection[0]) : null;
        }

        private TagCollection GetTrendingData(Company company, CompanyOverviewNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var codedSearchRequest = CreateCodedStructuredSearchRequest(company, request.UseCustomDateRange, request.StartDate, request.EndDate, 0, 0, preferences);
            codedSearchRequest.KeywordBlackList = GetBlackList();

            codedSearchRequest.MetaDataController = new MetaDataController
            {
                TimeNavigatorMode = TimeNavigatorMode.None,
                ReturnCollectionCounts = false,
                ReturnKeywordsSet = true,
                Mode = CodeNavigatorMode.None
            };

            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
            var lightweightUser = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewTrendingCacheKeyGenerator(request.ModuleId, company.Code)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightUser = generator.GetCacheControlData(lightweightUser);
            }

            var searchManager = new SearchManager(lightweightUser, preferences.InterfaceLanguage);
            
            // Record the Transaction
            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(FreeSearchRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                    {
                        response = manager.PerformCodedStructuredSearch<FreeSearchRequest, FreeSearchResponse>(codedSearchRequest);
                    }, 
                searchManager);

            var assembler = new KeywordSetTagConverter(response.ContentSearchResult.KeywordSet);
            var rules = new TagCloudGenerationRules();
            var tagCollection = new TagCollection(assembler.Process<Tag>(rules));
            foreach (var tag in tagCollection)
            {
                tag.SearchContextRef = GenerateSearchContextRef(request, tag.Text);
            }

            return tagCollection;
        }
        
        private IEnumerable<CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>> GetParts(CompanyResult company, IList<CompanyOverviewParts> uniqueParts, CompanyOverviewNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            // if only one part keep it on the same thread););
            if (uniqueParts.Count == 1)
            {
                switch (uniqueParts[0])
                {
                    case CompanyOverviewParts.Chart:
                        return new[] { GetCompanyChartPackage(company.ResultCompany, request, controlData, preferences) };
                    case CompanyOverviewParts.RecentArticles:
                        return new[] { GetCompanyRecentArticlesPackage(company.ResultCompany, request, controlData, preferences) };
                    case CompanyOverviewParts.SnapShot:
                        return new[] { GetCompanySnapShotPackage(company.ResultCompany, request, controlData, preferences) };
                    case CompanyOverviewParts.Trending:
                        return new[] { GetCompanyTrendingPackage(company.ResultCompany, request, preferences) };
                }
            }
            else
            {
                var tasks = new List<Task<CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>>>();
                foreach (var part in uniqueParts)
                {
                    switch (part)
                    {
                        case CompanyOverviewParts.Chart:
                            tasks.Add(TaskFactory.StartNew(() => GetCompanyChartPackage(company.ResultCompany, request, controlData, preferences)));
                            break;
                        case CompanyOverviewParts.RecentArticles:
                            tasks.Add(TaskFactory.StartNew(() => GetCompanyRecentArticlesPackage(company.ResultCompany, request, controlData, preferences)));
                            break;
                        case CompanyOverviewParts.SnapShot:
                            tasks.Add(TaskFactory.StartNew(() => GetCompanySnapShotPackage(company.ResultCompany, request, controlData, preferences)));
                            break;
                        case CompanyOverviewParts.Trending:
                            tasks.Add(TaskFactory.StartNew(() => GetCompanyTrendingPackage(company.ResultCompany, request, preferences)));
                            break;
                    }
                }

                Task.WaitAll(tasks.ToArray(), PartResultTimeout);
                return tasks.Select(task => task.Result).ToList();
            }

            return null;
        }

        private PortalHeadlineListDataResult GetZacksReports(Company company, AbstractModuleGetRequest request, IPreferences preferences)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);

            var getReportListRequest = new GetReportListExRequest
                                           {
                                               Fcode = company.Code,
                                               Category = ReportCategory.Company,
                                               Adoctype = new ArrayOfString(),
                                               Subtype = new ArrayOfString(),
                                               ReportListOptionalCompanyElements = new ArrayOfReportListOptionalCompanyElements(),
                                               FirstResultToReturn = 0,
                                               MaxResultsToReturn = 2,
                                               SortBy = ReportListSortBy.PublishedDate,
                                               SortOrder = CompanyProfilesSortOrder.Descending,
                                               SymbolCodeScheme = SymbolCodeScheme.FII,
                                               ReturnReplyItem = true,
                                               ReturnRestrictors = false,
                                               ReturnExistenceFlags = true,
                                           };

            // update the adoctype
            getReportListRequest.Adoctype.StringCollection.Add("analyst");

            // analyst reports
            getReportListRequest.Subtype.StringCollection.Add("zachs");

            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);
            var dataRetrievalManager = new ModuleDataRetrievalManager(lightweightProxy, preferences);

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewReportListCacheKeyGenerator(request.ModuleId, company.Code, CompanyOverviewReportListCacheKeyGenerator.ReportType.Zacks)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            GetReportListExResponse response = null;
            RecordTransaction(
                typeof(GetReportListExRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<GetReportListExResponse>(getReportListRequest, lightweightProxy).ObjectResponse;
                },
                dataRetrievalManager);
            
            var assembler = new PortalHeadlineListDataResultAssembler(dateTimeFormatter);
            if (response != null &&
                response.GetReportListExResult != null &&
                response.GetReportListExResult.ReportListResult != null &&
                response.GetReportListExResult.ReportListResult.Reports != null &&
                response.GetReportListExResult.ReportListResult.Reports.ReportCollection != null &&
                response.GetReportListExResult.ReportListResult.Reports.ReportCollection.Count > 0)
            {
                return assembler.Convert(response.GetReportListExResult.ReportListResult.Reports.ReportCollection, PortalHeadlineListDataResultAssembler.ReportTypeEx.Zacks);
            }

            return assembler.Convert(null, PortalHeadlineListDataResultAssembler.ReportTypeEx.Zacks);
        }

        private PortalHeadlineListDataResult GetInvestextReports(Company company, AbstractModuleGetRequest request, IPreferences preferences)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);

            var getReportListRequest = new GetReportListExRequest
                                           {
                                               Fcode = company.Code,
                                               Category = ReportCategory.Company,
                                               LanguageList = string.Join(" ", preferences.ContentLanguages),
                                               Adoctype = new ArrayOfString(),
                                               Subtype = new ArrayOfString(),
                                               ReportListOptionalCompanyElements = new ArrayOfReportListOptionalCompanyElements(),
                                               FirstResultToReturn = 0,
                                               MaxResultsToReturn = 2,
                                               SortBy = ReportListSortBy.PublishedDate,
                                               SortOrder = CompanyProfilesSortOrder.Descending,
                                               SymbolCodeScheme = SymbolCodeScheme.FII,
                                               ReturnReplyItem = true,
                                               ReturnRestrictors = false,
                                               ReturnExistenceFlags = true,
                                           };
            
            // update the adoc-type
            getReportListRequest.Adoctype.StringCollection.Add("analyst");

            // analyst reports
            getReportListRequest.Subtype.StringCollection.Add("invest");

            // get user credentials
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewReportListCacheKeyGenerator(request.ModuleId, company.Code, CompanyOverviewReportListCacheKeyGenerator.ReportType.Investext)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            GetReportListExResponse response = null;
            RecordTransaction(
                typeof(GetReportListExRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<GetReportListExResponse>(getReportListRequest, lightweightProxy).ObjectResponse;
                },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));

            var assembler = new PortalHeadlineListDataResultAssembler(dateTimeFormatter);
            if (response != null &&
                 response.GetReportListExResult != null &&
                 response.GetReportListExResult.ReportListResult != null &&
                 response.GetReportListExResult.ReportListResult.Reports != null &&
                 response.GetReportListExResult.ReportListResult.Reports.ReportCollection != null &&
                 response.GetReportListExResult.ReportListResult.Reports.ReportCollection.Count > 0)
            {
                return assembler.Convert(response.GetReportListExResult.ReportListResult.Reports.ReportCollection, PortalHeadlineListDataResultAssembler.ReportTypeEx.Investext);
            }

            return assembler.Convert(null, PortalHeadlineListDataResultAssembler.ReportTypeEx.Investext);
        }

        private PortalHeadlineListDataResult GetDataMonitorReports(Company company, CompanyOverviewNewsPageModuleDataRequest request, IPreferences preferences)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);
            var codedSearchRequest = GetDataMonitorSearchRequest(company, preferences);

            // get user credentials
            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
            var lightweightUser = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewDataMonitorCacheKeyGenerator(request.ModuleId, company.Code)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightUser = generator.GetCacheControlData(lightweightUser);
            }

            var searchManager = new SearchManager(lightweightUser, preferences.InterfaceLanguage);

            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(FreeSearchRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.PerformCodedStructuredSearch<FreeSearchRequest, FreeSearchResponse>(codedSearchRequest);
                },
                searchManager);

            var conversionManager = new HeadlineListConversionManager(dateTimeFormatter);
            return conversionManager.Process(response).Convert(request.TruncationType);
        }

        private CompanyResult GetCompany(string fcode, AbstractModuleGetRequest request, IPreferences preferences)
        {
            var elements = new List<CompanyElements>
                                  {
                                      CompanyElements.AllCodes,
                                      CompanyElements.Status,
                                      CompanyElements.FactivaTaxonomyCodes
                                  };

            var getCompaniesRequest = new GetCompaniesRequest();
            getCompaniesRequest.CodeCollection.Add(fcode);
            getCompaniesRequest.CodeType = CompanyCodeType.FactivaCompany;
            getCompaniesRequest.ElementsToReturnCollection.AddRange(elements);
            getCompaniesRequest.Language = preferences.InterfaceLanguage;

            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewSymbologyCacheKeyGenerator(request.ModuleId, fcode, preferences.InterfaceLanguage)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            GetCompaniesResponse getCompaniesResponse = null;
            RecordTransaction(
                typeof(GetCompaniesRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                    {
                        getCompaniesResponse = manager.Invoke<GetCompaniesResponse>(getCompaniesRequest, lightweightProxy).ObjectResponse;
                    },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));

            if (getCompaniesResponse.CompanyResultSet.Count > 0)
            {
                if (getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status == null ||
                    getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value == 0)
                {
                    return getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0];
                }

                if (getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value != 0)
                {
                    throw new DowJonesUtilitiesException(getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value);
                }
            }

            throw new DowJonesUtilitiesException();
        }

        private PortalHeadlineListDataResult GetRecentArticles(Company company, CompanyOverviewNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var codedSearchRequest = CreateCodedStructuredSearchRequest(
                company, 
                request.UseCustomDateRange, 
                request.StartDate, 
                request.EndDate, 
                request.FirstResultToReturn, 
                request.MaxResultsToReturn, 
                preferences);
            
            if (hasCacheBeenEnabled &&
                request.FirstResultToReturn == 0 && 
                !request.UseCustomDateRange)
            {
                var generator = new CompanyOverviewRecentHeadlinesCacheKeyGenerator(request.ModuleId, company.Code, request.MaxResultsToReturn, preferences.ContentLanguages.ToArray())
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                controlData = generator.GetCacheControlData(controlData);
            }

            var searchManager = new SearchManager(controlData, preferences.InterfaceLanguage);

            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(FreeSearchRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                    {
                        response = manager.PerformCodedStructuredSearch<FreeSearchRequest, FreeSearchResponse>(codedSearchRequest);
                    },
                searchManager);

            var conversionManager = new HeadlineListConversionManager(dateTimeFormatter);
            return conversionManager.Process(response).Convert(request.TruncationType);
        }
        
        private HistoricalStockDataResult GetMarketNewsData(Company company,  AbstractModuleGetRequest request, ControlData controlData, IPreferences preferences)
        {
            if (company.CompanyStatus.ListingStatus == ListingStatus.Listed &&
                company.CompanyStatus.ActiveStatus == CompanyActiveStatus.Active)
            {
                var historicalDataByTimePeriodRequest = new GetHistoricalDataByTimePeriodRequest
                                  {
                                      timePeriod = TimePeriod.ThreeMonths,
                                      frequency = DataPointFrequency.Daily,
                                      symbols = new[] { company.PrimaryDowJonesTicker },
                                      Usage = UsageType.InteractiveChart,
                                      CodeScheme = CodeScheme.DJ
                                  };

                //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
                
                var dataRetrievalManager = new ModuleDataRetrievalManager(controlData, preferences);
                var tempControlData = dataRetrievalManager.ControlData;
                if (hasCacheBeenEnabled)
                {
                    var generator = new CompanyOverviewHistoricalMarketDataCacheKeyGenerator(request.ModuleId, company.Code)
                                        {
                                            CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                        };
                    tempControlData = generator.GetCacheControlData(tempControlData);
                }

                //// Record the transactions
                GetHistoricalDataByTimePeriodResponse response = null;
                RecordTransaction(
                    typeof(GetHistoricalDataByTimePeriodResponse).FullName,
                    MethodBase.GetCurrentMethod().Name,
                    manager =>
                    {
                        try
                        {
                            response = manager.Invoke<GetHistoricalDataByTimePeriodResponse>(historicalDataByTimePeriodRequest, tempControlData).ObjectResponse;
                        }
                        catch (DowJonesUtilitiesException dex)
                        {
                            // 120027 means User does not have access to MDS Services, just return null for market data chart part instead of erroring out the whole package
                            if (dex.ReturnCode != 120027)
                                throw;
                        }
                    },
                    dataRetrievalManager);

                var assembler = new HistoricalStockDataResultAssembler(dateTimeFormatter);
                return response != null ? assembler.Convert(response, company) : null;
            }

            return null;
        }

        private Additionaldata GetAdditionalData(Company company, AbstractModuleGetRequest request, IPreferences preferences)
        {
            var companyScreeningListExRequest = new GetCompanyScreeningListExRequest();
            var symbolCriteria = new CompanySymbolCriteria();
            symbolCriteria.SymbolCollection.Add(new SymbolCriteria
                                                    {
                                                        Code = company.Code,
                                                        SymbolCodeScheme = SymbolCodeScheme.FII,
                                                    });

             var group = new CompanyCriteriaGroup();
            group.CriteriaCollection.Add(symbolCriteria);

            companyScreeningListExRequest.CriteriaGroups.CriteriaGroupCollection.Add(group);
            companyScreeningListExRequest.MaxResultsToReturn = 1;
            companyScreeningListExRequest.DisplayOption = ScreeningListDisplayOptions.StandardPlusCustom;
            companyScreeningListExRequest.CompanyListOptionalElements.Add(CompanyListOptionalElements.MarketCap);
            companyScreeningListExRequest.CompanyListOptionalElements.Add(CompanyListOptionalElements.MostRecentClose);
            companyScreeningListExRequest.IncludeDescriptors = true;
            companyScreeningListExRequest.Language = preferences.InterfaceLanguage;

            // Get the credentials 
            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
            var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewScreeningCacheKeyGenerator(request.ModuleId, company.Code, preferences.InterfaceLanguage)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                lightweightProxy = generator.GetCacheControlData(lightweightProxy);
            }

            // Record the Transaction
            GetCompanyScreeningListExResponse response = null;
            RecordTransaction(
                typeof(GetCompanyScreeningListExResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<GetCompanyScreeningListExResponse>(companyScreeningListExRequest, lightweightProxy).ObjectResponse;
                },
                new ModuleDataRetrievalManager(lightweightProxy, preferences));

            if (response == null || 
                response.CompanyScreeningListResult == null || 
                response.CompanyScreeningListResult.Companies == null || 
                response.CompanyScreeningListResult.Companies.Count == 0 ||
                response.CompanyScreeningListResult.Companies.CompanyCollection == null ||
                response.CompanyScreeningListResult.Companies.CompanyCollection.Count == 0)
            {
                return new Additionaldata { MarketCap = new DoubleMoney((long?)0) };
            }

            var tempCompany = response.CompanyScreeningListResult.Companies.CompanyCollection.First();
            var additionalData = new Additionaldata
                                     {
                                         MarketCap = new DoubleMoney((long?)tempCompany.MarketCap, tempCompany.Currency),
                                         Exchange = tempCompany.PrimaryExchange,
                                         ExchangeDescriptor = tempCompany.PrimaryExchangeDescriptor,
                                     };
            return additionalData;
        }

        private Quote GetQuote(Company company, AbstractModuleGetRequest request, ControlData controlData, IPreferences preferences)
        {
            LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);
            var quoteRequest = new GetQuoteRequest
                              {
                                  quickQuote = false, 
                                  codeScheme = "ric", 
                                  symbols = new[] { company.PrimaryRIC }
                              };

            // Get the credentials 
            //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, ProxyUser);
           
            var tempControlData = controlData;

            if (hasCacheBeenEnabled)
            {
                var generator = new CompanyOverviewQuoteCacheKeyGenerator(request.ModuleId, company.Code, preferences.InterfaceLanguage)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                tempControlData = generator.GetCacheControlData(tempControlData);
            }

            // Record the Transaction
            GetQuoteResponse response = null;
            RecordTransaction(
                typeof(GetQuoteRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                    {
                        response = manager.Invoke<GetQuoteResponse>(quoteRequest, tempControlData).ObjectResponse;
                    },
                new ModuleDataRetrievalManager(controlData, preferences));

            if (response == null || response.quoteResponse == null || response.quoteResponse.quoteResultSet == null || response.quoteResponse.quoteResultSet.quote == null || response.quoteResponse.quoteResultSet.quote.Length <= 0)
            {
                return null;
            }

            // Assemble the model
            var assembler = new QuoteAssembler(dateTimeFormatter);
            var quote = assembler.Convert(response.quoteResponse.quoteResultSet.quote[0], company);

            if (quote == null)
            {
                return null;
            }

            quote.FCode = company.Code;
            quote.CompanyName = company.Name.Value;
            return quote;
        }

        internal class Additionaldata
        {
            internal DoubleMoney MarketCap { get; set; }

            internal string Exchange { get; set; }

            internal string ExchangeDescriptor { get; set; }
        }
    }
}
