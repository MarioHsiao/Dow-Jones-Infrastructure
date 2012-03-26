using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Articles;
using DowJones.Assemblers.Articles;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.Search.ViewModels;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.Models.Article;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.MarketData.V1_0;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ArticleConversionManager _articleConversionManger;

        public ArticleController(IArticleService articleService, ArticleConversionManager articleConversionManger)
        {
            _articleService = articleService;
            this._articleConversionManger = articleConversionManger;
        }

        public ActionResult Index(string acn = "DATMON0020110429e74e001t8", DisplayOptions option = DisplayOptions.Full)
        {
            return Article(acn, option);
        }

        [Route("article/{accessionNumber}")]
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
            _articleConversionManger.PictureSize = PictureSize.Small;

            var model = new ArticleModel
            {
                ArticleDataSet = _articleConversionManger.Convert(article),
                ArticleDisplayOptions = option,
                ShowPostProcessing = true,
                ShowSourceLinks = true,
                ShowSocialButtons = true,
                SocialButtons = new SocialButtonsModel
                {
                    ImageSize = ImageSize.Small,
                    Target = "_blank",
                    Title = "Dow Jones",
                    Description = "Financial",
                    Url = "http://dowjones.com",
                    Keywords = "factiva, socials, bank"
                },
            };

            if (Request.IsAjaxRequest())
            {
                return ViewComponent(model, callback);
            }

            return View("Index", model);
        }


        public ActionResult Articles(string ans)
        {
            string[] ids = ans.Split(',');

            var articleResponse = _articleService.GetArticles(new GetArticleRequest { accessionNumbers = ids });
            var articlesModel = new ArticlesModel(articleResponse, _articleConversionManger)
            {
                ShowPostProcessing = false,
                ShowReadSpeaker = false,
                ShowSocialButtons = false,
                ShowTranslator = false,
                ShowSourceLinks = true,
                ShowAuthorLinks = true,
            };

            return ViewComponent(articlesModel);

        }

        public ActionResult MixedArticles(string pub, string web, string pic, string mul)
        {
            //JOCOM00020080929dc9t0005q JOCOM00020080929dc9t0004q J000000020090710e57b0003j J000000020090710e57a00016 
            //BALO000020090710e57a00023 BALO000020090710e57a00022 
            //EDIFIN0020031215dzbr000nd EDIFIN0020031215dzbr000x3 
            //WC93409020090709e5780000j WC93409020090709e5780000g 
            //BOARDB0020090710e57a011xf BOARDB0020090710e57a011n7 
            //BOARDR0020090710e57a08zb7 BOARDR0020090710e57a08z6f 
            //X900550020090710e57a0005l X900550020090710e57a0002t 
            //MMSAUX0020090710e57a0005l MMSAUX0020090710e57a0002u 
            //MMSAJZ0020090710e57a00001 MMSAJZ0020090709e5790002t

            IList<ArticleReference> list = new List<ArticleReference>();
            AddReference(list, pub, "article", "J000000020090710e57b0003j JOCOM00020080929dc9t0005q");
            AddReference(list, web, "web", "WC93409020090709e5780000j,WC93409020090709e5780000");
            AddReference(list, pic, "pic", "X900550020090710e57a0005l,X900550020090710e57a0002t");
            AddReference(list, mul, "mul", "MMSAUX0020090710e57a0005l,MMSAUX0020090710e57a0002u,MMSAJZ0020090710e57a00001,MMSAJZ0020090709e5790002t");
            _articleService.GetArticles(new MixedContentArticleRequest()
            {
                ArticleReferences = list,
                ResponseDataSet = new ResponseDataSet
                {
                    articleFormat = ArticleFormatType.FULL
                }
            });


            return new EmptyResult();
        }

        private static void AddReference(IList<ArticleReference> list, string ans, string format, string defaultValue)
        {
            if (ans == null)
            {
                ans = defaultValue;
            }
            var ids = ans.Split(',');
            foreach (var r in ids.Select(id => new ArticleReference { AccessionNumber = id, ContentType = format }))
            {
                list.Add(r);
            }
        }
    }


}
