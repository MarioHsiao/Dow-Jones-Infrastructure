namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class VideoPlayerModel : ViewComponentModel
    {
        #region ..:: Client Properties ::..
        
        [ClientProperty("width")]
        public int Width { get; set; }

        [ClientProperty("height")]
        public int Height { get; set; }

        [ClientProperty("autoPlay")]
        public bool AutoPlay { get; set; }

        [ClientProperty("playerPath")]
        public string PlayerPath { get; set; }

        [ClientProperty("rtmpPluginPath")]
        public string RTMPPluginPath { get; set; }

        [ClientProperty("splashImagePath")]
        public string SplashImagePath { get; set; }

        [ClientProperty("playerKey")]
        public string PlayerKey { get; set; }
        #endregion

        #region ..:: Client Data ::..

        [ClientData]
        public ClipCollection PlayList { get; set; }

        #endregion

        #region ..:: Client Event Handlers ::..
        
        
        #endregion

    }
}
