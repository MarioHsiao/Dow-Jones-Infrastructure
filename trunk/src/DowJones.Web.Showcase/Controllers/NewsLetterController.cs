using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using DowJones.Ajax.Article;
using DowJones.Articles;
using DowJones.Assemblers.Articles;
using DowJones.Infrastructure;
using DowJones.Url;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.Models.Article;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using Factiva.Gateway.Messages.MarketData.V1_0;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class NewsLetterController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ArticleConversionManager _articleConversionManger;

        public NewsLetterController(IArticleService articleService, ArticleConversionManager articleConversionManger)
        {
            _articleService = articleService;
            _articleConversionManger = articleConversionManger;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [Route("newsletter/{accessionNumber}")]
        public ActionResult Article(string accessionNumber, DisplayOptions option = DisplayOptions.Full, ImageType imageType = ImageType.Display, string callback = null, string canonicalSearchString = "T|microsoft T|phone O|+ T|en T|pt O|, T|es O|, N|la O|c O|+ T|article T|file O|, T|report O|, N|fmt O|c O|+ N|pd D|-0090 D| O|d O|+")
        {
            new GetHistoricalDataByTimePeriodRequest
            {
                adjustForCapitalChanges = true
            };

            var article = _articleService.GetArticle(accessionNumber, canonicalSearchString);

            _articleConversionManger.ShowCompanyEntityReference = true;
            _articleConversionManger.ShowExecutiveEntityReference = true;
            _articleConversionManger.EnableELinks = true;
            _articleConversionManger.EmbedHtmlBasedArticles = true;
            _articleConversionManger.EmbedHtmlBasedExternalLinks = true;
            _articleConversionManger.EmbededImageType = imageType;
            _articleConversionManger.ShowImagesAsFigures = true;
            _articleConversionManger.PictureSize = PictureSize.Small;

            var urlBuilder = new UrlBuilder("~/newsletter/" + accessionNumber);
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

            return Request.IsAjaxRequest() ? ViewComponent(model, callback) : View("Article", model);
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
