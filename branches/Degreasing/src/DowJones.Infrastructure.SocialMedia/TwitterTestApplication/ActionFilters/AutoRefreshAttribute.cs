// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoRefreshAttribute.cs" company="Dow Jones & Co.">
//   
// </copyright>
// <summary>
//   The auto refresh attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace TwitterTestApplication.ActionFilters
{
    /// <summary>
    /// The auto refresh attribute.
    /// </summary>
    public class AutoRefreshAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The default duration in seconds.
        /// </summary>
        public const int DefaultDurationInSeconds = 300; // 5 Minutes

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRefreshAttribute"/> class.
        /// </summary>
        public AutoRefreshAttribute()
        {
            this.DurationInSeconds = DefaultDurationInSeconds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRefreshAttribute"/> class.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public AutoRefreshAttribute(int seconds)
        {
            this.DurationInSeconds = seconds;
        }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        /// <value>
        /// The duration in seconds.
        /// </value>
        public int DurationInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the name of the route.
        /// </summary>
        /// <value>
        /// The name of the route.
        /// </value>
        public string RouteName { get; set; }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var url = this.BuildUrl(filterContext);
            var headerValue = string.Concat(this.DurationInSeconds, ";Url=", url);
            filterContext.HttpContext.Response.AppendHeader("Refresh", headerValue);
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// The build url.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>
        /// A url stinrg.
        /// </returns>
        private string BuildUrl(ControllerContext filterContext)
        {
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            string url;

            if (!string.IsNullOrEmpty(this.RouteName))
            {
                url = urlHelper.RouteUrl(this.RouteName);
            }
            else if (!string.IsNullOrEmpty(this.ControllerName) && !string.IsNullOrEmpty(this.ActionName))
            {
                url = urlHelper.Action(this.ActionName, this.ControllerName);
            }
            else if (!string.IsNullOrEmpty(this.ActionName))
            {
                url = urlHelper.Action(this.ActionName);
            }
            else
            {
                url = filterContext.HttpContext.Request.RawUrl;
            }

            return url;
        }
    }
}