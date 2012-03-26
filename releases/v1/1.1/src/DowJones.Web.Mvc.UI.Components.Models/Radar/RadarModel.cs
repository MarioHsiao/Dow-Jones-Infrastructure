namespace DowJones.Web.Mvc.UI.Components.Radar
{
    public class RadarModel : ViewComponentModel
    {
        #region ..:: Client Properties ::..
        [ClientProperty("pageSize")]
        public int PageSize { get; set; }
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