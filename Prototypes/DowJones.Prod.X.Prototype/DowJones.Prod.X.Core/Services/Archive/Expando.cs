using DowJones.Assemblers.Articles;

namespace DowJones.Prod.X.Core.Services.Archive
{
    public static class Expando
    {
        public static void Expand(this ArticleConversionManager articleConversionManger, ConvertionOptions options)
        {
            articleConversionManger.ShowCompanyEntityReference = options.ShowCompanyEntityReference;
            articleConversionManger.ShowExecutiveEntityReference = options.ShowExecutiveEntityReference;
            articleConversionManger.EnableELinks = options.EnableELinks;
            articleConversionManger.EmbedHtmlBasedArticles = options.EmbedHtmlBasedArticles;
            articleConversionManger.EmbedHtmlBasedExternalLinks = options.EmbedHtmlBasedExternalLinks;
            articleConversionManger.ShowImagesAsFigures = options.ShowImagesAsFigures;
            articleConversionManger.SuppressLinksInHeadlineTitle = options.SuppressLinksInHeadlineTitle;
            articleConversionManger.EmbededImageType = options.EmbededImageType;
            articleConversionManger.PictureSize = options.PictureSize;
            articleConversionManger.EmbededImageType = options.EmbededImageType;
        }
    }
}
