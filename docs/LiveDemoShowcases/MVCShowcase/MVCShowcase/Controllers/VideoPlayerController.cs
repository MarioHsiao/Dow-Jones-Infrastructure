using System.Configuration;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.MvcShowcase.Controllers
{
    public class VideoPlayerController : BaseController
    {
        //
        // GET: /VideoPlayer/

        public ActionResult Index()
        {
            var appPath = ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"];
            var data = new ClipCollection(new[]
                                              {
                                                  new Clip
                                                      {
                                                          Url = appPath + "/styles/views/videoplayer/media/demo.mp4",
                                                          Medium = Medium.Video,
                                                          Duration = "188",
                                                          Type = "video/mp4",
                                                          BitRate = "1500",
                                                          FrameRate = "29.97",
                                                          Width = "670",
                                                          Height = "300"
                                                      }
                                              });
            var videoPlayerModel = new VideoPlayerModel
            {
                AutoPlay = false,
                Width = 650,
                Height = 288,
                PlayList = data,
                ControlBarPath = appPath + "/styles/views/videoplayer/img/flowplayer.controls-3.2.5.swf",
                PlayerPath = appPath + "/styles/views/videoplayer/img/flowplayer.unlimited-3.2.7.swf",
                RTMPPluginPath = appPath + "/styles/views/videoplayer/img/flowplayer.rtmp-3.2.3.swf",
                SplashImagePath = appPath + "/styles/views/videoplayer/img/FlowPlayer/play_text_large.png",
                PlayerKey = "75a6c4404d9ffa80a63",
            };

            return View(videoPlayerModel);
        }
    }
}
