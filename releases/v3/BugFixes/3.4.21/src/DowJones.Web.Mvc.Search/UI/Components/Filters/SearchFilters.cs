using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests.Filters;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.Search.UI.Components.Filters
{
    public class SearchFilters
    {
        public IEnumerable<SearchFilterGroup> Groups { get; set; }

        public IEnumerable<QueryFilter> Filters
        {
            get { return (Groups ?? Enumerable.Empty<SearchFilterGroup>()).SelectMany(x => x); }
        }

        public bool HasFilters
        {
            get { return (Filters ?? Enumerable.Empty<QueryFilter>()).Any(); }
        }

        public string SerializedFilters
        {
            get { return Filters == null ? "[]" : JsonConvert.SerializeObject(Filters); }
        }

    }

    public class SearchFilterGroup : List<QueryFilter>
    {
        public NewsFilterCategory Category { get; set; }

        public SearchFilterGroup()
        {
        }

        public SearchFilterGroup(IEnumerable<QueryFilter> filters)
            : base(filters)
        {
        }
    }

    public class QueryFiltersToSearchFiltersMapper : TypeMapper<QueryFilters, SearchFilters>
    {
        public override SearchFilters Map(QueryFilters source)
        {
            var filtersByCategory = source.GroupBy(x => x.Category);
            var groups = filtersByCategory.Select(x => new SearchFilterGroup(x) {Category = x.Key});

            return new SearchFilters { Groups = groups };
        }
    }
}