using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Attributes;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Managers.Search.Preference;
using DowJones.Managers.SearchContext;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Search.Attributes;
using DowJones.Search.Core;
using DowJones.Search.Filters;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using DateRange = DowJones.Infrastructure.DateRange;
using DeduplicationMode = Factiva.Gateway.Messages.Search.V2_0.DeduplicationMode;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using SortOrder = DowJones.Search.SortOrder;
using SourceEntityFilter = Factiva.Gateway.Messages.Assets.Queries.V1_0.SourceEntityFilter;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Managers.Search
{
    /// <summary>
    /// PerformContentSearchRequest Builder
    /// </summary>
    public class SearchQueryBuilder
    {
        private const int MaxNavigatorBuckets = 10;
        private const int DefaultMaxResultCount = 20;

        private readonly IPreferences _preferences;
        private readonly ISearchQueryResourceManager _searchQueryResourceManager;
        private string _productCode;
        private ISearchPreferenceService _searchPreferenceService;
        private SearchContextManager _searchContextManager;

        public SearchQueryBuilder(IPreferences preferences, ISearchQueryResourceManager searchQueryResourceManager, ISearchPreferenceService searchPreferenceService, SearchContextManager searchContextService)
        {
            _preferences = preferences;
            _searchQueryResourceManager = searchQueryResourceManager;
            _searchPreferenceService = searchPreferenceService;
            _searchContextManager = searchContextService;
        }

        public T GetRequest<T>(AbstractBaseSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            _productCode = request.ProductId;
            if (request is SimpleSearchQuery)
            {
                return GetRequest<T>((SimpleSearchQuery)request);
            }
            if (request is FreeTextSearchQuery)
            {
                return GetRequest<T>((FreeTextSearchQuery)request);
            }
            if (request is SearchFormSearchQuery)
            {
                return GetRequest<T>((SearchFormSearchQuery)request);
            }
            if (request is ModuleSearchQuery)
            {
                return GetBaseRequest<T>((ModuleSearchQuery)request);
            }
            if (request is AbstractSearchQuery)
            {
                return GetBaseRequest<T>((AbstractSearchQuery)request);
            }
            return GetBaseSearchQuery<T>(request);
        }

        private T GetRequest<T>(SimpleSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            var searchRequest = GetBaseRequest<T>(request);

            SearchString searchString;
            if (!String.IsNullOrEmpty(request.Keywords))
            {
                searchString = new SearchString { Id = "FreeText", Type = SearchType.Free, Value = request.Keywords, Mode = SearchMode.Simple };
                searchRequest.StructuredSearch.Query.SearchStringCollection.Add(searchString);
            }

            Linguistics linguistics = searchRequest.StructuredSearch.Linguistics ?? new Linguistics();
            linguistics.LemmatizationOn = false;
            linguistics.SpellCheckMode = LinguisticsMode.Suggest;
            linguistics.SymbolRecognitionMode = LinguisticsMode.Suggest;
            searchRequest.StructuredSearch.Linguistics = linguistics;

            if (!String.IsNullOrEmpty(request.Source))
            {
                var pst = _searchQueryResourceManager.PrimarySourceTypesByProductDefineCode(_productCode, request.Source);
                long sourceId;
                if (long.TryParse(request.Source, out sourceId)) // Assume ID means source list!
                {
                    searchString = GetSearchStringBySourceGroup(request.Source);
                    if (searchString != null)
                    {
                        searchRequest.StructuredSearch.Query.SearchStringCollection.Add(searchString);
                    }
                }
                else if (pst.Count() > 0)
                {
                    searchRequest.StructuredSearch.Query.SearchStringCollection.Add(CreateSearchString("pst=(" + pst.Join(" OR ") + ")"));
                    searchRequest.NavigationControl.CollectionCountControl.SourceTypeCollection.Clear();
                    searchRequest.NavigationControl.CollectionCountControl.SourceTypeCollection.AddRange(pst);
                }
            }
            return searchRequest;
        }

        private T GetRequest<T>(FreeTextSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            var searchRequest = GetAdvancedBaseRequest<T>(request);

            var freeTextIn =
                ((AssignedString)
                 Attribute.GetCustomAttribute(typeof(SearchFreeTextArea).GetField(request.FreeTextIn.ToString()),
                                              typeof(AssignedString))).Value;

            var searchString = CreateFreeTextSearchString(request.FreeText, new List<string> { freeTextIn });
            if (searchString != null)
            {
                searchRequest.StructuredSearch.Query.SearchStringCollection.Add(searchString);
            }
            return searchRequest;
        }

        private T GetRequest<T>(SearchFormSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            var searchRequest = GetAdvancedBaseRequest<T>(request);

            SearchString searchString;
            var collection = searchRequest.StructuredSearch.Query.SearchStringCollection;

            if (!String.IsNullOrEmpty(request.Keywords))
            {
                searchString = new SearchString { Id = "FTKeywords", Type = SearchType.Free, Value = request.Keywords, Mode = SearchMode.All };
                collection.Add(searchString);
            }
            if (!String.IsNullOrEmpty(request.AnyWords))
            {
                searchString = new SearchString { Id = "FTAnyWords", Type = SearchType.Free, Value = request.AnyWords, Mode = SearchMode.Any };
                collection.Add(searchString);
            }
            if (!String.IsNullOrEmpty(request.NotWords))
            {
                searchString = new SearchString { Id = "FTNotWords", Type = SearchType.Free, Value = request.NotWords, Mode = SearchMode.None };
                collection.Add(searchString);
            }
            if (!String.IsNullOrEmpty(request.ExactPhrase))
            {
                searchString = new SearchString { Id = "FTExactPhrase", Type = SearchType.Free, Value = request.ExactPhrase, Mode = SearchMode.Phrase };
                collection.Add(searchString);
            }

            return searchRequest;
        }

        private T GetAdvancedBaseRequest<T>(AdvancedSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            var searchRequest = GetBaseRequest<T>(request);
            
            //Apply exclusion filters
            var searchString = GetExcludedSubjects(request.Exclusions);
            if (searchString != null)
            {
                searchRequest.StructuredSearch.Query.SearchStringCollection.Add(searchString);
            }

            var searchCollection = searchRequest.StructuredSearch.Query.SearchStringCollection;
            searchCollection.AddRange(CreateSearchStrings(request.Author));
            searchCollection.AddRange(CreateSearchStrings(request.Executive));
            searchCollection.AddRange(CreateSearchStrings(request.Industry));
            searchCollection.AddRange(CreateSearchStrings(request.Region));
            searchCollection.AddRange(CreateSearchStrings(request.Subject));
            searchCollection.AddRange(CreateSearchStrings(request.Company));
            searchCollection.AddRange(CreateSearchStrings(request.Source));

            var d = GetDate(request, _searchPreferenceService.DateFormat);
            if (d != null)
            {
                searchRequest.StructuredSearch.Query.Dates = d;
            }
            searchRequest.StructuredSearch.Linguistics = new Linguistics
                                                             {
                                                                 LemmatizationOn = false
                                                             };

            return searchRequest;
        }

        private static Dates GetDate(AdvancedSearchQuery request, PreferenceDateFormat dateFormat)
        {
            DateRange dateRange = null;
            if (request.Filters !=  null)
            {
                dateRange = request.Filters.OfType<DateRangeQueryFilter>().Select(x => x.DateRange).FirstOrDefault();
            }
            if (dateRange == null && request.DateRange == SearchDateRange.Custom)
            {
                dateRange = request.CustomDateRange;
            }
            return GetDate(dateRange, dateFormat);
        }
        private static Dates GetDate(AbstractBaseSearchQuery request, PreferenceDateFormat dateFormat)
        {
            DateRange dateRange = null;
            if (request.Filters != null)
            {
                dateRange = request.Filters.OfType<DateRangeQueryFilter>().Select(x => x.DateRange).FirstOrDefault();
            }
            return GetDate(dateRange, dateFormat);
        }
        private static Dates GetDate(DateRange dateRange , PreferenceDateFormat dateFormat)
        {
            if (dateRange == null || dateRange.Start == null || dateRange.End == null)
            {
                return null;
            }
            return DateRangeToSearchDatesMapper.Map(dateRange, dateFormat);
        }
        
        private SearchMode Map(SearchOperator @operator)
        {
            switch (@operator)
            {
                case SearchOperator.And:
                    return SearchMode.All;
                default:
                    return SearchMode.Any;
            }
        }

        private SearchString GetSearchStringBySourceGroup(string sourceListId)
        {
            var list = _searchQueryResourceManager.SourceList(sourceListId);

            var anyList = new List<string>();
            //Only OR group supported for sources
            foreach (var eachGroup in from gp in list where gp.FilterGroup is OrFilterGroup select gp.FilterGroup)
            {
                //Selected pills
                var pills = eachGroup.Filters.OfType<SourceEntityFilter>();

                foreach (SourceEntityFilter pill in pills)
                {
                    foreach (var sourceEntities in pill.SourceEntitiesCollection)
                    {
                        IEnumerable<SourceQueryFilterEntity> sourceFilters = sourceEntities.SourceEntityCollection.Select(entity => new SourceQueryFilterEntity() { SourceCode = entity.Value, SourceType = Mapper.Map<SourceFilterType>(entity.Type) });
                        var str = GetSourceQueryFilter(sourceFilters);
                        if (str != null)
                        {
                            anyList.Add(str);
                        }
                    }

                }
            }
            if (anyList.Count > 0)
            {
                return CreateSearchString(String.Join(" OR ", anyList.ToArray()));
            }
            return null;


        }

        private T GetBaseRequest<T>(ModuleSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            var moduleAbstractSearchQuery = request as AbstractSearchQuery;
            
            /*Remove default setting since we are overriding language and inclusion flag from search context */
            moduleAbstractSearchQuery.ContentLanguages = null;
            moduleAbstractSearchQuery.Inclusions = null;

            var search = GetBaseRequest<T>(moduleAbstractSearchQuery);

            int start = 0;
            int count = DefaultMaxResultCount;
            if (request.Pagination != null)
            {
                start = request.Pagination.StartIndex.GetValueOrDefault();
                count = request.Pagination.MaxResults.GetValueOrDefault();
            }

            IPerformContentSearchRequest contextQuery =
                _searchContextManager.CreateSearchRequest<PerformContentSearchRequest>(request.SearchContext, start,
                                                                                       count);
            if (contextQuery != null && 
                contextQuery.StructuredSearch != null &&
                contextQuery.StructuredSearch.Query != null)
            {
                StructuredQuery query = contextQuery.StructuredSearch.Query;
                if (query.SearchStringCollection != null)
                {
                    search.StructuredSearch.Query.SearchStringCollection.AddRange(query.SearchStringCollection);
                }
                if (query.Dates != null)
                {
                    search.StructuredSearch.Query.Dates = query.Dates;
                }

                if (contextQuery.StructuredSearch.Linguistics != null)
                {
                    search.StructuredSearch.Linguistics = contextQuery.StructuredSearch.Linguistics;
                }

                search.StructuredSearch.Query.SearchUncodedContent = query.SearchUncodedContent;
            }

            //Override dates if date filter is applied!
            var d = GetDate(request, _searchPreferenceService.DateFormat);
            if (d != null)
            {
                search.StructuredSearch.Query.Dates = d;
            }
            return search;
        }

        private T GetBaseRequest<T>(AbstractSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            IPerformContentSearchRequest search = GetBaseSearchQuery<T>(request);
            search.SearchContext = request.ContextId;

            var query = search.StructuredSearch.Query;
            
            //Date range
            query.Dates = ProcessDates(request.DateRange);

            //Content languates
            if (request.ContentLanguages != null && request.ContentLanguages.Count() > 0)
            {
                query.SearchStringCollection.Add(CreateSearchString("la", request.ContentLanguages, SearchMode.Any));
            }

            //Sort
            search.StructuredSearch.Formatting.SortOrder = MapSearchSortSorder(request.Sort);

            //Author markup for MRM users
            search.NavigationControl.ReturnHeadlineCoding = true;

            //Override dates if date filter is applied!
            var d = GetDate(request, _searchPreferenceService.DateFormat);
            if (d != null)
            {
                search.StructuredSearch.Query.Dates = d;
            }

            //Social media content
            search.StructuredSearch.Query.SearchUncodedContent = request.Inclusions != null && request.Inclusions.Any(x => x == InclusionFilter.SocialMedia);

            return (T)search;
        }

        private T GetBaseSearchQuery<T>(AbstractBaseSearchQuery request) where T : IPerformContentSearchRequest, new()
        {
            IPerformContentSearchRequest search = new T();
            var structuredSearch = new StructuredSearch();
            var query = new StructuredQuery();


            //Pagination
            var pagination = request.Pagination ?? new SearchQueryPagination();
            search.FirstResult = pagination.StartIndex.GetValueOrDefault();
            search.MaxResults = pagination.MaxResults.GetValueOrDefault(DefaultMaxResultCount);

            search.StructuredSearch = structuredSearch;

            //Process navigation and collection counts
            var primarySourceTypesForAProduct = _searchQueryResourceManager.PrimarySourceTypesByProductId(_productCode);
            search.NavigationControl = ProcessNavigationControl(primarySourceTypesForAProduct);
            search.DescriptorControl = new DescriptorControl { Mode = DescriptorControlMode.All, Language = _preferences.InterfaceLanguage };

            var cs = request.CustomNavigation;
            if (cs != null )
            {
                var nav = search.NavigationControl;
                if (cs.CustomNavigationId != null && cs.CustomNavigationId.Any())
                {
                    var c = new NavigatorControlCollection();
                    cs.CustomNavigationId.Each(item => c.Add(new NavigatorControl() { Id = item, MaxBuckets = cs.MaxBuckets }));
                    nav.CodeNavigatorControl.CustomCollection = c;
                    nav.CodeNavigatorControl.__maxBucketsSpecified = false;
                    nav.CodeNavigatorControl.__modeSpecified = false;
                }
                if (!cs.KeywordNavigator)
                {
                    nav.KeywordControl = null;
                }
                if (!cs.TimeNavigator)
                {
                    nav.TimeNavigatorMode = TimeNavigatorMode.None;
                }
                nav.ReturnCollectionCounts = cs.ReturnCollectionCount;
            }

            structuredSearch.Formatting = ProcessResultFormating(request);
            structuredSearch.Query = query;

            //Rank sources
            query.RankSearchStringCollection.AddRange(ProcessSourceBoosting(request.SourceRanks));

            //Filters
            var filterSearchStrings = ProcessFilters(request.Filters);
            query.SearchStringCollection.AddRange(filterSearchStrings);

            //Process selected source group
            string selectedSourceGroup = String.IsNullOrEmpty(request.SecondarySourceGroupId)
                                             ? request.PrimarySourceGroupId
                                             : request.SecondarySourceGroupId;


            var selectedPrimarySourceTypes = Enumerable.Empty<string>();
            if (!String.IsNullOrEmpty(selectedSourceGroup))
            {
                selectedPrimarySourceTypes = _searchQueryResourceManager.PrimarySourceTypesByProductDefineCode(_productCode, selectedSourceGroup);
            }
            if (!selectedPrimarySourceTypes.Any())
            {
                selectedPrimarySourceTypes = primarySourceTypesForAProduct;
            }
            query.SourceTypeCollection.AddRange(selectedPrimarySourceTypes);

            //Override dates if date filter is applied!
            var d = GetDate(request, _searchPreferenceService.DateFormat);
            if (d != null)
            {
                search.StructuredSearch.Query.Dates = d;
            }

            return (T)search;
        }


        private NavigationControl ProcessNavigationControl(IEnumerable<string> sourceTypeCollection)
        {
            var nav = new NavigationControl { CodeNavigatorControl = new CodeNavigatorControl { MaxBuckets = MaxNavigatorBuckets, MinBucketValue = 0 }, TimeNavigatorMode = TimeNavigatorMode.AutoDetect };
            nav.CodeNavigatorControl.Mode = CodeNavigatorMode.All;

            nav.KeywordControl = new KeywordControl { MaxKeywords = MaxNavigatorBuckets };

            nav.ReturnCollectionCounts = true;
            nav.CollectionCountControl.SourceTypeCollection.AddRange(sourceTypeCollection);

            return nav;
        }

        private IEnumerable<SearchString> ProcessFilters(SearchQueryFilters searchQueryFilters)
        {
           if (searchQueryFilters == null)
           {
               return Enumerable.Empty<SearchString>();
           }
            var list = new List<SearchString>();
            
            list.AddRange(CreateSearchStrings(searchQueryFilters.Keyword, SearchMode.All));
            list.AddRange(CreateSearchStrings(searchQueryFilters.Company, SearchMode.All));
            list.AddRange(CreateSearchStrings(searchQueryFilters.Executive, SearchMode.All));
            list.AddRange(CreateSearchStrings(searchQueryFilters.Industry, SearchMode.All));
            list.AddRange(CreateSearchStrings(searchQueryFilters.Subject, SearchMode.All));
            list.AddRange(CreateSearchStrings(searchQueryFilters.Author, SearchMode.All));
            list.AddRange(CreateSearchStrings(searchQueryFilters.Region, SearchMode.All));

            list.AddRange(CreateSearchStrings(searchQueryFilters.Source));

            return list.WhereNotNull();
        }

        private Dates ProcessDates(SearchDateRange dateRange)
        {
            var dates = new Dates { Type = DateType.Publication };
            switch (dateRange)
            {
                case SearchDateRange.All:
                    dates.All = true;
                    break;
                case SearchDateRange.Custom:
                    break;
                default:
                    string timeSlice = GetTimeSlice(dateRange);
                    if (timeSlice != null)
                    {
                        dates.After = timeSlice;
                    }
                    break;
            }
            return dates;
        }

        private IEnumerable<RankSearchString> ProcessSourceBoosting(SearchRank rankSources)
        {
            var l = new List<RankSearchString>(2);

            if (rankSources != null)
            {
                if (rankSources.Up != null && rankSources.Up.Count() > 0)
                {
                    var upString = new RankSearchString { Mode = SearchMode.Traditional, Value = "rst=(" + GetBss(rankSources.Up, SearchOperator.Or) + ")" };
                    l.Add(upString);
                }
                if (rankSources.Down != null && rankSources.Down.Count() > 0)
                {
                    var downString = new RankSearchString { Mode = SearchMode.Traditional, Boost = "-", Value = "rst=(" + GetBss(rankSources.Down, SearchOperator.Or) + ")" };
                    l.Add(downString);
                }
            }

            return l;
        }

        private ResultFormatting ProcessResultFormating(AbstractBaseSearchQuery request)
        {
            var formatting = new ResultFormatting
                                 {
                                     DeduplicationMode = Mapper.Map<DeduplicationMode>(request.Duplicates),
                                     MarkupType = MarkupType.Highlight,
                                     SnippetType =
                                         (_searchPreferenceService.LeadSentenceStyle == LeadSentenceStyleType.Contextual)
                                             ? SnippetType.Contextual
                                             : SnippetType.Fixed,
                                     ClusterMode = ClusterMode.Off,
                                     ExtendedFields = true
                                 };

            switch (formatting.SortOrder)
            {
                case ResultSortOrder.Relevance:
                case ResultSortOrder.RelevanceHighFreshness:
                case ResultSortOrder.RelevanceMediumFreshness:
                    formatting.FreshnessDate = DateTime.Today.ToUniversalTime();
                    break;
            }
            return formatting;
        }

        public SearchString CreateSearchString(string strValue)
        {
            SearchString searchString = null;
            if (!String.IsNullOrEmpty(strValue))
            {
                strValue = String.Format("({0})", strValue);
                searchString = new SearchString { Mode = SearchMode.Traditional, Id = "BSSSource", Value = strValue };
            }
            return searchString;
        }

        public SearchString CreateSearchString(string key, IEnumerable<string> items, SearchMode mode)
        {
            if (items == null || items.Count() == 0)
            {
                return null;
            }
            return CreateSearchString(key, String.Join(" ", items), mode);
        }

        private string MapEntityTypeToCode(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Author:
                    return "au";
                case EntityType.Company:
                    return "fds";
                case EntityType.Executive:
                    return "pe";
                case EntityType.Industry:
                    return "in";
                case EntityType.Region:
                    return "re";
                case EntityType.Subject:
                    return "ns";
            }

            return null;
        }

        public SearchString CreateSearchString(string key, string strValue, SearchMode mode)
        {
            if (String.IsNullOrEmpty(strValue))
                return null;

            return new SearchString
            {
                Id = key + mode,
                Filter = true,
                Mode = mode,
                Scope = key,
                Type = SearchType.Controlled,
                Value = strValue,
            };
        }

        public static string GetTimeSlice<T>(T value)
        {
            Type enumType = typeof(T);
            string fieldName = value.ToString();
            var attribute = (TimeSlice)Attribute.GetCustomAttribute(enumType.GetField(fieldName), typeof(TimeSlice));
            return attribute != null ? attribute.Slice.ToString() : null;
        }

        public string GetBss(IEnumerable<string> rankDown, SearchOperator @operator)
        {
            var sb = new StringBuilder();
            string operatorValue = ConvertOperator(@operator);
            int length = operatorValue.Length;
            foreach (string s in rankDown)
            {
                sb.Append(s).Append(operatorValue);
            }
            if (sb.Length > length)
            {
                sb.Remove(sb.Length - length, length);
            }
            return sb.ToString();
        }

        private static ResultSortOrder MapSearchSortSorder(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.PublicationDateOldestFirst:
                    return ResultSortOrder.PublicationDateChronological;
                case SortOrder.Relevance:
                    return ResultSortOrder.Relevance;
                default:
                    return ResultSortOrder.PublicationDateReverseChronological;
            }
        }

        private static IEnumerable<string> GetExcludedSubject<T>(T value)
        {
            Type enumType = typeof(T);
            string fieldName = value.ToString();
            var attribute = (ExcludeSubject)Attribute.GetCustomAttribute(enumType.GetField(fieldName), typeof(ExcludeSubject));
            return attribute != null ? attribute.Subjects : null;
        }

        public SearchString GetKeywordFilter(KeywordQueryFilter filter)
        {
            if (filter == null || filter.Count == 0)
            {
                return null;
            }
            var searchString = new SearchString { Id = "keyword", Type = SearchType.Free, Value = String.Join(" ", filter), Mode = SearchMode.Phrase };
            return searchString;
        }


        public SearchString GetExcludedSubjects(IEnumerable<ExclusionFilter> exclusionFilter)
        {
            SearchString list = null;
            if (exclusionFilter != null)
            {
                var codes = new List<string>();
                foreach (ExclusionFilter exclusionFilte in exclusionFilter)
                {
                    codes.AddRange(GetExcludedSubject(exclusionFilte));
                }
                if (codes.Count > 0)
                {
                    list = CreateSearchString("ns", codes, SearchMode.None);
                }
            }
            return list;
        }

        public SearchString CreateFreeTextSearchString(string freeText, IList<string> freeTextIn)
        {
            SearchString searchString = null;
            if (!String.IsNullOrEmpty(freeText))
            {
                searchString = new SearchString();
                searchString.Id = "FreeText";
                searchString.Type = SearchType.Free;
                searchString.Value = freeText;
                searchString.Mode = SearchMode.Traditional;

                if (freeTextIn != null && freeTextIn.Count > 0)
                {

                    var sb = new StringBuilder();
                    for (int i = 0; i < freeTextIn.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(freeTextIn[i]))
                        {
                            sb.Append(freeTextIn[i]).Append("=").Append("(").Append(freeText).Append(")");
                            sb.Append(ConvertOperator(SearchOperator.Or));
                        }
                    }
                    if (sb.Length > 0)
                    {
                        searchString.Value = sb.Remove(sb.Length - 4, 4).ToString();
                    }
                }
            }
            return searchString;
        }

        public IEnumerable<SearchString> CreateSearchStrings(IQueryFilter filter, SearchMode searchMode = SearchMode.Any)
        {
            if (filter == null)
                return Enumerable.Empty<SearchString>();

            // The Date Range Query Filter is not really a filter...
            if (filter is DateRangeQueryFilter)
                return Enumerable.Empty<SearchString>();

            var compoundFilter = filter as CompoundQueryFilter;
            if (compoundFilter != null)
            {
                return GetCompoundQuerySearchString(compoundFilter);
            }

            var entityFilter = filter as EntitiesQueryFilter;
            if (entityFilter != null)
            {
                var key = MapEntityTypeToCode(entityFilter.EntityType);
                return new[] { CreateSearchString(key, entityFilter, searchMode) }.WhereNotNull();
            }

            var sourceQueryFilters = filter as SourceQueryFilters;
            if (sourceQueryFilters != null)
            {
                return new[] { GetSourceQueryFilter(sourceQueryFilters) }.WhereNotNull();
            }

            var sourceQueryFilter = filter as SourceQueryFilterEntities;
            if (sourceQueryFilter != null)
            {
                string sourceBss = GetSourceQueryFilter(sourceQueryFilter, SearchOperator.Or);
                return new[] {CreateSearchString(sourceBss)};
            }

            var keywordFilter = filter as KeywordQueryFilter;
            if (keywordFilter != null)
            {
                return new[] { GetKeywordFilter(keywordFilter) }.WhereNotNull();
            }

            throw new NotImplementedException();
        }
        private IEnumerable<SearchString> GetCompoundQuerySearchString(CompoundQueryFilter filter)
        {
            if (filter == null)
            {
                return Enumerable.Empty<SearchString>();
            }

            //Only two types are supported 1) EntitiesQueryFilter 2) SourceQueryFilter
            //
            var list = new List<SearchString>();

            //Build one searchstring for all included codes
            var includedFilters = filter.Include ?? Enumerable.Empty<IQueryFilter>();
            var entityFilters = includedFilters.OfType<EntitiesQueryFilter>().WhereNotNull();
            if (entityFilters.Any())
            {
                var key = MapEntityTypeToCode(entityFilters.FirstOrDefault().EntityType);
                var incString = CreateSearchString(key, entityFilters.SelectMany(d => d), Map(filter.Operator));
                list.Add(incString);
            }

            var sourceEntityFilters = includedFilters.OfType<SourceQueryFilterEntities>().WhereNotNull();
            if (sourceEntityFilters.Any())
            {
                list.Add(GetSourceQueryFilter(sourceEntityFilters));
            }

            //Build one searchstring for all excluded codes 
            var excludedFilters = filter.Exclude ?? Enumerable.Empty<IQueryFilter>();
            entityFilters = excludedFilters.OfType<EntitiesQueryFilter>().WhereNotNull();
            if (entityFilters.Any())
            {
                var key = MapEntityTypeToCode(entityFilters.FirstOrDefault().EntityType);
                var excString = CreateSearchString(key, entityFilters.SelectMany(d => d), SearchMode.None);
                list.Add(excString);
            }
            
            if (filter.ListType == CompoundQueryListType.Source)
            {
                long sourceId;
                if (long.TryParse(filter.ListId, out sourceId))
                {
                    var searchString = GetSearchStringBySourceGroup(filter.ListId);
                    if (searchString != null)
                    {
                        list.Add(searchString);
                    }
                }
            }
            return list;
        }


        private SearchString GetSourceQueryFilter(IEnumerable<SourceQueryFilterEntities> filters)
        {
            var anyList = filters.Select(GetSourceQueryFilter).Where(str => str != null).ToList();
            if (anyList.Count > 0)
            {
                return CreateSearchString(Join(SearchOperator.Or, anyList.ToArray()));
            }
            return null;
        }

        private string GetSourceQueryFilter(IEnumerable<SourceQueryFilterEntity> filters)
        {
            return GetSourceQueryFilter(filters, SearchOperator.And);
        }


        private string GetSourceQueryFilter(IEnumerable<SourceQueryFilterEntity> filters, SearchOperator searchOperator = SearchOperator.And)
        {
            if (filters == null || !filters.Any())
                return null;

            var bssFragments = filters.Select(GetSourceFragment).WhereNotNullOrEmpty();

            if (!bssFragments.Any())
                return null;

            string searchString;

            if (bssFragments.Count() == 1)
            {
                searchString = bssFragments.First();
            }
            else
            {
                searchString = String.Format("({0})", Join(searchOperator, bssFragments));
            }

            return searchString;
        }

        public string GetSourceFragment(SourceQueryFilterEntity item)
        {
            switch (item.SourceType)
            {
                case SourceFilterType.ProductDefineCode:
                    IEnumerable<string> pstCodes = _searchQueryResourceManager.PrimarySourceTypesByProductDefineCode(_productCode, item.SourceCode);
                    return String.Format("pst=({0})", Join(SearchOperator.Or, pstCodes));
                case SourceFilterType.Byline:
                    return "by=\"" + item.SourceCode + "\"";
                case SourceFilterType.SourceName:
                    return "sn=\"" + item.SourceCode + "\"";
                case SourceFilterType.Restrictor:
                case SourceFilterType.SourceCode:
                    return "rst=" + item.SourceCode;
            }
            return null;
        }

        protected static string Join(SearchOperator @operator, IEnumerable<string> values)
        {
            return string.Join(ConvertOperator(@operator), values);
        }

        protected static string ConvertOperator(SearchOperator @operator)
        {
            return string.Format(" {0} ", @operator.ToString().ToUpper());
        }
    }
}
