using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Infrastructure.Common;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Search;
using DowJones.Search.Filters;
using DowJones.Token;
using DowJones.Web.Mvc.Search.Requests;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.Search.Requests.Mappers;
using DowJones.Web.Mvc.Search.Requests.Modules;
using DowJones.Web.Mvc.Search.UI.Components.Builders.Alert;
using DowJones.Web.Mvc.Search.UI.Components.Builders.FreeText;
using DowJones.Web.Mvc.Search.UI.Components.Builders.Module;
using DowJones.Web.Mvc.Search.UI.Components.Builders.Simple;
using DowJones.Web.Mvc.Search.UI.Components.DidYouMean;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Search.UI.Components.Builders
{
    public interface ISearchBuilderFactory
    {
        SearchBuilder GetSearchBuilder(SearchResponse response, string configurationAssetName, QueryFilterNameResolver nameResolver, SearchRequest request);
    }

    /// TODO: Break this class into its corresponding mappers
    public class SearchBuilderFactory : ISearchBuilderFactory
    {
        [Inject("Avoiding constructor injection in abstract class")]
        protected ISourceListService SourceListService { get; set; }

        protected internal Func<Type, SearchBuilder> CreateBuilderThunk = ServiceLocator.Resolve<SearchBuilder>;

        private readonly ITokenRegistry _tokenRegistry;
        private readonly AdvancedSearchRequestMapper _searchRequestMapper;

        public SearchBuilderFactory(ITokenRegistry tokenRegistry, AdvancedSearchRequestMapper searchRequestMapper)
        {
            _tokenRegistry = tokenRegistry;
            _searchRequestMapper = searchRequestMapper;
        }

        public virtual SearchBuilder GetSearchBuilder(SearchResponse response, string configurationAssetName, QueryFilterNameResolver nameResolver, SearchRequest request)
        {
            SearchBuilder model = CreateInstance(response);

            var sourceCollection = SourceListService.GetAllSources(configurationAssetName);


            MapSearchBuilder(model, response);
            MapSimpleSearchBuilder(model as SimpleSearchBuilder, response, sourceCollection);
            MapFreeTextSearchBuilder(model as FreeTextSearchBuilder, response, nameResolver);
            MapAlertSearchBuilder(model as AlertSearchBuilder, response);
            MapModuleSearchBuilder(model as ModuleSearchBuilder, response, request as ModuleSearchRequest);

            return model;
        }

        private void MapModuleSearchBuilder(ModuleSearchBuilder builder, SearchResponse source, ModuleSearchRequest moduleSearchRequest)
        {
            if (moduleSearchRequest == null)
            {
                return;
            }
            builder.SearchContext = moduleSearchRequest.SearchContext;
            builder.ChartId = moduleSearchRequest.ChartId;
            builder.PageId = moduleSearchRequest.PageId;
            builder.PageName = moduleSearchRequest.PageName;
            builder.ModuleId = moduleSearchRequest.ModuleId;
            builder.ModulePart = moduleSearchRequest.ModulePart;
        }

        private SearchBuilder CreateInstance(SearchResponse response)
        {
            // TODO:  Do this more awesomely (i.e. not statically)

            if (response.Query is SimpleSearchQuery)
                return CreateBuilderThunk(typeof(SimpleSearchBuilder));

            else if (response.Query is FreeTextSearchQuery)
                return CreateBuilderThunk(typeof(FreeTextSearchBuilder));

            else if (response.Query is AlertSearchQuery)
                return CreateBuilderThunk(typeof(AlertSearchBuilder));

            else if (response.Query is ModuleSearchQuery)
                return CreateBuilderThunk(typeof(ModuleSearchBuilder));

            string notImplementedMessage = string.Format(
                "The search builder for search query type {0} has not been implemented",
                response.Query.GetType());

            throw new NotImplementedException(notImplementedMessage);
        }

        private void MapAlertSearchBuilder(AlertSearchBuilder builder, SearchResponse response)
        {
            if (builder == null || response == null)
            {
                return;
            }

            var query = response.Query as AlertSearchQuery;
            if (query == null) return;


            builder.AlertHeadlineViewType = (String.IsNullOrEmpty(query.Sessionmark))? query.ViewType : AlertHeadlineViewType.Session;
            builder.SelectedAlertId = query.AlertId;
        
            var alertInfo = response.AlertInfo;
            if (alertInfo != null)
            {
                builder.Sessionmark = alertInfo.sessionmark;
                builder.Bookmark = alertInfo.bookmark;
                builder.SelectedAlertId = alertInfo.folderID;
                builder.SelectedAlertText = alertInfo.folderName;
            }

        }

        protected void MapSearchBuilder(SearchBuilder builder, SearchResponse source)
        {
            // Push down specific mapping to each type mapping method.
            builder.Duplicates = source.Query.Duplicates;
            builder.RankUp = source.Query.SourceRanks.Up;
            builder.RankDown = source.Query.SourceRanks.Down;
            
            var query = source.Query as AbstractSearchQuery;

            if (query == null)
                return;

            if (query.CustomDateRange != null)
            {
                builder.StartDate = query.CustomDateRange.Start;
                builder.EndDate = query.CustomDateRange.End;
            }
        }

        protected IEnumerable<SelectListItem> GetDateRanges(SearchDateRange selectedRange)
        {
            return DateRangeHelper.GetDateRange(true).ToSelectList(((int)selectedRange).ToString());
        }

        protected string GetSourceDescriptor(SourceCollection sources, string currentSource)
        {
            string text = _tokenRegistry.Get("allSources");

            if (sources == null || (sources.TopLevelSourceGrouping == null && sources.SourceList == null))
            {
                return text;
            }

            if (sources.TopLevelSourceGrouping != null)
            {
                foreach (var keyValuePair in sources.TopLevelSourceGrouping.Where(keyValuePair => keyValuePair.Key == currentSource))
                {
                    return keyValuePair.Value;
                }
            }

            if (sources.SourceList != null)
            {
                foreach (var keyValuePair in sources.SourceList.Where(keyValuePair => keyValuePair.Key == currentSource))
                {
                    return keyValuePair.Value;
                }
            }

            return text;
        }


        protected void MapSimpleSearchBuilder(SimpleSearchBuilder builder, SearchResponse source, SourceCollection sources)
        {
            if (builder == null || source == null)
            {
                return;
            }

            var query = source.Query as SimpleSearchQuery;
            if (query != null)
            {
                builder.DateRange = query.DateRange;
                builder.DateRangeSelections = GetDateRanges(builder.DateRange);
                builder.FreeText = query.Keywords;
                if (query.ContentLanguages != null && !query.ContentLanguages.IsEmpty())
                {
                    builder.Languages = string.Join(",", query.ContentLanguages);
                }
                builder.SaveOptions = GetSaveMenuOptions();
                builder.Sort = query.Sort;
                builder.Source = query.Source;
                builder.SourceCollection = sources;
                builder.SourceText = GetSourceDescriptor(sources, builder.Source);
                builder.SuggestServiceUrl = DowJones.Properties.Settings.Default.SuggestServiceURL;
                builder.Inclusions = query.Inclusions;
            }

            RecognizedEntities recognitionSet = source.RecognizedEntities;
            if (recognitionSet != null)
            {
                builder.SpellCorrection = recognitionSet.SpellCorrection;
                builder.SearchText = recognitionSet.SearchText;

                IEnumerable<DidYouMeanGroup> entityGroups =
                    from resultEntity in recognitionSet.Entities ?? Enumerable.Empty<RecognizedEntity>()
                    let entity = new DidYouMeanEntity
                    {
                        Code = resultEntity.Code,
                        Name = resultEntity.Name,
                        SearchTerm = resultEntity.SearchTerm,
                    }
                    group entity by resultEntity.Type
                        into grouped
                        select new DidYouMeanGroup(grouped.ToArray())
                        {
                            Category = MapNewsFilterCategory(grouped.Key),
                            Name = _tokenRegistry.Get(GetEntityTypeLabel(grouped.Key, grouped.Count() > 1))
                        };

                builder.DidYouMean = new DidYouMean.DidYouMean { Groups = entityGroups.Where(d=>d.Category != NewsFilterCategory.Unknown) };
                var entities = recognitionSet.Entities;
                builder.DidYouMean.Context= entities.ToJson();
            }
        }

        private static NewsFilterCategory MapNewsFilterCategory(string resultEntityType)
        {
            switch (resultEntityType.ToLower())
            {
                case ("fds"):
                case ("co"):
                    return NewsFilterCategory.Company;

                case ("sc"):
                    return NewsFilterCategory.Source;

                default:
                    return NewsFilterCategory.Unknown;
            }
        }

        private static string GetEntityTypeLabel(string resultEntityType, bool plurals)
        {
            switch (resultEntityType.ToLower())
            {
                case ("fds"):
                case ("co"):
                    return (plurals) ? "companiesLabel" : "companyLabel";
                case ("sc"):
                    return (plurals) ? "sourcesLabel" : "sourceLabel";
            }
            return string.Empty;
        }

        protected void MapFreeTextSearchBuilder(FreeTextSearchBuilder builder, SearchResponse response, QueryFilterNameResolver nameResolver)
        {
            if (builder == null || response == null)
            {
                return;
            }

            var query = response.Query as FreeTextSearchQuery;
            if (query == null) return;

            builder.Author = MapSearchFilters(query.Author, nameResolver);
            builder.Company = MapSearchFilters(query.Company, nameResolver);
            builder.DateRange = Mapper.Map<SearchDateRange>(query.DateRange);
            builder.Duplicates = query.Duplicates;
            builder.DeduplicationMode = (query.Duplicates == DeduplicationMode.Off) ? _tokenRegistry.Get("off") : _tokenRegistry.Get("on");
            builder.Exclusions = query.Exclusions;
            builder.Inclusions = query.Inclusions;
            builder.SocialMedia = (query.Inclusions != null && query.Inclusions.Any(inclusion => inclusion == InclusionFilter.SocialMedia)) ? _tokenRegistry.Get("on") : _tokenRegistry.Get("off");
            builder.FreeText = query.FreeText;
            builder.FreeTextIn = query.FreeTextIn;
            builder.Industry = MapSearchFilters(query.Industry, nameResolver);
            builder.Executive = MapSearchFilters(query.Executive, nameResolver);
            if (query.ContentLanguages != null && !query.ContentLanguages.IsEmpty())
            {
                builder.Languages = string.Join(",", query.ContentLanguages);
            }
            builder.Region = MapSearchFilters(query.Region, nameResolver);
            builder.Sort = query.Sort;
            builder.Source = MapSourceSearchFilters(query.Source ?? new CompoundQueryFilter(), nameResolver);
            builder.Subject = MapSearchFilters(query.Subject, nameResolver);

            builder.SaveOptions = GetSaveMenuOptions(true, builder);
        }

        private IEnumerable<SelectListItem> GetSaveMenuOptions(bool isFreeText = false, FreeTextSearchBuilder builder = null)
        {

            var lstOptions = new List<SelectListItem>
                                     {
                                         new SelectListItem {Text = _tokenRegistry.Get("saveAsTopic"), Value = "topic"},
                                         new SelectListItem {Text = _tokenRegistry.Get("saveAsAlert"), Value = "news"}
                                     };

            if (isFreeText && builder != null)
            {
                if(builder.Author != null && builder.Author.Include != null && builder.Author.Include.Count > 0)
                {
                    lstOptions.Add(new SelectListItem { Text = _tokenRegistry.Get("saveAsAuthorAlert"), Value = "author" });
                }
                if(builder.Source != null && ((builder.Source.Include != null && builder.Source.Include.Count > 0) || 
                    !string.IsNullOrEmpty(builder.Source.ListId)))
                {
                    lstOptions.Add(new SelectListItem { Text = _tokenRegistry.Get("saveAsNewAuthorAlert"), Value = "news" });
                }
            }

            return lstOptions;
        }

        private SearchFilter MapSearchFilters(CompoundQueryFilter filter, QueryFilterNameResolver nameResolver)
        {
            var mappedFilter = Mapper.Map<CompoundQueryFilter, SearchFilter>(filter ?? new CompoundQueryFilter());

            if (mappedFilter != null)
            {
                nameResolver.ResolveNames(mappedFilter.Include);
                nameResolver.ResolveNames(mappedFilter.Exclude);
            }

            return mappedFilter;
        }

        private SourceSearchFilter MapSourceSearchFilters(CompoundQueryFilter filter, QueryFilterNameResolver nameResolver)
        {
            var mappedFilter = _searchRequestMapper.MapSourceSearchFilter(filter);

            if(mappedFilter != null)
            {
                nameResolver.ResolveNames(mappedFilter.Include);
                nameResolver.ResolveNames(mappedFilter.Exclude);
            }

            return mappedFilter;
        }
    }

    public class CompoundQueryFilterToSearchFilterMapper : TypeMapper<CompoundQueryFilter, SearchFilter>
    {
        private readonly SearchQueryFiltersToQueryFiltersMapper _searchQueryToQueryFiltersMapper;

        public CompoundQueryFilterToSearchFilterMapper(SearchQueryFiltersToQueryFiltersMapper searchQueryToQueryFiltersMapper)
        {
            _searchQueryToQueryFiltersMapper = searchQueryToQueryFiltersMapper;
        }

        public override SearchFilter Map(CompoundQueryFilter source)
        {
            // Copy to SearchQueryFilters because, well... we already have that mapper
            // NOTE: this might indicate that we have one too many classes 
            //       and CompoundQueryFilter should just use SearchQueryFilters?
            var includedFilters = new SearchQueryFilters(source.Include ?? Enumerable.Empty<IQueryFilter>());
            var excludedFilters = new SearchQueryFilters(source.Exclude ?? Enumerable.Empty<IQueryFilter>());

            return new SearchFilter
                       {
                           Exclude = _searchQueryToQueryFiltersMapper.Map(excludedFilters),
                           Include = _searchQueryToQueryFiltersMapper.Map(includedFilters),
                           Operator = source.Operator,
                       };
        }
    }
}
