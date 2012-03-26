using System.Collections.Generic;
using Track = Factiva.Gateway.Messages.Track.V1_0;
using DowJones.Utilities.Search.Core;
using DowJones.Search;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    
    public class SearchNewsFilterModel : ViewComponentModel
    {
        #region ..:: Public Properties ::..
        public SearchNewsFilters Filters { get; set; }
        #endregion

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

        #region ..:: Constructor ::..

        public SearchNewsFilterModel()
        {
        }

        #endregion
    }
}
