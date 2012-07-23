using System.Collections.Generic;
using DowJones.Search;
using Factiva.Gateway.Messages.Preferences.V1_0;
using SortOrder = DowJones.Search.SortOrder;

namespace DowJones.Web.Mvc.Search.Managers.Preferences
{
    public interface ISearchBuilderPreferences
    {
        SearchDateRange SimpleSearchDateRange { get; }
        SearchDateRange DateRange { get; }
        SortOrder SortOrder { get; }
        SearchBuilderSourcesPreferenceItem SourcesPreferenceItem { get; }
        SimpleSearchSourcePreferenceItem SimpleSearchSource { get; }
        SearchFreeTextArea SearchIn { get; }
        IEnumerable<ExclusionFilter> ExclusionFilters { get; }
        bool Duplicates { get; }
    }
}