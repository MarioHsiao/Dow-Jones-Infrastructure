using DowJones.OperationalData.EntryPoint;

namespace DowJones.OperationalData.Article
{
    /// <summary>
    /// Represents the OperationalData for the origin (Search Pages) from where the article was viewed in product.
    /// </summary>
    public class ArticleSearchViewOperationalData : BaseArticleOperationalData
    {
        public ArticleSearchViewOperationalData()
        {
            Origin = OriginType.Search;
        }

        /// <summary>
        /// Gets or sets the article origin details (from where the article was viewed).
        /// </summary>
        /// <value>The article origin details.</value>
        public string OriginDetails
        {
            get { return Get(ODSConstants.KEY_ORIGIN_ADDITIONAL); }
            set { Add(ODSConstants.KEY_ORIGIN_ADDITIONAL, value); }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the origin (Alert Page) from where the article was viewed in product.
    /// </summary>
    public class ArticleAlertViewOperationalData : BaseArticleOperationalData
    {
        public ArticleAlertViewOperationalData()
        {
            Origin = OriginType.Alert;
        }

        /// <summary>
        /// Gets or sets the article origin details (from where the article was viewed).
        /// </summary>
        /// <value>The article origin details.</value>
        public string OriginDetails
        {
            get { return Get(ODSConstants.KEY_ORIGIN_ADDITIONAL); }
            set { Add(ODSConstants.KEY_ORIGIN_ADDITIONAL, value); }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the origin (Newsletter Page) from where the article was viewed in product.
    /// </summary>
    public class ArticleNewsletterViewOperationalData : BaseArticleOperationalData
    {
        public ArticleNewsletterViewOperationalData()
        {
            Origin = OriginType.Newsletter;
        }

        /// <summary>
        /// Gets or sets the article origin details (from where the article was viewed).
        /// </summary>
        /// <value>The article origin details.</value>
        public string OriginDetails
        {
            get { return Get(ODSConstants.KEY_ORIGIN_ADDITIONAL); }
            set { Add(ODSConstants.KEY_ORIGIN_ADDITIONAL, value); }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the origin (Workspace Page) from where the article was viewed in product.
    /// </summary>
    public class ArticleWorkspaceViewOperationalData : BaseArticleOperationalData
    {
        public ArticleWorkspaceViewOperationalData()
        {
            Origin = OriginType.Workspace;
        }

        /// <summary>
        /// Gets or sets the article origin details (from where the article was viewed).
        /// </summary>
        /// <value>The article origin details.</value>
        public string OriginDetails
        {
            get { return Get(ODSConstants.KEY_ORIGIN_ADDITIONAL); }
            set { Add(ODSConstants.KEY_ORIGIN_ADDITIONAL, value); }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the origin (PHP Page) from where the article was viewed in product.
    /// </summary>
    public class ArticlePHPViewOperationalData : BaseArticleOperationalData
    {
        public ArticlePHPViewOperationalData()
        {
            Origin = OriginType.PHP;
        }

        /// <summary>
        /// Gets or sets the article origin details (from where the article was viewed).
        /// </summary>
        /// <value>The article origin details.</value>
        public string OriginDetails
        {
            get { return Get(ODSConstants.KEY_ORIGIN_ADDITIONAL); }
            set { Add(ODSConstants.KEY_ORIGIN_ADDITIONAL, value); }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the origin (Newspage Page) from where the article was viewed in product.
    /// </summary>
    public class ArticleNewspageViewOperationalData : BaseArticleOperationalData
    {
        public ArticleNewspageViewOperationalData()
        {
            Origin = OriginType.Newspage;
        }

        /// <summary>
        /// Gets or sets the article origin details (from where the article was viewed).
        /// </summary>
        /// <value>The article origin details.</value>
        public string OriginDetails
        {
            get { return Get(ODSConstants.KEY_ORIGIN_ADDITIONAL); }
            set { Add(ODSConstants.KEY_ORIGIN_ADDITIONAL, value); }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the origin (PostProcessing Page) from where the article was viewed in product.
    /// </summary>
    public class ArticlePostProcessingViewOperationalData : BaseArticleOperationalData
    {
        public ArticlePostProcessingViewOperationalData()
        {
            Origin = OriginType.PostProcessing;
        }
    }

    /// <summary>
    /// Represents the OperationalData for the origin (Other Pages) from where the article was viewed in product.
    /// </summary>
    public class ArticleOtherViewOperationalData : BaseArticleOperationalData
    {
        public ArticleOtherViewOperationalData()
        {
            Origin = OriginType.Other;
        }
    }


    /// <summary>
    /// Represents the OperationalData for the origin of an entrypoint where the entry point is logged and 
    /// from where the article was viewed in product.
    /// </summary>
    public class ArticleEntryPointViewOperationalData : BaseArticleOperationalData
    {
        public ArticleEntryPointViewOperationalData()
        {
            Origin = OriginType.EntryPoint;
        }

    }

    /// <summary>
    /// Represents the OperationalData for the origin of an entrypoint where the entry point is logged and 
    /// from where the article was viewed in product.
    /// </summary>
    public class ArticleNewsstandViewOperationalData : BaseArticleOperationalData
    {
        public ArticleNewsstandViewOperationalData()
        {
            Origin = OriginType.Newsstand;
        }

    }

    /// <summary>
    /// Represents the OperationalData for the origin of an entrypoint where the entry point is logged and 
    /// from where the article was viewed in product.
    /// </summary>
    public class ArticleEditorsChoiceViewOperationalData : BaseArticleOperationalData
    {
        public ArticleEditorsChoiceViewOperationalData()
        {
            Origin = OriginType.EditorsChoice;
        }

    }
}
