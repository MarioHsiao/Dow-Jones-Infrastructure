using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using DowJones.Ajax.Article;
using DowJones.Articles;
using DowJones.Assemblers.Articles;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure;
using DowJones.Preferences;
using DowJones.Url;
using DowJones.Web.Handlers.DJInsider;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.Models.Article;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using DowJones.Web.Showcase.Models.Newsletter;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class NewsLetterController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ArticleConversionManager _articleConversionManger;
        private readonly IPreferences _preferences;
        private readonly DateTimeFormatter _dateTimeFormatter;

        public NewsLetterController(IArticleService articleService, ArticleConversionManager articleConversionManger, IPreferences preferences)
        {
            _articleService = articleService;
            _articleConversionManger = articleConversionManger;
            _preferences = preferences;
            _dateTimeFormatter = new DateTimeFormatter(_preferences);
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [Route("newsletter/{accessionNumber}")]
        public ActionResult Article(string accessionNumber, DisplayOptions option = DisplayOptions.Full, ImageType imageType = ImageType.Display)
        {
            var model = new EditionModel
                            {
                                Meta = new Meta
                                           {
                                               NewsletterName = "Factiva Energy Monitor",
                                               Timestamp = new DateTime(2012,3,22,8,1,1),
                                           },
                                ArticleModel = GetArticleModel(accessionNumber, option, imageType),
                            };

            model.Meta.TimestampDescripter = _dateTimeFormatter.FormatLongDateTime(model.Meta.Timestamp);
            return  View("Article", model);
        }

        private ArticleModel GetArticleModel(string accessionNum, DisplayOptions option = DisplayOptions.Full, ImageType imageType = ImageType.Display, string canonicalSearchString = "")
        {
            var article = _articleService.GetArticle(accessionNum, canonicalSearchString);

            if (article.status != null && article.status.value != 0)
            {
                throw new DowJonesInsiderException(article.status.value);
            }

            _articleConversionManger.ShowCompanyEntityReference = true;
            _articleConversionManger.ShowExecutiveEntityReference = true;
            _articleConversionManger.EnableELinks = true;
            _articleConversionManger.EmbedHtmlBasedArticles = true;
            _articleConversionManger.EmbedHtmlBasedExternalLinks = true;
            _articleConversionManger.EmbededImageType = imageType;
            _articleConversionManger.ShowImagesAsFigures = true;
            _articleConversionManger.PictureSize = PictureSize.Small;

            var urlBuilder = new UrlBuilder("~/newsletter/" + accessionNum);
            var articleDataSet = _articleConversionManger.Convert(article);

            var model = new ArticleModel
            {
                ArticleDataSet = articleDataSet,
                ArticleDisplayOptions = option,
                ShowPostProcessing = true,
                ShowSourceLinks = true,
                ShowSocialButtons = true,
                SocialButtons = new SocialButtonsModel
                {
                    Url = urlBuilder.ToString(),
                    Description = "",
                    Target = "_blank",
                    ImageSize = ImageSize.Small,
                    Title = ProcessHeadlineRenderItems(articleDataSet.Headline),
                    SocialNetworks = new[] { SocialNetworks.LinkedIn, SocialNetworks.Twitter, SocialNetworks.Facebook, },
                    Keywords = "",
                    ID = "socialButtons",
                    ShowCustomTooltip = false,
                },
            };
            return model;
        }

        private static string ProcessHeadlineRenderItems(IEnumerable<RenderItem>items)
        {
            var sb = new StringBuilder();

            foreach (var item in items)
            {
                switch (item.ItemMarkUp)
                {
                    case MarkUpType.Plain:
                    case MarkUpType.Anchor:
                        sb.Append(item.ItemText);
                        break;
                }    
            }
            return sb.ToString();
        }
    }
}
