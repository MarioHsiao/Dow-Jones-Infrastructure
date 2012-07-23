using DowJones.Web.Mvc.UI;
using System.ComponentModel;
using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.HeadlineCarousel;

namespace DowJones.Web.Mvc.UI.Components.HeadlineCarousel
{
    public enum CarouselMode
    {
        Video,
        Ticker
    }

    public enum CarouselOrientation
    {
        Vertical,
        Horizontal
    }

    public enum TickerPackageType
    {
        HeadlineHitCountsPackage,
        DiscoveredEntitiesPackage
    }

    public class HeadlineCarouselModel : ViewComponentModel
    {
        [ClientProperty("orientation")]
        [DefaultValue(CarouselOrientation.Horizontal)]
        [TypeConverter(typeof(StringConverter))]
        public CarouselOrientation Orientation { get; set; }
    
        [ClientProperty("mode")]
        [DefaultValue(CarouselMode.Video)]
        [TypeConverter(typeof(StringConverter))]
        public CarouselMode Mode { get; set; }

        [ClientProperty("packageType")]
        [DefaultValue(TickerPackageType.DiscoveredEntitiesPackage)]
        [TypeConverter(typeof(StringConverter))]
        public TickerPackageType PackageType { get; set; }

        [ClientProperty("display")]
        [DefaultValue(1)]
        public int Display { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display no results token.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if display no results token; otherwise, <c>false</c> hide it.
        /// </value>
        [ClientProperty("displayNoResultsToken")]
        public bool DisplayNoResultsToken { get; set; }

        #region ..:: Client Event Handlers ::..

        /// <summary>
        /// Gets or sets the client side OnHeadlineClick event handler.
        /// </summary>
        /// <value>The OnHeadlineClick event handler name.</value>
        [ClientEventHandler("dj.HeadlineCarousel.headlineClick")]
        public string OnHeadlineClick { get; set; }

        #endregion

        #region ..:: Client Tokens ::..

        [ClientTokens]
        public HeadlineCarouselTokens Tokens { get; set; }

        #endregion

        public HeadlineCarouselModel()
        {
            Tokens = new HeadlineCarouselTokens();
        }
    }
    
}