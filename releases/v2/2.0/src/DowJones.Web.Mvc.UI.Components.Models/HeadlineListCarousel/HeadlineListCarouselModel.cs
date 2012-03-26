using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DowJones.Ajax.HeadlineList;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Common.Types;

namespace DowJones.Web.Mvc.UI.Components.HeadlineListCarousel
{
    public enum HeadlineListCarouselOrientation
    {
        Vertical = 0,
        Horizontal
    }

    public class HeadlineListCarouselModel : ViewComponentModel
    {
        #region Event Handlers

        /// <summary>
        /// Gets or sets the client side OnHeadlineClick event handler.
        /// </summary>
        /// <value>The OnHeadlineClick event handler name.</value>
        [ClientEventHandler("dj_headlineListCarousel.HeadlineClick")]
        public string OnHeadlineClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnExtensionItemClick event handler.
        /// </summary>
        /// <value>The OnExtensionItemClick event handler name.</value>
        [ClientEventHandler("dj_headlineListCarousel.ExtensionItemClick")]
        public string OnExtensionItemClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnHeadlineDeleteClick event handler.
        /// </summary>
        /// <value>The OnHeadlineDeleteClick event handler name.</value>
        [ClientEventHandler("dj_headlineListCarousel.HeadlineImageClick")]
        public string OnHeadlineImageClick { get; set; }

        #endregion

        #region Options

        [ClientProperty("numberOfHeadlinesToScrollBy")]
        public int NumberOfHeadlinesToScrollBy { get; set; }

        [ClientProperty("displaySnippets")]
        public SnippetDisplayType DisplaySnippets { get; set; }

        [ClientProperty("displayTime")]
        public bool DisplayTime { get; set; }

        [ClientProperty("extension")]
        public string Extension { get; set; }

        [ClientProperty("skinName")]
        public string SkinName { get; set; }

        [ClientProperty("orientation")]
        [TypeConverter(typeof(StringConverter))]
        public HeadlineListCarouselOrientation Orientation { get; set; }

        [ClientProperty("autoScrollSpeed")]
        public string AutoScrollSpeed { get; set; }

        #endregion

        [ClientTokens]
        public HeadlineListTokens Tokens { get; set; }

        [ClientData]
        public HeadlineListDataResult Result { get; set; }

        public bool HasHeadlines
        {
            get { return Result == null || Result.resultSet == null || Result.resultSet.count.IsPositive; }
        }

        public IEnumerable<HeadlineModel> Headlines
        {
            get
            {
                return (HasHeadlines)
                    ? Enumerable.Empty<HeadlineModel>()
                    : Result.resultSet.headlines.Select(x => new HeadlineModel(x));
            }
        }


        public HeadlineListCarouselModel()
        {
            DisplaySnippets = SnippetDisplayType.None;
            DisplayTime = true;
            Extension = string.Empty;
            NumberOfHeadlinesToScrollBy = 3;
            Tokens = new HeadlineListTokens();
            SkinName = "consultant";
        }
    }

    public class HeadlineModel
    {
        public HeadlineInfo HeadlineInfo
        {
            get { return headlineInfo; }
            set
            {
                // Accept a null HeadlineInfo, 
                // but don't ever let headlineInfo be null
                headlineInfo = value ?? new HeadlineInfo();
            }
        }
        private HeadlineInfo headlineInfo;

        public bool HasThumbnailImage
        {
            get { return HeadlineInfo.thumbnailImage != null; }
        }

        public string ThumbnailHref
        {
            get
            {
                if (!HasThumbnailImage)
                    return string.Empty;

                var href = HeadlineInfo.thumbnailImage.URI;
                if (string.IsNullOrWhiteSpace(href))
                {
                    href = "javascript:void(0)";
                }

                return href;
            }
        }

        public string ThumbnailSourceUri
        {
            get
            {
                if (HasThumbnailImage)
                    return HeadlineInfo.thumbnailImage.SRC ?? string.Empty;
                else
                    return string.Empty;
            }
        }

        public string ExternalUri
        {
            get
            {
                if (HeadlineInfo.reference != null)
                    return HeadlineInfo.reference.externalUri ?? string.Empty;
                else
                    return string.Empty;
            }
        }

        public IEnumerable<string> TitleItems
        {
            get
            {
                if (HeadlineInfo.title != null)
                    return HeadlineInfo.title
                            .SelectMany(title => title.items)
                            .Where(item => item != null)
                            .Select(item => item.value)
                            .ToArray();
                else
                    return Enumerable.Empty<string>();
            }
        }

        public string Time
        {
            get { return HeadlineInfo.time ?? string.Empty; }
        }



        public HeadlineModel(HeadlineInfo headlineInfo = null)
        {
            HeadlineInfo = headlineInfo;
        }
    }
}