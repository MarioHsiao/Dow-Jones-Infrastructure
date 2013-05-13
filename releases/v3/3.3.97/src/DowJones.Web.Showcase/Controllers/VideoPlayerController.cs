using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.VideoPlayer;

namespace DowJones.Web.Showcase.Controllers
{
    public class VideoPlayerController : Controller
    {
        //
        // GET: /VideoPlayer/

        public ActionResult Index()
        {
            var mediaContent = new Clip
                                   {
                                       Url = "http://m.wsj.net/video/20120601/060112hubamGOOGLE/060112hubamGOOGLE_1500k.mp4",
                                       Medium = Medium.Video,
                                       Duration = "188",
                                       Type = "video/mp4",
                                       BitRate = "1500",
                                       FrameRate = "29.97",
                                       Width = "640",
                                       Height = "360"
                                   };
            var videoPlayerModel = new VideoPlayerModel
            {
                AutoPlay = false,
                Width = 620,
                Height = 348,
                Data = new ClipCollection(new[] { Mapper.Map<Clip>(mediaContent) }),
                ControlBarPath= "http://fdevweb3.win.dowjones.net/widgets/3.0/assets/FlowPlayer/flowplayer.controls-3.2.5.swf",
                PlayerPath= "http://fdevweb3.win.dowjones.net/widgets/3.0/assets/FlowPlayer/flowplayer.unlimited-3.2.7.swf",
                RTMPPluginPath= "http://fdevweb3.win.dowjones.net/widgets/3.0/assets/FlowPlayer/flowplayer.rtmp-3.2.3.swf",
                SplashImagePath= "http://fdevweb3.win.dowjones.net/widgets/3.0/assets/FlowPlayer/play_text_large.png",
                PlayerKey = "75a6c4404d9ffa80a63",
            };
            return View(videoPlayerModel);
        }

    }
}
