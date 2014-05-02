using System;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.HeadlineList;

namespace DowJones.Web.Mvc.UI.Components.PortalHeadlineList
{
    public enum PortalHeadlineListLayout
    {
        HeadlineLayout,
        AuthorLayout,
        TimelineLayout,
    }

    public class PortalHeadlineListModel : ViewComponentModel
    {
        const int DefaultNumHeadlinesToShow = 5;
        private bool _sourceClickable;
        private bool _authorClickable;

        #region ..:: Client Properties ::..

        /// <summary>
        /// Gets or sets the maximum number of headlines to show.
        /// </summary>
        /// <value>The max num headlines to show.</value>
        [ClientProperty("maxNumHeadlinesToShow")]
        public int MaxNumHeadlinesToShow { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of headlines to show.
        /// </summary>
        /// <value>The max num headlines to show.</value>
        [ClientProperty("layout")]
        public PortalHeadlineListLayout Layout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected GUID.
        /// </summary>
        /// <value>
        /// The selected GUID.
        /// </value>
        [ClientProperty("selectedGuid")]
        public string SelectedGuid { get; set; }

        /// <summary>
        /// Gets or sets the display snippets.
        /// </summary>
        /// <value>The display snippets.</value>
        [ClientProperty("displaySnippets")]
        public SnippetDisplayType DisplaySnippets { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display no results token.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if display no results token; otherwise, <c>false</c> hide it.
        /// </value>
        [ClientProperty("displayNoResultsToken")]
        public bool DisplayNoResultsToken { get; set; }      
        
        /// <summary>
        /// Gets or sets a value indicating whether 'Source' should be rendered as a hyperlink.
        /// </summary>
        /// <value><c>true</c> if there is an event handler attached to OnSourceClick; otherwise, <c>false</c>.</value>
        [ClientProperty("sourceClickable")]
        public bool SourceClickable
        {
            get { return  _sourceClickable || !string.IsNullOrWhiteSpace(OnSourceClick); }
            set { _sourceClickable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether 'Author' should be rendered as a hyperlink.
        /// </summary>
        /// <value><c>true</c> if there is an event handler attached to OnAuthorClick; otherwise, <c>false</c>.</value>
        [ClientProperty("authorClickable")]
        public bool AuthorClickable
        {
            get { return _authorClickable || !string.IsNullOrWhiteSpace(OnAuthorClick); }
            set { _authorClickable = value; }
        }

        /// <summary>
        /// Gets or Sets a value indicating whether 'Author' should be rendered (as a span).
        /// </summary>
        [ClientProperty("showAuthor")]
        public bool ShowAuthor { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether 'Source' should be rendered.
        /// </summary>
        [ClientProperty("showSource")]
        public bool ShowSource { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether 'Source' should be rendered.
        /// </summary>
        [ClientProperty("showPublicationDateTime")]
        public bool ShowPublicationDateTime { get; set; }

        [ClientProperty("useTimeLineLayout")]
        [Obsolete("Set layout to TimelineLayout")]
        public bool UseTimeLineLayout { 
            get
            {
                return ( Layout == PortalHeadlineListLayout.TimelineLayout );
            }
 
            set
            {
                if (value)
                {
                    Layout = PortalHeadlineListLayout.TimelineLayout;
                }
            } 
        }

        /// <summary>
        /// Gets or Sets a value indicating whether truncated title should be rendered.
        /// </summary>
        [ClientProperty("showTruncatedTitle")]
        public bool ShowTruncatedTitle { get; set; }


        /// <summary>
        /// Gets or Sets a value indicating whether multimedia related icons/divs should be shown.
        /// </summary>
        [ClientProperty("multimediaMode")]
        public bool MultimediaMode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether headlines can be paginated.
		/// </summary>
		/// <value>
		///   <c>true</c> if [allow pagination]; otherwise, <c>false</c>.
		/// </value>
		[ClientProperty("allowPagination")]
		public bool AllowPagination { get; set; }

		/// <summary>
		/// Gets or sets the size of the page (no. of items to scroll) when pagination is enabled.
		/// </summary>
		/// <value>
		/// The size of the page.
		/// </value>
		[ClientProperty("pageSize")]
		public uint PageSize { get; set; }


		/// <summary>
		/// Gets or sets the page direction - horizontal (default) or vertical.
		/// </summary>
		/// <value>
		/// The page direction.
		/// </value>
		[ClientProperty("pageDirection")]
		public Direction PageDirection { get; set; }

		/// <summary>
		/// Gets or sets the page prev selector.
		/// </summary>
		/// <value>
		/// The page prev selector.
		/// </value>
		[ClientProperty("pagePrevSelector")]
		public string PagePrevSelector { get; set; }

		/// <summary>
		/// Gets or sets the page next selector.
		/// </summary>
		/// <value>
		/// The page next selector.
		/// </value>
		[ClientProperty("pageNextSelector")]
		public string PageNextSelector { get; set; }

        /// <summary>
        /// Gets or sets the additional css class.
        /// </summary>
        /// <value>
        /// The additional css class.
        /// </value>
        [ClientProperty("extraCssClass")]
        public string ExtraCssClass { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether 'Source' should be rendered.
        /// </summary>
        [ClientProperty("showWordCount")]
        public bool ShowWordCount { get; set; }
        #endregion


        #region ..:: Client Data ::..

        [ClientData]
        public PortalHeadlineListDataResult Result { get; set; }

        #endregion


        #region ..:: Client Tokens ::..

        [ClientTokens]
        public HeadlineListTokens Tokens { get; set; }

        #endregion


        #region ..:: Client Event Handlers ::..

        /// <summary>
        /// Gets or sets the client side OnHeadlineClick event handler.
        /// </summary>
        /// <value>The OnHeadlineClick event handler name.</value>
        [ClientEventHandler("dj.PortalHeadlineList.headlineClick")]
        public string OnHeadlineClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnSourceClick event handler.
        /// </summary>
        /// <value>The OnSourceClick event handler name.</value>
        [ClientEventHandler("dj.PortalHeadlineList.sourceClick")]
        public string OnSourceClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnAuthorClick event handler.
        /// </summary>
        /// <value>The OnAuthorClick event handler name.</value>
        [ClientEventHandler("dj.PortalHeadlineList.authorClick")]
        public string OnAuthorClick { get; set; }

        #endregion      

        private void InitializeDefaults()
        {
            Tokens = new HeadlineListTokens();

            // setting defaults
            MaxNumHeadlinesToShow = DefaultNumHeadlinesToShow;
            ShowSource = true;
            ShowAuthor = true;
            ShowPublicationDateTime = true;
            ShowTruncatedTitle = true;
            SourceClickable = true;
            AuthorClickable = true;
            DisplaySnippets = SnippetDisplayType.Hover;
            ExtraCssClass = "";
            ShowWordCount = false;
        }

        public PortalHeadlineListModel()
        {
              InitializeDefaults();
        }

        public PortalHeadlineListModel(PortalHeadlineListDataResult dataResult = null)
        {
            Result = dataResult; 
            InitializeDefaults();
        }      
    }
}
