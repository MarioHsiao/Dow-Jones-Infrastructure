using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Articles;
using DowJones.Preferences;
using Factiva.Gateway.Messages.Preferences.V1_0;
using DowJones.Search;

namespace DowJones.Managers.Search.Preference
{
    public class SearchPreferenceService : AbstractPreferenceLoader, ISearchPreferenceService
    {
        private const int _defaultPageSize = 20;

        public override IEnumerable<PreferenceClassID> PreferenceClassIds
        {
            get
            {
                return new[]
                           {
                               /* Result */
                               PreferenceClassID.LeadSentenceStyle,
                               PreferenceClassID.HeadlineFormat,
                               PreferenceClassID.HighLight,
                               PreferenceClassID.ShowAccessionNumber,
                               PreferenceClassID.ByLine,
                               PreferenceClassID.ScreenDisplay,
                               PreferenceClassID.SearchSorting,
                               PreferenceClassID.SearchHeadlinesToShow,
                               PreferenceClassID.SearchDuplicateIdentification,
                               PreferenceClassID.SimpleSearchSource,
                               PreferenceClassID.DefaultSimpleSearchDateRange,
                               /* General */
                               PreferenceClassID.DateFormat,
                               PreferenceClassID.ArticleDisplayFormat,
                               PreferenceClassID.SearchLanguage,
                               PreferenceClassID.PictureSize,
                               /* Alert */
                               PreferenceClassID.AlertSorting,
                               PreferenceClassID.AlertHeadlinesToShow,
                               PreferenceClassID.AlertDuplicateIdentification
                           };
            }
        }

        #region ISearchPreferenceService Members

        public LeadSentenceStyleType LeadSentenceStyle
        {
            get
            {
                var item = PreferenceResponse.LeadSentenceStyle;
                if (item != null)
                {
                    return item.LeadSentenceStyle;
                }
                return LeadSentenceStyleType.Traditional;
            }
        }

        public PreferenceHeadlineFormat HeadlineFormat
        {
            get
            {
                var item = PreferenceResponse.HeadlineFormat;
                return item != null ? item.HeadlineFormat : PreferenceHeadlineFormat.INLINE;
            }
        }

        public PreferenceDateFormat DateFormat
        {
            get
            {
                var item = PreferenceResponse.DateFormat;
                return item != null ? item.DateFormat : PreferenceDateFormat.MDY;
            }
        }

        public bool Highlight
        {
            get
            {
                var item = PreferenceResponse.HighLight;
                return item != null && item.HighLight;
            }
        }

        public bool IncludeAccessionNumber
        {
            get
            {
                var item = PreferenceResponse.ShowAccessionNumber;
                return item != null && item.ShowAccessionNumber;
            }
        }

        public bool IncludeByLine
        {
            get
            {
                var item = PreferenceResponse.ByLine;
                return item != null && item.ByLine;
            }
        }

        public PreferenceScreenDisplay ScreenDisplay
        {
            get
            {
                var item = PreferenceResponse.ScreenDisplay;
                return item != null ? item.ScreenDisplay : PreferenceScreenDisplay.NoFrameView;
            }
        }

        public PreferenceSearchSorting SortOption
        {
            get
            {
                var item = PreferenceResponse.SearchSorting;
                return item != null ? item.SearchSorting : PreferenceSearchSorting.MostRecentFirst;
            }
        }

        public PreferenceAlertSorting AlertSortOption
        {
            get
            {
                var item = PreferenceResponse.AlertSorting;
                return item != null ? item.AlertSorting : PreferenceAlertSorting.MostRecentFirst;
            }
        }

        public int AlertPageSize
        {
            get
            {
                var item = PreferenceResponse.AlertHeadlinesToShow;
                return item != null ? item.AlertHeadlinesToShow : _defaultPageSize;
            }
        }

        public bool RemoveDuplicateFromEmail
        {
            get
            {
                var item = PreferenceResponse.AlertDuplicateIdentification;
                return (item != null && item.AlertDuplicateIdentification != PreferenceDeduplication.NONE);
            }
        }

        public PictureSize PictureSize
        {
            get
            {
                var item = PreferenceResponse.PictureSize;
                return (MapPreferencePictureSize(item));
            }
            
        }

        public int PageSize
        {
            get
            {
                var item = PreferenceResponse.SearchHeadlinesToShow;
                return item != null ? item.SearchHeadlinesToShow : _defaultPageSize;
            }
        }

        public PreferenceDeduplication Duplicate
        {
            get
            {
                var item = PreferenceResponse.SearchDuplicateIdentification;
                return item != null ? item.SearchDuplicateIdentification : PreferenceDeduplication.NONE;
            }
        }

        public ArticleDisplayFormat ArticleDisplay
        {
            get
            {
                var item = PreferenceResponse.ArticleDisplayFormat;
                return item != null ? item.ArticleDisplayFormatField : ArticleDisplayFormat.FullArticle;
            }
        }

        public IEnumerable<string> Languages
        {
            get
            {
                var item = PreferenceResponse.SearchLanguage;
                var list =
                    ((item != null && !String.IsNullOrEmpty(item.SearchLanguage))
                         ? item.SearchLanguage.Split(',')
                         : Enumerable.Empty<string>()).ToArray();
                if (list.Count() == 1 && list[0].Equals("all", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Enumerable.Empty<string>();
                }
                return list;
            }
        }

        public string DefaultSimpleSearchSources
        {
            get
            {
                var item = PreferenceResponse.SimpleSearchSource;
                if (item != null)
                {
                    if (!item.IncludeAllSources && item.SourceEntityFilter != null)
                    {
                        return item.SourceEntityFilter.value;
                    }
                }
                return String.Empty;
            }
        }

        public DefaultSimpleSearchDateRange DefaultSimpleSearchDateRange
        {
            get
            {
                var item = PreferenceResponse.DefaultSimpleSearchDateRange;
                return item != null ? item.Value : DefaultSimpleSearchDateRange.LastWeek;
            }
        }

        private static PictureSize MapPreferencePictureSize(PictureSizePreferenceItem item)
        {
            if (item != null)
            {
                switch (item.PictureSize)
                {
                    case PreferencePictureSize.Small:
                        return PictureSize.Small;
                }
            }
            return PictureSize.Large;
        }
        #endregion
    }
}