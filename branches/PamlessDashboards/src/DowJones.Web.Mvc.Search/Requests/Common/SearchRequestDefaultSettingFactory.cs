using System;
using DowJones.Articles;
using DowJones.Extensions;
using DowJones.Managers.Search.Preference;
using DowJones.Web.Mvc.Search.Requests.Alert;
using DowJones.Web.Mvc.Search.Results;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using Factiva.Gateway.Messages.Preferences.V1_0;
using SortOrder = DowJones.Search.SortOrder;

namespace DowJones.Web.Mvc.Search.Requests.Common
{
    public interface ISearchRequestDefaultSettingFactory
    {
        SnippetDisplayType DisplaySnippets { get; }
        bool ShowAccessionNo { get; }
        bool ShowByLine { get; }
        bool HighlightSearchTermInDocument { get; }
        PictureSize PictureSize { get; }
        SearchRequest ApplyDefaults(SearchRequest request);
        SearchResultsOptions ApplyDefaults(SearchResultsOptions request);
    }

    public class SearchRequestDefaultSettingFactory : ISearchRequestDefaultSettingFactory
    {
        public SearchRequestDefaultSettingFactory(ISearchPreferenceService searchPreferenceService)
        {
            SearchPreferenceService = searchPreferenceService;
        }

        private ISearchPreferenceService SearchPreferenceService { get; set; }

        #region ISearchRequestDefaultSettingFactory Members

        public bool HighlightSearchTermInDocument
        {
            get { return SearchPreferenceService.Highlight; }
        }

        public PictureSize PictureSize
        {
            get { return SearchPreferenceService.PictureSize; }
        }

        public SearchRequest ApplyDefaults(SearchRequest request)
        {
            if (!request.ShowDuplicates.HasValue)
            {
                request.ShowDuplicates = Map(SearchPreferenceService.Duplicate);
            }
            if (request is SimpleSearchRequest)
            {
                request = ApplyDefaultsForSimpleSearch((SimpleSearchRequest) request);
            }
            else if (request is FreeTextSearchRequest)
            {
                request = ApplyDefaultsForFreetextSearch((FreeTextSearchRequest) request);
            }
            else if (request is AlertSearchRequest)
            {
                request = ApplyDefaultsForAlertResult((AlertSearchRequest) request);
            }

            return request;
        }

        public SearchResultsOptions ApplyDefaults(SearchResultsOptions request)
        {
            if (!request.ArticleDisplayOption.HasValue)
            {
                request.ArticleDisplayOption = Map(SearchPreferenceService.ArticleDisplay);
            }
            if (!request.Layout.HasValue)
            {
                request.Layout = (SearchPreferenceService.ScreenDisplay == PreferenceScreenDisplay.FramesView)
                                     ? ResultsLayout.Split
                                     : ResultsLayout.Full;
            }
            return request;
        }

        public SnippetDisplayType DisplaySnippets
        {
            get { return Map(SearchPreferenceService.HeadlineFormat); }
        }

        public bool ShowAccessionNo
        {
            get { return SearchPreferenceService.IncludeAccessionNumber; }
        }

        public bool ShowByLine
        {
            get { return SearchPreferenceService.IncludeByLine; }
        }

        #endregion

        private SearchRequest ApplyDefaultsForAlertResult(AlertSearchRequest request)
        {
            if (!request.PageSize.HasValue)
            {
                request.PageSize = SearchPreferenceService.AlertPageSize;
            }

            if (!request.Sort.HasValue)
            {
                request.Sort = Map(SearchPreferenceService.AlertSortOption);
            }
            return request;
        }

        private SearchRequest ApplyDefaultsForSimpleSearch(SimpleSearchRequest request)
        {
            ApplyDefaultsForSearchResultView(request);

            if (request.Languages == null || request.Languages.IsEmpty())
            {
                if (SearchPreferenceService.Languages != null && !SearchPreferenceService.Languages.IsEmpty())
                    request.Languages = string.Join(",", SearchPreferenceService.Languages);
            }
            if (request.Source == null)
            {
                request.Source = SearchPreferenceService.DefaultSimpleSearchSources;
            }
            return request;
        }

        private SearchRequest ApplyDefaultsForFreetextSearch(FreeTextSearchRequest request)
        {
            ApplyDefaultsForSearchResultView(request);

            return request;
        }

        private SearchRequest ApplyDefaultsForSearchResultView(SearchRequest request)
        {
            if (!request.PageSize.HasValue)
            {
                request.PageSize = SearchPreferenceService.PageSize;
            }
            if (!request.Sort.HasValue)
            {
                request.Sort = Map(SearchPreferenceService.SortOption);
            }
            return request;
        }


        private SnippetDisplayType Map(PreferenceHeadlineFormat headlineFormat)
        {
            switch (headlineFormat)
            {
                case PreferenceHeadlineFormat.MOUSEOVER:
                    return SnippetDisplayType.Hover;
                case PreferenceHeadlineFormat.NONE:
                    return SnippetDisplayType.None;
                default:
                    return SnippetDisplayType.Inline;
            }
        }

        private static ShowDuplicates Map(PreferenceDeduplication duplicate)
        {
            switch (duplicate)
            {
                case PreferenceDeduplication.HIGH:
                case PreferenceDeduplication.MEDIUM:
                    return ShowDuplicates.On;
                default:
                    return ShowDuplicates.Off;
            }
        }

        private static SortOrder Map(PreferenceSearchSorting sortOption)
        {
            switch (sortOption)
            {
                case PreferenceSearchSorting.OldestFirst:
                    return SortOrder.PublicationDateOldestFirst;
                case PreferenceSearchSorting.Relevance:
                    return SortOrder.Relevance;
                default:
                    return SortOrder.PublicationDateMostRecentFirst;
            }
        }

        private static SortOrder Map(PreferenceAlertSorting sortOption)
        {
            switch (sortOption)
            {
                case PreferenceAlertSorting.OldestFirst:
                    return SortOrder.PublicationDateOldestFirst;
                case PreferenceAlertSorting.Relevance:
                    return SortOrder.Relevance;
                case PreferenceAlertSorting.ArrivalTime:
                    return SortOrder.ArrivalTime;
                default:
                    return SortOrder.PublicationDateMostRecentFirst;
            }
        }

        private static DisplayOptions Map(ArticleDisplayFormat articleDisplay)
        {
            switch (articleDisplay)
            {
                case ArticleDisplayFormat.FullArticlePlusIndexing:
                    return DisplayOptions.Indexing;
                case ArticleDisplayFormat.HeadlineLeadParagraphPlusIndexing:
                    return DisplayOptions.Headline;
                case ArticleDisplayFormat.KeyWordsInContext:
                    return DisplayOptions.Keywords;
                default:
                    return DisplayOptions.Full;
            }
        }
    }
}