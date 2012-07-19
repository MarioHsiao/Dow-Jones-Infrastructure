using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Infrastructure;
using DowJones.Managers.Search;
using DowJones.Search;
using DowJones.Search.Core;
using DowJones.Search.Filters;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Screening.V1_1;

namespace DowJones.Assemblers.Search
{
    /// <summary>
    ///
    /// </summary>
    public class SearchQueryToQueryAssetMapper
    {
        public void Map(AdvancedSearchQuery searchQuery, IList<Group> groups)
        {
            var baseObject = searchQuery as AbstractSearchQuery;
            Map(baseObject, groups);

            FilterGroup andFilterGroup = GetGroup(GroupOperator.And, groups).FilterGroup;

            #region AUTHOR

            if (searchQuery.Author != null)
            {
                if (searchQuery.Author.Include != null)
                {
                    var authorCodeFilter = new AuthorCodeFilter
                                               {
                                                   Operator = Operator.Or,
                                                   PersonCodes = new PersonCodeCollection()
                                               };
                    authorCodeFilter.PersonCodes.AddRange(GetCodes(searchQuery.Author.Include.Cast<EntitiesQueryFilter>()));

                    andFilterGroup.Filters.Add(authorCodeFilter);
                }
                if (searchQuery.Author.Exclude != null)
                {
                    var authorCodeFilter = new AuthorCodeFilter
                                               {
                                                   Operator = Operator.Not,
                                                   PersonCodes = new PersonCodeCollection()
                                               };
                    authorCodeFilter.PersonCodes.AddRange(GetCodes(searchQuery.Author.Exclude.Cast<EntitiesQueryFilter>()));

                    andFilterGroup.Filters.Add(authorCodeFilter);
                }
            }

            #endregion


            #region EXECUTIVE

            if (searchQuery.Executive != null)
            {
                if (searchQuery.Executive.Include != null)
                {
                    var executiveCodeFilter = new DJPersonCodeFilter
                    {
                        Operator = MapOperator(searchQuery.Executive.Operator),
                        PersonCodes = new PersonCodeCollection()
                    };
                    executiveCodeFilter.PersonCodes.AddRange(GetCodes(searchQuery.Executive.Include.Cast<EntitiesQueryFilter>()));

                    andFilterGroup.Filters.Add(executiveCodeFilter);
                }
                if (searchQuery.Executive.Exclude != null)
                {
                    var executiveCodeFilter = new DJPersonCodeFilter
                    {
                        Operator = Operator.Not,
                        PersonCodes = new PersonCodeCollection()
                    };
                    executiveCodeFilter.PersonCodes.AddRange(GetCodes(searchQuery.Executive.Exclude.Cast<EntitiesQueryFilter>()));

                    andFilterGroup.Filters.Add(executiveCodeFilter);
                }
            }

            #endregion

            #region Company
            if (searchQuery.Company != null)
            {
                if (searchQuery.Company.Include != null)
                {
                    var djOrganizationCodeFilter = new DJOrganizationCodeFilter { Operator = MapOperator(searchQuery.Company.Operator) };
                    IEnumerable<EntitiesQueryFilter> entityFilters = searchQuery.Company.Include.OfType<EntitiesQueryFilter>();
                    djOrganizationCodeFilter.CompanyNewsQueries = GetCompanyNewsQueryCollection(entityFilters);
                    andFilterGroup.Filters.Add(djOrganizationCodeFilter);
                }
                if (searchQuery.Company.Exclude != null)
                {
                    var notFilter = new DJOrganizationCodeFilter { Operator = Operator.Not };
                    IEnumerable<EntitiesQueryFilter> entityFilters = searchQuery.Company.Exclude.OfType<EntitiesQueryFilter>();
                    notFilter.CompanyNewsQueries = GetCompanyNewsQueryCollection(entityFilters);
                    andFilterGroup.Filters.Add(notFilter);
                }
            }

            #endregion 

            #region Industry
            if (searchQuery.Industry != null)
            {
                if (searchQuery.Industry.Include != null)
                {
                    var filter = new IndustryCodeFilter
                                     {
                                         Operator = MapOperator(searchQuery.Industry.Operator),
                                         CodeCollection = GetIndustryCodeCollection(searchQuery.Industry.Include.OfType<EntitiesQueryFilter>())
                                     };
                    andFilterGroup.Filters.Add(filter);
                }
                if (searchQuery.Industry.Exclude != null)
                {
                    var filter = new IndustryCodeFilter
                                     {
                                         Operator = Operator.Not,
                                         CodeCollection = GetIndustryCodeCollection(searchQuery.Industry.Exclude.OfType<EntitiesQueryFilter>())
                                     };
                    andFilterGroup.Filters.Add(filter);
                }
            }

            #endregion 
            
            #region Region
            if (searchQuery.Region != null)
            {
                if (searchQuery.Region.Include != null)
                {
                    var filter = new RegionCodeFilter
                                     {
                                         Operator = MapOperator(searchQuery.Region.Operator),
                                         CodeCollection = GetRegionCodeCollection(searchQuery.Region.Include.OfType<EntitiesQueryFilter>())
                                     };
                    andFilterGroup.Filters.Add(filter);
                }
                if (searchQuery.Region.Exclude != null)
                {
                    var filter = new RegionCodeFilter
                                     {
                                         Operator = Operator.Not,
                                         CodeCollection = GetRegionCodeCollection(searchQuery.Region.Exclude.OfType<EntitiesQueryFilter>())
                                     };
                    andFilterGroup.Filters.Add(filter);
                }
            }

            #endregion 
            
            #region Subject
            if (searchQuery.Subject != null)
            {
                if (searchQuery.Subject.Include != null)
                {
                    var filter = new NewsSubjectCodeFilter
                                     {
                                         Operator = MapOperator(searchQuery.Subject.Operator),
                                         CodeCollection = GetNewsSubjectCodeCollection(searchQuery.Subject.Include.OfType<EntitiesQueryFilter>())
                                     };
                    andFilterGroup.Filters.Add(filter);
                }
                if (searchQuery.Subject.Exclude != null)
                {
                    var filter = new NewsSubjectCodeFilter
                                     {
                                         Operator = Operator.Not,
                                         CodeCollection = GetNewsSubjectCodeCollection(searchQuery.Subject.Exclude.OfType<EntitiesQueryFilter>())
                                     };
                    andFilterGroup.Filters.Add(filter);
                }
            }

            #endregion 

            #region Source
            if (searchQuery.Source != null)
            {
                if (searchQuery.Source.Include != null)
                {
                    var sf = new SourceEntityFilter();
                    var includedEntities = new SourceEntitiesCollection();
                    foreach (SourceQueryFilterEntities queryFilter in searchQuery.Source.Include)
                    {
                        var eachPill = new SourceEntityCollection();
                        foreach (var eachSubItem in queryFilter.Select(entity => new SourceEntity {Type = Map(entity.SourceType), Value = entity.SourceCode}))
                        {
                            eachPill.Add( eachSubItem);
                        }
                        includedEntities.Add(new SourceEntities() {SourceEntityCollection = eachPill});
                    }
                    sf.SourceEntitiesCollection = includedEntities;
                    andFilterGroup.Filters.Add(sf);
                }
                if (!String.IsNullOrEmpty(searchQuery.Source.ListId)) // Assume ID means source list!
                {
                    var listIdFilter = new SourceEntityListIDFilter();
                    listIdFilter.Operator = Operator.Or;
                    listIdFilter.IdCollectionCollection = new IdCollectionCollection();
                    listIdFilter.IdCollectionCollection.Add(searchQuery.Source.ListId);
                    andFilterGroup.Filters.Add(listIdFilter);
                }
            }
            #endregion

            #region Exclusions

            if (searchQuery.Exclusions != null)
            {
                var ef = new SearchExclusionFilter();
                foreach (ExclusionFilter exclusionFilter in searchQuery.Exclusions)
                {
                    switch (exclusionFilter)
                    {
                        case ExclusionFilter.AbstractsCaptionsAndHeadline:
                            ef.AbstractsCaptionsAndHeadline = true;
                            break;
                        case ExclusionFilter.CalendarsAndMediaAdvisories:
                            ef.CalendarsAndMediaAdvisories = true;
                            break;
                        case ExclusionFilter.CommentaryOpinionAndAnalysis:
                            ef.CommentaryOpinionAndAnalysis = true;
                            break;
                        case ExclusionFilter.Crime:
                            ef.Crime = true;
                            break;
                        case ExclusionFilter.EntertainmentAndLifestyle:
                            ef.EntertainmentAndLifestyle = true;
                            break;
                        case ExclusionFilter.NewsSummaries:
                            ef.NewsSummaries = true;
                            break;
                        case ExclusionFilter.RepublishedNews:
                            ef.RepublishedNews = true;
                            break;
                        case ExclusionFilter.RoutineGeneralNews:
                            ef.RoutineGeneralNews = true;
                            break;
                        case ExclusionFilter.RoutineMarketFinancialNews:
                            ef.RoutineMarketFinancialNews = true;
                            break;
                        case ExclusionFilter.Sports:
                            break;
                        case ExclusionFilter.Transcripts:
                            ef.Transcripts = true;
                            break;
                    }
                }
                andFilterGroup.Filters.Add(ef);
            }
            #endregion
        }

        private SourceEntityType Map(SourceFilterType sourceType)
        {
            switch (sourceType)
            {
                case SourceFilterType.Byline:
                    return SourceEntityType.BY;
                case SourceFilterType.ProductDefineCode:
                    return SourceEntityType.PDF;
                case SourceFilterType.Restrictor:
                    return SourceEntityType.RST;
                case SourceFilterType.SourceCode:
                    return SourceEntityType.SC;
                case SourceFilterType.SourceName:
                    return SourceEntityType.SN;
                default:
                    return SourceEntityType.Unspecified;
            }
        }

        public void Map(AbstractSearchQuery searchQuery, IList<Group> groups)
        {
            var baseObject = searchQuery as AbstractBaseSearchQuery;
            Map(baseObject,groups);

            FilterGroup andFilterGroup = GetGroup(GroupOperator.And, groups).FilterGroup;

            #region LANGUAGES 

            var contentLanguageFilter = new ContentLanguageFilter {IsAllLanguages = true, Operator = Operator.Or};
            if (searchQuery.ContentLanguages != null && searchQuery.ContentLanguages.Count() > 0)
            {
                contentLanguageFilter.IsAllLanguages = false;
                contentLanguageFilter.LanguageCodes.AddRange(MapLanguage(searchQuery.ContentLanguages));
            }
            andFilterGroup.Filters.Add(contentLanguageFilter);

            #endregion

            #region DATE FILTER

            switch (searchQuery.DateRange)
            {
                case SearchDateRange.LastDay:
                case SearchDateRange.LastWeek:
                case SearchDateRange.LastMonth:
                case SearchDateRange.LastThreeMonths:
                case SearchDateRange.LastSixMonths:
                case SearchDateRange.LastYear:
                case SearchDateRange.LastTwoYears:
                    andFilterGroup.Filters.Add(GetDaysFilter(searchQuery.DateRange));
                    break;
                case SearchDateRange.Custom:
                    andFilterGroup.Filters.Add(GetCustomDateFilter(searchQuery.CustomDateRange));
                    break;
                default:
                    break;
            }

            var unCodedContentFilter = new SearchUnCodedContentFilter();
            unCodedContentFilter.IncludeUncodedContent = searchQuery.Inclusions != null && searchQuery.Inclusions.Any(x => x == InclusionFilter.SocialMedia);
            andFilterGroup.Filters.Add(unCodedContentFilter);

            #endregion
        }

        public void Map(AbstractBaseSearchQuery searchQuery, IList<Group> groups)
        {
            #region DUPLICATES

            var dups = new SearchDeduplicationFilter
                           {
                               Operator = Operator.And
                           };
            Group group = GetGroup(GroupOperator.And, groups);
            QueryFilterCollection filterCollection = new QueryFilterCollection();
            dups.DeduplicationType = MapDuplicate(searchQuery.Duplicates);
            filterCollection.Add(dups);

            #endregion

            #region NEWS FILTERS

            SearchAdditionalFilters newsFilter = GetNewsFilter(searchQuery.Filters);
            if (newsFilter != null)
            {
                filterCollection.Add(newsFilter);
            }
            if (filterCollection.Count() > 0 )
            {
                if(group.FilterGroup == null)
                {
                    group.FilterGroup = new AndFilterGroup();
                }
                group.FilterGroup.Filters = filterCollection;
            }

            #endregion
        }

        #region PRIVATE FUNCTIONS

        private static NewsSubjectCodeCollection GetNewsSubjectCodeCollection(IEnumerable<EntitiesQueryFilter> source)
        {
            var r = new NewsSubjectCodeCollection();
            r.AddRange(GetCodes(source.OfType<EntitiesQueryFilter>()));
            return r;
        }

        private static RegionCodeCollection GetRegionCodeCollection(IEnumerable<EntitiesQueryFilter> source)
        {
            var r = new RegionCodeCollection();
            r.AddRange(GetCodes(source.OfType<EntitiesQueryFilter>()));
            return r;
        }

        private static IndustryCodeCollection GetIndustryCodeCollection(IEnumerable<EntitiesQueryFilter> source)
        {
            var r = new IndustryCodeCollection();
            r.AddRange(GetCodes(source.OfType<EntitiesQueryFilter>()));
            return r;
        }

        private static companyNewsQueryCollection GetCompanyNewsQueryCollection(IEnumerable<EntitiesQueryFilter> source)
        {
            IEnumerable<companyNewsQuery> list = source.SelectMany(entityFilter => entityFilter).Select(code => new companyNewsQuery {djOrgCode = code, scope = NewsQueryScope.About}).ToList();
            var queryCollection = new companyNewsQueryCollection();
            queryCollection.AddRange(list);
            return queryCollection;
        }

        private static Operator MapOperator(SearchOperator searchOperator)
        {
            return (searchOperator == SearchOperator.And) ? Operator.And : Operator.Or;
        }

        private static IEnumerable<string> GetCodes(IEnumerable<EntitiesQueryFilter> entitiesFilter)
        {
            var list = new List<string>();
            foreach (EntitiesQueryFilter entitiesQueryFilter in entitiesFilter)
            {
                list.AddRange(entitiesQueryFilter);
            }
            return list;
        }

        private static DateFilter GetCustomDateFilter(DateRange range)
        {
            var dateFilter = new DateFilter
                                 {
                                     StartDate = range.Start.GetValueOrDefault(DateTime.MinValue),
                                     EndDate = range.End.GetValueOrDefault(DateTime.Now),
                                     Operator = Operator.And,
                                     Target = DateFilterTarget.PublicationDate
                                 };

            return dateFilter;
        }

        private static DaysFilter GetDaysFilter(SearchDateRange dateRange)
        {
            var daysFilter = new DaysFilter
                                 {
                                     Operator = Operator.And,
                                     SinceDays = Int32.Parse(SearchQueryBuilder.GetTimeSlice(dateRange)),
                                     Target = DateFilterTarget.PublicationDate
                                 };

            return daysFilter;
        }

        private static IEnumerable<SearchLanguageCode> MapLanguage(IEnumerable<string> contentLanguages)
        {
            var list = new List<SearchLanguageCode>();

            #region Language switch clause

            foreach (string lang in contentLanguages)
            {
                switch (lang.ToUpper())
                {
                    case "ALL":
                        break;
                    case "BG":
                        list.Add(SearchLanguageCode.Bulgarian);
                        break;
                    case "CA":
                        list.Add(SearchLanguageCode.Catalan);
                        break;
                    case "CS":
                        list.Add(SearchLanguageCode.Czech);
                        break;
                    case "DA":
                        list.Add(SearchLanguageCode.Danish);
                        break;
                    case "DE":
                        list.Add(SearchLanguageCode.German);
                        break;
                    case "EN":
                        list.Add(SearchLanguageCode.English);
                        break;
                    case "ES":
                        list.Add(SearchLanguageCode.Spanish);
                        break;
                    case "FI":
                        list.Add(SearchLanguageCode.Finnish);
                        break;
                    case "FR":
                        list.Add(SearchLanguageCode.French);
                        break;
                    case "HU":
                        list.Add(SearchLanguageCode.Hungarian);
                        break;
                    case "IT":
                        list.Add(SearchLanguageCode.Italian);
                        break;
                    case "JA":
                        list.Add(SearchLanguageCode.Japanese);
                        break;
                    case "KO":
                        list.Add(SearchLanguageCode.Korean);
                        break;
                    case "NL":
                        list.Add(SearchLanguageCode.Dutch);
                        break;
                    case "NO":
                        list.Add(SearchLanguageCode.Norwegian);
                        break;
                    case "PL":
                        list.Add(SearchLanguageCode.Polish);
                        break;
                    case "PT":
                        list.Add(SearchLanguageCode.Portuguese);
                        break;
                    case "RU":
                        list.Add(SearchLanguageCode.Russian);
                        break;
                    case "SK":
                        list.Add(SearchLanguageCode.Slovak);
                        break;
                    case "SV":
                        list.Add(SearchLanguageCode.Swedish);
                        break;
                    case "TR":
                        list.Add(SearchLanguageCode.Turkish);
                        break;
                    case "ZHCN":
                        list.Add(SearchLanguageCode.SimplifiedChinese);
                        break;
                    case "ZHTW":
                        list.Add(SearchLanguageCode.TraditionalChinese);
                        break;
                    case "ID":
                        list.Add(SearchLanguageCode.BahasaIndonesia);
                        break;
                    case "MS":
                        list.Add(SearchLanguageCode.BahasaMalaysia);
                        break;
                    case "AR":
                        list.Add(SearchLanguageCode.Arabic);
                        break;
                    default:
                        break;
                }
            }

            #endregion

            return list;
        }

        private static SearchAdditionalFilters GetNewsFilter(SearchQueryFilters newsFilter)
        {
            if (newsFilter == null)
            {
                return null;
            }

            var filters = new SearchAdditionalFilters();
            codeCollection codesCollection = filters.AuthorFilters.CodeCollection;
            codesCollection.AddRange(newsFilter.Author ?? Enumerable.Empty<string>());

            codesCollection = filters.CompanyFilters.CodeCollection;
            codesCollection.AddRange(newsFilter.Company ?? Enumerable.Empty<string>());

            if (newsFilter.DateRange != null && newsFilter.DateRange.DateRange != null)
            {
                var dateRange = newsFilter.DateRange.DateRange;
                if (dateRange.Start.HasValue && dateRange.End.HasValue)
                {
                    var dateFilter = new SearchAdditionalDateFilterItem
                                         {
                                             Operator = Operator.And,
                                             Target = DateFilterTarget.PublicationDate,
                                             StartDate = dateRange.Start.GetValueOrDefault(DateTime.MinValue),
                                             EndDate = dateRange.End.GetValueOrDefault(DateTime.MaxValue)
                                         };
                    filters.DateFilter = dateFilter;
                }
            }

            codesCollection = filters.ExecutiveFilters.CodeCollection;
            codesCollection.AddRange(newsFilter.Executive ?? Enumerable.Empty<string>());

            codesCollection = filters.IndustryFilters.CodeCollection;
            codesCollection.AddRange(newsFilter.Industry ?? Enumerable.Empty<string>());

            if (newsFilter.Keyword != null)
            {
                filters.Keywords.CodeCollection.AddRange(newsFilter.Keyword);
            }

            codesCollection = filters.SubjectFilters.CodeCollection;
            codesCollection.AddRange(newsFilter.Subject ?? Enumerable.Empty<string>());

            codesCollection = filters.RegionFilters.CodeCollection;
            codesCollection.AddRange(newsFilter.Region ?? Enumerable.Empty<string>());

            #region Source Filter

            var src = newsFilter.Source;
            if (src != null)
            {
                var sourceFilters = new SearchAdditionalSourceFilterItems() {SourceCollection = new SearchAdditionalSourceFilterItemCollection()};
                foreach (var queryFilter in src)
                {
                    var item = new SearchAdditionalSourceFilterItem
                                   {
                                       Code = queryFilter.SourceCode,
                                       SearchFormatCategory =
                                           (queryFilter.SourceType == SourceFilterType.ProductDefineCode)
                                               ? SearchFormatCategory.ProductDefineCode
                                               : SearchFormatCategory.Restrictor
                                   };
                    
                    sourceFilters.SourceCollection.Add(item);
                }
                filters.SourceFilters = sourceFilters;
            }
            #endregion

            return filters;
        }

        private static SearchDeduplicationType MapDuplicate(DeduplicationMode duplicates)
        {
            switch (duplicates)
            {
                case DeduplicationMode.Similar:
                    return SearchDeduplicationType.Similar;

                case DeduplicationMode.NearExact:
                    return SearchDeduplicationType.VirtuallyIdentical;
            }
            return SearchDeduplicationType.Off;
        }

        public static Group GetGroup(GroupOperator op, IList<Group> groups)
        {
            return groups.First(g => g.Operator == op);
        }

        #endregion
    }
}