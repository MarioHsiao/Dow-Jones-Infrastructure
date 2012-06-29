using System.Collections.Generic;
using DowJones.Utilities.Search.Core;
using DowJones.Search;
using FilterItem = DowJones.Utilities.Search.Core.FilterItem;

namespace DowJones.Web.Mvc.UI.Components.SearchNewsFilter
{
    public class SearchNewsFilterModel : ViewComponentModel
    {
        public SearchNewsFilters Filters { get; set; }

        public string GetToken(NewsFilterCategory category, int filterCount)
        {
            switch (category)
            {
                case NewsFilterCategory.Company:
                    return filterCount > 1 ? "companies" : "companyLabel";
                case NewsFilterCategory.Executive:
                    return filterCount > 1 ? "executives" : "executive";
                case NewsFilterCategory.Author:
                    return filterCount > 1 ? "authors" : "author";
                case NewsFilterCategory.Industry:
                    return filterCount > 1 ? "industries" : "industry";
                case NewsFilterCategory.Subject:
                    return filterCount > 1 ? "subjects" : "subject";
                case NewsFilterCategory.Region:
                    return filterCount > 1 ? "regions" : "regionLabel";
                case NewsFilterCategory.Source:
                    return filterCount > 1 ? "sources" : "sourcesLabel";
                case NewsFilterCategory.DateRange:
                    return "date";
                case NewsFilterCategory.Keyword:
                    return filterCount > 1 ? "keyword" : "keywords";
            }
            return null;
        }

        public IEnumerable<FilterItem> GetFilters(NewsFilterCategory category)
        {
            if (Filters != null)
            {
                switch (category)
                {
                    case NewsFilterCategory.Company:
                        return Filters.Company;
                    case NewsFilterCategory.Executive:
                        return Filters.Executive;
                    case NewsFilterCategory.Author:
                        return Filters.Author;
                    case NewsFilterCategory.Industry:
                        return Filters.Industry;
                    case NewsFilterCategory.Subject:
                        return Filters.Subject;
                    case NewsFilterCategory.Region:
                        return Filters.Region;
                    case NewsFilterCategory.Source:
                        return Filters.Source;
                }
            }
            return null;
        }
    }
}
