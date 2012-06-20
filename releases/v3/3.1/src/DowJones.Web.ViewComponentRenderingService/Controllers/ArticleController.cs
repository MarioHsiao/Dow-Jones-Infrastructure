using System.IO;
using System.Linq;
using System.Web.Mvc;
using DowJones.Ajax.Article;
using DowJones.Extensions;
using DowJones.Articles;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Mvc.UI.Components.Models.Article;
using Newtonsoft.Json;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.ViewComponentRenderingService.Controllers
{
    public class ArticleController : ControllerBase
    {
        public ActionResult Index()
        {
            return Render();
        }

        public ActionResult Render()
        {
            string articleJson = new StreamReader(Request.InputStream).ReadToEnd();
            var article = JsonConvert.DeserializeObject<ArticleResultset>(articleJson);
            var model = new ArticleModel {ArticleDataSet = article};

            return ViewComponent(model);
        }

        public ActionResult RenderWithVideo()
        {
            string articleJson = new StreamReader(Request.InputStream).ReadToEnd();
            var request = JsonConvert.DeserializeObject<ArticleRendererRequest>(articleJson);
            var model = new ArticleModel
                            {
                                ArticleDataSet = request.ArticleResultset,
                                ShowPostProcessing = false,
                                ShowSourceLinks = true,
                                ShowSocialButtons = false,
                            };
            if (request.MultimediaPackage != null)
            {
                if (request.MultimediaPackage.MustPlayFromSource != null)
                {
                    if (request.MultimediaPackage.MustPlayFromSource.Status)
                    {
                        model.ArticleDataSet.ExternalUri = request.MultimediaPackage.MustPlayFromSource.Url;
                    }
                }

                if (request.MultimediaPackage.MediaContents != null && request.MultimediaPackage.MediaContents.Count > 0)
                {
                    var mediaContents = request.MultimediaPackage.MediaContents;
                    var mediaContent = mediaContents.First();
                    if (mediaContents.Count > 1)
                    {
                        mediaContent = mediaContents[1];
                    }

                    model.VideoPlayerModel = new VideoPlayerModel
                    {
                        AutoPlay = request.MultimediaPlayerOptions.AutoPlay,
                        PlayList = new ClipCollection(new[] { Mapper.Map<Clip>(mediaContent) }),
                        PlayerKey = "75a6c4404d9ffa80a63",
                    };

                    if (request.MultimediaPlayerOptions != null)
                    {
                        if (mediaContent.Medium == "audio")
                        {
                            model.VideoPlayerModel.Width = request.MultimediaPlayerOptions.AvailableWidth > 0 ? request.MultimediaPlayerOptions.AvailableWidth : 300;
                            model.VideoPlayerModel.Height = 30;
                        }
                        else
                        {
                            int mediaContentWidth, mediaContentHeight;

                            if (request.MultimediaPlayerOptions.AvailableWidth > 0 &&
                                int.TryParse(mediaContent.Width, out mediaContentWidth) &&
                                int.TryParse(mediaContent.Height, out mediaContentHeight))
                            {
                                if (mediaContentWidth > request.MultimediaPlayerOptions.AvailableWidth)
                                {
                                    // media dimensions are bigger than available space, resize keeping ratio
                                    model.VideoPlayerModel.Width = request.MultimediaPlayerOptions.AvailableWidth;
                                    model.VideoPlayerModel.Height = (request.MultimediaPlayerOptions.AvailableWidth * mediaContentHeight) / mediaContentWidth;
                                }
                                else
                                {
                                    model.VideoPlayerModel.Width = mediaContentWidth;
                                    model.VideoPlayerModel.Height = mediaContentHeight;
                                }
                            }
                        }

                        if (!request.MultimediaPlayerOptions.ControlBarPath.IsNullOrEmpty())
                        {
                            model.VideoPlayerModel.ControlBarPath = request.MultimediaPlayerOptions.ControlBarPath;
                        }

                        if (!request.MultimediaPlayerOptions.PlayerPath.IsNullOrEmpty())
                        {
                            model.VideoPlayerModel.PlayerPath = request.MultimediaPlayerOptions.PlayerPath;
                        }

                        if (!request.MultimediaPlayerOptions.RTMPPluginPath.IsNullOrEmpty())
                        {
                            model.VideoPlayerModel.RTMPPluginPath = request.MultimediaPlayerOptions.RTMPPluginPath;
                        }

                        if (!request.MultimediaPlayerOptions.SplashImagePath.IsNullOrEmpty())
                        {
                            model.VideoPlayerModel.SplashImagePath = request.MultimediaPlayerOptions.SplashImagePath;
                        }
                    }
                }
            }
            return ViewComponent(model);
        }
    }
}