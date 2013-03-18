using System.Web.Mvc;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests;
using System.Collections.Generic;
using DowJones.Web.Mvc.Search.Requests.Filters;

namespace DowJones.Web.Mvc.Search.UI.Components.Builders.FreeText
{
    public class FreeTextSearchBuilder : SearchBuilder
    {
        public string DeduplicationMode { get; set; }
        public string SocialMedia { get; set; }

        public SearchFreeTextArea FreeTextIn { get; set; }

        public IEnumerable<ExclusionFilter> Exclusions { get; set; }
        public IEnumerable<InclusionFilter> Inclusions { get; set; }
        public SearchFilter Company { get; set; }
        public SearchFilter Region { get; set; }
        public SearchFilter Author { get; set; }
        public SourceSearchFilter Source { get; set; }
        public SearchFilter Subject { get; set; }
        public SearchFilter Industry { get; set; }
        public SearchFilter Executive { get; set; }

        public IEnumerable<SelectListItem> SaveOptions { get; set; }
    }
}
