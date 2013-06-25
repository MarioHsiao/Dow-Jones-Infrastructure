using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Prod.X.Web.Filters
{
    public class ExcludeFilterProvider : IFilterProvider
    {
        private readonly FilterProviderCollection _filterProviders;

        public ExcludeFilterProvider(IList<IFilterProvider> filters)
        {
            _filterProviders = new FilterProviderCollection(filters);
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = _filterProviders.GetFilters(controllerContext, actionDescriptor).ToArray();
            var excludeFilters = (from f in filters where f.Instance is ExcludeFilterAttribute select f.Instance as ExcludeFilterAttribute);
            var filterTypesToRemove = excludeFilters.Select(excludeFilter => excludeFilter.FilterType).ToList();
            var res = (from filter in filters where !filterTypesToRemove.Contains(filter.Instance.GetType()) select filter);
            return res;
        }
    } 
}