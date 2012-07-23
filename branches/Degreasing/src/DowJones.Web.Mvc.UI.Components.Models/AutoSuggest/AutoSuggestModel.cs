using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.AutoSuggest
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AutoCompletionType
    {
        Company,

        Executive,

        Source,

        Region,

        NewsSubject,

        Industry,

        Keyword,

        Outlet,

        Author,

        PublisherMetaData,

        PublisherCity,

        Categories,

        CalendarCompany,

        CalendarKeyword,

        PrivateMarkets
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthType
    {
        SessionId,

        EncryptedToken,

        SuggestContext

    }

    public class AutoSuggestModel : ViewComponentModel
    {
        #region ..:: Client Properties ::..

        //Gets or sets Suggest Service Options
        [ClientProperty("suggestServiceUrl")]
        public string SuggestServiceUrl { get; set; }
        
        /// <summary>
        /// Gets or Sets Autocompletion type
        /// </summary>
        [ClientProperty("autocompletionType")]
        
        public AutoCompletionType AutocompletionType { get; set; }
        
        /// <summary>
        /// Gets or Sets Autocompletion type
        /// </summary>
        [ClientProperty("controlId")]
        public string ControlId { get; set; }

        /// <summary>
        /// Gets or Sets Auto Suggest Rest Service Options
        /// </summary>
        [ClientProperty("serviceOptions")]
        public string ServiceOptions { get; set; }

        /// <summary>
        /// Gets or Sets Auto Suggest Tokens
        /// </summary>
        [ClientProperty("tokens")]
        public string Tokens { get; set; }

        /// <summary>
        /// Gets or Sets Auto Suggest Tokens
        /// </summary>
        [ClientProperty("columns")]
        public string Columns { get; set; }

        /// <summary>
        /// Gets or Sets SuggestContext
        /// </summary>
        [ClientProperty("suggestContext")]
        public string SuggestContext { get; set; }

        /// <summary>
        /// Gets or Sets AutenticationType Value
        /// </summary>
        [ClientProperty("authTypeValue")]
        public string AuthTypeValue { get; set; }

        /// <summary>
        /// Gets or Sets AutenticationType
        /// </summary>
        [ClientProperty("authType")]
        public AuthType AuthType { get; set; }

        /// <summary>
        /// Gets or Sets Encrypted Token
        /// </summary>
        [ClientProperty("useEncryptedKey")]
        public string UseEncryptedKey { get; set; }

        /// <summary>
        /// Gets or Sets Session ID
        /// </summary>
        [ClientProperty("useSessionId")]
        public string UseSessionId { get; set; }

        /// <summary>
        /// Gets or Sets Select First. If true, selects the first row from the suggest list.
        /// </summary>
        /// [DefaultValue(false)]
        [ClientProperty("selectFirst")]
        public bool SelectFirst { get; set; }

        /// <summary>
        /// Gets or Sets FillInputOnKeyUpDown. If true , automatically fills the input on KEY UP or KEY DOWN.
        /// </summary>
        /// [DefaultValue(false)]
        [ClientProperty("fillInputOnKeyUpDown")]
        public bool FillInputOnKeyUpDown { get; set; }

        /// <summary>
        /// Gets or Sets ShowHelp. If true, shows the help row at the top of the suggest list.
        /// </summary>
        [DefaultValue(false)]
        [ClientProperty("showHelp")]
        public bool ShowHelp { get; set; }

        /// <summary>
        /// Gets or Sets ShowViewAll. If true, shows the viewAll link at the bottom of the suggest list.
        /// </summary>
        [DefaultValue(false)]
        [ClientProperty("showViewAll")]
        public bool ShowViewAll { get; set; }
        
        #endregion

        #region ..:: Client Event Handlers ::..

        /// <summary>
        /// Gets or sets the client side OnItemClick event handler.
        /// </summary>
        /// <value>The OnItemClick event handler name.</value>
        [ClientEventHandler("dj.AutoSuggest.headlineClick")]
        public string OnItemClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnInfoClick event handler.
        /// </summary>
        /// <value>The OnInfoClick event handler name.</value>
        [ClientEventHandler("dj.PortalHeadlineList.sourceClick")]
        public string OnInfoClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnPromoteClick event handler.
        /// </summary>
        /// <value>The OnPromoteClick event handler name.</value>
        [ClientEventHandler("dj.AutoSuggest.authorClick")]
        public string OnPromoteClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnNotClick event handler.
        /// </summary>
        /// <value>The OnNotClick event handler name.</value>
        [ClientEventHandler("dj.AutoSuggest.headlineClick")]
        public string OnNotClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnHelpRowClick event handler.
        /// </summary>
        /// <value>The OnHelpRowClick event handler name.</value>
        [ClientEventHandler("dj.AutoSuggest.sourceClick")]
        public string OnHelpRowClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnViewAllClick event handler.
        /// </summary>
        /// <value>The OnViewAllClick event handler name.</value>
        [ClientEventHandler("dj.AutoSuggest.viewAllClick")]
        public string OnViewAllClick { get; set; }


        /// <summary>
        /// Gets or sets the client side OnViewMorePrivateMarketsClick event handler.
        /// </summary>
        /// <value>The OnViewMorePrivateMarketsClick event handler name.</value>
        [ClientEventHandler("dj.AutoSuggest.viewMorePrivateMarketsClick")]
        public string OnViewMorePrivateMarketsClick { get; set; }

        /// <summary>
        /// Gets or sets the client side OnError event handler.
        /// </summary>
        /// <value>The OnError event handler name.</value>
        [ClientEventHandler("dj.AutoSuggest.authorClick")]
        public string OnError { get; set; }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AutoSuggestModel()
        {
            SuggestServiceUrl = "http://suggest.factiva.com/Search/1.0";
        }
    }

}