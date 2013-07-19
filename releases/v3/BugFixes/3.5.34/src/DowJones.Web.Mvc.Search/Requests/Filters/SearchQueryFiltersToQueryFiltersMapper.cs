using System;
using System.Linq;
using DowJones.Infrastructure;
using DowJones.Mapping;
using DowJones.Search;
using DowJones.Search.Filters;

namespace DowJones.Web.Mvc.Search.Requests.Filters
{
    public class SearchQueryFiltersToQueryFiltersMapper : TypeMapper<SearchQueryFilters, QueryFilters>
    {
        private static readonly TimeSpan OneDay = TimeSpan.FromDays(1);

        protected string DateFormat
        {
            get { return "d MMMM yyyy"; }
        }

        public override QueryFilters Map(SearchQueryFilters source)
        {
            var entityFilters = 
                from entities in source.OfType<EntitiesQueryFilter>()
                from entity in entities
                let category = Mapper.Map<NewsFilterCategory>(entities.EntityType)
                select new QueryFilter
                           {
                               Category = category,
                               Code = entity
                           };

            var sourceFilters = 
                from entities in source.OfType<SourceQueryFilterEntities>()
                from entity in entities
                select new QueryFilter
                           {
                               Category = NewsFilterCategory.Source,
                               Code = entity.SourceCode,
                               CodeType = entity.SourceType.ToString(),
                           };

            var keywordFilters =
                from keywords in source.OfType<KeywordQueryFilter>()
                from keyword in keywords
                select new QueryFilter
                {
                    Category = NewsFilterCategory.Keyword,
                    Code = keyword,
                    Name = keyword
                };

            var dateFilters = 
                from filter in source.OfType<DateRangeQueryFilter>()
                let dateRange = filter.DateRange
                where dateRange != null && dateRange.Start.HasValue && dateRange.Start.Value != DateTime.MinValue
                let startText = DisplayDate(dateRange.Start)
                let endText = DisplayDate(dateRange.End)
                let name = (dateRange.End - dateRange.Start <= OneDay) 
                                ? endText
                                : string.Format("{0} - {1}", startText, endText)
                select new QueryFilter
                           {
                               Category = NewsFilterCategory.DateRange,
                               Code = DateRange.Serialize(dateRange),
                               Name = name,
                           };

            var filters = sourceFilters.Union(keywordFilters).Union(entityFilters).Union(dateFilters);

            return new QueryFilters(filters);
        }

        private string DisplayDate(DateTime? dateTime)
        {
            if(dateTime == null)
                return string.Empty;

            return dateTime.Value.ToString(DateFormat);
        }
    }
}
