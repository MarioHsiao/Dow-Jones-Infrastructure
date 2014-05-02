using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Globalization;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Search.UI.Components.Builders
{
    public abstract class SearchBuilder : ViewComponentModel
    {
        public SearchDateRange DateRange { get; set; }

        public IEnumerable<SelectListItem> DateRangeSelections { get; set; }

        public DeduplicationMode Duplicates { get; set; }

        public DateTime? EndDate { get; set; }
        
        public string FreeText { get; set; }

        //MVC Modal binding was failing with IEnumerable<string> so for now changed to <string>
        public string Languages { get; set; }

        public IEnumerable<string> RankUp { get; set; }

        public IEnumerable<string> RankDown { get; set; }

        public SortOrder Sort { get; set; }

        public DateTime? StartDate { get; set; }

        //This is only sort term solution to persist original request!
        //public string SearchBuilderContext { get; set; }

        public SearchRequest SearchBuilderContext { get; set; }

    }
}