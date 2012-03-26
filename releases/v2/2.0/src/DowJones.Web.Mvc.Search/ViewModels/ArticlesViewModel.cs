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
using DowJones.Web.Mvc.UI.Components.Models.Article;
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
        private bool showPostProcessing;

        /// <summary>
        /// Gets or sets a value indicating whether ShowSourceLinks.
        /// </summary>
        private bool showReadSpeaker;

        private bool showSocialButtons;
        private bool showTranslator;
        private bool showSourceLinks;
        private bool showAuthorLinks;

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

        public bool ShowReadSpeaker
        {
            get { return showReadSpeaker; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ShowReadSpeaker = value;
                    }
                }
                showReadSpeaker = value;
            }
        }

        public bool ShowSourceLinks
        {
            get { return showSourceLinks; }
            set
            {
                if( ArticleModels.Count > 0 )
                {
                    foreach( var articleModel in ArticleModels )
                    {
                        articleModel.ShowSourceLinks = value;
                    }
                }
                showSourceLinks = value;
            }
        }

        public bool ShowAuthorLinks
        {
            get
            {
                return showAuthorLinks;
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
                showAuthorLinks = value;
            }
        }

        public bool ShowSocialButtons
        {
            get { return showSocialButtons; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ShowSocialButtons = value;
                    }
                }
                showSocialButtons = value;
            }
        }

        public bool ShowTranslator
        {
            get { return showTranslator; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ShowTranslator = value;
                    }
                }
                showTranslator = value;
            }
        }

        public bool ShowPostProcessing
        {
            get { return showPostProcessing; }
            set
            {
                if (ArticleModels.Count > 0)
                {
                    foreach (var articleModel in ArticleModels)
                    {
                        articleModel.ShowPostProcessing = value;
                    }
                }
                showPostProcessing = value;
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