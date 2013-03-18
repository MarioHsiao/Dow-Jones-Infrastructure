using System;
using DowJones.Ajax.HeadlineList;
using System.Collections.Generic;
using System.Linq;
using DowJones.Converters;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using Newtonsoft.Json;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.UI.Components.HeadlineList
{

    /// <summary>
    /// Show Similar, Exact or No duplicates.
    /// </summary>
    /// <remarks>
    /// There might be more values coming in later.
    /// </remarks>
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum ShowDuplicates
    {
        On,
        Off,
    }

    /// <summary>
    /// 
    /// </summary>
    public class HeadlineListModel : ViewComponentModel
    {
        [ClientProperty("showDuplicates")]
        public ShowDuplicates ShowDuplicates { get; set; }

        [ClientProperty("showCheckboxes")]
        public bool ShowCheckboxes { get; set; }

        [ClientProperty("showAccessionNo")]
        public bool ShowAccessionNo { get; set; }

        [ClientProperty("showByLine")]
        public bool ShowByLine { get; set; }

        [ClientProperty("displaySnippets")]
        public SnippetDisplayType DisplaySnippets { get; set; }

        [ClientProperty("showContentType")]
        public bool ShowContentType { get; set; }

        [ClientProperty("showThumbnail")]
        public ThumbnailDisplayType ShowThumbnail { get; set; }

        [ClientProperty("headlineClickable")]
        public bool HeadlineClickable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NLA press clips are enabled.
        /// </summary>
        /// </value>
        [ClientProperty("showPressClip")]
        public bool ShowPressClip { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of headlines that can be selected.
        /// </summary>
        /// <value>
        /// The max select.
        /// </value>
        /// <remarks>Default is 0 which allows unlimited selection</remarks>
        [ClientProperty("maxSelect")]
        public int MaxSelect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether post processing actions are shown or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show post processing]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowPostProcessing { 
            get
            {
                return PostProcessingOptions != null && PostProcessingOptions.Any();
            }
        }

        /// <summary>
        /// Gets or sets the post processing options.
        /// </summary>
        /// <value>
        /// The post processing options.
        /// </value>
        public IEnumerable<PostProcessingOptions> PostProcessingOptions { get; set; }
        

        public double TotalRecords { get; set; }

        public IEnumerable<HeadlineModel> Headlines { get; set; }

        public HeadlineListModel()
        {
            Headlines = Enumerable.Empty<HeadlineModel>();
            MaxSelect = 0;      // allow unlimited selections
        }


        public HeadlineListModel(HeadlineListDataResult headlineResult)
            : this()
        {
            Headlines = headlineResult.resultSet.headlines.Select(h => new HeadlineModel(h));
            TotalRecords = headlineResult.hitCount.Value;
        }

        #region ..:: View Helpers ::..


        /// <summary>
        /// Gets the hover snippet.
        /// </summary>
        /// <param name="snippet">The snippet.</param>
        /// <param name="headlineIndex">Index of the headline.</param>
        /// <returns></returns>
        public string GetHoverSnippet(string snippet, int headlineIndex)
        {
            if (DisplaySnippets == SnippetDisplayType.Hover)
                return snippet.EscapeForHtml();

            // return snippet for the first headline line only
            if (DisplaySnippets == SnippetDisplayType.HybridHover && headlineIndex == 0)
                return snippet.EscapeForHtml();

            return string.Empty;
        }


        /// <summary>
        /// Determines whether thumbnail is visible for the specified headline index.
        /// </summary>
        /// <param name="headlineIndex">Index of the headline in headlines result set.</param>
        /// <param name="headline">The headline.</param>
        /// <returns>
        ///   <c>true</c> if thumbnail is visible for the specified headline index; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThumbnailVisible(int headlineIndex, HeadlineModel headline)
        {
            switch (ShowThumbnail)
            {
                case ThumbnailDisplayType.Hybrid:
                    return headlineIndex == 0 && (headline != null && headline.HasThumbnail);
                case ThumbnailDisplayType.Inline:
                    return headline != null && headline.HasThumbnail;
                case ThumbnailDisplayType.None:
                default:
                    return false;
            }
        }


        #endregion

    }
}