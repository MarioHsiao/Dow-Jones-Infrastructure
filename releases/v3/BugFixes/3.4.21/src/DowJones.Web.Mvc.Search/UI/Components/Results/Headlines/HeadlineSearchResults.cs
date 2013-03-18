using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Articles;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Results;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.DateHistogram;
using DowJones.Web.Mvc.UI.Components.CompositeHeadline;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Discovery;
using DowJones.Web.Mvc.UI.Components.RelatedConcepts;

namespace DowJones.Web.Mvc.Search.UI.Components.Results.Headlines
{

    public class HeadlineSearchResults : SearchResults
    {
        private ShowDuplicates showDuplicates = ShowDuplicates.On;

        public HeadlineSearchResults()
        {
            Headlines = new CompositeHeadlineModel();
            ArticleUrl = "articles";
        }

        public HeadlineSearchResults(CompositeHeadlineModel headlineModel)
        {
            Headlines = new CompositeHeadlineModel();
            ArticleUrl = "articles";
        }

        public string CanonicalQueryString { get; set; }

        public uint Server { get; set; }

        public string ContextId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalResultCount { get; set; }

        public int FirstResultIndex { get; set; }

        public int[] FirstResultIndexReference { get; set; }

        public int DuplicateCount { get; set; }

        public int NextResultIndex
        {
            get
            {
                return FirstResultIndex + PageSize + DuplicateCount;
            }
        }

        public int PreviousResultIndex
        {
            get
            {
                var indexReference = FirstResultIndexReference;
                if (indexReference == null || indexReference.Length < 2)
                {
                    return 0;
                }
                return indexReference[indexReference.Length - 2];
            }
        }

        public DateHistogramModel NewsVolume { get; set; }

        private CompositeHeadlineModel _headlines = null;

        public CompositeHeadlineModel Headlines
        {
            get
            {
                var index = (PageIndex * PageSize);
                var totalDupSofar = (FirstResultIndex - index) + DuplicateCount;
                var delta = TotalResultCount - (index + totalDupSofar);
                var lastIndex = index + (delta < PageSize ? delta : PageSize);

                _headlines.FirstResultIndex = index;
                _headlines.CanPagePrevious = CanPagePrevious;
                _headlines.CanPageNext = CanPageNext;
                _headlines.TotalDuplicateCount = totalDupSofar;
                _headlines.LastResultIndex = lastIndex;

                return _headlines;
            }
            set { _headlines = value; }
        }

        public RelatedConceptsComponentModel RelatedConcepts { get; set; }

        public ShowDuplicates ShowDuplicates
        {
            get
            {
                return this.showDuplicates;
            }

            set
            {
                this.showDuplicates = value;
                var hld = this.Headlines;
                if (hld == null)
                {
                    return;
                }
                hld.ShowDuplicates = this.showDuplicates;
                if (hld.HeadlineList != null)
                {
                    hld.HeadlineList.ShowDuplicates = this.showDuplicates;
                }
            }
        }

        private bool CanPagePrevious
        {
            get { return FirstResultIndex > 0; }
        }

        private bool CanPageNext
        {
            get { return (FirstResultIndex + PageSize + DuplicateCount) < TotalResultCount; }
        }

        public DisplayOptions ArticleDisplayOption { get; set; }

        public IEnumerable<SelectListItem> ArticleDisplayOptions { get; set; }

        public SortOrder HeadlineSort { get; set; }

        [ClientProperty]
        public string ArticleUrl { get; set; }

        public override bool HasResults
        {
            get { return TotalResultCount > 0; }
        }

        public bool HideRelatedConcepts { get; set; }

        public bool HideNewsVolume { get; set; }

        [ClientProperty]
        public ResultsLayout Layout { get; set; }

        public PictureSize PictureSize { get; set; }
    }
}