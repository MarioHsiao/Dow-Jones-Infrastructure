using System.Collections.Generic;
using DowJones.Articles;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using DowJones.Web.Mvc.UI.Components.SocialButtons;

namespace DowJones.Prod.X.Core.Services.Archive
{
    public class ConvertionOptions
    {
        public bool ShowCompanyEntityReference { get; set; }
        public bool ShowExecutiveEntityReference { get; set; }
        public bool ShowSourceLogo { get; set; }
        public bool ShowImagesAsFigures { get; set; }
        public bool EnableELinks { get; set; }
        public bool EnableEnlargedImage { get; set; }
        public bool EmbedHtmlBasedExternalLinks { get; set; }
        public bool EmbedHtmlBasedArticles { get; set; }
        public bool SuppressLinksInHeadlineTitle { get; set; }
        public bool EnableTitleAsLink { get; set; }
        public bool ShowAuthorLinks { get; set; }
        public string FileHandlerUrl { get; set; }
        public ArticleReference ArticleReference { get; set; }
        public DisplayOptions ArticleDisplayOptions { get; set; }
        public PostProcessing PostProcessing { get; set; }
        public ImageType EmbededImageType { get; set; }
        public PictureSize PictureSize { get; set; }
        public SocialButtonsModel SocialButtons { get; set; }
        public bool ShowPostProcessing { get; set; }
        public bool ShowSourceLinks { get; set; }
        public bool ShowSocialButtons { get; set; }
        public IEnumerable<PostProcessingOptions> PostProcessingOptions { get; set; }

        public ConvertionOptions()
        {
            FileHandlerUrl = "~/DowJones.Web.Handlers.Article.ContentHandler.ashx";
            ShowSourceLogo = true;
            EnableELinks = true;
            ArticleDisplayOptions = DisplayOptions.Full;
            EmbededImageType = ImageType.Display;
            PictureSize = PictureSize.Large;
            ShowPostProcessing = false;
            ShowSourceLinks = true;
            ShowSocialButtons = false;
        }
    }
}
