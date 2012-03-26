using DowJones.Web.Mvc.UI.Components.Common.Types;
using Factiva.Gateway.Messages.CodedNews;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Tools.Ajax.NewsStandTicker;
using System.ComponentModel;

namespace DowJones.Web.Mvc.UI.Components.NewsStandTicker
{
    public enum TickerDirection
    {
        Left,
        Right
    }

    public class NewsStandTickerModel : ViewComponentModel
    {
        #region ..:: Client Properties ::..

        /// <summary>
        /// Gets or sets ticker speed - has an inverse effect on speed. therefore, a value of 10000 will 
        /// scroll very slowly while a value of 50 will scroll very quickly.
        /// </summary>
        [ClientProperty("tickerSpeed")]
        [DefaultValue(15)]
        public int TickerSpeed { get; set; }

        /// <summary>
        /// Gets or sets direction in which ticker show will traverse
        /// </summary>
        /// <value>'next', 'prev'</value>
        [ClientProperty("tickerDirection")]        
        [TypeConverter(typeof(StringConverter))]
        [DefaultValue(TickerDirection.Left)]
        public TickerDirection TickerDirection { get; set; }
       

        #endregion
        
        #region ..:: Client Tokens ::..

        [ClientTokens]
        public NewsStandTickerTokens Tokens { get; set; }

        #endregion
        
        #region ..:: Client Event Handlers ::..

        /// <summary>
        /// Gets or sets the client side OnHeadlineClick event handler.
        /// </summary>
        /// <value>The OnHeadlineClick event handler name.</value>
        [ClientEventHandler("dj.NewsStandTicker.sourceTitleClick")]
        public string OnSourceTitleClick { get; set; }

        #endregion

        public NewsStandTickerModel()
        {
            Tokens = new NewsStandTickerTokens();
            TickerDirection = TickerDirection.Left;
            TickerSpeed = 15;
        }
        
    }
}
