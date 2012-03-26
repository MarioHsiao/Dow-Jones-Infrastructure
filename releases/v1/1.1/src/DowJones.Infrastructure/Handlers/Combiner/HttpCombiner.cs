// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpCombiner.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web;
using System.Web.Caching;

namespace DowJones.Utilities.Handlers.Combiner
{
    /// <summary>
    /// The http combiner.
    /// </summary>
    public class HttpCombiner : IHttpHandler
    {
        //// private const bool DO_GZIP = true;
        
        /// <summary>
        /// The cache duration.
        /// </summary>
        private static readonly TimeSpan CACHE_DURATION = TimeSpan.FromDays(30);

        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether IsReusable.
        /// </summary>
        public bool IsReusable
        { 
            get { return true; }
        }

        /// <summary>
        /// The process request.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            // Read setName, contentType and version. All are required. They are
            // used as cache key
            var setName = request["s"] ?? string.Empty;
            var contentType = request["t"] ?? string.Empty;
            var version = request["v"] ?? string.Empty;

            // Decide if browser supports compressed response
            var isCompressed = CanGZip(context.Request);

            // Response is written as UTF8 encoding. If you are using languages like
            // Arabic, you should change this to proper encoding 
            //  var encoding = new UTF8Encoding(false);

            // If the set has already been cached, write the response directly from
            // cache. Otherwise generate the response and cache it
            if (WriteFromCache(context, setName, version, isCompressed, contentType))
            {
                return;
            }

            using (var memoryStream = new MemoryStream(5000))
            {
                // Decide regular stream or GZipStream based on whether the response
                // can be cached or not
                using (var writer = isCompressed ? (Stream)(new GZipStream(memoryStream, CompressionMode.Compress)) : memoryStream)
                {
                    // Load the files defined in <appSettings> and process each file
                    var setDefinition = ConfigurationManager.AppSettings[setName] ?? string.Empty;
                    var fileNames = setDefinition.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var fileName in fileNames)
                    {
                        var fileBytes = GetFileBytes(context, fileName.Trim());
                        writer.Write(fileBytes, 0, fileBytes.Length);
                    }

                    writer.Close();
                }

                // Cache the combined response so that it can be directly written
                // in subsequent calls 
                var responseBytes = memoryStream.ToArray();
                context.Cache.Insert(GetCacheKey(setName, version, isCompressed), responseBytes, null, Cache.NoAbsoluteExpiration, CACHE_DURATION);

                // Generate the response
                WriteBytes(responseBytes, context, isCompressed, contentType);
            }
        }
           
        #endregion

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
        /// <param name="setName">Name of the set.</param>
        /// <param name="version">The version.</param>
        /// <param name="isCompressed">if set to <c>true</c> [is compressed].</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The write from cache.</returns>
        private static bool WriteFromCache(HttpContext context, string setName, string version, bool isCompressed, string contentType)
        {
            var responseBytes = context.Cache[GetCacheKey(setName, version, isCompressed)] as byte[];

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
        private static void WriteBytes(byte[] bytes, HttpContext context, bool isCompressed, string contentType)
        {
            var response = context.Response;

            response.AppendHeader("Content-Length", bytes.Length.ToString());
            response.ContentType = contentType;
            if (isCompressed)
            {
                response.AppendHeader("Content-Encoding", "gzip");
            }

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.Add(CACHE_DURATION));
            context.Response.Cache.SetMaxAge(CACHE_DURATION);
            context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.Flush();
        }

        /// <summary>
        /// Determines whether this instance [can G zip] the specified request.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// <c>true</c> if this instance [can G zip] the specified request; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanGZip(HttpRequest request)
        {
            var str = request.ContentType.ToLower(CultureInfo.InvariantCulture);
            if ((str.StartsWith("application/x-www-form-urlencoded") || str.StartsWith("application/json")) && (!request.Browser.IsBrowser("IE") || (request.Browser.MajorVersion > 6)))
            {
                var acceptEncoding = request.Headers["Accept-Encoding"];
                return !string.IsNullOrEmpty(acceptEncoding) && (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"));
            }

            return false;
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
            return "HttpCombiner." + setName + "." + version + "." + isCompressed;
        }
    }
}