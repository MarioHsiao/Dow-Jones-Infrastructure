// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleControllerBase.cs" company="Dow Jones">
//       All Rights Resevered. A Dow Jones Company.
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Articles;
using DowJones.Assemblers.Articles;
using DowJones.Web.Mvc.Search.ViewModels;
using DowJones.Web.Mvc.UI.Components.Article;
using Factiva.Gateway.Messages.Archive.V2_0;

namespace DowJones.Web.Mvc.Search.Controllers
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ArticleControllerBase : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ArticleConversionManager _articleConversionManger;

        public ArticleControllerBase(IArticleService articleService, ArticleConversionManager articleConversionManger)
        {
            _articleService = articleService;
            this._articleConversionManger = articleConversionManger;
            this._articleConversionManger.ShowCompanyEntityReference = true;
            this._articleConversionManger.ShowExecutiveEntityReference = false;
            this._articleConversionManger.EnableELinks = true;
        }

        /// <summary>
        /// The articles.
        /// </summary>
        /// <param name="refs">The refs.</param>
        /// <param name="highlight">The highlight.</param>
        /// <param name="options">The options.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>
        /// An ActionResult object
        /// </returns>
        public virtual ActionResult Articles(IEnumerable<ArticleReference> refs, string highlight, DisplayOptions options = DisplayOptions.Full, string usage = null)
        {
            var request = new MixedContentArticleRequest
                              {
                                  ArticleReferences = new List<ArticleReference>(refs), 
                                  CanonicalSearchString = highlight, // TODO::Hrushi: Get this from request, comes from search/alert result response
                                  ResponseDataSet = new ResponseDataSet
                                                        {
                                                            articleFormat = Map(options)
                                                        }, 
                                  UsageAggregator = usage, // TODO::Hrushi: Get this from request, only time needed when you view article for Alert headlines 
                              };

            var response = this._articleService.GetArticles(request);
            var articlesModel = new ArticlesModel(response, _articleConversionManger)
                                    {
                                        ShowPostProcessing = true,
                                        ShowReadSpeaker = false,
                                        ShowSocialButtons = false,
                                        ShowTranslator = false,
                                    };

            return ViewComponent(articlesModel);
        }

        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>An ArticleFormatType.</returns>
        private static ArticleFormatType Map(DisplayOptions options)
        {
            switch (options)
            {
                case DisplayOptions.Headline:
                    return ArticleFormatType.HLPI;
                case DisplayOptions.Indexing:
                    return ArticleFormatType.FULR;
                case DisplayOptions.Keywords:
                    return ArticleFormatType.KWIC;
                default:
                    return ArticleFormatType.FULL;
            }
        }
    }
}