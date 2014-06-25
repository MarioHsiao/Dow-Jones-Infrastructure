using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.Search.Preference;
using DowJones.Search;
using Factiva.Gateway.Messages.Preferences.V1_0;
using SortOrder = DowJones.Search.SortOrder;

namespace DowJones.Web.Mvc.Search.Managers.Preferences
{
    public class SearchBuilderPreferences : AbstractPreferenceLoader, ISearchBuilderPreferences
    {
        public override IEnumerable<PreferenceClassID> PreferenceClassIds
        {
            get
            {
                return new[]
                           {
                               PreferenceClassID.SimpleSearchSource,
                               PreferenceClassID.DefaultSimpleSearchDateRange,
                               PreferenceClassID.SearchBuilderSources,
                               PreferenceClassID.SearchForFreeTextIn,
                               PreferenceClassID.SearchDateRange,
                               PreferenceClassID.ExclusionFilters,
                               PreferenceClassID.SearchSorting,
                               PreferenceClassID.SearchDuplicateIdentification
                           };
            }
        }

        public SearchDateRange DateRange
        {
            get
            {
                var item = PreferenceResponse.SearchDateRange;
                return item != null ? MapDateRange(item.SearchDateRange) : SearchDateRange.LastThreeMonths;
            }
        }

        public SearchDateRange SimpleSearchDateRange
        {
            get
            {
                var item = PreferenceResponse.DefaultSimpleSearchDateRange;
                return item != null ? MapDateRange(item.Value) : SearchDateRange.LastThreeMonths;
            }
        }

        private static SearchDateRange MapDateRange(DefaultSimpleSearchDateRange searchDateRange)
        {
            switch (searchDateRange)
            {
                case DefaultSimpleSearchDateRange.LastDay:
                    return SearchDateRange.LastDay;
                case DefaultSimpleSearchDateRange.LastWeek:
                    return SearchDateRange.LastWeek;
                case DefaultSimpleSearchDateRange.LastMonth:
                    return SearchDateRange.LastMonth;
                case DefaultSimpleSearchDateRange.Last3Months:
                    return SearchDateRange.LastThreeMonths;
                case DefaultSimpleSearchDateRange.Last6Months:
                    return SearchDateRange.LastSixMonths;
                case DefaultSimpleSearchDateRange.LastYear:
                    return SearchDateRange.LastYear;
                case DefaultSimpleSearchDateRange.Last2Years:
                    return SearchDateRange.LastTwoYears;
                case DefaultSimpleSearchDateRange.Last5Years:
                    return SearchDateRange.LastFiveYears;
                case DefaultSimpleSearchDateRange.All:
                    return SearchDateRange.All;
                default:
                    return SearchDateRange.LastThreeMonths;
            }
        }

        public SortOrder SortOrder
        {
            get
            {
                var item = PreferenceResponse.SearchSorting;
                return item != null ? MapSortOrder(item.SearchSorting) : SortOrder.PublicationDateMostRecentFirst;
            }
        }

        public SearchBuilderSourcesPreferenceItem SourcesPreferenceItem
        {
            get { return PreferenceResponse.SearchBuilderSources; }
        }

        public SimpleSearchSourcePreferenceItem SimpleSearchSource
        {
            get { return PreferenceResponse.SimpleSearchSource; }
        }

        public SearchFreeTextArea SearchIn
        {
            get
            {
                var item = PreferenceResponse.SearchForFreeTextIn;
                return MapSearchForFreeTextIn((item != null) ? item.SearchForFreeTextIn :  PreferenceSearchForFreeTextIn.FullArticle);
            }
        }

        public IEnumerable<ExclusionFilter> ExclusionFilters
        {
            get { return MapSelectedExclusionFilter(PreferenceResponse.ExclusionFilters); }
        }

        public bool Duplicates
        {
            get
            {
                var item = PreferenceResponse.SearchDuplicateIdentification;
                return item != null ? item.SearchDuplicateIdentification != PreferenceDeduplication.NONE : false;
            }
        }

        public static SearchDateRange MapDateRange(PreferenceSearchDateRange searchDateRange)
        {
            switch (searchDateRange)
            {
                case PreferenceSearchDateRange.LastDay:
                    return SearchDateRange.LastDay;
                case PreferenceSearchDateRange.LastWeek:
                    return SearchDateRange.LastWeek;
                case PreferenceSearchDateRange.LastMonth:
                    return SearchDateRange.LastMonth;
                case PreferenceSearchDateRange.LastThreeMonths:
                    return SearchDateRange.LastThreeMonths;
                case PreferenceSearchDateRange.LastSixMonths:
                    return SearchDateRange.LastSixMonths;
                case PreferenceSearchDateRange.LastYear:
                    return SearchDateRange.LastYear;
                case PreferenceSearchDateRange.LastTwoYears:
                    return SearchDateRange.LastTwoYears;
                case PreferenceSearchDateRange.LastFiveYears:
                    return SearchDateRange.LastFiveYears;
                case PreferenceSearchDateRange.AllDates:
                    return SearchDateRange.All;
                default:
                    return SearchDateRange.LastThreeMonths;
            }
        }

        public static SortOrder MapSortOrder(PreferenceSearchSorting searchSorting)
        {
            switch (searchSorting)
            {
                case PreferenceSearchSorting.OldestFirst:
                    return SortOrder.PublicationDateOldestFirst;
                case PreferenceSearchSorting.Relevance:
                    return SortOrder.Relevance;
                default:
                    return SortOrder.PublicationDateMostRecentFirst;
            }
        }

        public static SearchFreeTextArea MapSearchForFreeTextIn(PreferenceSearchForFreeTextIn searchForFreeTextIn)
        {
            switch (searchForFreeTextIn)
            {
                case PreferenceSearchForFreeTextIn.FullArticle:
                    return SearchFreeTextArea.FullArticle;
                case PreferenceSearchForFreeTextIn.Headline:
                    return SearchFreeTextArea.Headline;
                case PreferenceSearchForFreeTextIn.HeadlineLeadParagraph:
                    return SearchFreeTextArea.HeadlineAndLeadParagraph;
                case PreferenceSearchForFreeTextIn.Author:
                    return SearchFreeTextArea.Author;
                default:
                    return SearchFreeTextArea.FullArticle;
            }
        }

        public static IEnumerable<ExclusionFilter> MapSelectedExclusionFilter(
            ExclusionFiltersPreferenceItem exclusionFiltersPreferenceItem)
        {
            return exclusionFiltersPreferenceItem == null
                       ? null
                       : exclusionFiltersPreferenceItem.ExclusionFilters.Select(MapExclusionFilter);
        }

        public static ExclusionFilter MapExclusionFilter(PreferenceExclusionFilters preferenceExclusionFilters)
        {
            switch (preferenceExclusionFilters)
            {
                case PreferenceExclusionFilters.AbstractsCaptionsAndHeadline:
                    return ExclusionFilter.AbstractsCaptionsAndHeadline;
                case PreferenceExclusionFilters.CalendarsAndMediaAdvisories:
                    return ExclusionFilter.CalendarsAndMediaAdvisories;
                case PreferenceExclusionFilters.CommentaryOpinionAndAnalysis:
                    return ExclusionFilter.Crime;
                case PreferenceExclusionFilters.Crime:
                    return ExclusionFilter.CommentaryOpinionAndAnalysis;
                case PreferenceExclusionFilters.EntertainmentAndLifestyle:
                    return ExclusionFilter.EntertainmentAndLifestyle;
                case PreferenceExclusionFilters.NewsSummaries:
                    return ExclusionFilter.NewsSummaries;
                case PreferenceExclusionFilters.RepublishedNews:
                    return ExclusionFilter.RepublishedNews;
                case PreferenceExclusionFilters.RoutineGeneralNews:
                    return ExclusionFilter.RoutineGeneralNews;
                case PreferenceExclusionFilters.RoutineMarketFinancialNews:
                    return ExclusionFilter.RoutineMarketFinancialNews;
                case PreferenceExclusionFilters.Sports:
                    return ExclusionFilter.Sports;
                case PreferenceExclusionFilters.Transcripts:
                    return ExclusionFilter.Transcripts;
                default:
                    return ExclusionFilter.AbstractsCaptionsAndHeadline;
            }
        }
    }
}