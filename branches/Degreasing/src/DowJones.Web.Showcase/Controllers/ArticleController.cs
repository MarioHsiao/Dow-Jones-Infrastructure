using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DowJones.Ajax;
using DowJones.Ajax.Article;
using DowJones.Extensions;
using DowJones.Articles;
using DowJones.Assemblers.Articles;
using DowJones.Infrastructure;
using DowJones.Exceptions;
using DowJones.Managers.Multimedia;
using DowJones.Url;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.Search.ViewModels;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.PostProcessing;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using DowJones.Web.Mvc.UI.Components.VideoPlayer;
using Factiva.Gateway.Messages.Archive.V2_0;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;
using DowJones.Token;
using DowJones.Web.Mvc.UI.Components.PortalArticle;
using DowJones.Ajax.PortalArticle;

namespace DowJones.Web.Showcase.Controllers
{
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ArticleConversionManager _articleConversionManager;
        private readonly MultimediaManager _multimediaManager;
        private readonly HttpContextBase _httpContextBase;
        private const string DefaultCanonicalSearchString = "T|microsoft T|en T|es O|, T|pt O|, N|la O|c O|+ T|article N|fmt O|c T|report N|fmt O|c O|, T|file N|fmt O|c O|, T|webpage N|fmt O|c O|, T|blog N|fmt O|c O|, T|multimedia N|fmt O|c O|, T|picture N|fmt O|c O|, T|tmnb N|rst O|c T|tmnb N|rst O|c O|, O|+ O|+ T|article T|file O|, T|report O|, N|fmt O|c O|+ N|pd D|-0008 D| O|d O|+";

        public ArticleController(IArticleService articleService, ArticleConversionManager articleConversionManager, MultimediaManager multimediaManager, HttpContextBase httpContextBase)
        {
            _articleService = articleService;
            _articleConversionManager = articleConversionManager;
            _multimediaManager = multimediaManager;
            _httpContextBase = httpContextBase;
        }

        public ActionResult Index(string accessionNumber = "DJFVW00020120326e83qkgx46", DisplayOptions option = DisplayOptions.Full)
        {
            return Article(accessionNumber, option);
        }

        public ActionResult ComponentExplorerDemo(string accessionNumber = "DJFVW00020120326e83qkgx46", DisplayOptions option = DisplayOptions.Full)
        {
            var model = GetArticle(accessionNumber, DefaultCanonicalSearchString, ImageType.Thumbnail, PictureSize.Large, option);
            return View("Index", "_Layout_ComponentExplorer", model);
        }

        [Route("article/{accessionNumber}")]
        public ActionResult Article(string accessionNumber, DisplayOptions option = DisplayOptions.Full,
            ImageType imageType = ImageType.Thumbnail, PictureSize pictureSize = PictureSize.Large,
            string callback = null, string canonicalSearchString = DefaultCanonicalSearchString)
        {
            var model = GetArticle(accessionNumber, canonicalSearchString, imageType, pictureSize, option);

            return Request.IsAjaxRequest() ? ViewComponent(model, callback) : View("Index", model);
        }

		[Route("article2/{accessionNumber}")]
		public JsonResult Article2(string accessionNumber, DisplayOptions option = DisplayOptions.Full,
			ImageType imageType = ImageType.Thumbnail, PictureSize pictureSize = PictureSize.Large,
			string callback = null, string canonicalSearchString = DefaultCanonicalSearchString)
		{
			
			var oldModel = GetArticle(accessionNumber, canonicalSearchString, imageType, pictureSize, option);

			var model = new PortalArticleModel(TokenRegistry)
			{
				ShowAuthorLinks = true,
				ShowPostProcessing = true,
				ShowSourceLinks = true,
				PostProcessingOptions = new [] {
					PostProcessingOptions.Print, PostProcessingOptions.Save, 
					PostProcessingOptions.Email, PostProcessingOptions.Listen, 
					PostProcessingOptions.Translate, PostProcessingOptions.Share
				},
				Result = Mapper.Map<PortalArticleResultSet>(oldModel.ArticleDataSet)
			};

			return Json(model, JsonRequestBehavior.AllowGet);
		}

		[Route("article2/clientside")]
		public ActionResult ClientSide()
		{
			return View();
		}

        [Route("article/video/{accessionNumber}")]
        [Route("article/multimedia/{accessionNumber}")]
        public ActionResult Multimedia(string accessionNumber)
        {
            var model = GetArticle(accessionNumber, "video");

            return Request.IsAjaxRequest() ? ViewComponent(model) : View("Index", model);
        }

        [Route("article/document/{accessionNumber}")]
        public ActionResult Document(string accessionNumber, DisplayOptions option = DisplayOptions.Full)
        {
            var model = GetDocument(accessionNumber, option);
            return Request.IsAjaxRequest() ? ViewComponent(model) : View("Index", model);
        }

        private ArticleModel GetDocument(string accessionNumber, DisplayOptions option = DisplayOptions.Full)
        {
            var article = _articleService.GetDocument(accessionNumber);

            if (article == null || (article.status != null && article.status.value != 0))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidDataRequest);
            }

            _articleConversionManager.ShowCompanyEntityReference = true;
            _articleConversionManager.ShowExecutiveEntityReference = true;
            _articleConversionManager.EnableELinks = true;
            _articleConversionManager.EmbedHtmlBasedArticles = true;
            _articleConversionManager.EmbedHtmlBasedExternalLinks = true;
            _articleConversionManager.ShowImagesAsFigures = true;
            _articleConversionManager.EnableEnlargedImage = true;

            return new ArticleModel
            {
                ArticleDataSet = _articleConversionManager.Convert(article),
                ArticleDisplayOptions = option,
                ShowPostProcessing = true,
                ShowSourceLinks = true,
                PostProcessingOptions = new[]
    		       		                        	{
    		       		                        		PostProcessingOptions.Print,
    		       		                        		PostProcessingOptions.Save,
    		       		                        		PostProcessingOptions.PressClips,
    		       		                        		PostProcessingOptions.Email, 
    		       		                        		PostProcessingOptions.Listen,
    		       		                        		PostProcessingOptions.Translate,
    		       		                        		PostProcessingOptions.Share
    		       		                        	}.Distinct()
            };
        }

        private MultiMediaItemModel GetMultimediaModel(string accessionnumber, ContentSubCategory contentSubCategory)
        {
            var response = _multimediaManager.GetMultiMediaResult(accessionnumber, false);
            var multimediaItemModel = new MultiMediaItemModel();

            if (response.Status > 0)
            {
                throw new DowJonesUtilitiesException(response.Status);
            }

            if (response.MultimediaResult != null)
            {
                if (response.MultimediaResult.MustPlayFromSource != null)
                {
                    multimediaItemModel.MustPlayFromSource = response.MultimediaResult.MustPlayFromSource.Status;
                    if (multimediaItemModel.MustPlayFromSource)
                    {
                        multimediaItemModel.ExternalUrl = response.MultimediaResult.MustPlayFromSource.Url;
                    }
                }

                if (response.MultimediaResult.MediaContents != null && response.MultimediaResult.MediaContents.Count > 0)
                {
                    var mediaContents = response.MultimediaResult.MediaContents;
                    var mediaContent = mediaContents.First();
                    if (mediaContents.Count > 1)
                    {
                        mediaContent = mediaContents[1];
                    }

                    var browsers = IdentifyBrowsers(_httpContextBase);

                    switch (contentSubCategory)
                    {
                        case ContentSubCategory.Audio:
                            {
                                var videoPlayerModel = new VideoPlayerModel
                                {
                                    AutoPlay = true,
                                    Data = new ClipCollection(new[] { Mapper.Map<Clip>(mediaContent) }),
                                    Width = GetAudioWidth(browsers),
                                    Height = GetAudioHeight(),
                                    PlayerKey = "75a6c4404d9ffa80a63",
                                };
                                multimediaItemModel.MultiMediaPlayerModel = videoPlayerModel;
                            }
                            break;
                        case ContentSubCategory.Video:
                            {
                                var aspectRatio = GetVideoAspectRatio(browsers);
                                var width = (int.Parse(mediaContent.Width) / aspectRatio).ConvertTo<int>();
                                var height = (int.Parse(mediaContent.Height) / aspectRatio).ConvertTo<int>();
                                var videoPlayerModel = new VideoPlayerModel
                                {
                                    AutoPlay = true,
                                    Width = width,
                                    Height = height,
                                    Data = new ClipCollection(new[] { Mapper.Map<Clip>(mediaContent) }),
                                    PlayerKey = "75a6c4404d9ffa80a63",
                                };

                                //videoPlayerModel.PlayList.First().Title = JsonUtility.EncodeStringValue(portalHeadlineInfo.Title);
                                multimediaItemModel.MultiMediaPlayerModel = videoPlayerModel;
                            }
                            break;
                    }
                }
            }
            return multimediaItemModel;
        }

        private static int GetAudioHeight()
        {
            return 30;
        }

        private static int GetAudioWidth(Browsers browsers)
        {
            if (browsers.IsMobileAndroid || browsers.IsMobileSafari)
            {
                return 300;
            }

            return 500;
        }

        private double GetVideoAspectRatio(Browsers browsers)
        {
            if (browsers.IsTabletSafari)
            {
                return 1.5;
            }

            if (browsers.IsMobileAndroid || browsers.IsMobileSafari)
            {
                return 2.4;
            }

            return 1.1;
        }

        public Browsers IdentifyBrowsers(HttpContextBase context)
        {
            var browsers = new Browsers();
            if (context != null)
            {
                var userAgent = context.Request.UserAgent;

                if (!string.IsNullOrEmpty(userAgent))
                {
                    var ipodIndex = userAgent.IndexOf("iPod", StringComparison.Ordinal);
                    var iphoneIndex = userAgent.IndexOf("iPhone", StringComparison.Ordinal);
                    var ipadIndex = userAgent.IndexOf("iPad", StringComparison.Ordinal);
                    var androidIndex = userAgent.IndexOf("Android", StringComparison.Ordinal);

                    if (iphoneIndex + ipodIndex > -1) browsers.IsMobileSafari = true;
                    if (ipadIndex > -1) browsers.IsTabletSafari = true;
                    if (androidIndex > -1) browsers.IsMobileAndroid = true;
                    return browsers;
                }
            }
            return browsers;
        }

        private ArticleModel GetArticle(string accessionNumber, string contentType)
        {
            var articleReference = new ArticleReference
            {
                AccessionNumber = accessionNumber,
                ContentType = contentType
            };

            var request = new MixedContentArticleRequest
            {
                ArticleReferences = new List<ArticleReference>(new[] { articleReference }),
                DisplayFormat = DisplayFormat.Article,
                ResponseDataSet = new ResponseDataSet
                {
                    articleFormat = ArticleFormatType.FULR
                },

            };

            var articleResponseSet = _articleService.GetArticles(request);

            if (articleResponseSet.article.IsNullOrEmpty())
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidDataRequest);
            }

            var article = articleResponseSet.article.First();

            _articleConversionManager.ShowCompanyEntityReference = true;
            _articleConversionManager.ShowExecutiveEntityReference = true;
            _articleConversionManager.EnableELinks = true;
            _articleConversionManager.EmbedHtmlBasedArticles = true;
            _articleConversionManager.EmbedHtmlBasedExternalLinks = true;
            _articleConversionManager.ShowImagesAsFigures = true;
            _articleConversionManager.EnableEnlargedImage = true;

            var urlBuilder = new UrlBuilder("~/article/" + accessionNumber);
            var articleDataSet = _articleConversionManager.Convert(article);

            return new ArticleModel
            {
                ArticleDataSet = articleDataSet,
                ShowPostProcessing = true,
                ShowSourceLinks = true,
                ShowSocialButtons = true,
                VideoPlayer = GetMultimediaModel(articleDataSet.AccessionNo, articleDataSet.ContentSubCategory).MultiMediaPlayerModel,
                SocialButtons = new SocialButtonsModel
                {
                    Url = urlBuilder.ToString(),
                    Description = "",
                    Target = "_blank",
                    Title = ProcessHeadlineRenderItems(articleDataSet.Headline),
                    SocialNetworks = new[] { SocialNetworks.LinkedIn, SocialNetworks.Twitter, SocialNetworks.Facebook },
                    Keywords = "",
                    ID = "socialButtons",
                    ShowCustomTooltip = false,
                },
                PostProcessingOptions = new[]
    		       		                        	{
    		       		                        		PostProcessingOptions.Print,
    		       		                        		PostProcessingOptions.Save,
    		       		                        		PostProcessingOptions.PressClips,
    		       		                        		PostProcessingOptions.Email, 
    		       		                        		PostProcessingOptions.Listen,
    		       		                        		PostProcessingOptions.Translate,
    		       		                        		PostProcessingOptions.Share
    		       		                        	}.Distinct()
            };
        }


        private ArticleModel GetArticle(string accessionNumber, string canonicalSearchString, ImageType imageType, PictureSize pictureSize, DisplayOptions option)
        {

            //canonicalSearchString = "T|djdn000020120216e82g0lkzf N|an O|: T|and O|+ T|sipc O|+ T|and O|+ T|businesswire O|+ T|and O|+ T|schwab O|+ T|and O|+ T|aboutschwab O|+ T|en T|ru O|, T|de O|, N|la O|c O|+ T|nnam T|nrmf O|, T|nrgn O|, N|ns O|c O|- T|article T|file O|, T|report O|, T|webpage O|, T|blog O|, T|picture O|, T|multimedia O|, T|board O|, T|customerdoc O|, N|fmt O|c O|+";
            var article = _articleService.GetArticle(accessionNumber, canonicalSearchString);

            if (article == null || (article.status != null && article.status.value != 0))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidDataRequest);
            }

            _articleConversionManager.ShowCompanyEntityReference = true;
            _articleConversionManager.ShowExecutiveEntityReference = true;
            _articleConversionManager.EnableELinks = true;
            _articleConversionManager.EmbedHtmlBasedArticles = true;
            _articleConversionManager.EmbedHtmlBasedExternalLinks = true;
            _articleConversionManager.EmbededImageType = imageType;
            _articleConversionManager.ShowImagesAsFigures = true;
            _articleConversionManager.PictureSize = pictureSize;
            _articleConversionManager.EnableEnlargedImage = true;

            var urlBuilder = new UrlBuilder("~/article/" + accessionNumber);
            var articleDataSet = _articleConversionManager.Convert(article);

            return new ArticleModel
            {
                ArticleDataSet = _articleConversionManager.Convert(article),
                ArticleDisplayOptions = option,
                ShowPostProcessing = true,
                ShowSourceLinks = true,
                ShowSocialButtons = true,
                SocialButtons = new SocialButtonsModel
                {
                    Url = urlBuilder.ToString(),
                    Description = "",
                    Target = "_blank",
                    Title = ProcessHeadlineRenderItems(articleDataSet.Headline),
                    SocialNetworks = new[] { SocialNetworks.LinkedIn, SocialNetworks.Twitter, SocialNetworks.Facebook },
                    Keywords = "",
                    ID = "socialButtons",
                    ShowCustomTooltip = false,
                },
                PostProcessingOptions = new[]
    		       		                        	{
    		       		                        		PostProcessingOptions.Print,
    		       		                        		PostProcessingOptions.Save,
    		       		                        		PostProcessingOptions.PressClips,
    		       		                        		PostProcessingOptions.Email, 
    		       		                        		PostProcessingOptions.Listen,
    		       		                        		PostProcessingOptions.Translate,
    		       		                        		PostProcessingOptions.Share
    		       		                        	}.Distinct()
            };
        }


        private static string ProcessHeadlineRenderItems(IEnumerable<RenderItem> items)
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

        public ActionResult Articles(string ans)
        {
            string[] ids = ans.Split(',');

            var articleResponse = _articleService.GetArticles(new GetArticleRequest { accessionNumbers = ids });
            var articlesModel = new ArticlesModel(articleResponse, _articleConversionManager)
            {
                ShowPostProcessing = false,
                ShowSocialButtons = false,
                ShowTranslator = false,
                ShowSourceLinks = true,
                ShowAuthorLinks = true
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
            _articleService.GetArticles(new MixedContentArticleRequest
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

        public class Browsers
        {
            public bool IsTabletSafari { get; set; }
            public bool IsMobileSafari { get; set; }
            public bool IsMobileAndroid { get; set; }
        }
        public class MultiMediaItemModel
        {
            public bool MustPlayFromSource { get; set; }
            public string ExternalUrl { get; set; }
            public VideoPlayerModel MultiMediaPlayerModel { get; set; }
        }

    }


}
