using System.Collections.Generic;
using DowJones.Articles;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Managers.Search.Preference
{
    public interface IPreferencesLoader
    {
        IEnumerable<PreferenceClassID> PreferenceClassId { get; }
        PreferenceResponse PreferenceResponse { get; set; }
    }

    public interface ISearchPreferenceService
    {
        LeadSentenceStyleType LeadSentenceStyle { get; }
        PreferenceHeadlineFormat HeadlineFormat { get; }
        PreferenceDateFormat DateFormat { get; }

        bool Highlight { get; }
        bool IncludeAccessionNumber { get; }
        bool IncludeByLine { get; }

        PreferenceScreenDisplay ScreenDisplay { get; }
        PreferenceSearchSorting SortOption { get; }


        int PageSize { get; }
        PreferenceDeduplication Duplicate { get; }

        ArticleDisplayFormat ArticleDisplay { get; }
        IEnumerable<string> Languages { get; }
        string DefaultSimpleSearchSources { get; }

        PreferenceAlertSorting AlertSortOption { get; }
        int AlertPageSize { get; }

        bool RemoveDuplicateFromEmail { get; }

        PictureSize PictureSize { get; }
    }
}