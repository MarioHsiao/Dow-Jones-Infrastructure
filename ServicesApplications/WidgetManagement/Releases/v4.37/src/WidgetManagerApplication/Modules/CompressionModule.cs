// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressionModule.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CompressionModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO.Compression;
using System.Web;
using log4net;

namespace EMG.widgets.ui.Modules
{
    /// <summary>
    /// Compression Module for handling various compression models
    /// </summary>
    public class CompressionModule : IHttpModule
    {
        #region Private Members

        private readonly ILog _log = LogManager.GetLogger(typeof(CompressionModule));

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.PreRequestHandlerExecute += PreRequestHandlerExecute;
            app.PostRequestHandlerExecute += PostRequestHandlerExecute;
        }

        #endregion

        private static bool CanCompressAjaxCall(string str, HttpRequest request)
        {
            return (str.StartsWith("application/x-www-form-urlencoded") || str.StartsWith("application/json")) && (!request.Browser.IsBrowser("IE") || (request.Browser.MajorVersion > 6));
        }

        private static bool CanCompressExternalResource(HttpRequest request)
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

        private static void PrepareResponseCache(HttpResponse response)
        {
            /* 
            HttpCachePolicy cache = response.Cache;
            DateTime now = DateTime.Now;
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByParams["d"] = true;
            cache.SetOmitVaryStar(true);
            cache.SetExpires(now + TimeSpan.FromDays(365.0));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(now);
            */

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

        private static void PostRequestHandlerExecute(object sender, EventArgs e)
        {
        }

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
            }
            else if (acceptEncoding.Contains("deflate"))
            {
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                response.AddHeader("Content-Encoding", "deflate");
            }
        }

        private void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var request = application.Request;
            var response = application.Response;
            _log.Debug(request.Url.AbsoluteUri.ToLowerInvariant());

            // Check content type [Short Circuit]
            var str = request.ContentType.ToLowerInvariant();
            _log.Debug(response.ContentType.ToLowerInvariant());

            if (CanCompressAjaxCall(str, request))
            {
                Compress(request, response);
            }
            else if (CanCompressExternalResource(request))
            {
               Compress(request, response);
            }
        }
    }
}
