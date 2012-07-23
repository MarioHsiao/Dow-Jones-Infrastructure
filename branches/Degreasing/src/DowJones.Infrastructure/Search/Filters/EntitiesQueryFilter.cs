using System.Collections.Generic;
using System.Linq;

namespace DowJones.Search
{
    public enum EntityType
    {
        Unknown,
        Author,
        Company,
        Executive,
        Industry,
        Subject,
        Region,
    }
    

    public class EntitiesQueryFilter : List<string>, IQueryFilter
    {
        public EntityType EntityType { get; set; }

        public EntitiesQueryFilter()
        {
        }

        public EntitiesQueryFilter(EntityType entityType, IEnumerable<string> entities = null)
            : base(entities ?? Enumerable.Empty<string>())
        {
            EntityType = entityType;
        }
    }
}