// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleSearchUtility.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
    internal class SearchFilterUtility
    {

        private SearchFilterUtility()
        {
        }

        public static List<SearchString> ProcessNewsFilters(NewsFilters NewsFilter)
        {
            List<SearchString> lstSearchString = new List<SearchString>();
            if (NewsFilter != null)
            {
                if (NewsFilter.author != null)
                {
                    lstSearchString.Add(CreateSearchString("au", GetFilterValue(NewsFilter.author), SearchMode.All, SearchType.Controlled));
                }
                if (NewsFilter.company != null)
                {
                    lstSearchString.Add(CreateSearchString("fds", GetFilterValue(NewsFilter.company), SearchMode.All, SearchType.Controlled));
                }
                if (NewsFilter.executive != null)
                {
                    lstSearchString.Add(CreateSearchString("pe", GetFilterValue(NewsFilter.executive), SearchMode.All, SearchType.Controlled));
                }
                if (NewsFilter.industry != null)
                {
                    lstSearchString.Add(CreateSearchString("in", GetFilterValue(NewsFilter.industry), SearchMode.All, SearchType.Controlled));
                }
                if (NewsFilter.newsSubject != null)
                {
                    lstSearchString.Add(CreateSearchString("ns", GetFilterValue(NewsFilter.newsSubject), SearchMode.All, SearchType.Controlled));
                }
                if (NewsFilter.source != null)
                {
                    lstSearchString.Add(CreateSearchString("sc", GetFilterValue(NewsFilter.source), SearchMode.Any, SearchType.Controlled));
                }
                if (NewsFilter.keywords != null)
                {
                    lstSearchString.Add(CreateSearchString("keyword", string.Join(" ", NewsFilter.keywords), SearchMode.Simple, SearchType.Free));
                }
            }
            return lstSearchString;
        }

        public static SearchString GetFreeText(string freeText)
        {
            var searchString = new SearchString();
            searchString.Id = "FreeText";
            searchString.Type = SearchType.Free;
            searchString.Value = freeText;
            searchString.Mode = SearchMode.Simple;
            return searchString;
        }

        public static SearchString GetLanguageFilter(string contentLanguage)
        {
            if (!string.IsNullOrEmpty(contentLanguage))
            {
                contentLanguage = contentLanguage.Replace(",", " ");
                var searchString = new SearchString();
                searchString.Id = "la";
                searchString.Scope = "la";
                searchString.Type = SearchType.Controlled;
                searchString.Mode = SearchMode.Any;
                searchString.Filter = true;
                searchString.Value = contentLanguage;
                return searchString;
            }
            return null;
        }

        private static SearchString CreateSearchString(string id, string value, SearchMode searchMode, SearchType searchType)
        {
            SearchString searchString = new SearchString();
            searchString.Mode = searchMode;
            searchString.Id = id;
            searchString.Value = value;
            if (id != "keyword")
            {
                searchString.Scope = id;
                searchString.Filter = true;
            }
            searchString.Type = searchType;
            searchString.Filter = true;
            searchString.Validate = false;
            return searchString;
        }

        private static string GetFilterValue(FilterItem[] filterItem)
        {
            string value = "";
            if (filterItem != null && filterItem.Length > 0)
            {
                foreach (FilterItem item in filterItem)
                    value += item.code + " ";

                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }

        private static string GetFilterValue(FilterSourceItem[] filterItem)
        {
            string value = "";
            if (filterItem != null && filterItem.Length > 0)
            {
                foreach (FilterSourceItem item in filterItem)
                    value += item.code + " ";

                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }
    }
}