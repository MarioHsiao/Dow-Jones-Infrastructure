using DowJones.Ajax;
using DowJones.Ajax.Article;
using DowJones.Ajax.PortalArticle;
using DowJones.Articles;
using DowJones.Assemblers.Articles;
using DowJones.Exceptions;
using DowJones.Prod.X.Core.Interfaces;
using DowJones.Web.Mvc.UI.Components.Article;
using Factiva.Gateway.Messages.Archive.V2_0;

namespace DowJones.Prod.X.Core.Services.Archive
{
    public class ArticleRetrivalService : IArticleRetrivalService
    {

        private readonly IArticleService _articleService;
        private readonly ArticleConversionManager _articleConversionManger;

        public ArticleRetrivalService(IArticleService articleService, ArticleConversionManager articleConversionManager)
        {
            _articleService = articleService;
            _articleConversionManger = articleConversionManager;
        }

        public PortalArticleResultSet GetPortalArticleResultSet(Article article, ConvertionOptions options)
        {
            return Mapper.Map<PortalArticleResultSet>(GetArticle(article, options));
        }

        public PortalArticleResultSet GetPortalArticleResultSet(string accessionNumber, ConvertionOptions options, string canonicalSearchString = null)
        {
            var article = _articleService.GetArticle(accessionNumber, canonicalSearchString ?? string.Empty);

            if (article.status != null && article.status.value != 0)
            {
                throw new DowJonesUtilitiesException(article.status.value);
            }

            return GetPortalArticleResultSet(article, options);
        }

        public ArticleModel GetArticleModel(Article article, ConvertionOptions options)
        {
            var articleDataSet = GetArticle(article, options);
            var model = new ArticleModel
            {
                ArticleDataSet = articleDataSet,
                ArticleDisplayOptions = options.ArticleDisplayOptions,
                ShowPostProcessing = options.ShowPostProcessing,
                ShowSourceLinks = options.ShowSourceLinks,
                ShowSocialButtons = options.ShowSocialButtons,
                PostProcessingOptions = options.PostProcessingOptions,
                EnableTitleAsLink = options.EnableTitleAsLink,
                PostProcessing = options.PostProcessing,
                ShowAuthorLinks = options.ShowAuthorLinks,
                SocialButtons = options.SocialButtons,
            };
            return model;
        }

        public ArticleModel GetArticleModel(string accessionNumber, ConvertionOptions options, string canonicalSearchString = null)
        {
            var article = _articleService.GetArticle(accessionNumber, canonicalSearchString ?? string.Empty);

            if (article.status != null && article.status.value != 0)
            {
                throw new DowJonesUtilitiesException(article.status.value);
            }

            return GetArticleModel(article, options);
        }

        public string GetWebArticleUrl(string accessionNumber)
        {
            return _articleService.GetWebArticleUrl(accessionNumber);
        }

        public string GetMultiMediaArticleUrl(string accessionNumber)
        {
            return _articleService.GetMultiMediaArticleUrl(accessionNumber);
        }

        public GetBinaryResponse GetBinary(string accessionNumber, string reference, string imageType, string mimeType)
        {
            return _articleService.GetBinary(new GetBinaryRequest
            {
                reference = reference,
                imageType = imageType,
                mimeType = mimeType,
                accessionNumber = accessionNumber
            });
        }

        private ArticleResultset GetArticle(Article article, ConvertionOptions options)
        {
            if (options != null)
            {
                _articleConversionManger.Expand(options);
            }

            var articleDataSet = _articleConversionManger.Convert(article);
            if (articleDataSet != null)
            {
                if (articleDataSet.ContentSubCategory == ContentSubCategory.HTML ||
                    articleDataSet.ContentSubCategory == ContentSubCategory.PDF)
                {
                    articleDataSet.LeadParagraph = null;
                    articleDataSet.TailParagraphs = null;
                    articleDataSet.Notes = null;
                }

                return articleDataSet;
            }

            return null;
        }

        public ArticleResultset GetArticle(string accessionNumber, ConvertionOptions options, string canonicalSearchString = null)
        {
            var article = _articleService.GetArticle(accessionNumber, canonicalSearchString ?? string.Empty);

            if (article.status != null && article.status.value != 0)
            {
                throw new DowJonesUtilitiesException(article.status.value);
            }

            return GetArticle(article, options);
        }
    }
}
