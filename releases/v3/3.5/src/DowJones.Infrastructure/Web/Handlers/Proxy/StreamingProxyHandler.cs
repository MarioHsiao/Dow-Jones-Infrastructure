// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamingProxyHandler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Linq;
using System.Text;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Properties;
using DowJones.Web.Handlers.Proxy.Core;

namespace DowJones.Web.Handlers.Proxy
{
    /// <summary>
    /// A proxy the uses the IHttpAsyncHandler to process requests
    /// </summary>
    public class StreamingProxyHandler : IHttpAsyncHandler
    {
        private const int BufferSize = 8 * 1024;
        private readonly List<string> _whiteListedDomains = new List<string>(new[]
                                                                             {
                                                                                 "fdevweb3.win.dowjones.net", 
                                                                                 "api.dowjones.com", "api.int.dowjones.com",
                                                                                 "m.wsj.net", "i.mktw.net", "www.factiva.com" 
                                                                             });

        private readonly List<string> _contentTypes = new List<string>(new[]
                                                                       {
                                                                          "image/png", "image/jpeg", 
                                                                           "image/gif", "application/json", 
                                                                           "text/css", "text/javascript", 
                                                                           "application/javascript", "application/x-shockwave-flash"
                                                                       });
       
        private PipeStream _pipeStream;
        private Stream _responseStream;

        protected bool IncludeContentDisposition { get; set; }
        protected string DefinedTargetUrl { get; set; }

        #region IHttpAsyncHandler Members

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public virtual void ProcessRequest(HttpContext context)
        {
            var origRequest = context.Request;

            var url = origRequest["url"];
            var cacheDuration = Convert.ToInt32(origRequest["cache"] ?? "0");

            if (!IsValidUrl(url))
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                context.Response.Write("Unauthorized Access");
                context.Response.End();
                return;
            }
            
            // See if the user wants to add the content-disposition attribute to the header.
            IncludeContentDisposition = bool.Parse(context.Request["iDisposition"] ?? "true");

            Logger.WriteEntry("--- " + url + " ----");

            if (cacheDuration > 0)
            {
                if (context.Cache[url] != null)
                {
                    var content = context.Cache[url] as CachedContent;
                    if (content != null)
                    {
                        if (!string.IsNullOrEmpty(content.ContentEncoding))
                        {
                            context.Response.AppendHeader("Content-Encoding", content.ContentEncoding);
                        }

                        if (!string.IsNullOrEmpty(content.ContentLength))
                        {
                            context.Response.AppendHeader("Content-Length", content.ContentLength);
                        }

                        context.Response.ContentType = content.ContentType;
                        content.Content.Position = 0;
                        content.Content.WriteTo(context.Response.OutputStream);
                    }
                }
            }

            using (new TimedLog("StreamingProxy\t" + url))
            {
                var proxyRequest = HttpHelper.CreateScalableHttpWebRequest(url, origRequest);
                //// As we will stream the response, don't want to automatically decompress the content
                //// when source sends compressed content
                proxyRequest.AutomaticDecompression = DecompressionMethods.None;

                proxyRequest.Headers.Add(origRequest.Headers);

                if (origRequest.HttpMethod != "GET")
                {
                    var requestBytes = Encoding.UTF8.GetBytes(origRequest.InputStream.GetReader().ReadToEnd());
                    using (var requestStream = proxyRequest.GetRequestStream())
                    {
                        requestStream.Write(requestBytes, 0, requestBytes.Length);
                    }
                }

                using (new TimedLog("StreamingProxy\tTotal GetResponse and transmit data"))
                {
                    using (var response = proxyRequest.GetResponse() as HttpWebResponse)
                    {
                        DownloadData(proxyRequest, response, context, cacheDuration);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
        }

        public StreamingProxyHandler()
        {
            IncludeContentDisposition = true;
        }


        /// <summary>
        /// Initiates an asynchronous call to the HTTP handler.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="cb">The <see cref="T:System.AsyncCallback"/> to call when the asynchronous method call is complete. If <paramref name="cb"/> is null, the delegate is not called.</param>
        /// <param name="extraData">Any extra data needed to process the request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"/> that contains information about the status of the process.
        /// </returns>
        public virtual IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var origRequest = context.Request;
            var url = DefinedTargetUrl ?? origRequest["url"];
            var cacheDuration = Convert.ToInt32(origRequest["cache"] ?? "0");

            if (!IsValidUrl(url))
            {
                var result = new InvalidRequestResult
                {
                    Context = context
                };
                return result;
            }

            if (cacheDuration > 0)
            {
                if (context.Cache[url] != null)
                {
                    // We have response to this URL already cached
                    var result = new SyncResult
                                     {
                                         Context = context,
                                         Content = context.Cache[url] as CachedContent
                                     };
                    return result;
                }
            }

            var proxyRequest = HttpHelper.CreateScalableHttpWebRequest(url, origRequest);
            //// As we will stream the response, don't want to automatically decompress the content
            //// when source sends compressed content
            proxyRequest.AutomaticDecompression = DecompressionMethods.None;

            CopyHeaders(origRequest, proxyRequest);

            if (origRequest.HttpMethod != "GET")
            {
                var requestBytes = (new UTF8Encoding()).GetBytes(origRequest.InputStream.GetReader(Encoding.UTF8).ReadToEnd());
                using (var requestStream = proxyRequest.GetRequestStream())
                {
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                }
            }

            var state = new AsyncState { Context = context, Url = url, CacheDuration = cacheDuration, Request = proxyRequest };
            return proxyRequest.BeginGetResponse(cb, state);
        }

        private static void CopyHeaders(HttpRequest origRequest, HttpWebRequest proxyRequest)
        {
            var credentials = origRequest.Headers["credentials"];
            if (!string.IsNullOrWhiteSpace(credentials))
            {
                proxyRequest.Headers.Add("credentials", credentials);
            }

            var preferences = origRequest.Headers["preferences"];
            if (!string.IsNullOrWhiteSpace(preferences))
            {
                proxyRequest.Headers.Add("preferences", preferences);
            }

            var product = origRequest.Headers["product"];
            if (!string.IsNullOrWhiteSpace(product))
            {
                proxyRequest.Headers.Add("product", product);
            }
        }

        /// <summary>
        /// Provides an asynchronous process End method when the process ends.
        /// </summary>
        /// <param name="result">An <see cref="T:System.IAsyncResult"/> that contains information about the status of the process.</param>
        public void EndProcessRequest(IAsyncResult result)
        {
            var invalidRequestResult = result as InvalidRequestResult;

            if (invalidRequestResult != null)
            {
                invalidRequestResult.Context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                invalidRequestResult.Context.Response.Write("Unauthorized Access");
                invalidRequestResult.Context.Response.End();
                return;
            }

            var syncResult = result as SyncResult;
            if (syncResult != null)
            {
                syncResult.Context.Response.ContentType = syncResult.Content.ContentType;
                syncResult.Context.Response.AppendHeader("Content-Encoding", syncResult.Content.ContentEncoding);
                syncResult.Context.Response.AppendHeader("Content-Length", syncResult.Content.ContentLength);

                syncResult.Content.Content.Seek(0, SeekOrigin.Begin);
                syncResult.Content.Content.WriteTo(syncResult.Context.Response.OutputStream);
                return;
            }

            // Content is not available in cache and needs to be downloaded from external source
            var state = result.AsyncState as AsyncState;

            if (state == null)
            {
                return;
            }
            
            state.Context.Response.Buffer = false;
            var request = state.Request;

            HttpWebResponse response = null;
            try
            {
                response = request.EndGetResponse(result) as HttpWebResponse;
                DownloadData(request, response, state.Context, state.CacheDuration);
            }
            catch (WebException ex)
            {
                // pass thru the response body
                DownloadData(request, ex.Response as HttpWebResponse, state.Context, state.CacheDuration);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        #endregion

        /// <summary>
        /// Downloads the data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="context">The context.</param>
        /// <param name="cacheDuration">The cache duration.</param>
        private void DownloadData(WebRequest request, HttpWebResponse response, HttpContext context, int cacheDuration)
        {
            var responseBuffer = new MemoryStream();
            context.Response.Buffer = false;

            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    context.Response.TrySkipIisCustomErrors = true;
                    context.Response.StatusCode = (int)response.StatusCode;
                }

                using (var readStream = response.GetResponseStream())
                {
                    if (context.Response.IsClientConnected &&
                        IsValidContentType(response.ContentType))
                    {

                        string contentLength;
                        string contentEncoding;
                        string contentDisposition;
                        ProduceResponseHeader(response, context, cacheDuration, out contentLength, out contentEncoding, out contentDisposition);

                        var totalBytesWritten = TransmitDataAsynchronously(context, readStream, responseBuffer);

                        Logger.WriteEntry("Response generated: " + DateTime.Now);
                        Logger.WriteEntry(string.Format("Content Length vs. Bytes Written: {0} vs. {1} ", contentLength, totalBytesWritten));

                        if (response.StatusCode == HttpStatusCode.OK && cacheDuration > 0)
                        {
                            // Cache the content on server for specific duration
                            var cache = new CachedContent
                                        {
                                            Content = responseBuffer,
                                            ContentEncoding = contentEncoding,
                                            ContentDisposition = contentDisposition,
                                            ContentLength = contentLength,
                                            ContentType = response.ContentType,
                                        };

                            context.Cache.Insert(
                                request.RequestUri.ToString(),
                                cache,
                                null,
                                Cache.NoAbsoluteExpiration,
                                TimeSpan.FromMinutes(cacheDuration),
                                CacheItemPriority.Normal,
                                null);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.Write("Unable to process Uri");
                    }

                    using (new TimedLog("StreamingProxy\tResponse Flush"))
                    {
                        context.Response.Flush();
                    }
                }
            }
            catch (Exception x)
            {
                Logger.WriteEntry(x.ToString());
                request.Abort();
            }
        }

        /// <summary>
        /// Transmits the data async optimized.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="readStream">The read stream.</param>
        /// <param name="responseBuffer">The response buffer.</param>
        /// <returns></returns>
        private int TransmitDataAsynchronously(HttpContext context, Stream readStream, Stream responseBuffer)
        {
            _responseStream = readStream;

            _pipeStream = new PipeStreamBlock(10000);

            var buffer = new byte[BufferSize];

            // Asynchronously read content form response stream
            var readerThread = new Thread(ReadData);
            readerThread.Start();

            // Write to response 
            var totalBytesWritten = 0;

            var outputBuffer = new byte[BufferSize];
            var responseBufferPos = 0;

            using (new TimedLog("StreamingProxy\tTotal read and write"))
            {
                int dataReceived;
                while ((dataReceived = _pipeStream.Read(buffer, 0, BufferSize)) > 0)
                {
                    // if about to overflow, transmit the response buffer and restart
                    var bufferSpaceLeft = BufferSize - responseBufferPos;

                    if (bufferSpaceLeft < dataReceived)
                    {
                        Buffer.BlockCopy(buffer, 0, outputBuffer, responseBufferPos, bufferSpaceLeft);

                        using (new TimedLog("StreamingProxy\tWrite " + BufferSize + " to response"))
                        {
                            context.Response.OutputStream.Write(outputBuffer, 0, BufferSize);
                            responseBuffer.Write(outputBuffer, 0, BufferSize);
                            totalBytesWritten += BufferSize;
                        }

                        // Initialize response buffer and copy the bytes that were not sent
                        var bytesLeftOver = dataReceived - bufferSpaceLeft;
                        Buffer.BlockCopy(buffer, bufferSpaceLeft, outputBuffer, 0, bytesLeftOver);
                        responseBufferPos = bytesLeftOver;
                    }
                    else
                    {
                        Buffer.BlockCopy(buffer, 0, outputBuffer, responseBufferPos, dataReceived);
                        responseBufferPos += dataReceived;
                    }
                }

                // If some data left in the response buffer, send it
                if (responseBufferPos > 0)
                {
                    using (new TimedLog("StreamingProxy\tWrite " + responseBufferPos + " to response"))
                    {
                        context.Response.OutputStream.Write(outputBuffer, 0, responseBufferPos);
                        responseBuffer.Write(outputBuffer, 0, responseBufferPos);
                        totalBytesWritten += responseBufferPos;
                    }
                }
            }

            Logger.WriteEntry("StreamingProxy\tSocket read " + _pipeStream.TotalWrite + " bytes and response written " + totalBytesWritten + " bytes");

            _pipeStream.Dispose();

            return totalBytesWritten;
        }

        /// <summary>
        /// Produces the response header.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="context">The context.</param>
        /// <param name="cacheDuration">The cache duration.</param>
        /// <param name="contentLength">Length of the content.</param>
        /// <param name="contentEncoding">The content encoding.</param>
        /// <param name="contentDisposition"></param>
        private void ProduceResponseHeader(HttpWebResponse response, HttpContext context, int cacheDuration, out string contentLength, out string contentEncoding, out string contentDisposition)
        {
            // produce cache headers for response caching
            if (response.StatusCode == HttpStatusCode.OK && cacheDuration > 0)
            {
                context.Response.SetHeaderCacheDuration(cacheDuration);
            }
            else
            {
                context.Response.DoNotCache();
            }

            // If content length is not specified, this the response will be sent as Transfer-Encoding: chunked
            contentLength = response.GetResponseHeader("Content-Length");
            if (!string.IsNullOrEmpty(contentLength))
            {
                context.Response.AppendHeader("Content-Length", contentLength);
            }

            // If downloaded data is compressed, Content-Encoding will have either gzip or deflate
            contentEncoding = response.GetResponseHeader("Content-Encoding");
            if (!string.IsNullOrEmpty(contentEncoding))
            {
                context.Response.AppendHeader("Content-Encoding", contentEncoding);
            }

            // If downloaded data is compressed, Content-Encoding will have either gzip or deflate
            contentDisposition = response.GetResponseHeader("Content-Disposition");
            if (!string.IsNullOrEmpty(contentDisposition) && IncludeContentDisposition)
            {
                context.Response.AppendHeader("Content-Disposition", contentDisposition);
            }

            context.Response.ContentType = response.ContentType;

            context.Response.AppendHeader("x-Served-By", "StreamingProxy on {0}".FormatWith(Environment.MachineName));
        }


        private bool IsValidUrl(string url)
        {
            var uri = new Uri(url);
            if (!Settings.Default.EnableProxyBlocking)
            {
                return true;
            }

            return _whiteListedDomains.Any(uri.Host.ToLowerInvariant().Contains);
        }

        private bool IsValidContentType(string contentType)
        {
            var len = contentType.IndexOf(";");
            if (len > -1)
            {
                contentType = contentType.Substring(0, len).Trim().ToLowerInvariant();
            }
            return !Settings.Default.EnableProxyBlocking || _contentTypes.Any(contentType.Contains);
        }

        private void ReadData()
        {
            var buffer = new byte[BufferSize];
            var totalBytesFromSocket = 0;

            using (new TimedLog("StreamingProxy\tTotal Read from socket"))
            {
                try
                {
                    int dataReceived;
                    while ((dataReceived = _responseStream.Read(buffer, 0, BufferSize)) > 0)
                    {
                        _pipeStream.Write(buffer, 0, dataReceived);
                        totalBytesFromSocket += dataReceived;
                    }
                }
                catch (Exception x)
                {
                    Logger.WriteEntry(x.ToString());
                }
                finally
                {
                    Logger.WriteEntry("Total bytes read from socket " + totalBytesFromSocket + " bytes");
                    _responseStream.Dispose();
                    _pipeStream.Flush();
                }
            }
        }
    }
}