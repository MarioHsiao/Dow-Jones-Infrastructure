// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JSONPHttpModule.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved
// </copyright>
// <summary>
//   Defines the JSONPHttpModule type.
// </summary>
// <author>
//   David Da Costa
// </author>
// <lastModified>
//  <entry><date>5/10/2008</date><user>dacostad</user></entry>
// </lastModified>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web;

namespace DowJones.Web.Modules.JSONP
{
    using HttpApplication = System.Web.HttpApplication;

    /// <summary>
    /// The jsonp http module.
    /// </summary>
    public class JSONPHttpModule : IHttpModule
    {
        /// <summary>
        /// The jso n_ conten t_ type.
        /// </summary>
        private const string JSONContentType = "application/json; charset=utf-8";

        #region IHttpModule Members

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Inits the specified app.
        /// </summary>
        /// <param name="application">The application.</param>
        public void Init(HttpApplication application)
        {
            application.BeginRequest += OnBeginRequest;
            application.ReleaseRequestState += OnReleaseRequestState;
        }

        #endregion

        /// <summary>
        /// Called when [begin request].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void OnBeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;

            if (!Apply(app.Context.Request))
            {
                return;
            }

            if (string.IsNullOrEmpty(app.Context.Request.ContentType))
            {
                app.Context.Request.ContentType = JSONContentType;
            }
        }


        /// <summary>
        /// Called when [release request state].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void OnReleaseRequestState(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;

            if (!Apply(app.Context.Request))
            {
                return;
            }

            app.Context.Response.Filter = new JSONResponseFilter(app.Context.Response.Filter, app.Context);
        }

        /// <summary>
        /// Applies the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The Aapply.</returns>
        private static bool Apply(HttpRequest request)
        {
            if (!request.Url.AbsolutePath.Contains(".asmx"))
            {
                return false;
            }

            return "json" == request.QueryString.Get("format");
        }
    }
}