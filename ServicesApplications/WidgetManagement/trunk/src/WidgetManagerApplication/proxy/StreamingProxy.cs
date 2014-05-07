using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Caching;
using EMG.widgets.ui.proxy.core;

namespace EMG.widgets.ui.proxy
{
    /// <summary>
    /// 
    /// </summary>
    public class SteamingProxy : IHttpAsyncHandler
    {
        private const int BufferSize = 8*1024;

        private PipeStream _pipeStream;
        private Stream _responseStream;

        #region IHttpAsyncHandler Members

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            string url = context.Request["url"];
            int cacheDuration = Convert.ToInt32(context.Request["cache"] ?? "0");
            string contentType = context.Request["type"];

            Logger.WriteEntry("--- " + url + " ----");

            if (cacheDuration > 0)
            {
                if (context.Cache[url] != null)
                {
                    var content = context.Cache[url] as CachedContent;
                    if (content!=null)
                    {
                       if (!string.IsNullOrEmpty(content.ContentEncoding))
                        context.Response.AppendHeader("Content-Encoding", content.ContentEncoding);
                    if (!string.IsNullOrEmpty(content.ContentLength))
                        context.Response.AppendHeader("Content-Length", content.ContentLength);

                    context.Response.ContentType = content.ContentType;

                    content.Content.Position = 0;
                    content.Content.WriteTo(context.Response.OutputStream); 
                    }
                    
                }
            }

            using (new TimedLog("StreamingProxy\t" + url))
            {
                HttpWebRequest request = HttpHelper.CreateScalableHttpWebRequest(url);
                // As we will stream the response, don't want to automatically decompress the content
                // when source sends compressed content
                request.AutomaticDecompression = DecompressionMethods.None;

                if (!string.IsNullOrEmpty(contentType))
                    request.ContentType = contentType;

                using (new TimedLog("StreamingProxy\tTotal GetResponse and transmit data"))
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    DownloadData(request, response, context, cacheDuration);
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

        /// <summary>
        /// Initiates an asynchronous call to the HTTP handler.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="cb">The <see cref="T:System.AsyncCallback"/> to call when the asynchronous method call is complete. If <paramref name="cb"/> is null, the delegate is not called.</param>
        /// <param name="extraData">Any extra data needed to process the request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"/> that contains information about the status of the process.
        /// </returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            string url = context.Request["url"];
            int cacheDuration = Convert.ToInt32(context.Request["cache"] ?? "0");
            string contentType = context.Request["type"];

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

            HttpWebRequest request = HttpHelper.CreateScalableHttpWebRequest(url);
            // As we will stream the response, don't want to automatically decompress the content
            // when source sends compressed content
            request.AutomaticDecompression = DecompressionMethods.None;

            if (!string.IsNullOrEmpty(contentType))
                request.ContentType = contentType;

            var state = new AsyncState
                        {
                            Context = context, 
                            Url = url, 
                            CacheDuration = cacheDuration, 
                            Request = request
                        };

            return request.BeginGetResponse(cb, state);
        }

        /// <summary>
        /// Provides an asynchronous process End method when the process ends.
        /// </summary>
        /// <param name="result">An <see cref="T:System.IAsyncResult"/> that contains information about the status of the process.</param>
        public void EndProcessRequest(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
            {
                // Content is already available in the cache and can be delivered from cache
                var syncResult = result as SyncResult;
                if (syncResult == null)
                {
                    return;
                }
                syncResult.Context.Response.ContentType = syncResult.Content.ContentType;
                syncResult.Context.Response.AppendHeader("Content-Encoding", syncResult.Content.ContentEncoding);
                syncResult.Context.Response.AppendHeader("Content-Length", syncResult.Content.ContentLength);

                syncResult.Content.Content.Seek(0, SeekOrigin.Begin);
                syncResult.Content.Content.WriteTo(syncResult.Context.Response.OutputStream);
            }
            else
            {
                // Content is not available in cache and needs to be downloaded from external source
                var state = result.AsyncState as AsyncState;
                if (state == null)
                {
                    return;
                }
                state.Context.Response.Buffer = false;
                var request = state.Request;

                using (var response = request.EndGetResponse(result) as HttpWebResponse)
                {
                    DownloadData(request, response, state.Context, state.CacheDuration);
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
        private void DownloadData(HttpWebRequest request, HttpWebResponse response, HttpContext context, int cacheDuration)
        {
            var responseBuffer = new MemoryStream();
            context.Response.Buffer = false;

            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    context.Response.StatusCode = (int) response.StatusCode;
                    return;
                }
                using (Stream readStream = response.GetResponseStream())
                {
                    if (context.Response.IsClientConnected)
                    {
                        string contentLength;
                        string contentEncoding;
                        ProduceResponseHeader(response, context, cacheDuration, out contentLength, out contentEncoding);

                        //int totalBytesWritten = TransmitDataInChunks(context, readStream, responseBuffer);
                        //int totalBytesWritten = TransmitDataAsync(context, readStream, responseBuffer);
                        int totalBytesWritten = TransmitDataAsyncOptimized(context, readStream, responseBuffer);

                        Logger.WriteEntry("Response generated: " + DateTime.Now);
                        Logger.WriteEntry(string.Format("Content Length vs Bytes Written: {0} vs {1} ", contentLength, totalBytesWritten));

                        if (cacheDuration > 0)
                        {
                            #region Cache Response in memory

                            // Cache the content on server for specific duration
                            var cache = new CachedContent
                                        {
                                            Content = responseBuffer, 
                                            ContentEncoding = contentEncoding, 
                                            ContentLength = contentLength, 
                                            ContentType = response.ContentType
                                        };

                            context.Cache.Insert(request.RequestUri.ToString(), cache, null,
                                                 Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromMinutes(cacheDuration),
                                                 CacheItemPriority.Normal, null);

                            #endregion
                        }
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

        /*
        private int TransmitDataInChunks(HttpContext context, Stream readStream, MemoryStream responseBuffer)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            int bytesRead;
            int totalBytesWritten = 0;

            using (new TimedLog("StreamingProxy\tTotal read from socket and write to response"))
                while ((bytesRead = readStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                {
                    using (new TimedLog("StreamingProxy\tWrite " + bytesRead + " to response"))
                        context.Response.OutputStream.Write(buffer, 0, bytesRead);

                    responseBuffer.Write(buffer, 0, bytesRead);

                    totalBytesWritten += bytesRead;
                }

            return totalBytesWritten;
        }

        private int TransmitDataAsync(HttpContext context, Stream readStream, MemoryStream responseBuffer)
        {
            _ResponseStream = readStream;

            _PipeStream = new PipeStreamBlock(5000);

            byte[] buffer = new byte[BUFFER_SIZE];

            Thread readerThread = new Thread(ReadData);
            readerThread.Start();
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReadData));

            int totalBytesWritten = 0;
            int dataReceived;

            using (new TimedLog("StreamingProxy\tTotal read and write"))
            {
                while ((dataReceived = _PipeStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                {
                    using (new TimedLog("StreamingProxy\tWrite " + dataReceived + " to response"))
                    {
                        context.Response.OutputStream.Write(buffer, 0, dataReceived);
                        responseBuffer.Write(buffer, 0, dataReceived);
                        totalBytesWritten += dataReceived;
                    }
                }
            }

            _PipeStream.Dispose();

            return totalBytesWritten;
        }
        */

        /// <summary>
        /// Transmits the data async optimized.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="readStream">The read stream.</param>
        /// <param name="responseBuffer">The response buffer.</param>
        /// <returns></returns>
        private int TransmitDataAsyncOptimized(HttpContext context, Stream readStream, MemoryStream responseBuffer)
        {
            _responseStream = readStream;

            _pipeStream = new PipeStreamBlock(10000);
            //_PipeStream = new Utility.PipeStream(10000);

            var buffer = new byte[BufferSize];

            // Asynchronously read content form response stream
            var readerThread = new Thread(ReadData);
            readerThread.Start();
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReadData));

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
                    int bufferSpaceLeft = BufferSize - responseBufferPos;

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
                        int bytesLeftOver = dataReceived - bufferSpaceLeft;
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
        private static void ProduceResponseHeader(HttpWebResponse response, HttpContext context, int cacheDuration, out string contentLength, out string contentEncoding)
        {
            // produce cache headers for response caching
            if (cacheDuration > 0)
                HttpHelper.CacheResponse(context, cacheDuration);
            else
                HttpHelper.DoNotCacheResponse(context);

            // If content length is not specified, this the response will be sent as Transfer-Encoding: chunked
            contentLength = response.GetResponseHeader("Content-Length");
            if (!string.IsNullOrEmpty(contentLength))
                context.Response.AppendHeader("Content-Length", contentLength);

            // If downloaded data is compressed, Content-Encoding will have either gzip or deflate
            contentEncoding = response.GetResponseHeader("Content-Encoding");
            if (!string.IsNullOrEmpty(contentEncoding))
                context.Response.AppendHeader("Content-Encoding", contentEncoding);

            context.Response.ContentType = response.ContentType;
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