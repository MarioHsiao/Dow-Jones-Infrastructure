// -----------------------------------------------------------------------
// <copyright file="ArticlesViewModel.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DowJones.Assemblers.Articles;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using Factiva.Gateway.Messages.Archive.V2_0;

namespace DowJones.Web.Mvc.Search.ViewModels
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Guid("1512EDF7-ECA2-4E45-BB2C-4C3812D45447")]
    public class ArticlesModel : ViewComponentModel
    {
        private DisplayOptions _articleDisplayOptions = DisplayOptions.Full;
        private bool _showPostProcessing;
        private bool _showSocialButtons;
        private bool _showTranslator;
        private bool _showSourceLinks;
        private bool _showAuthorLinks;
        private IEnumerable<PostProcessingOptions> _postProcessingOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticlesModel"/> class.
        /// </summary>
        /// <param name="articleModels">The article models.</param>
        public ArticlesModel(IList<ArticleModel> articleModels)
        {
            ArticleModels = articleModels;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticlesModel"/> class.
        /// </summary>
        /// <param name="responseSet">The response set.</param>
        /// <param name="manager"></param>
        public ArticlesModel(ArticleResponseSet responseSet, ArticleConversionManager manager)
        {
            ArticleModels = (from article in responseSet.article
                             select new ArticleModel
                                        {
                                            ArticleDataSet = manager.Convert(article),
                                        }).ToList();
        }

        public IList<ArticleModel> ArticleModels { get; set; }

        public bool ShowSourceLinks
        {
            get { return _showSourceLinks; }
            set
            {
                if( ArticleModels.Count > 0 )
                {
                    foreach( var articleModel in ArticleModels )
                    {
                        articleModel.ShowSourceLinks = value;
                    }
                }
                _showSourceLinks = value;
            }
        }

        public bool ShowAuthorLinks
        {
            get
            {
                return _showAuthorLinks;
            }
            set
            {
                if( ArticleModels.Count > 0 )
                {
                    foreach( var articleModel in ArticleModels )
                    {
                        articleModel.ShowAuthorLinks = value;
                    }
                }
                _showAuthorLinks = value;
            }
        }

        public bool ShowSocialButtons
        {
            get { return _showSocialButtons; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ShowSocialButtons = value;
                    }
                }
                _showSocialButtons = value;
            }
        }

        public bool ShowTranslator
        {
            get { return _showTranslator; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ShowTranslator = value;
                    }
                }
                _showTranslator = value;
            }
        }

        public bool ShowPostProcessing
        {
            get { return _showPostProcessing; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ShowPostProcessing = value;
                    }
                }
                _showPostProcessing = value;
            }
        }


        /// <summary>
        /// Gets or sets the post processing options.
        /// </summary>
        /// <value>
        /// The post processing options.
        /// </value>
        public IEnumerable<PostProcessingOptions> PostProcessingOptions
        {
            get { return _postProcessingOptions; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.PostProcessingOptions = value;
                    }
                }
                _postProcessingOptions = value;
            }
        }

        public DisplayOptions ArticleDisplayOptions
        {
            get { return _articleDisplayOptions; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ArticleDisplayOptions = value;
                    }
                }
                _articleDisplayOptions = value;
            }
        }
    }
}