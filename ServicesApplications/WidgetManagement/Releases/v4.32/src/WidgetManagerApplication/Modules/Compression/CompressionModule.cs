// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressionModule.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.IO.Compression;
using System.Web;
using log4net;

namespace EMG.widgets.ui.Modules.Compression
{
    /// <summary>
    /// Compression Module for handling various compression models
    /// </summary>
    public sealed class CompressionModule : IHttpModule
    {
        /// <summary>
        /// The current log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(CompressionModule));

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
        /// <param name="app">The HttpApplication.</param>
        public void Init(HttpApplication app)
        {
            // app.ReleaseRequestState += CompressExecute;
            // app.PreSendRequestHeaders += CompressExecute;
            app.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        #endregion

        /// <summary>
        /// The can compress ajax call.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// <c>true</c> you can compress a ajax call.
        /// </returns>
        private static bool CanCompressAjaxCall(string contentType, HttpRequest request)
        {
            return (contentType.StartsWith("application/x-www-form-urlencoded") || contentType.StartsWith("application/json")) && (!request.Browser.IsBrowser("IE") || (request.Browser.MajorVersion > 6));
        }

        /// <summary>
        /// The can compress custom resources.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// <c>true</c> if this instance [can compress custom resources] the specified request; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanCompressCustomResources(HttpRequest request)
        {
            if (request.Url.Segments.Length > 0)
            {
                if (request.Url.Segments[request.Url.Segments.Length - 1].ToLowerInvariant() == "render.aspx" ||
                    request.Url.Segments[request.Url.Segments.Length - 1].ToLowerInvariant() == "insertwidget.aspx" ||
                    request.Url.Segments[request.Url.Segments.Length - 1].ToLowerInvariant() == "render.ashx" ||
                    request.Url.Segments[request.Url.Segments.Length - 1].ToLowerInvariant() == "insertwidget.ashx")
                {
                    return !request.Browser.IsBrowser("IE") || (request.Browser.MajorVersion > 6);
                }
            }

            return false;
        }

        /// <summary>
        /// The compress.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        private static void Compress(HttpRequest request, HttpResponse response)
        {
            // Check Accept Encoding [Short Circuit]
            var acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return;
            }

            // Check Content Encoding [Short Circuit]
            var contentEncoding = request.Headers["Content-Encoding"];
            if (!string.IsNullOrEmpty(contentEncoding))
            {
                return;
            }

            // Process
            acceptEncoding = acceptEncoding.ToLower(CultureInfo.InvariantCulture);
            if (acceptEncoding.Contains("gzip"))
            {
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                response.AddHeader("Content-Encoding", "gzip");
                return;
            }

            if (acceptEncoding.Contains("deflate"))
            {
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                response.AddHeader("Content-Encoding", "deflate");
                return;
            }

            return;
        }

        /// <summary>
        /// The pre compress execute.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var application = (HttpApplication) sender;
            var request = application.Request;
            var response = application.Response;

            log.Debug(request.Url.AbsoluteUri.ToLowerInvariant());

            // Check content type [Short Circuit]
            var str = request.ContentType.ToLowerInvariant();
            log.Debug(response.ContentType.ToLowerInvariant());

            if (CanCompressAjaxCall(str, request))
            {
                Compress(request, response);
            }
            else if (CanCompressCustomResources(request))
            {
                Compress(request, response);
            }
        }

        #region ..:: Unused Code ::.. 
        /*
        /// <summary>
        /// The post compress execute.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void PostCompressExecute(object sender, EventArgs e)
        {
            var application = (HttpApplication) sender;
            var request = application.Request;
            var response = application.Response;

            log.Debug(request.Url.AbsoluteUri.ToLowerInvariant());

            if (!CanCompressOtherResources(request, response))
            {
                return;
            }

            var encodingMgr = new EncodingManager(application.Context);

            if (encodingMgr.IsEncodingEnabled)
            {
                encodingMgr.CompressResponse();
                encodingMgr.SetResponseEncodingType();
                PrepareResponseCache(response);
            }
        }
         /// <summary>
        /// The can compress other resources.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns>
        /// <c>true</c> if this instance [can compress other resources] the specified request; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanCompressOtherResources(HttpRequest request, HttpResponse response)
        {
            if (request.Url.Segments[request.Url.Segments.Length - 1].ToLowerInvariant() == "webresource.axd")
            {
                if (response.ContentType.ToLowerInvariant() == "text/css" ||
                    response.ContentType.ToLowerInvariant() == "text/javascript")
                {
                    return !request.Browser.IsBrowser("IE") || (request.Browser.MajorVersion > 6);
                }
            }

            return false;
        }

        /// <summary>
        /// The prepare response cache.
        /// </summary>
        /// <param name="response">The response.</param>
        private static void PrepareResponseCache(HttpResponse response)
        {
            // Set the same (~forever) caching rules that ScriptResource.axd uses
            var cache = response.Cache;
            var now = DateTime.Now;
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByContentEncodings["gzip"] = true;
            cache.VaryByContentEncodings["deflate"] = true;
            cache.SetOmitVaryStar(true);
            cache.VaryByParams["*"] = true;
            cache.SetExpires(now + TimeSpan.FromDays(365.0));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(now);
        }
        */
        #endregion
    }
}