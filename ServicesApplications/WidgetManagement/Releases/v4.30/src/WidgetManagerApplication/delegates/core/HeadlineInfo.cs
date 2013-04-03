using System;
using Factiva.BusinessLayerLogic;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using ContentCategory=Factiva.BusinessLayerLogic.ContentCategory;

namespace EMG.widgets.ui.delegates.core
{
    /// <summary>
    /// 
    /// </summary>
    public class HeadlineInfo
    {
        /// <summary>
        /// Acession Number.
        /// </summary>
        public string AccessionNumber;

        /// <summary>
        /// Acession Number.
        /// </summary>
        public string ByLine;

        /// <summary>
        /// 
        /// </summary>
        public ContentLanguage ContentLanguage;

        /// <summary>
        /// 
        /// </summary>
        public ContentCategory ContentCategory = ContentCategory.Unknown;

        /// <summary>
        /// Comments added by user.
        /// </summary>
        public string Comment;

        /// <summary>
        /// 
        /// </summary>
        public string ContentType;

        /// <summary>
        /// Content language.
        /// </summary>
        public string Lang;

        /// <summary>
        /// Merged Publication DateTime
        /// </summary>
        public DateTime PublicationDateTime;

        /// <summary>
        /// Merged Publication DateTime
        /// </summary>
        public String PubDateTime;

        /// <summary>
        /// HTML Text of the snippet.
        /// </summary>
        public string Snippet;

        /// <summary>
        /// Source Code of the Headline.
        /// </summary>
        public string SrcCode;

        /// <summary>
        /// Source Name of the Headline.
        /// </summary>
        public string SrcName;

        /// <summary>
        /// Text of the headline.
        /// </summary>
        public string Text;

        /// <summary>
        /// Trucated text of the headline.
        /// </summary>
        public string TruncText;

        /// <summary>
        /// Url of the headline. This should represent a Cyclone url.
        /// </summary>
        public string Url;

        /// <summary>
        /// Numbers of words in the article.
        /// </summary>
        public string WordCount;

        /// <summary>
        /// Numbers of words in the article.
        /// </summary>
        public int WC;

        /// <summary>
        /// Icon url 
        /// </summary>
        public string IconUrl;

        /// <summary>
        /// Is Factiva Content
        /// </summary>
        public bool IsFactivaContent = true;


        /// <summary>
        /// Importance of the headline.
        /// </summary>
        public Importance Importance = Importance.Normal;
    }
}