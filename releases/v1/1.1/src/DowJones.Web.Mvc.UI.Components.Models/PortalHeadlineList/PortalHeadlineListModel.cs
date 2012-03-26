using System;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Tools.Ajax.PortalHeadlineList;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class PortalHeadlineListModel : ViewComponentModel
    {
        const int DefaultNumHeadlinesToShow = 5;

        #region ..:: Client Properties ::..

        /// <summary>
        /// Gets or sets the maximum number of headlines to show.
        /// </summary>
        /// <value>The max num headlines to show.</value>
        [ClientProperty("maxNumHeadlinesToShow")]
        public int MaxNumHeadlinesToShow { get; set; }

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
        /// Gets a value indicating whether 'Source' should be rendered as a hyperlink.
        /// </summary>
        /// <value><c>true</c> if there is an event handler attached to OnSourceClick; otherwise, <c>false</c>.</value>
        [ClientProperty("sourceClickable")]
        public bool SourceClickable { get { return !string.IsNullOrWhiteSpace(_onSourceClick); } }

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
        public bool UseTimeLineLayout { get; set; }

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
        public string OnSourceClick
        {
            get { return _onSourceClick; }
            set { _onSourceClick = value; }
        }
        private string _onSourceClick;


        #endregion


        public PortalHeadlineListModel(HeadlineListDataResult headlineListDataResult = null)
        {
            if (headlineListDataResult != null)
            {
                Result = PortalHeadlineConversionManager.Convert(headlineListDataResult);
            }
            Tokens = new HeadlineListTokens();

            // setting defaults
            MaxNumHeadlinesToShow = DefaultNumHeadlinesToShow;
            ShowSource = true;
            ShowPublicationDateTime = true;
            ShowTruncatedTitle = true;
            DisplaySnippets = SnippetDisplayType.Hover;
        }

    }
}
