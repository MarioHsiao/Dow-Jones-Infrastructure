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
            string includesStr = null, excludesStr = null;
            if (searchFilter != null)
            {
                includesStr = CombineFilters(searchFilter.Include, searchFilter.Operator.GetValueOrDefault(SearchOperator.And));
                excludesStr = CombineFilters(searchFilter.Exclude, SearchOperator.Or);
            }
            var filterString = CombineFilters(includesStr, excludesStr);
            return ToHtmlString(filterString, emptyValueToken);
        }

        public static IHtmlString DisplayFilters(this ViewComponentFactory factory, SourceSearchFilter sourceSearchFilter, string emptyValueToken)
        {
            string includesStr = null, excludesStr = null, filterString = null;
            if (sourceSearchFilter != null)
            {
                var includes = sourceSearchFilter.Include ?? new List<QueryFilters>();
                var excludes = sourceSearchFilter.Exclude ?? new List<QueryFilters>();
                includesStr = CombineFilters(includes.SelectMany(x => x), SearchOperator.Or);
                excludesStr = CombineFilters(excludes.SelectMany(x => x), SearchOperator.Or);
                filterString = CombineFilters(includesStr, excludesStr);
                if (string.IsNullOrEmpty(filterString) && !string.IsNullOrEmpty(sourceSearchFilter.ListName))
                    filterString = sourceSearchFilter.ListName;
            }
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

            var separator = string.Format(" <span class='dj_search-modifier'>{0}</span> ", TokenRegistryThunk().Get(@operator));

            var combined = string.Join(separator, filters.Select(x => x.Name.Trim())).Trim();

            return combined;
        }

        private static string CombineFilters(string includesStr, string excludesStr)
        {
            var combinedStr = new StringBuilder();
            if (!string.IsNullOrEmpty(includesStr) && !string.IsNullOrEmpty(excludesStr))
            {
                combinedStr.AppendFormat("({0}) <br /><span class='dj_search-modifier'>{1}</span> ({2})", includesStr, TokenRegistryThunk().Get(SearchOperator.Not), excludesStr);
            }
            else if (!string.IsNullOrEmpty(includesStr) && string.IsNullOrEmpty(excludesStr))
            {
                combinedStr.AppendFormat("{0}", includesStr);
            }
            else if (string.IsNullOrEmpty(includesStr) && !string.IsNullOrEmpty(excludesStr))
            {
                combinedStr.AppendFormat("<span class='dj_search-modifier'>{0}</span> ({1})", TokenRegistryThunk().Get(SearchOperator.Not), excludesStr);
            }
            return combinedStr.ToString();
        }
    }
}
