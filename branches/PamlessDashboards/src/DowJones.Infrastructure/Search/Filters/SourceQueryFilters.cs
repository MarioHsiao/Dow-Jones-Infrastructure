using System.Collections.Generic;

namespace DowJones.Search
{
    public class SourceQueryFilters : List<SourceQueryFilterEntities>, IQueryFilter
    {
        public SourceQueryFilters()
        {
        }

        public SourceQueryFilters(IEnumerable<SourceQueryFilterEntities> filters)
            : base(filters)
        {
        }
    }
}