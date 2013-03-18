using System.ComponentModel;
using System.Web.UI;
using DowJones.Ajax.HeadlineList;
using DowJones.Converters;
using DowJones.Formatters.Globalization.DateTime;

namespace DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList
{
    public class RealtimeHeadlineListModel : ViewComponentModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets container width
        /// </summary>
        public int ContainerWidth { get; set; }

        #endregion

        #region Event Handlers

        ///// <summary>
        ///// Gets or sets the client side OnViewAllClick event handler.
        ///// </summary>
        ///// <value>The OnViewAllClick event handler name.</value>
        //[ClientEventHandler("dj.RealtimeHeadlineList.ViewAllClick")]
        //public string OnViewAllClick { get; set; }

        /// <summary>
        /// Gets or sets the client side Headline Click event Handler
        /// </summary>
        [ClientEventHandler("dj.RealtimeHeadlineList.HeadlineClick")]
        public string OnHeadlineClick { get; set; }

        #endregion

        #region Options

        [ClientProperty("alertContext")]
        public string AlertContext { get; set; }

        [ClientProperty("maxHeadlinesToReturn")]
        public int MaxHeadlinesToReturn { get; set; }

        [ClientProperty("dateTimeFormatingPreference")]
        public string DateTimeFormatingPreference { get; set; }

        [ClientProperty("clockType")]
        public ClockType ClockType { get; set; }

        /// <summary>
        /// The path to the web service
        /// </summary>
        /// <value>The service path.</value>
        [UrlProperty]
        [TypeConverter(typeof(WebServicePathConverter))]
        [DefaultValue("")]
        [ClientProperty("RealtimeHeadlineListServiceUrl")]
        public string RealtimeHeadlineListServiceUrl
        { get; set; }

        #endregion

        [ClientTokens]
        public RealtimeHeadlineListTokens Tokens { get; set; }

        [ClientData]
        public HeadlineListDataResult Result { get; set; }

        public RealtimeHeadlineListModel()
        {
            AlertContext = null;
            MaxHeadlinesToReturn = 10;
            DateTimeFormatingPreference = "";
            ClockType = ClockType.TwelveHours;
            Tokens = new RealtimeHeadlineListTokens();
            RealtimeHeadlineListServiceUrl = "~DowJones.Tools.WebServices.RealtimeHeadlineListService.asmx";
        }
    }
}
