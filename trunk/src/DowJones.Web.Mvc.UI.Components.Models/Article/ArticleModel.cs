// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Article.cs" company="Dow Jones & Company">
//   © 2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The article model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Ajax;
using DowJones.Ajax.Article;
using DowJones.Articles;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using DowJones.Web.Mvc.UI.Components.VideoPlayer;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Article
{
    public class ParagraphModel : ViewComponentModel
    {
        public IEnumerable<IRenderItem> Items { get; set; }
        public string ClassName { get; set; }
        public string TagName { get; set; }

        public ParagraphModel()
        {
            Items = Enumerable.Empty<IRenderItem>();
        }
    }

    public class ArticleRef
    {

        [DataMember(Name = "guid")]
        [JsonProperty("guid")]
        public string AccessionNo
        {
            get;
            set;
        }

        [DataMember(Name = "contentCategory")]
        [JsonProperty("contentCategory")]
        public ContentCategory ContentCategory
        {
            get;
            set;
        }

        [DataMember(Name = "contentSubCategory")]
        [JsonProperty("contentSubCategory")]
        public ContentSubCategory ContentSubCategory
        {
            get;
            set;
        }

        [DataMember(Name = "contentCategoryDescriptor")]
        [JsonProperty("contentCategoryDescriptor")]
        public string ContentCategoryDescriptor
        {
            get;
            set;
        }

        [DataMember(Name = "contentSubCategoryDescriptor")]
        [JsonProperty("contentSubCategoryDescriptor")]
        public string ContentSubCategoryDescriptor
        {
            get;
            set;
        }

        [DataMember(Name = "originalContentCategory")]
        [JsonProperty("originalContentCategory")]
        public string OriginalContentCategory
        {
            get;
            set;
        }

        [DataMember(Name = "externalUri")]
        [JsonProperty("externalUri")]
        public string ExternalUri
        {
            get;
            set;
        }

        [DataMember(Name = "mimetype")]
        [JsonProperty("mimetype")]
        public string MimeType
        {
            get;
            set;
        }

        [DataMember(Name = "ref")]
        [JsonProperty("ref")]
        public string @ref
        {
            get;
            set;
        }

        [DataMember(Name = "subType")]
        [JsonProperty("subType")]
        public string SubType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// The article model.
    /// </summary>
    public class ArticleModel : ViewComponentModel
    {
        #region Client Properties

        /// <summary>
        /// Gets or sets a value indicating whether ShowSocialButtons.
        /// </summary>
        [ClientProperty("showSocialButtons")]
        public bool ShowSocialButtons { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowTranslator.
        /// </summary>
        [ClientProperty("showTranslator")]
        public bool ShowTranslator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowPostProcessing.
        /// </summary>
        [ClientProperty("showPostProcessing")]
        public bool ShowPostProcessing { get; set; }

        /// <summary>
        /// Gets or sets the post processing options.
        /// </summary>
        /// <value>
        /// The post processing options.
        /// </value>
        public IEnumerable<PostProcessingOptions> PostProcessingOptions { get; set; }

        #endregion
        
        /// <summary>
        /// Gets or sets ArticleDataSet.
        /// </summary>
        public ArticleResultset ArticleDataSet { get; set; }

        /// <summary>
        /// Gets or sets SocialButtons.
        /// </summary>
        public SocialButtons.SocialButtonsModel SocialButtons { get; set; }


        public VideoPlayerModel VideoPlayer { get; set; }

        /// <summary>
        /// Gets a value indicating whether HasSocialNetworks.
        /// </summary>
        public bool HasSocialNetworks
        {
            get
            {
                return SocialButtons != null && SocialButtons.SocialNetworks.Any();
            }
        }

        /// <summary>
        /// Gets or sets Translator.
        /// </summary>
        public ArticleTranslator.ArticleTranslatorModel Translator { get; set; }

        /// <summary>
        /// Gets a value indicating whether HasTranslator.
        /// </summary>
        public bool HasTranslator
        {
            get { return Translator != null; }
        }

        public bool EnableTitleAsLink { get; set; }

        public bool ShowSourceLinks { get; set; }

        public bool ShowAuthorLinks { get; set; }

        public ArticleReference ArticleReference { get; set; }
                                                                   
        /// <summary>
        /// Gets or sets ArticleDisplayOptions.
        /// </summary>
        public DisplayOptions ArticleDisplayOptions { get; set; }

        /// <summary>
        /// Gets or sets PostProcessing.
        /// </summary>
        public DowJones.Infrastructure.PostProcessing PostProcessing { get; set; }

        public ParagraphModel GetParagraphModel(IEnumerable<IRenderItem> items, string tagName = "p", string className = "")
        {
            return new ParagraphModel
            {
                Items = items ?? Enumerable.Empty<IRenderItem>(),
                TagName = tagName,
                ClassName = className,
            };
        }

        public ArticleRef ArticleRef
        {
            get
            {
                if (_articleRef == null && ArticleDataSet != null)
                {
                    _articleRef = new ArticleRef
                    {
                        AccessionNo = ArticleDataSet.AccessionNo,
                        ContentCategory = ArticleDataSet.ContentCategory,
                        ContentCategoryDescriptor = ArticleDataSet.ContentCategoryDescriptor,
                        ContentSubCategory = ArticleDataSet.ContentSubCategory,
                        ContentSubCategoryDescriptor = ArticleDataSet.ContentSubCategoryDescriptor,
                        OriginalContentCategory = ArticleDataSet.OriginalContentCategory,
                        ExternalUri = ArticleDataSet.ExternalUri,
                        MimeType = ArticleDataSet.MimeType,
                        @ref = ArticleDataSet.Ref,
                        SubType = ArticleDataSet.SubType
                    };
                }

                return _articleRef ?? new ArticleRef();
            }
            set { _articleRef = value; }
        }
        private ArticleRef _articleRef;
    }
}