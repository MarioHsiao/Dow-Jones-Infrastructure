using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Ajax.CompanySparkline;
using DowJones.Articles;
using DowJones.Assemblers.Articles;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Generators;
using DowJones.Managers.RelatedConcept;
using DowJones.Managers.Search;
using DowJones.Managers.Sparkline;
using DowJones.Models.Search;
using DowJones.Search;
using DowJones.Search.Filters;
using DowJones.Web.Mvc.Search.Requests;
using DowJones.Web.Mvc.Search.Requests.Alert;
using DowJones.Web.Mvc.Search.Requests.Article;
using DowJones.Web.Mvc.Search.Requests.Common;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.Search.Resources;
using DowJones.Web.Mvc.Search.Results;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using DowJones.Web.Mvc.Search.UI.Components.Builders.Simple;
using DowJones.Web.Mvc.Search.UI.Components.Filters;
using DowJones.Web.Mvc.Search.UI.Components.Results;
using DowJones.Web.Mvc.Search.UI.Components.Results.Headlines;
using DowJones.Web.Mvc.Search.ViewModels;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.CompositeHeadline;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Mvc.UI.Components.Models.CompanySparkline;
using DowJones.Web.Mvc.UI.Components.Models.Discovery;
using DowJones.Web.Mvc.UI.Components.Models.RelatedConcepts;

namespace DowJones.Web.Mvc.Search.Controllers
{
    public abstract class SearchControllerBase : ControllerBase
    {
        #region SERVICES

        [Inject("Avoiding constructor injection in abstract class")]
        protected IArticleService ArticleService { get; set; }

        [Inject("Avoiding constructor injection in abstract class")]
        protected ISearchBuilderFactory SearchBuilderFactory { get; set; }

        [Inject("Avoiding constructor injection in abstract class")]
        protected ISearchService SearchService { get; set; }


        [Inject("Avoiding constructor injection in abstract class")]
        protected ISparklineService SparklineService { get; set; }

        [Inject("Avoiding constructor injection in abstract class")]
        protected IRelatedConceptService RelatedConceptService { get; set; }

        [Inject("Avoiding constructor injection in abstract class")]
        protected ISearchRequestDefaultSettingFactory SearchRequestDefaultSettingFactory { get; set; }

        [Inject("Avoiding constructor injection in abstract class")]
        protected ArticleConversionManager ArticleConversionManger { get; set; }

        private static readonly string[] ExcludeNavigatorForUncodedContentSearch = new[] { "Industry", "Subject", "Region" };

        public virtual ActionResult Articles(ArticlesRequest articlesRequest, bool showPostProcessing = true, bool showSourceLinks = true, bool showAuthorLinks = true)
        {

            ArticleConversionManger.ShowCompanyEntityReference = true;
            ArticleConversionManger.ShowExecutiveEntityReference = true;
            ArticleConversionManger.EnableELinks = true;
            ArticleConversionManger.PictureSize = articlesRequest.PictureSize;

            var requestArt = articlesRequest.MapToMixedContentArticleRequest();

            var response = ArticleService.GetArticles(requestArt);

            if (articlesRequest.IsValid)
            {
                var postProcessOptions = new List<PostProcessingOptions>();
                postProcessOptions.AddRange(new [] {PostProcessingOptions.Save, PostProcessingOptions.PrintLabel, PostProcessingOptions.Email, });
                if (UserContext.Principle.CoreServices.AlertsService.HasPressClipsEnabled)
                {
                    postProcessOptions.Add(PostProcessingOptions.PressClips);
                }
                var articlesComponent = new ArticlesModel( response, ArticleConversionManger )
                                            {
                                                ID = RandomKeyGenerator.GetRandomKey(4, RandomKeyGenerator.CharacterSet.Numeric),
                                                ArticleDisplayOptions = articlesRequest.Options,
                                                ShowPostProcessing = true,
                                                ShowSourceLinks = true,
                                                ShowAuthorLinks = true,
                                                PostProcessingOptions = postProcessOptions,
                                            };
                return ViewComponent(articlesComponent);
            }
            //TODO: define new error code
            throw new DowJonesUtilitiesException("Missing article id in request.");
        }

        public virtual ActionResult Sparklines(SparklineServiceRequest request)
        {
            IEnumerable<SparklineDataSet> sparklines = SparklineService.GetData(request);

            IEnumerable<CompanySparklineDataResult> companySparklines =
                sparklines.Select(Mapper.Map<CompanySparklineDataResult>);

            return Json(companySparklines, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Related(string simpleSearchText, int maxTerms)
        {
            ConceptSearchResult conceptSearchResult = RelatedConceptService.PerformConceptSearch(simpleSearchText,
                                                                                                 maxTerms);

            var concepts = Mapper.Map<RelatedConceptsDataResult>(conceptSearchResult);

            return Json(concepts, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SEARCH ACTION

        public virtual ActionResult Results(SearchRequest request, SearchResultsOptions options)
        {
            request = SearchRequestDefaultSettingFactory.ApplyDefaults(request);
            options = SearchRequestDefaultSettingFactory.ApplyDefaults(options);

            var query = Mapper.Map<AbstractBaseSearchQuery>(request);

            SearchResultsViewModel model = ExecuteQuery(query, request, options);
            model.RelatedConceptsServiceUrl = Url.Action("Related");
            model.SearchResultsServiceUrl = Url.Action("Results");
            
            if (Request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            RegisterStartupScript();
            return View("Results", model);
        }

        #endregion

        #region PROTECTED VIRTUAL METHODs TO EXTEND RESULT FUNCTIONALITY 

        protected virtual SearchResultsViewModel ExecuteQuery(AbstractBaseSearchQuery query, SearchRequest request,
                                                              SearchResultsOptions options)
        {
            //Execute service
            SearchResponse searchResponse = PerformSearch(query);

            //Resolve entity name
            var nameResolver = new QueryFilterNameResolver(request);
            SearchFilters filters = ResolveEntityFilterName(query, request, nameResolver);

            //Build search result model
            SearchResults searchResults = GetSearchResultsModel(searchResponse, request, options);

            ResultsLayout layout = options.Layout.GetValueOrDefault(ResultsLayout.Full);
            //Build navigator model
            SearchNavigator sourcesFilter = GetSearchNavigatorModel(searchResponse, layout);

            //Build search query builder
            SearchBuilder searchBuilder = SearchBuilderFactory.GetSearchBuilder(searchResponse, Product.SourceGroupConfigurationId, nameResolver, request);
            //searchBuilder.SearchBuilderContext = request.ToJson();

            searchBuilder.SearchBuilderContext = request;

            if (searchResponse.Query is SimpleSearchQuery)
            {
                var SimpleSearchBuilder = ((SimpleSearchBuilder) searchBuilder);
                if(SimpleSearchBuilder.DidYouMean != null)
                {
                    SimpleSearchBuilder.DidYouMean.HideEntities = request.HideDYM;
                }
            }

            //Build search result view model
            return new SearchResultsViewModel
                       {
                           CompanyProfiles = GetCompaniesSparklinesComponentModel(query),
                           Filters = filters,
                           Navigator = sourcesFilter,
                           Results = searchResults,
                           SearchBuilder = searchBuilder,
                           Layout = layout
                       };
        }

        protected virtual SearchResponse PerformSearch(AbstractBaseSearchQuery query)
        {
            return SearchService.PerformSearch(query);
        }

        protected virtual SearchFilters ResolveEntityFilterName(AbstractBaseSearchQuery query, SearchRequest request,
                                                                QueryFilterNameResolver nameResolver)
        {
            QueryFilters queryFilters = Mapper.Map<SearchQueryFilters, QueryFilters>(query.Filters);

            SearchFilters filters = Mapper.Map<QueryFilters, SearchFilters>(queryFilters);

            if (filters != null && filters.HasFilters)
            {
                nameResolver.ResolveNames(filters.Filters);
            }

            return filters;
        }

        protected virtual SearchNavigator GetSearchNavigatorModel(SearchResponse searchResponse, ResultsLayout layout)
        {
            var sourcesFilter = Mapper.Map<SearchNavigator>(searchResponse);
            if (IsAbstractSearchQuery(searchResponse.Query))
            {
                IEnumerable<InclusionFilter> inclusion = ((AbstractSearchQuery)searchResponse.Query).Inclusions;

                if (inclusion != null && inclusion.Contains(InclusionFilter.SocialMedia))
                {
                    var list = sourcesFilter.EntityFilters.ToList();
                    IList<SearchNavigatorNode> finalList = list.Where(node => !ExcludeNavigatorForUncodedContentSearch.Contains(node.GroupCode)).ToList();
                    sourcesFilter.EntityFilters = finalList;
                }
            }
            sourcesFilter.Layout = layout;
            return sourcesFilter;
        }

        protected virtual CompaniesSparklinesComponentModel GetCompaniesSparklinesComponentModel(
            AbstractBaseSearchQuery query)
        {
            var companyProfiles = new CompaniesSparklinesComponentModel();
            if (query.Filters != null && query.Filters.Company != null)
            {
                companyProfiles = new CompaniesSparklinesComponentModel(query.Filters.Company);
            }
            return companyProfiles;
        }

        protected virtual SearchResults GetSearchResultsModel(SearchResponse searchResponse, SearchRequest request,
                                                              SearchResultsOptions options)
        {
            var startIndexReference = new List<int> {0};
            int pageIndex = 0;
            int pageSize = request.PageSize.GetValueOrDefault(20);
            if (!string.IsNullOrEmpty(request.StartIndexReference))
            {
                string[] arr = request.StartIndexReference.Split(',');
                startIndexReference = arr.Select(Int32.Parse).ToList();
                int start = (request.Start != null) ? request.Start.Value : 0;
                pageIndex = startIndexReference.IndexOf(start);
                if (pageIndex > 0)
                {
                    startIndexReference.RemoveRange(pageIndex + 1, startIndexReference.Count - (pageIndex + 1));
                }
                pageIndex = startIndexReference.IndexOf(start);
                if (pageIndex < 0)
                {
                    startIndexReference.Add(start);
                    pageIndex = startIndexReference.Count - 1;
                }
            }

            var searchResults = new HeadlineSearchResults
                                    {
                                        ArticleUrl = Url.Action("Articles"),
                                        Headlines = Mapper.Map<CompositeHeadlineModel>(searchResponse),
                                        Server = searchResponse.ContentServerAddress,
                                        ContextId = searchResponse.ContextId,
                                        ArticleDisplayOption = options.ArticleDisplayOption ?? DisplayOptions.Full,
                                        HeadlineSort = request.Sort ?? SortOrder.PublicationDateMostRecentFirst,
                                        Layout = options.Layout.GetValueOrDefault(ResultsLayout.Full),
                                        PictureSize = SearchRequestDefaultSettingFactory.PictureSize
                                    };

            if (searchResponse.Results != null)
            {
                searchResults.NewsVolume = new DateHistogramModel(searchResponse.Histogram);
                if (searchResponse.Response != null && searchResponse.Response.ContentSearchResult != null && SearchRequestDefaultSettingFactory.HighlightSearchTermInDocument)
                {
                   searchResults.CanonicalQueryString =  searchResponse.Response.ContentSearchResult.CanonicalQueryString;
                }
                searchResults.PageSize = pageSize;
                searchResults.FirstResultIndexReference = startIndexReference.ToArray();
                searchResults.PageIndex = pageIndex;
                searchResults.FirstResultIndex = Convert.ToInt32(searchResponse.Results.resultSet.first.Value);
                searchResults.TotalResultCount = Convert.ToInt32(searchResponse.Results.hitCount.Value);
                searchResults.DuplicateCount = Convert.ToInt32(searchResponse.Results.resultSet.duplicateCount.Value);

                searchResults.HideNewsVolume = request.HideNewsVolume;
            }
            searchResults.ArticleDisplayOptions = new EnumSelectListWithTranslatedText<DisplayOptions>(searchResults.ArticleDisplayOption);
            searchResults.ShowDuplicates = request.ShowDuplicates ?? ShowDuplicates.On;

            if (searchResults.Headlines != null)
            {
                searchResults.Headlines.EnableDuplicateOption = true;

                var headlineListModel = searchResults.Headlines.HeadlineList;
                if (headlineListModel != null)
                {
                    headlineListModel.DisplaySnippets = SearchRequestDefaultSettingFactory.DisplaySnippets;
                    headlineListModel.ShowAccessionNo = SearchRequestDefaultSettingFactory.ShowAccessionNo;
                    headlineListModel.ShowByLine = SearchRequestDefaultSettingFactory.ShowByLine;

                }
            }

            //Specific request processing
            if ((request is SimpleSearchRequest) && !string.IsNullOrWhiteSpace(request.FreeText))
            {
                searchResults.RelatedConcepts = new RelatedConceptsComponentModel
                                                    {
                                                        Keywords = request.FreeText,
                                                        MaxNumberOfTerms = 10,
                                                    };

                searchResults.HideRelatedConcepts = request.HideRelatedConcepts;
            }

            var excludeEnumItems = new[] { SortOrder.ArrivalTime };
            if (IsAlertResult(request))
            {
                excludeEnumItems = new SortOrder[0];
                searchResults.ShowDuplicates = ShowDuplicates.Off;



                if (searchResults.Headlines != null && searchResults.Headlines.HeadlineList != null)
                {
                    searchResults.Headlines.ShowPressClip = UserContext.Principle.CoreServices.AlertsService.HasPressClipsEnabled;
                    if (searchResults.Headlines.PostProcessing != null)
                    {
                        searchResults.Headlines.PostProcessing.EnableDuplicateOption = false;

                        if (searchResults.Headlines.ShowPressClip)
                        {
                            searchResults.Headlines.PostProcessing.PostProcessingOptions
                                = searchResults.Headlines.PostProcessing.PostProcessingOptions.Concat(new[] { PostProcessingOptions.PressClips });
                        }
                    }
                    searchResults.Headlines.HeadlineList.ShowPressClip = searchResults.Headlines.ShowPressClip;
                    searchResults.Headlines.EnableDuplicateOption = false;


                }
            }
            searchResults.Headlines.HeadlineSortOptions = new EnumSelectListWithTranslatedText<SortOrder>(searchResults.HeadlineSort, excludeEnumItems);

            return searchResults;
        }

        private static bool IsAlertResult(SearchRequest request)
        {
            return (typeof (AlertSearchRequest).IsAssignableFrom(request.GetType()));
        }

        private static bool IsAbstractSearchQuery(AbstractBaseSearchQuery query)
        {
            return (typeof(AbstractSearchQuery).IsAssignableFrom(query.GetType()));
        }

        protected virtual void RegisterStartupScript()
        {
            ScriptRegistry
                .WithSearchResultsPage()
                .OnDocumentReady("$('body').dj_SearchResultsPage();");
        }

        #endregion

        
    }
}
