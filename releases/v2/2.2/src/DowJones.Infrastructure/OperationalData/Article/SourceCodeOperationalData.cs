using System;
using System.Collections.Generic;
using DowJones.OperationalData.EntryPoint;

namespace DowJones.OperationalData.Article
{
    public class SourceCodeOperationalData : AbstractOperationalData
    {
        private bool sourceCodeSpecified;
       
        /// <summary>
        /// Gets or sets the source code. This is required.
        /// </summary>
        /// <value>The source code.</value>
        public string SourceCode
        {
            get { return Get(ODSConstants.KEY_SOURCE_CODE); }
            set
            {
                Add(ODSConstants.KEY_SOURCE_CODE, value);
                sourceCodeSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets the source name.
        /// </summary>
        /// <value>The source name.</value>
        public string SourceName
        {
            get { return Get(ODSConstants.KEY_SOURCE_NAME); }
            set { Add(ODSConstants.KEY_SOURCE_NAME, value); }
        }

        /// <summary>
        /// Gets or sets the publisher name.
        /// </summary>
        /// <value>The publisher name.</value>
        public string PublisherName
        {
            get { return Get(ODSConstants.KEY_PUBLISHER_NAME); }
            set { Add(ODSConstants.KEY_PUBLISHER_NAME, value); }
        }

        /// <summary>
        /// Gets or sets the content language.
        /// </summary>
        /// <value>The content language.</value>
        public string ContentLanguage
        {
            get { return Get(ODSConstants.KEY_CONTENT_LANGUAGE); }
            set { Add(ODSConstants.KEY_CONTENT_LANGUAGE, value); }
        }

        /// <summary>
        /// Gets or sets the article category.
        /// </summary>
        /// <value>The article category.</value>
        public ArticleCategoryType ArticleCategory
        {
            get { return (ArticleCategoryType) Enum.Parse(typeof (ArticleCategoryType), Get(ODSConstants.KEY_ARTICLE_CATEGORY)); }  
            set { Add(ODSConstants.KEY_ARTICLE_CATEGORY, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the article category detail.
        /// </summary>
        /// <value>The carticle category detail.</value>
        public ArticleCategorySubType ArticleCategoryDetail
        {
            get { return (ArticleCategorySubType)Enum.Parse(typeof(ArticleCategorySubType), Get(ODSConstants.KEY_ARTICLE_CATEGORY_ADDITIONAL)); }  
            set { Add(ODSConstants.KEY_ARTICLE_CATEGORY_ADDITIONAL, value.ToString()); }
        }

        public override IDictionary<string, string> GetKeyValues
        {
            get
            {
                if (!sourceCodeSpecified)
                {
                    throw new MissingFieldException("Sorce Code is not specified");
                }

                return base.GetKeyValues;
            }
        }

    }
}