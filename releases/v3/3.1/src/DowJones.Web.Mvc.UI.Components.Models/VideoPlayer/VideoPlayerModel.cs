using System;
using System.Reflection;
using System.Web.UI;
using DowJones.Extensions;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.VideoPlayer;

[assembly: WebResource(VideoPlayerModel.Player, KnownMimeTypes.ShockwaveFlash)]
[assembly: WebResource(VideoPlayerModel.FlowplayerRtmp, KnownMimeTypes.ShockwaveFlash)]
[assembly: WebResource(VideoPlayerModel.ControlBar, KnownMimeTypes.ShockwaveFlash)]
[assembly: WebResource(VideoPlayerModel.SplashImage, KnownMimeTypes.PngImage)]

namespace DowJones.Web.Mvc.UI.Components.VideoPlayer  
{
    
    public class VideoPlayerModel : ViewComponentModel
    {
        // Seam for replace non-test-friendly code
        internal static Func<Assembly, string, string> ResolveWebResourceUrlThunk =
            (assembly, resourceName) => assembly.GetWebResourceUrl(resourceName);

        private string _rtmpPluginPath;
        private string _playerPath;
        private string _splashImagePath;
        private string _controlBarPath;

        internal const string Player = "DowJones.Web.Mvc.UI.Components.VideoPlayer.Resources.flowplayer.unlimited-3.2.7.swf";
        internal const string ControlBar = "DowJones.Web.Mvc.UI.Components.VideoPlayer.Resources.flowplayer.controls-3.2.5.swf";
        internal const string FlowplayerRtmp = "DowJones.Web.Mvc.UI.Components.VideoPlayer.Resources.flowplayer.rtmp-3.2.3.swf";
        internal const string SplashImage = "DowJones.Web.Mvc.UI.Components.VideoPlayer.Resources.play_text_large.png";
        
        #region ..:: Client Properties ::..
        
        [ClientProperty("width")]
        public int Width { get; set; }

        [ClientProperty("height")]
        public int Height { get; set; }

        [ClientProperty("autoPlay")]
        public bool AutoPlay { get; set; }

        [ClientProperty("controlBarPath")]
        public string ControlBarPath
        {
            get
            {
                return _controlBarPath.IsNullOrEmpty() ? ResolveWebResourceUrlThunk(GetType().Assembly, ControlBar) : _controlBarPath;
            }

            set { _controlBarPath = value; }
        }

        [ClientProperty("playerPath")]
        public string PlayerPath
        {
            get
            {
                return _playerPath.IsNullOrEmpty() ? ResolveWebResourceUrlThunk(GetType().Assembly, Player) : _playerPath;
            }

            set { _playerPath = value; }
        }

        [ClientProperty("rtmpPluginPath")]
        public string RTMPPluginPath
        {
            get
            {
                return _rtmpPluginPath.IsNullOrEmpty() ? ResolveWebResourceUrlThunk(GetType().Assembly, FlowplayerRtmp) : _rtmpPluginPath;
            }

            set { _rtmpPluginPath = value; }
        }

        [ClientProperty("splashImagePath")]
        public string SplashImagePath
        {
            get
            {
                return _splashImagePath.IsNullOrEmpty() ? ResolveWebResourceUrlThunk(GetType().Assembly, SplashImage) : _splashImagePath;
            }

            set { _splashImagePath = value; }
        }

        [ClientProperty("playerKey")]
        public string PlayerKey { get; set; }
        #endregion

        #region ..:: Client Data ::..

        [ClientData]
        public ClipCollection Data { get; set; }

        #endregion

        #region ..:: Client Event Handlers ::..
        
        
        #endregion

    }
}
