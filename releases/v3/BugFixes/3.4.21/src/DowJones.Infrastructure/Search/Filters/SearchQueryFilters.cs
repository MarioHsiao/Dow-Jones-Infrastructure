using System.Collections.Generic;
using System.Linq;

namespace DowJones.Search.Filters
{
    public class SearchQueryFilters : List<IQueryFilter>
    {
        public EntitiesQueryFilter Author
        {
            get { return GetEntitiesQueryFilter(EntityType.Author); }
        }

        public EntitiesQueryFilter Company
        {
            get { return GetEntitiesQueryFilter(EntityType.Company); }
        }

        public DateRangeQueryFilter DateRange
        {
            get { return SingleFilter<DateRangeQueryFilter>(); }
        }

        public EntitiesQueryFilter Executive
        {
            get { return GetEntitiesQueryFilter(EntityType.Executive); }
        }

        public EntitiesQueryFilter Industry
        {
            get { return GetEntitiesQueryFilter(EntityType.Industry); }
        }

        public KeywordQueryFilter Keyword
        {
            get { return SingleFilter<KeywordQueryFilter>(); }
        }

        public EntitiesQueryFilter Region
        {
            get { return GetEntitiesQueryFilter(EntityType.Region); }
        }

        public SourceQueryFilterEntities Source
        {
            get { return SingleFilter<SourceQueryFilterEntities>(); }
        }

        public EntitiesQueryFilter Subject
        {
            get { return GetEntitiesQueryFilter(EntityType.Subject); }
        }


        public SearchQueryFilters()
        {
        }

        public SearchQueryFilters(IEnumerable<IQueryFilter> filters)
            : base(filters)
        {
        }


        protected TFilter SingleFilter<TFilter>() where TFilter : class, new()
        {
            return this.OfType<TFilter>().FirstOrDefault() ?? new TFilter();
        }

        protected EntitiesQueryFilter GetEntitiesQueryFilter(EntityType entityType)
        {
            var filter =
                this.OfType<EntitiesQueryFilter>()
                    .FirstOrDefault(x => x.EntityType == entityType);

            return filter ?? new EntitiesQueryFilter(entityType);
        }
    }
}
