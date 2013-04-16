using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Net;
using System.Threading;
using System.IO;
using DowJones.Properties;
using log4net;

namespace DowJones.Web.Handlers.Proxy.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpHelper
    {
        public const string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.6) Gecko/20070725 Firefox/2.0.0.6";

        public static int HttpWebRequestTimeoutInSeconds
        {
            get { return Settings.Default.HttpWebRequestTimeoutInSeconds; }
        }

        /// <summary>
        /// Creates the scalable HTTP web request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="origRequest"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateScalableHttpWebRequest(string url, HttpRequest origRequest)
        {
            var urlBuilder = new StringBuilder();

            urlBuilder.AppendFormat("{0}{1}", url, url.EndsWith("?") ? "" : "?");

            // copy query string params
            // skip the first one as it's the target URL itself
            for (var i = 1; i < origRequest.QueryString.Keys.Count; i++)
            {
                urlBuilder.AppendFormat("{0}={1}&", origRequest.QueryString.Keys[i], origRequest.QueryString[i]);
            }

            var timeoutMilliseconds = (int)TimeSpan.FromSeconds(HttpWebRequestTimeoutInSeconds).TotalMilliseconds;
            var request = WebRequest.Create(urlBuilder.ToString().TrimEnd("?&".ToCharArray())) as HttpWebRequest;
            if (request != null)
            {
                request.Headers["Accept-Language"] = "en-US";
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.MaximumAutomaticRedirections = 2;
                request.ReadWriteTimeout = timeoutMilliseconds * 2;
                request.Timeout = timeoutMilliseconds;
                request.Accept = "*/*";
                request.Method = origRequest.HttpMethod;
                request.UserAgent = UserAgent;
                request.ContentType = origRequest.ContentType;
                request.Headers[HttpRequestHeader.AcceptCharset] = string.IsNullOrWhiteSpace(origRequest.Headers["Accept-Charset"]) ? "utf-8" : origRequest.Headers["Accept-Charset"];

                if (!string.IsNullOrWhiteSpace(origRequest.Headers["sessionid"]))
                    request.Headers["sessionid"] = origRequest.Headers["sessionid"];

                if (!string.IsNullOrWhiteSpace(origRequest.Headers["encryptedtoken"]))
                    request.Headers["encryptedtoken"] = origRequest.Headers["encryptedtoken"];
                
                return request;
            }

            return null;
        }

        /// <summary>
        /// Caches the response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="durationInMinutes">The duration in minutes.</param>
        public static void CacheResponse(HttpContext context, int durationInMinutes)
        {
            TimeSpan duration = TimeSpan.FromMinutes(durationInMinutes);

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.Add(duration));
            context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            context.Response.Cache.SetMaxAge(duration);
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class CachedContent
    {
        /// <summary>
        /// 
        /// </summary>
        public string ContentType;
        /// <summary>
        /// 
        /// </summary>
        public string ContentEncoding;
        /// <summary>
        /// 
        /// </summary>
        public string ContentLength;

        /// <summary>
        /// 
        /// </summary>
        public string ContentDisposition;
        /// <summary>
        /// 
        /// </summary>
        public MemoryStream Content;
    }

    /// <summary>
    /// 
    /// </summary>
    public class AsyncState
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpContext Context;
        /// <summary>
        /// 
        /// </summary>
        public string Url;
        /// <summary>
        /// 
        /// </summary>
        public int CacheDuration;
        /// <summary>
        /// 
        /// </summary>
        public HttpWebRequest Request;
    }

    /// <summary>
    /// 
    /// </summary>
    public class SyncResult : IAsyncResult
    {
        /// <summary>
        /// 
        /// </summary>
        public CachedContent Content;
        /// <summary>
        /// 
        /// </summary>
        public HttpContext Context;

        #region IAsyncResult Members

        object IAsyncResult.AsyncState
        {
            get { return new object(); }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get { return new ManualResetEvent(true); }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get { return true; }
        }

        bool IAsyncResult.IsCompleted
        {
            get { return true; }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Logger));
        /// <summary>
        /// Writes the entry.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void WriteEntry(string msg)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(msg);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimedLog : IDisposable
    {
        private readonly string _message;
        private readonly Stopwatch _startupWatch;

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedLog"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public TimedLog(string msg)
        {
            _message = msg;
            _startupWatch = Stopwatch.StartNew();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Logger.WriteEntry(_startupWatch.Elapsed+ "\t" + _message + "\n");

                    if (_startupWatch.IsRunning)
                    {
                        _startupWatch.Stop();
                    }
                }
            }

            // Code to dispose unmanaged resources held by the class
            _isDisposed = true;
        }

         /// <summary>
        /// Finalizes an instance of the <see cref="TimedLog"/> class. 
        /// </summary>
        ~TimedLog()
        {
            Dispose(false);
        }

        #endregion
    }
}
