// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpResource.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the HttpResourceHandler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using EMG.widgets.ui.utility;

namespace EMG.widgets.ui.Handlers
{
    /// <summary>
    /// Http Resource Handler for use with static js and css files.
    /// </summary>
    public class HttpResourceHandler : IHttpHandler
    {
        private int expirationTimeInMinutes = 365 * 24 * 60;

        /// <summary>
        /// Gets the expiration time in minutes.
        /// </summary>
        /// <value>The expiration time in minutes.</value>
        public int ExpirationTimeInMinutes
        {
            get { return expirationTimeInMinutes; }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var assemblyModifiedDate = GetAssemblyModifiedTime();

            // See "if Modified since" was requested in the http headers, and check it with the assembly modified time
            var s = context.Request.Headers["If-Modified-Since"];
            DateTime tempDate;
            if (((s != null) && DateTime.TryParse(s, out tempDate)) && (tempDate >= assemblyModifiedDate))
            {
                context.Response.StatusCode = 0x130;
                return;
            }

            // Read setName, contentType and version. All are required. They are
            // used as cache key
            var resourceName = request["n"] ?? string.Empty;
            var contentType = request["t"] ?? string.Empty;
            var version = request["v"] ?? string.Empty;
            var directory = request["d"] ?? string.Empty;
            var exTime = request["et"] ?? string.Empty;

            if (!string.IsNullOrEmpty(exTime))
            {
                int t;
                if (Int32.TryParse(exTime, NumberStyles.Any, CultureInfo.InvariantCulture, out t))
                {
                    expirationTimeInMinutes = t;
                }
            }

            // Load the files defined in <appSettings> and process each file
            var fileName = string.IsNullOrEmpty(directory) ?
                string.Format("~{0}.{1}", resourceName, contentType) :
                string.Format("~/{0}/{1}.{2}", directory, resourceName, contentType);

            resourceName = fileName.Replace('/', '.');

            // Decide if browser supports compressed response
            var isCompressed = CanCompress(context.Request);

            // If the set has already been cached, write the response directly from
            // cache. Otherwise generate the response and cache it
            if (WriteFromCache(context, resourceName, version, isCompressed, contentType))
            {
                return;
            }

            using (var memoryStream = new MemoryStream(5000))
            {
                // Decide regular stream or GZipStream based on whether the response
                // can be cached or not
                using (var writer = GetStream(memoryStream, isCompressed, request))
                {
                    var fileBytes = GetFileBytes(context, fileName.Trim());
                    writer.Write(fileBytes, 0, fileBytes.Length);
                    writer.Close();
                }

                // Cache the combined response so that it can be directly written
                // in subsequent calls 
                var responseBytes = memoryStream.ToArray();
                context.Cache.Insert(GetCacheKey(resourceName, version, isCompressed), responseBytes, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(ExpirationTimeInMinutes));

                // Generate the response
                WriteBytes(responseBytes, context, isCompressed, contentType);
            }
        }

        /// <summary>
        /// Gets the assembly modified time.
        /// </summary>
        /// <returns><c ref="DateTime">DateTime object</c></returns>
        private static DateTime GetAssemblyModifiedTime()
        {
            var lastWriteTime = File.GetLastWriteTime(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath);
            return new DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day, lastWriteTime.Hour, lastWriteTime.Minute, 0);
        }

        /// <summary>
        /// The get cache key.
        /// </summary>
        /// <param name="setName">The set name.</param>
        /// <param name="version">The version.</param>
        /// <param name="isCompressed">The is compressed.</param>
        /// <returns>A string that is the cache key .</returns>
        private static string GetCacheKey(string setName, string version, bool isCompressed)
        {
            return "HttpResourceHandler." + setName + "." + version + "." + isCompressed;
        }

        /// <summary>
        /// Determines whether this instance [can GZip] the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// <c>true</c> if this instance [can G zip] the specified request; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanCompress(HttpRequest request)
        {
            // var str = request.ContentType.ToLower(CultureInfo.InvariantCulture);
            if (!request.Browser.IsBrowser("IE") || (request.Browser.MajorVersion > 6))
            {
                // Check Accept Encoding [Short Circuit]
                var acceptEncoding = request.Headers["Accept-Encoding"];
                if (string.IsNullOrEmpty(acceptEncoding))
                {
                    return false;
                }

                // Check Content Encoding [Short Circuit]
                var contentEncoding = request.Headers["Content-Encoding"];
                if (!string.IsNullOrEmpty(contentEncoding))
                {
                    return false;
                }

                // Process
                acceptEncoding = acceptEncoding.ToLower(CultureInfo.InvariantCulture);
                return !string.IsNullOrEmpty(acceptEncoding) && (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"));
            }

            return false;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="memoryStream">The memory stream.</param>
        /// <param name="isCompressed">if set to <c>true</c> [is compressed].</param>
        /// <param name="request">The request.</param>
        /// <returns>A Stream object.</returns>
        private static Stream GetStream(Stream memoryStream, bool isCompressed, HttpRequest request)
        {
            // Check Accept Encoding [Short Circuit]
            var acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return memoryStream;
            }

            // Check Content Encoding [Short Circuit]
            var contentEncoding = request.Headers["Content-Encoding"];
            if (!string.IsNullOrEmpty(contentEncoding))
            {
                return memoryStream;
            }

            // Process
            acceptEncoding = acceptEncoding.ToLower(CultureInfo.InvariantCulture);
            if (isCompressed)
            {
                if (acceptEncoding.Contains("gzip"))
                {
                    return new GZipStream(memoryStream, CompressionMode.Compress);
                }

                if (acceptEncoding.Contains("deflate"))
                {
                    return new DeflateStream(memoryStream, CompressionMode.Compress);
                }
            }

            return memoryStream;
        }

        /// <summary>
        /// Gets the file bytes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>A byte array</returns>
        private static byte[] GetFileBytes(HttpContext context, string virtualPath)
        {
            if (virtualPath.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var client = new WebClient())
                {
                    return client.DownloadData(virtualPath);
                }
            }

            var physicalPath = context.Server.MapPath(virtualPath);
            var bytes = File.ReadAllBytes(physicalPath);
            return bytes;
        }

        /// <summary>
        /// Writes from cache.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="version">The version.</param>
        /// <param name="isCompressed">if set to <c>true</c> [is compressed].</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The write from cache.</returns>
        private bool WriteFromCache(HttpContext context, string resourceName, string version, bool isCompressed, string contentType)
        {
            var responseBytes = context.Cache[GetCacheKey(resourceName, version, isCompressed)] as byte[];

            if (null == responseBytes || 0 == responseBytes.Length)
            {
                return false;
            }

            WriteBytes(responseBytes, context, isCompressed, contentType);
            return true;
        }

        /// <summary>
        /// Writes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="context">The context.</param>
        /// <param name="isCompressed">if set to <c>true</c> [is compressed].</param>
        /// <param name="contentType">Type of the content.</param>
        private void WriteBytes(byte[] bytes, HttpContext context, bool isCompressed, string contentType)
        {
            var response = context.Response;
            var assemblyModifiedDate = GetAssemblyModifiedTime();
            var ct = (MimeType)Enum.Parse(typeof(MimeType), contentType, true);
            response.AppendHeader("Content-Length", bytes.Length.ToString());
            response.ContentType = utility.Utility.GetMimeType(ct);
            if (isCompressed)
            {
                response.AppendHeader("Content-Encoding", "gzip");
            }

            // Set the same (~forever) caching rules that ScriptResource.axd uses
            var cache = response.Cache;
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByContentEncodings["gzip"] = true;
            cache.VaryByContentEncodings["deflate"] = true;
            cache.SetOmitVaryStar(true);
            //// cache.VaryByParams["*"] = true;
            cache.VaryByParams["n"] = true;
            cache.VaryByParams["t"] = true;
            cache.VaryByParams["v"] = true;
            cache.VaryByParams["d"] = true;
            cache.VaryByParams["et"] = true;
            cache.SetExpires(assemblyModifiedDate + TimeSpan.FromMinutes(ExpirationTimeInMinutes));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(assemblyModifiedDate);
            
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.Flush();
        }
    }
}
