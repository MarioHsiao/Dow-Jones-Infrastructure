// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchContextUtility.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.PAM;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.CodedNews;
using Factiva.Gateway.Messages.CodedNews.Search;
using Factiva.Gateway.Messages.Newsstand.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class SearchContextUtility
    {
        #region <<< NEWSSTAND HEADLINES REQUEST >>>
        public static GetMultipleNewsstandSectionHeadlinesRequest CreateNewsstandHeadlinesRequest(string searchContextWrapperString, int firstResult, int maxResults, ControlData controlData, IPreferences preferences)
        {
            var searchContextWrapper = JsonConvert.DeserializeObject<SearchContextWrapper>(searchContextWrapperString);
            var module = GetModule(searchContextWrapper.PageId, searchContextWrapper.ModuleId, null, controlData, preferences);

            return CreateNewsstandHeadlinesRequest(searchContextWrapper, firstResult, maxResults, module, controlData, preferences);
        }

        internal static GetMultipleNewsstandSectionHeadlinesRequest CreateNewsstandHeadlinesRequest(SearchContextWrapper searchContextWrapper, int firstResult, int maxResults, Module module, ControlData controlData, IPreferences preferences)
        {
            if (searchContextWrapper.SearchContextType == typeof(NewsstandSectionSearchContext).Name)
                return CreateNewsstandHeadlinesRequest(NewsstandSectionSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults);

            if (searchContextWrapper.SearchContextType == typeof(NewsstandDiscoveredEntitiesSearchContext).Name)
            {
                return CreateNewsstandHeadlinesRequest(NewsstandDiscoveredEntitiesSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as NewsstandNewspageModule);
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
        }

        private static GetMultipleNewsstandSectionHeadlinesRequest CreateNewsstandHeadlinesRequest(NewsstandSectionSearchContext searchContext, int firstResult, int maxResults)
        {
            return NewsstandNewsPageModuleServiceResult.CreateGetMultipleNewsstandSectionHeadlinesRequest(searchContext.SectionId, firstResult, maxResults);
        }

        private static GetMultipleNewsstandSectionHeadlinesRequest CreateNewsstandHeadlinesRequest(NewsstandDiscoveredEntitiesSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, NewsstandNewspageModule module)
        {
            var newsstandHeadlinesRequest = NewsstandNewsPageModuleServiceResult.CreateGetMultipleNewsstandSectionHeadlinesRequest(module, preferences, firstResult, maxResults);
            
            SearchUtility.ScopeType scopeType;
            switch (searchContext.EntityType)
            {
                case Tools.Ajax.HeadlineList.EntityType.Organization:
                    scopeType = SearchUtility.ScopeType.AnyFDS;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.NewsSubject:
                    scopeType = SearchUtility.ScopeType.AnyNewsSubject;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.Person:
                    scopeType = SearchUtility.ScopeType.AnyPeople;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.Region:
                    scopeType = SearchUtility.ScopeType.AnyRegion;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.Industry:
                    scopeType = SearchUtility.ScopeType.AnyIndustry;
                    break;
                default:
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
            }

            foreach (var sc in newsstandHeadlinesRequest.SectionCollection)
            {
                sc.SearchRequest.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(scopeType, new[] { searchContext.Code }));
            }

            return newsstandHeadlinesRequest;
        }

        #endregion

        #region <<< CODEDSTRUCTUREDREQUEST >>>
        static private Company GetCompany(string fcode, ControlData controlData, IPreferences preferences)
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

            var dataRetrievalManager = new ModuleDataRetrievalManager(controlData, preferences);
            var proxyControlData = ControlDataManager.GetLightWeightUserControlData(Settings.Default.DataAccessProxyUser);
            var tempResponse = dataRetrievalManager.Invoke<GetCompaniesResponse>(getCompaniesRequest, proxyControlData);
            var getCompaniesResponse = tempResponse.ObjectResponse;

            if (getCompaniesResponse.CompanyResultSet.Count > 0)
            {
                if (getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status == null ||
                    getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value == 0)
                {
                    return getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].ResultCompany;
                }

                if (getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value != 0)
                {
                    throw new DowJonesUtilitiesException(getCompaniesResponse.CompanyResultSet.CompanyResultCollection[0].Status.Value);
                }
            }

            throw new DowJonesUtilitiesException();
        }

        public static CodedStructuredSearchRequest CreateCodedStructuredSearchRequest(string searchContextWrapperString, int firstResult, int maxResults, ControlData controlData, IPreferences preferences)
        {
            var searchContextWrapper = JsonConvert.DeserializeObject<SearchContextWrapper>(searchContextWrapperString);
            var module = GetModule(searchContextWrapper.PageId, searchContextWrapper.ModuleId, null, controlData, preferences) as CompanyOverviewNewspageModule;

            if (module == null)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);

            var company = GetCompany(module.FCodeCollection[0], controlData, preferences);
            
            return CreateCodedStructuredSearchRequest(searchContextWrapper, firstResult, maxResults, company, controlData, preferences);
        }

        internal static CodedStructuredSearchRequest CreateCodedStructuredSearchRequest(SearchContextWrapper searchContextWrapper, int firstResult, int maxResults, Company company, ControlData controlData, IPreferences preferences)
        {
            if (searchContextWrapper.SearchContextType == typeof(CompanyOverviewTrendingBubbleSearchContext).Name)
                return CreateCodedStructuredSearchRequest(CompanyOverviewTrendingBubbleSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, company);

            if (searchContextWrapper.SearchContextType == typeof(CompanyOverviewRecentArticlesViewAllSearchContext).Name)
                return CreateCodedStructuredSearchRequest(CompanyOverviewRecentArticlesViewAllSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, company);

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
        }

        private static CodedStructuredSearchRequest CreateCodedStructuredSearchRequest(CompanyOverviewRecentArticlesViewAllSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, Company company)
        {
            return CompanyOverviewNewsPageModuleServiceResult.CreateCodedStructuredSearchRequest(company,
                                                                                                 searchContext.UseCustomDateRange,
                                                                                                 searchContext.StartDate,
                                                                                                 searchContext.EndDate,
                                                                                                 firstResult,
                                                                                                 maxResults,
                                                                                                 preferences);
        }

        private static CodedStructuredSearchRequest CreateCodedStructuredSearchRequest(CompanyOverviewTrendingBubbleSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, Company company)
        {
            var searchRequest = CompanyOverviewNewsPageModuleServiceResult.CreateCodedStructuredSearchRequest(company,
                                                                                                              searchContext.UseCustomDateRange,
                                                                                                              searchContext.StartDate,
                                                                                                              searchContext.EndDate,
                                                                                                              firstResult,
                                                                                                              maxResults,
                                                                                                              preferences);

            if (searchRequest.NewsFilters == null)
                searchRequest.NewsFilters = new NewsFiltersExtended { keywords = new[] { searchContext.Keyword } };
            else
                searchRequest.NewsFilters.keywords = new[] { searchContext.Keyword };
            // TODO: Add keyword filter once Dave adds it to CodedStructuredSearchRequest

            return searchRequest;
        }

        #endregion

        #region <<< IPERFORMCONTENTSEARCH REQUEST >>>
        public static IPerformContentSearchRequest CreateSearchRequest<TSearch> (string searchContextWrapperString, int firstResult, int maxResults, ControlData controlData, IPreferences preferences)
            where TSearch : IPerformContentSearchRequest, new()
        {
            var searchContextWrapper = JsonConvert.DeserializeObject<SearchContextWrapper>(searchContextWrapperString);

            //first, get the module
            var module = GetModule(searchContextWrapper.PageId, searchContextWrapper.ModuleId, null, controlData, preferences);

            return CreateSearchRequest<TSearch>(searchContextWrapper, firstResult, maxResults, preferences, module);
        }

        internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(SearchContextWrapper searchContextWrapper, int firstResult, int maxResults, IPreferences preferences, Module module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            if (searchContextWrapper.SearchContextType == typeof(CustomTopicsViewAllSearchContext).Name)
                return CreateSearchRequest<TSearch>(CustomTopicsViewAllSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as CustomTopicsNewspageModule);

            if (searchContextWrapper.SearchContextType == typeof(RadarCellSearchContext).Name)
                return CreateSearchRequest<TSearch>(RadarCellSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as RadarNewspageModule);

            if (searchContextWrapper.SearchContextType == typeof(RegionalMapBubbleSearchContext).Name)
                return CreateSearchRequest<TSearch>(RegionalMapBubbleSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as RegionalMapNewspageModule);
            
            if (searchContextWrapper.SearchContextType == typeof(SourcesViewAllSearchContext).Name)
                return CreateSearchRequest<TSearch>(SourcesViewAllSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as SourcesNewspageModule);

            if (searchContextWrapper.SearchContextType == typeof(TopNewsViewAllSearchContext).Name)
                return CreateSearchRequest<TSearch>(TopNewsViewAllSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as TopNewsNewspageModule);

            if (searchContextWrapper.SearchContextType == typeof(TrendingItemSearchContext).Name)
                return CreateSearchRequest<TSearch>(TrendingItemSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as TrendingNewsPageModule);

            if (searchContextWrapper.SearchContextType == typeof(SummaryTrendingSearchContext).Name)
                return CreateSearchRequest<TSearch>(SummaryTrendingSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as SummaryNewspageModule);

            if (searchContextWrapper.SearchContextType == typeof(SummaryRegionalMapBubbleSearchContext).Name)
                return CreateSearchRequest<TSearch>(SummaryRegionalMapBubbleSearchContext.FromString(searchContextWrapper.Json), firstResult, maxResults, preferences, module as SummaryNewspageModule);
            //else if for other search contexts
                
            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
        }

        #region <<< SUMMARY TRENDING >>>
        private static IPerformContentSearchRequest CreateSearchRequest<TSearch>(SummaryTrendingSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, SummaryNewspageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            if (module == null)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);

            if (module.QueryEntityCollection.Count <= 0)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);

            var searchRequest = SummaryNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(module, firstResult, maxResults, preferences);

            SearchUtility.ScopeType scopeType;
            switch (searchContext.EntityType)
            {
                case Tools.Ajax.HeadlineList.EntityType.Company:
                    scopeType = SearchUtility.ScopeType.AnyFDS;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.Industry:
                    scopeType = SearchUtility.ScopeType.AnyIndustry;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.NewsSubject:
                    scopeType = SearchUtility.ScopeType.AnyNewsSubject;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.Person:
                    scopeType = SearchUtility.ScopeType.AnyPeople;
                    break;
                case Tools.Ajax.HeadlineList.EntityType.Textual:
                    scopeType = SearchUtility.ScopeType.Keywords;
                    break;
                default:
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
            }

            searchRequest.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(scopeType, new[] { searchContext.Code }));

            searchRequest.StructuredSearch.Formatting = new ResultFormatting
            {
                SortOrder = ResultSortOrder.PublicationDateReverseChronological
            };

            AddContentLanguageSearchString(searchRequest, preferences);

            return searchRequest;
        }
        #endregion

        #region <<< SUMMARY REGIONAL MAP >>>
        private static IPerformContentSearchRequest CreateSearchRequest<TSearch>(SummaryRegionalMapBubbleSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, SummaryNewspageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            if (module == null)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);

            if (module.RegionalMapQueryEntityCollection.Count <= 0)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);

            foreach (var searchRequest in from queryEntity in module.RegionalMapQueryEntityCollection
                                          where queryEntity.RegionFcode == searchContext.Code
                                          select SummaryNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(queryEntity.QueryCollection, firstResult, maxResults))
            {
                AddContentLanguageSearchString(searchRequest, preferences);
                return searchRequest;
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.RegionalMapQueryEntityNotFound);
        }
        #endregion

        #region <<< RADAR >>>
        internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(RadarCellSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, RadarNewspageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            var searchRequest = RadarNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(module, searchContext.TimeFrame, firstResult, maxResults);

            searchRequest.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(SearchUtility.ScopeType.AnyNewsSubject, new[] { searchContext.NewsSubjectCode }));
            searchRequest.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(SearchUtility.ScopeType.AnyFDS, new[] { searchContext.CompanyCode }));

            AddContentLanguageSearchString(searchRequest, preferences);
            return searchRequest;
        }
        #endregion

        #region <<< SOURCES >>>
        internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(SourcesViewAllSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, SourcesNewspageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            var searchRequest = SourcesNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(searchContext.SourceCode, firstResult, maxResults, preferences);

            return searchRequest;
        }
        #endregion

        #region <<< REGIONAL MAP >>>
        static IPerformContentSearchRequest CreateSearchRequest<TSearch>(RegionalMapBubbleSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, RegionalMapNewspageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            if (module == null)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);

            if (module.QueryEntityCollection.Count <= 0)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);

            var searchRequest = RegionalMapNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(module, searchContext.Code, searchContext.TimeFrame, firstResult, maxResults, preferences);

            AddContentLanguageSearchString(searchRequest, preferences);

            return searchRequest;
        }
        #endregion

        #region <<< CUSTOM TOPICS >>>
        private static IPerformContentSearchRequest CreateSearchRequest<TSearch>(CustomTopicsViewAllSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, CustomTopicsNewspageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            if (module == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);
            }

            if (module.CustomTopicCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyCustomTopicCollection);
            }

            return CustomTopicsNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(module, searchContext.CustomTopicIndex, searchContext.CustomTopicName, firstResult, maxResults, preferences);
        }
        #endregion

        #region <<< TRENDING >>>
        static IPerformContentSearchRequest CreateSearchRequest<TSearch>(TrendingItemSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, TrendingNewsPageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            if (module == null)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);

            if (module.QueryEntityCollection.Count <= 0)
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);

            var searchRequest = TrendingNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(
                                                                                        TrendingNewsPageModuleServiceResult.GetSearchStringCollection(module.QueryEntityCollection.First()),
                                                                                        new Dates
                                                                                        {
                                                                                            After = ((int)searchContext.TimeFrame).ToString()
                                                                                        },
                                                                                        searchContext.EntityType,
                                                                                        firstResult,
                                                                                        maxResults,
                                                                                        preferences);
            //// add additional filters based on moduletype/search reference/...
            SearchUtility.ScopeType scopeType;
            switch (searchContext.EntityType)
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
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
            }

            searchRequest.StructuredSearch.Query.SearchStringCollection.Add(SearchUtility.GetSearchStringByScopeType(scopeType, new[] { searchContext.Code }));

            AddContentLanguageSearchString(searchRequest, preferences);

            return searchRequest;
        }
        #endregion

        #region <<< TOPNEWS >>>
        static IPerformContentSearchRequest CreateSearchRequest<TSearch>(TopNewsViewAllSearchContext searchContext, int firstResult, int maxResults, IPreferences preferences, TopNewsNewspageModule module)
            where TSearch : IPerformContentSearchRequest, new()
        {
            if (module == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);
            }

            if (module.QueryEntityCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);
            }

            return TopNewsNewsPageModuleServiceResult.CreateSearchRequest<TSearch>(module, searchContext.TopNewsModulePart, firstResult, maxResults, preferences);
        }
        #endregion
        #endregion

        private static Module GetModule(string pageId, string moduleId, IPageAssetsManager pageAssetsManager, ControlData controlData, IPreferences preferences)
        {
            if (pageAssetsManager == null)
            {
                pageAssetsManager = new ChacheEnabledPageAssetsManagerFactory().CreateManager(controlData, preferences.InterfaceLanguage);
            }

            var module = pageAssetsManager.GetModuleById(pageId, moduleId);

            if (module == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);
            }

            return module;
        }

        private static void AddContentLanguageSearchString(IPerformContentSearchRequest performContentSearchRequest, IPreferences preferences)
        {
            if (preferences != null && preferences.ContentLanguages!= null && preferences.ContentLanguages.Count > 0)
            {
                performContentSearchRequest.StructuredSearch.Query.SearchStringCollection.Add(new SearchString
                {
                    Id = "la",
                    Scope = "la",
                    Type = SearchType.Controlled,
                    Filter = true,
                    Mode = SearchMode.Any,
                    Value = string.Join(" ", preferences.ContentLanguages)
                });
            }
        }
    }
}
