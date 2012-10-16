using System.Web.UI;
using DowJones.Extensions;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.Radar;

[assembly: WebResource(RadarModel.ResourceNodeTotalImageUrl, KnownMimeTypes.PngImage)]
[assembly: WebResource(RadarModel.ResourceNodeValueImageUrl, KnownMimeTypes.PngImage)]


namespace DowJones.Web.Mvc.UI.Components.Radar
{
    
    public class RadarModel : ViewComponentModel
    {
        internal const string ResourceNodeTotalImageUrl = "DowJones.Web.Mvc.UI.Components.Radar.Resources.bar-graph-ltBlue.png";
        internal const string ResourceNodeValueImageUrl = "DowJones.Web.Mvc.UI.Components.Radar.Resources.bar-graph-dkBlue.png";

        private string _nodeValueImageUrl;
        private string _nodeTotalImageUrl;

        #region ..:: Client Properties ::..
        
        [ClientProperty("pageSize")]
        public int PageSize { get; set; }

        [ClientProperty("totalImageUrl")]
        public string TotalImageUrl
        {
            get
            {
                return _nodeTotalImageUrl.IsNullOrEmpty() ? GetType().Assembly.GetWebResourceUrl(ResourceNodeTotalImageUrl) : _nodeTotalImageUrl;
            }
            set
            {
                _nodeTotalImageUrl = value;
            }
        }

        [ClientProperty("valueImageUrl")]
        public string ValueImageUrl
        {
            get
            {
                return _nodeValueImageUrl.IsNullOrEmpty() ? GetType().Assembly.GetWebResourceUrl(ResourceNodeValueImageUrl) : _nodeValueImageUrl;
            }
            set
            {
                _nodeValueImageUrl = value;
            }
        }   
        #endregion

        #region ..:: Client Data ::..
        #endregion

        #region ..:: Client Event Handlers ::..
        /// <summary>
        /// Gets or sets the client side onclick event handler.
        /// </summary>
        [ClientEventHandler("dj.Radar.onClick")]
        public string OnClick { get; set; }
        #endregion

        #region ..:: Client Tokens ::..
        [ClientTokens]
        public RadarTokens Tokens { get; set; }
        #endregion

        public RadarModel()
        {
            Tokens = new RadarTokens();
            PageSize = 6;
        }
    }
}