using System.Collections.Generic;

namespace DowJones.Search
{
    public class SourceQueryFilterEntities : List<SourceQueryFilterEntity>, IQueryFilter
    {
        public SourceQueryFilterEntities()
        {
        }

        public SourceQueryFilterEntities(IEnumerable<SourceQueryFilterEntity> filters)
            : base(filters)
        {
        }
    }

    public class SourceQueryFilterEntity
    {
        public SourceFilterType SourceType { get; set; }
        public string SourceCode { get; set; }
    }
}