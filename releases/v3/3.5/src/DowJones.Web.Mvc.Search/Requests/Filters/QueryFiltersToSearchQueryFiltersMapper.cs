using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Mapping;
using DowJones.Search;
using DowJones.Search.Filters;

namespace DowJones.Web.Mvc.Search.Requests.Filters
{
    public class QueryFiltersToSearchQueryFiltersMapper : TypeMapper<QueryFilters, SearchQueryFilters>
    {
        public override SearchQueryFilters Map(QueryFilters source)
        {
            var filters = (source ?? new QueryFilters()).WhereNotNull();
            
            if (filters == null)
            {
                return new SearchQueryFilters();
            }

            var groupings = filters.Where(x => x.Code.HasValue()).GroupBy(x => x.Category);

            var queryFilters = groupings.Select(MapQueryFilterGroup);

            return new SearchQueryFilters(queryFilters.ToArray());
        }

        private IQueryFilter MapQueryFilterGroup(IGrouping<NewsFilterCategory, QueryFilter> grouping)
        {
            var filters = grouping.Distinct(x => x.Code, StringComparer.OrdinalIgnoreCase);

            var category = grouping.Key;

            switch (category)
            {
                case NewsFilterCategory.Keyword:
                    return MapKeywordQueryFilter(filters);

                case NewsFilterCategory.Source:
                    return MapSourceQueryFilter(filters);

                case NewsFilterCategory.Company:
                case NewsFilterCategory.Industry:
                case NewsFilterCategory.Author:
                case NewsFilterCategory.Executive:
                case NewsFilterCategory.Region:
                case NewsFilterCategory.Subject:
                    var entityType = Mapper.Map<EntityType>(category);
                    return MapEntitiesQueryFilter(filters, entityType);

                case NewsFilterCategory.DateRange:
                    return MapDateRange(filters);
            }

            return null;
        }

        private DateRangeQueryFilter MapDateRange(IEnumerable<QueryFilter> filters)
        {
            return filters
                .WhereNotNull()
                .Select(filter => new DateRangeQueryFilter(filter.Code))
                .OrderBy(x => x.DateRange.TimeSpan)  // Prefer the shortest time span
                .FirstOrDefault();
        }

        private EntitiesQueryFilter MapEntitiesQueryFilter(IEnumerable<QueryFilter> filters, EntityType entityType)
        {
            return new EntitiesQueryFilter(entityType, filters.Select(a => a.Code));
        }

        private SourceQueryFilterEntities MapSourceQueryFilter(IEnumerable<QueryFilter> filters)
        {
            var entities = filters.Select(a =>
                                           new SourceQueryFilterEntity
                                               {
                                                   SourceCode = a.Code,
                                                   SourceType = a.CodeType.ToEnum(SourceFilterType.SourceCode)
                                               }
                );
            return new SourceQueryFilterEntities(entities);
        }

        private KeywordQueryFilter MapKeywordQueryFilter(IEnumerable<QueryFilter> filters)
        {
            return new KeywordQueryFilter(filters.Select(a => a.Code));
        }
    }
}