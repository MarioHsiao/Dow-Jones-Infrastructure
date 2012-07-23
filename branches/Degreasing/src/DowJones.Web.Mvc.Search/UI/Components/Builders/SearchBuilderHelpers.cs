using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Search.Core;
using DowJones.Token;
using DowJones.Web.Mvc.Search.Requests;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Search.UI.Components.Builders
{
    public static class SearchBuilderHelpers
    {
        internal static Func<ITokenRegistry> TokenRegistryThunk = ServiceLocator.Resolve<ITokenRegistry>;


        public static IHtmlString DisplayFilters(this ViewComponentFactory factory, SearchFilter searchFilter, string emptyValueToken)
        {
            var filter = new StringBuilder();

            if (searchFilter != null)
            {
                filter.Append(CombineFilters(searchFilter.Include, searchFilter.Operator.GetValueOrDefault(SearchOperator.And)));
                filter.Append(CombineFilters(searchFilter.Exclude, SearchOperator.Not));
            }

            var filterString = filter.ToString().Trim();
            return ToHtmlString(filterString, emptyValueToken);
        }

        public static IHtmlString DisplayFilters(this ViewComponentFactory factory, SourceSearchFilter sourceSearchFilter, string emptyValueToken)
        {
            var filter = new StringBuilder();

            if (sourceSearchFilter != null)
            {
                var includes = sourceSearchFilter.Include ?? new List<QueryFilters>();
                var excludes = sourceSearchFilter.Exclude ?? new List<QueryFilters>();
                filter.Append(CombineFilters(includes.SelectMany(x => x), SearchOperator.Or));
                filter.Append(CombineFilters(excludes.SelectMany(x => x), SearchOperator.Not));
                if (string.IsNullOrEmpty(filter.ToString()) && !string.IsNullOrEmpty(sourceSearchFilter.ListName))
                    filter.Append(sourceSearchFilter.ListName);
            }

            var filterString = filter.ToString().Trim();
            return ToHtmlString(filterString, "allSources");
        }

        public static IHtmlString CombineEnums<TEnum>(this ViewComponentFactory factory, IEnumerable<TEnum> enums, string emptyValueToken)
            where TEnum : struct
        {
            var tokenRegistry = TokenRegistryThunk();
            var tokenized = (enums ?? Enumerable.Empty<TEnum>()).Cast<Enum>().Select(tokenRegistry.Get);
            var combined = string.Join(", ", tokenized).Trim();
            return ToHtmlString(combined, emptyValueToken);
        }

        public static IHtmlString CombineLanguages(this ViewComponentFactory factory, string languages, string emptyValueToken)
        {
            var tokenRegistry = TokenRegistryThunk();
            string combined = null;
            if(!string.IsNullOrEmpty(languages))
            {
                var enums = languages.Split(",").Select(l => Enum.Parse(typeof(ContentLanguage), l)).Cast<ContentLanguage>();
                var tokenized = enums.Cast<Enum>().Select(tokenRegistry.Get);
                combined = string.Join(", ", tokenized).Trim();
            }
            return ToHtmlString(combined, emptyValueToken);            
        }


        public static IHtmlString ToHtmlString(string value, string emptyValueToken)
        {
            var valueOrDefault = value.HasValue() ? value :TokenRegistryThunk().Get(emptyValueToken);
            return new HtmlString(valueOrDefault);
        }

        private static string CombineFilters(IEnumerable<QueryFilter> filters, SearchOperator @operator = SearchOperator.And)
        {
            if (filters == null) return string.Empty;

            var separator = string.Format(" {0} ", TokenRegistryThunk().Get(@operator));

            var combined = string.Join(separator, filters.Select(x => x.Name.Trim())).Trim();

            return combined;
        }
    }
}
