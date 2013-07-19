using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.Search.Requests.Filters
{
    public class QueryFilterNameResolver
    {
        private readonly IDictionary<string, string> _filterNames;

        public QueryFilterNameResolver(SearchRequest request)
        {
            Guard.IsNotNull(request, "request");

            var queryFilters = (request.Filters ?? Enumerable.Empty<QueryFilter>());

            var advancedRequest = request as AdvancedSearchRequest;
            if (advancedRequest != null)
            {
                if (advancedRequest.Channels != null)
                    queryFilters = queryFilters
                        .Union(advancedRequest.Channels.SelectMany(x => x.Include))
                        .Union(advancedRequest.Channels.SelectMany(x => x.Exclude));

                if (advancedRequest.Source != null)
                    queryFilters = queryFilters
                        .Union(advancedRequest.Source.Include.SelectMany(x => x))
                        .Union(advancedRequest.Source.Exclude.SelectMany(x => x));
            }

            var tempQueryFilters =  queryFilters
                    .WhereNotNull()
                    .Distinct(new QueryFilterEqualityComparer());

            _filterNames = new Dictionary<string, string>();
            foreach (QueryFilter queryFilter in tempQueryFilters)
            {
               if (!_filterNames.ContainsKey(queryFilter.Code))
               {
                   _filterNames.Add(queryFilter.Code, queryFilter.Name);
               }
            }
        }

        public void ResolveNames(IEnumerable<QueryFilters> filters)
        {
            if (filters == null)
                return;

            foreach(var filter in filters)
                ResolveNames(filter);
        }

        public void ResolveNames(IEnumerable<QueryFilter> filters)
        {
            if (filters == null)
                return;

            var filtersWithNoName = filters.Where(x => x.Name.IsNullOrEmpty());

            foreach (var filter in filtersWithNoName)
            {
                string name;
                if (_filterNames.TryGetValue(filter.Code, out name))
                    filter.Name = name;
            }
        }


        class QueryFilterEqualityComparer : IEqualityComparer<QueryFilter>
        {
            public bool Equals(QueryFilter x, QueryFilter y)
            {
                if (ReferenceEquals(x, y)) return true;
                if(x == null && y == null) return true;
                if(x == null || y == null) return false;

                return String.Equals(x.Code, y.Code, StringComparison.OrdinalIgnoreCase)
                       && x.Category == y.Category;
            }

            public int GetHashCode(QueryFilter obj)
            {
                return string.Format("{0}{1}", obj.Category, obj.Code).GetHashCode();
            }
        }
    }
}