using System.Collections.Generic;

namespace DowJones.Web.Mvc.Search.Requests.Filters
{
    [JsonModelBinder]
    public class QueryFilters : List<QueryFilter>
    {
        public QueryFilters()
        {
        }

        public QueryFilters(IEnumerable<QueryFilter> filters)
            : base(filters)
        {
        }
    }
}