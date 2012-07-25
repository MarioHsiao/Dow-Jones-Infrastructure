using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Search;
using DowJones.Search.Attributes;
using DowJones.Search.Core;
using DowJones.Search.Filters;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Assemblers.Search
{
    public class QueryAssetToSearchQueryMapper
    {
        public void Map(IList<Group> groupList, AbstractBaseSearchQuery searchQuery)
        {
            IEnumerable filters = GetQueryFilterCollection(groupList);
            if (filters == null)
            {
                return;
            }

            SearchDeduplicationFilter dups = filters.OfType<SearchDeduplicationFilter>().FirstOrDefault();
            if (dups != null)
            {
                searchQuery.Duplicates = MapDuplicate(dups.DeduplicationType);
            }

            #region NEWS FILTERS

            SearchAdditionalFilters newsFilter = filters.OfType<SearchAdditionalFilters>().FirstOrDefault();
            if (newsFilter != null)
            {
                searchQuery.Filters = GetNewsFilter(newsFilter);
            }

            #endregion
        }

        public void Map(IList<Group> groupList, AbstractSearchQuery searchQuery)
        {
            var baseObject = searchQuery as AbstractBaseSearchQuery;
            Map(groupList, baseObject);

            IEnumerable filters = GetQueryFilterCollection(groupList);
            if (filters == null)
            {
                return;
            }

            #region LANGUAGES

            ContentLanguageFilter langs = filters.OfType<ContentLanguageFilter>().FirstOrDefault();
            if (langs != null && !langs.IsAllLanguages && langs.LanguageCodes != null)
            {
                searchQuery.ContentLanguages = langs.LanguageCodes.Select(GetSearchLanguageCode).WhereNotNullOrEmpty();
            }

            #endregion

            #region DATE FILTER

            DaysFilter daysFilter = filters.OfType<DaysFilter>().FirstOrDefault();
            searchQuery.DateRange = SearchDateRange.All;
            if (daysFilter != null)
            {
                searchQuery.DateRange = MapDateRange(daysFilter.SinceDays);
            }
            DateFilter dateFilter = filters.OfType<DateFilter>().FirstOrDefault();
            if (dateFilter != null)
            {
                searchQuery.CustomDateRange = MapDateRange(dateFilter);
                searchQuery.DateRange = SearchDateRange.Custom;
            }

            var unCodedContentFilter = filters.OfType<SearchUnCodedContentFilter>().FirstOrDefault();
            if (unCodedContentFilter != null)
            {
                searchQuery.Inclusions = MapUnCodedContent(unCodedContentFilter.IncludeUncodedContent);
            }
            #endregion
        }

        private static IEnumerable<InclusionFilter> MapUnCodedContent(bool includeUncodedContent)
        {
            var a = new List<InclusionFilter>{includeUncodedContent ? InclusionFilter.SocialMedia : InclusionFilter.None};
            return a;
        }

        public void Map(IList<Group> groupList, AdvancedSearchQuery searchQuery)
        {
            var baseObject = searchQuery as AbstractSearchQuery;
            Map(groupList, baseObject);

            IEnumerable filters = GetQueryFilterCollection(groupList);
            if (filters == null)
            {
                return;
            }

            #region AUTHORS

            var authorFilters = filters.OfType<AuthorCodeFilter>().WhereNotNull();
            if (authorFilters.Count() > 0)
            {
                var q = new CompoundQueryFilter();
                
                AuthorCodeFilter codeFilter = GetIncludedFilter(authorFilters);
                if (codeFilter != null)
                {
                    q.Include = GetCompoundQueryFilter(EntityType.Author, codeFilter.PersonCodes);
                    q.Operator = MapOperator(codeFilter.Operator);
                }
                codeFilter = GetExcludedFilter(authorFilters);
                if (codeFilter != null)
                {
                    q.Exclude = GetCompoundQueryFilter(EntityType.Author, codeFilter.PersonCodes);
                }

                searchQuery.Author = q;
            }

            #endregion

            #region EXECUTIVE

            var executiveFilters = filters.OfType<DJPersonCodeFilter>().WhereNotNull();
            if (executiveFilters.Count() > 0)
            {
                var q = new CompoundQueryFilter();

                DJPersonCodeFilter codeFilter = GetIncludedFilter(executiveFilters);
                if (codeFilter != null)
                {
                    q.Include = GetCompoundQueryFilter(EntityType.Executive, codeFilter.PersonCodes);
                    q.Operator = MapOperator(codeFilter.Operator);
                }
                codeFilter = GetExcludedFilter(executiveFilters);
                if (codeFilter != null)
                {
                    q.Exclude = GetCompoundQueryFilter(EntityType.Executive, codeFilter.PersonCodes);
                }

                searchQuery.Executive = q;
            }

            #endregion

            #region COMPANY

            IEnumerable<DJOrganizationCodeFilter> coFilters = filters.OfType<DJOrganizationCodeFilter>().WhereNotNull();
            if (coFilters.Count() > 0)
            {
                var q = new CompoundQueryFilter();
                
                DJOrganizationCodeFilter codeFilter = GetIncludedFilter(coFilters);
                if (codeFilter != null)
                {
                    q.Include = GetCompoundQueryFilter(EntityType.Company, codeFilter.CompanyNewsQueries.Select(a => a.djOrgCode));
                    q.Operator = MapOperator(codeFilter.Operator);
                }
                codeFilter = GetExcludedFilter(coFilters);
                if (codeFilter != null)
                {
                    q.Exclude = GetCompoundQueryFilter(EntityType.Company, codeFilter.CompanyNewsQueries.Select(a => a.djOrgCode));
                }

                searchQuery.Company = q;
            }

            #endregion

            #region INDUSTRY
            var industryCodeFilters = filters.OfType<IndustryCodeFilter>().WhereNotNull();
            if (industryCodeFilters.Count() > 0)
            {
                var q = new CompoundQueryFilter();

                var codeFilter = GetIncludedFilter(industryCodeFilters);
                if (codeFilter != null)
                {
                    q.Include = GetCompoundQueryFilter(EntityType.Industry, codeFilter.CodeCollection);
                    q.Operator = MapOperator(codeFilter.Operator);
                }
                codeFilter = GetExcludedFilter(industryCodeFilters);
                if (codeFilter != null)
                {
                    q.Exclude = GetCompoundQueryFilter(EntityType.Industry, codeFilter.CodeCollection);
                }

                searchQuery.Industry = q;
            }
            #endregion

            #region Region
            var regionCodeFilters = filters.OfType<RegionCodeFilter>().WhereNotNull();
            if (regionCodeFilters.Count() > 0)
            {
                var q = new CompoundQueryFilter();

                var codeFilter = GetIncludedFilter(regionCodeFilters);
                if (codeFilter != null)
                {
                    q.Include = GetCompoundQueryFilter(EntityType.Region, codeFilter.CodeCollection);
                    q.Operator = MapOperator(codeFilter.Operator);
                }
                codeFilter = GetExcludedFilter(regionCodeFilters);
                if (codeFilter != null)
                {
                    q.Exclude = GetCompoundQueryFilter(EntityType.Region, codeFilter.CodeCollection);
                }

                searchQuery.Region = q;
            }
            #endregion

            #region SUBJECT
            var subjectCodeFilters = filters.OfType<NewsSubjectCodeFilter>().WhereNotNull();
            if (subjectCodeFilters.Count() > 0)
            {
                var q = new CompoundQueryFilter();

                var codeFilter = GetIncludedFilter(subjectCodeFilters);
                if (codeFilter != null)
                {
                    q.Include = GetCompoundQueryFilter(EntityType.Subject, codeFilter.CodeCollection);
                    q.Operator = MapOperator(codeFilter.Operator);
                }
                codeFilter = GetExcludedFilter(subjectCodeFilters);
                if (codeFilter != null)
                {
                    q.Exclude = GetCompoundQueryFilter(EntityType.Subject, codeFilter.CodeCollection);
                }

                searchQuery.Subject = q;
            }
            #endregion

            #region SOURCES
            IEnumerable<SourceEntityFilter> sourceEntityFilters = filters.OfType<SourceEntityFilter>().WhereNotNull();
            if (sourceEntityFilters.Count() > 0)
            {
               var q = new CompoundQueryFilter();

               SourceEntityFilter codeFilter = GetIncludedFilter(sourceEntityFilters);
               if (codeFilter != null)
               {
                   q.Include = GetSourceEntityFilters(codeFilter.SourceEntitiesCollection);
                   q.Operator = SearchOperator.Or;
               }

               codeFilter = GetExcludedFilter(sourceEntityFilters);
               if (codeFilter != null)
               {
                   q.Exclude = GetSourceEntityFilters(codeFilter.SourceEntitiesCollection);
               }

               searchQuery.Source = q;
            }

            //Source list
            SourceEntityListIDFilter entityListIdFilter = filters.OfType<SourceEntityListIDFilter>().FirstOrDefault();
            if (entityListIdFilter != null && entityListIdFilter.IdCollectionCollection.Any())
            {
                searchQuery.Source = new CompoundQueryFilter() {ListId = entityListIdFilter.IdCollectionCollection[0]};
            }


            #endregion

            #region EXCLUSION
            var ef = filters.OfType<SearchExclusionFilter>().FirstOrDefault();
            if (ef != null)
            {
                var list = new List<ExclusionFilter>();
                if (ef.AbstractsCaptionsAndHeadline)
                {
                    list.Add(ExclusionFilter.AbstractsCaptionsAndHeadline);
                }
                if (ef.CalendarsAndMediaAdvisories)
                {
                    list.Add(ExclusionFilter.CalendarsAndMediaAdvisories);
                }
                if (ef.CommentaryOpinionAndAnalysis)
                {
                    list.Add(ExclusionFilter.CommentaryOpinionAndAnalysis);
                }
                if (ef.Crime)
                {
                    list.Add(ExclusionFilter.Crime);
                }
                if (ef.EntertainmentAndLifestyle)
                {
                    list.Add(ExclusionFilter.EntertainmentAndLifestyle);
                }
                if(ef.NewsSummaries)
                {
                    list.Add(ExclusionFilter.NewsSummaries);
                }
                if (ef.RepublishedNews)
                {
                    list.Add(ExclusionFilter.RepublishedNews);
                }
                if (ef.RoutineGeneralNews)
                {
                    list.Add(ExclusionFilter.RoutineGeneralNews);
                }
                if (ef.RoutineMarketFinancialNews)
                {
                    list.Add(ExclusionFilter.RoutineMarketFinancialNews);
                }
                if (ef.Sports)
                {
                    list.Add(ExclusionFilter.Sports);
                }
                if (ef.Transcripts)
                {
                    list.Add(ExclusionFilter.Transcripts);
                }
                searchQuery.Exclusions = list;
            }
            #endregion
        }

        private static IEnumerable<SourceQueryFilterEntities> GetSourceEntityFilters(IEnumerable<SourceEntities> sourceEntitiesCollection)
        {
           var list = new List<SourceQueryFilterEntities>();
            foreach (SourceEntities sourceEntities in sourceEntitiesCollection)
            {
                var target = new SourceQueryFilterEntities();
                target.AddRange(sourceEntities.SourceEntityCollection.Select(entity => new SourceQueryFilterEntity {SourceCode = entity.Value, SourceType = Map(entity.Type)}));
                list.Add(target);
            }
            return list;
        }

        private static SourceFilterType Map(SourceEntityType type)
        {
            switch (type)
            {
                case SourceEntityType.BY:
                    return SourceFilterType.Byline;
                case SourceEntityType.PDF:
                    return SourceFilterType.ProductDefineCode;
                case SourceEntityType.RST:
                    return SourceFilterType.Restrictor;
                case SourceEntityType.SC:
                    return SourceFilterType.SourceCode;
                case SourceEntityType.SN:
                    return SourceFilterType.SourceName;
                default:
                    return SourceFilterType.Unknown;
            }
        }

        private static T GetExcludedFilter<T>(IEnumerable<T> filters) where T : QueryFilter
        {
            return filters.FirstOrDefault(a => a.Operator == Operator.Not);
        }

        private static T GetIncludedFilter<T>(IEnumerable<T> filters) where T : QueryFilter
        {
            return filters.FirstOrDefault(a => a.Operator == Operator.And || a.Operator == Operator.Or);
        }

        private static SearchOperator MapOperator(Operator @operator)
        {
            return @operator == Operator.And ? SearchOperator.And : SearchOperator.Or;
        }

        private static IEnumerable<EntitiesQueryFilter> GetCompoundQueryFilter(EntityType entityType, IEnumerable<string> codes)
        {
            return new[] {new EntitiesQueryFilter(entityType, codes)};
        }

        private static DateRange MapDateRange(DateFilter dateFilter)
        {
            var range = new DateRange {Start = dateFilter.StartDate, End = dateFilter.EndDate};
            return range;
        }

        private static SearchDateRange MapDateRange(int sinceDays)
        {
            Type t = typeof (SearchDateRange);
            string[] a = Enum.GetNames(t);
            foreach (string s in a)
            {
                FieldInfo field = t.GetField(s);
                var attribute = (TimeSlice) field.GetCustomAttributes(typeof (TimeSlice), false).FirstOrDefault();
                if (attribute.Slice == sinceDays)
                {
                    return (SearchDateRange) Enum.Parse(typeof (SearchDateRange), s);
                }
            }
            return SearchDateRange.LastThreeMonths;
        }

        private static string GetSearchLanguageCode(SearchLanguageCode code)
        {
            switch (code)
            {
                case SearchLanguageCode.Bulgarian:
                    return "BG";

                case SearchLanguageCode.Catalan:
                    return "CA";

                case SearchLanguageCode.Czech:
                    return "CS";

                case SearchLanguageCode.Danish:
                    return "DA";

                case SearchLanguageCode.German:
                    return "DE";

                case SearchLanguageCode.English:
                    return "EN";

                case SearchLanguageCode.Spanish:
                    return "ES";

                case SearchLanguageCode.Finnish:
                    return "FI";

                case SearchLanguageCode.French:
                    return "FR";

                case SearchLanguageCode.Hungarian:
                    return "HU";

                case SearchLanguageCode.Italian:
                    return "IT";

                case SearchLanguageCode.Japanese:
                    return "JA";

                case SearchLanguageCode.Korean:
                    return "KO";

                case SearchLanguageCode.Dutch:
                    return "NL";

                case SearchLanguageCode.Norwegian:
                    return "NO";

                case SearchLanguageCode.Polish:
                    return "PL";

                case SearchLanguageCode.Portuguese:
                    return "PT";

                case SearchLanguageCode.Russian:
                    return "RU";

                case SearchLanguageCode.Slovak:
                    return "SK";

                case SearchLanguageCode.Swedish:
                    return "SV";

                case SearchLanguageCode.Turkish:
                    return "TR";

                case SearchLanguageCode.SimplifiedChinese:
                    return "ZHCN";

                case SearchLanguageCode.TraditionalChinese:
                    return "ZHTW";

                case SearchLanguageCode.BahasaIndonesia:
                    return "ID";

                case SearchLanguageCode.BahasaMalaysia:
                    return "MS";

                case SearchLanguageCode.Arabic:
                    return "AR";

                default:
                    return null;
            }
        }

        public static IEnumerable<QueryFilter> GetQueryFilterCollection(IList<Group> groupList)
        {
            if (groupList == null || groupList.Count == 0)
            {
                return null;
            }
            Group group = SearchQueryToQueryAssetMapper.GetGroup(GroupOperator.And, groupList);
            if (group == null)
            {
                return null;
            }
            return group.FilterGroup.Filters;
        }

        private static DeduplicationMode MapDuplicate(SearchDeduplicationType source)
        {
            switch (source)
            {
                case SearchDeduplicationType.Similar:
                    return DeduplicationMode.Similar;

                case SearchDeduplicationType.VirtuallyIdentical:
                    return DeduplicationMode.NearExact;
            }
            return DeduplicationMode.Off;
        }

        private static SearchQueryFilters GetNewsFilter(SearchAdditionalFilters filters)
        {
            var newsFilter = new SearchQueryFilters();

            if (filters.AuthorFilters != null && filters.AuthorFilters.CodeCollection != null)
            {
                var a = new EntitiesQueryFilter(EntityType.Author);
                a.AddRange(filters.AuthorFilters.CodeCollection);
                newsFilter.Add(a);
            }

            if (filters.CompanyFilters != null && filters.CompanyFilters.CodeCollection != null)
            {
                var a = new EntitiesQueryFilter(EntityType.Company);
                a.AddRange(filters.CompanyFilters.CodeCollection);
                newsFilter.Add(a);
            }

            if (filters.DateFilter != null)
            {
                var dateRange = new DateRange(filters.DateFilter.StartDate, filters.DateFilter.EndDate);
                newsFilter.Add(new DateRangeQueryFilter(dateRange));
            }
            if (filters.IndustryFilters != null && filters.IndustryFilters.CodeCollection != null)
            {
                var a = new EntitiesQueryFilter(EntityType.Industry);
                a.AddRange(filters.IndustryFilters.CodeCollection);
                newsFilter.Add(a);
            }
            if (filters.ExecutiveFilters != null && filters.ExecutiveFilters.CodeCollection != null)
            {
                var a = new EntitiesQueryFilter(EntityType.Executive);
                a.AddRange(filters.ExecutiveFilters.CodeCollection);
                newsFilter.Add(a);
            }

            if (filters.Keywords != null && filters.Keywords.CodeCollection != null)
            {
                var a = new KeywordQueryFilter(filters.Keywords.CodeCollection);
                newsFilter.Add(a);
            }

            if (filters.SubjectFilters != null && filters.SubjectFilters.CodeCollection != null)
            {
                var a = new EntitiesQueryFilter(EntityType.Subject);
                a.AddRange(filters.SubjectFilters.CodeCollection);
                newsFilter.Add(a);
            }
            if (filters.RegionFilters != null && filters.RegionFilters.CodeCollection != null)
            {
                var a = new EntitiesQueryFilter(EntityType.Region);
                a.AddRange(filters.RegionFilters.CodeCollection);
                newsFilter.Add(a);
            }

            return newsFilter;
        }
    }
}