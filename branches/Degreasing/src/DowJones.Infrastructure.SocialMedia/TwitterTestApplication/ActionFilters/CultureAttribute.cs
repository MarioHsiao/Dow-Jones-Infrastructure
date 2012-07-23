// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureAttribute.cs" company="Dow Jones & Co">
//   
// </copyright>
// <summary>
//   The culture attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace TwitterTestApplication.ActionFilters
{
    /// <summary>
    /// The culture attribute.
    /// </summary>
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        #region Enums

        /// <summary>
        /// Represents the location the culture code can be found
        /// </summary>
        public enum CultureLocation
        {
            /// <summary>
            /// This option should never be used.
            /// </summary>
            None = 0, 

            /// <summary>
            /// Use when the culture code is saved in a cookie.  
            /// When using be sure to specify the CookieName property.
            /// </summary>
            Cookie = 1, 

            /// <summary>
            /// Use when the culture code is specified in the query string.  
            /// When using be sure to specify the QueryStringParamName property.
            /// </summary>
            QueryString = 2, 

            /// <summary>
            /// Use when the culture code is saved in session state.  
            /// When using be sure to specify the SessionParamName property.
            /// </summary>
            Session = 4, 

            /// <summary>
            /// Use when the culture code is specified in the URL.  
            /// This assume a format of "{language}/{country}".
            /// When using be sure to specify the CountryActionParamName and 
            /// LanguageActionParamName properties.
            /// </summary>
            Url = 16
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the cookie containing the culture code.  Specify this value when CultureStore is set to Cookie.
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// Gets or sets the name of the action parameter containing the country code.  Specify this value when CultureStore is set to URL.
        /// </summary>
        public string CountryActionParamName { get; set; }

        /// <summary>
        /// Gets or sets CultureLocation where the culture code is to be read from.  This is required to be set.
        /// </summary>
        public CultureLocation CultureStore { get; set; }

        /// <summary>
        /// Gets or sets the name of the action parameter containing the language code.  Specify this value when CultureStore is set to URL.
        /// </summary>
        public string LanguageActionParamName { get; set; }

        /// <summary>
        /// Gets or sets the name of the query string parameter containing the country code.  Specify this value when CultureStore is set to QueryString.
        /// </summary>
        public string QueryStringParamName { get; set; }

        /// <summary>
        /// Gets or sets the  name of the session parameter containing the country code.  Specify this value when CultureStore is set to Session.
        /// </summary>
        public string SessionParamName { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IActionFilter

        /// <summary>
        /// The on action executed.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        /// <summary>
        /// The on action executing.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (this.CultureStore == CultureLocation.None)
            {
                return;
            }

            string cultureCode = this.GetCultureCode(filterContext);

            // now that we've collected the culture code, set the culture for the thread
            if (!string.IsNullOrEmpty(cultureCode))
            {
                try
                {
                    var culture = new CultureInfo(cultureCode);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
                catch
                {
                    // TODO: Handle error?  Really, what can we do besides log it?
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// The get culture code.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>
        /// The culture code string.
        /// </returns>
        protected string GetCultureCode(ActionExecutingContext filterContext)
        {
            // Everything but CultureLocation.URL requires a valid HttpContext
            if (this.CultureStore != CultureLocation.Url)
            {
                if (filterContext.RequestContext.HttpContext == null)
                {
                    return string.Empty;
                }
            }

            string cultureCode = string.Empty;

            if (this.CultureStore == CultureLocation.Cookie)
            {
                if (filterContext != null 
                    && filterContext.RequestContext != null
                    && filterContext.RequestContext.HttpContext != null
                    && filterContext.RequestContext.HttpContext.Request.Cookies[this.CookieName] != null)
                {
                    if (filterContext.RequestContext.HttpContext.Request.Cookies[this.CookieName].Value != string.Empty)
                    {
                        cultureCode = filterContext.RequestContext.HttpContext.Request.Cookies[this.CookieName].Value;
                    }
                }

                return cultureCode;
            }

            if (this.CultureStore == CultureLocation.QueryString)
            {
                cultureCode = filterContext.RequestContext.HttpContext.Request[this.QueryStringParamName];
                return cultureCode ?? string.Empty;
            }

            if (this.CultureStore == CultureLocation.Session)
            {
                if (filterContext.RequestContext.HttpContext.Session != null)
                {
                    if (filterContext.RequestContext.HttpContext.Session[this.SessionParamName] != null &&
                        filterContext.RequestContext.HttpContext.Session[this.SessionParamName].ToString() != string.Empty)
                    {
                        cultureCode = filterContext.RequestContext.HttpContext.Session[this.SessionParamName].ToString();
                    }
                }

                return cultureCode;
            }

            // if URL it is expected the URL path will contain the culture 
            if (this.CultureStore == CultureLocation.Url)
            {
                if (filterContext.ActionParameters[this.LanguageActionParamName] != null
                    && filterContext.ActionParameters[this.CountryActionParamName] != null
                    && filterContext.ActionParameters[this.LanguageActionParamName].ToString() != string.Empty
                    && filterContext.ActionParameters[this.CountryActionParamName].ToString() != string.Empty)
                {
                    string language = filterContext.ActionParameters[this.LanguageActionParamName].ToString();
                    string country = filterContext.ActionParameters[this.CountryActionParamName].ToString();
                    cultureCode = language + "-" + country;
                }

                return cultureCode;
            }

            return cultureCode ?? string.Empty;
        }

        #endregion
    }
}