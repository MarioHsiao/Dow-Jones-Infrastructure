using System.Xml.Serialization;
using Factiva.Gateway.Messages.Assets.Common.V2_0;

namespace EMG.widgets.ui.delegates.core.manualNewsletterWorkspace
{
    /// <summary>
    /// 
    /// </summary>
    public class EnhancedArticleItem : ArticleItem
    {
        private ArticleItem ArticleItem;

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public ArticleItem Original;

        /// <summary>
        /// 
        /// </summary>
        public HeadlineInfo HeadlineInfo;


        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedArticleItem"/> class.
        /// </summary>
        /// <param name="articleItem">The article item.</param>
        public EnhancedArticleItem(ArticleItem articleItem)
        {
            Id = articleItem.Id;
            AccessionNumber = articleItem.AccessionNumber;
            Comment = articleItem.Comment;
            ContentCategory = articleItem.ContentCategory;
            CreationDate = articleItem.CreationDate;
            Importance = articleItem.Importance;
            Position = articleItem.Position;
            Status = articleItem.Status;
            ArticleItem = articleItem;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedArticleItem"/> class.
        /// </summary>
        /// <param name="articleItem">The article item.</param>
        /// <param name="headlineInfo">The headline info.</param>
        public EnhancedArticleItem(ArticleItem articleItem, HeadlineInfo headlineInfo) : this(articleItem)
        {
            HeadlineInfo = headlineInfo;
        }
 

    }
}
