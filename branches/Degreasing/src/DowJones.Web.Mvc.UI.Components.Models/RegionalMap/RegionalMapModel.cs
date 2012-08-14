
using System.Web.UI;
using System.Collections.Generic; 
using DowJones.Web;
using DowJones.Extensions;
using DowJones.Models.Common;
using DowJones.Web.Mvc.UI.Components.RegionalMap;

[assembly: WebResource(RegionalMapModel.resourceWorldMapImage, KnownMimeTypes.PngImage)]
namespace DowJones.Web.Mvc.UI.Components.RegionalMap
{
    public class RegionalMapModel : ViewComponentModel
    {
        #region ..:: Client Properties ::..

        //[ClientProperty("height")]
        //public int Height { get; set; }

        //[ClientProperty("width")]
        //public int Width { get; set; }

        private string _imageMapPath = "";
        internal const string resourceWorldMapImage = "DowJones.Web.Mvc.UI.Components.RegionalMap.Resources.world_map.png";
        [ClientProperty("imageMapPath")]
        public string ImageMapPath
        {
            get
            {
                return _imageMapPath.IsNullOrEmpty() ? GetType().Assembly.GetWebResourceUrl(resourceWorldMapImage) : _imageMapPath;
            }
            set
            {
                _imageMapPath = value;
            }
        }    

        [ClientProperty("size")]
        public MapSize Size { get; set; }

        [ClientProperty("showTooltips")]
        public bool ShowTooltips { get; set; }

        [ClientProperty("showTextLabels")]
        public bool ShowTextLabels { get; set;} 
        #endregion

        #region ..:: Client Data ::..

        [ClientData]
        public RegionNewsVolumeResult Data { get; set; }
        
        #endregion

        #region ..:: Client Tokens ::..
        [ClientTokens]
        public RegionalMapTokens Tokens { get; set; }
        #endregion

        public RegionalMapModel()
        {
            //Tokens = new RegionalMapTokens();
            //Height = 430;
            //Width = 955;
            ShowTooltips = true;
        }
    }

    public enum MapSize
    {
        Large = 0,
        Small = 1
    }
}