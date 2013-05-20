using System;
using System.Web;
using System.Net;
using System.Threading;
using System.IO;
using log4net;

namespace EMG.widgets.ui.proxy.core
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpHelper
    {

        /// <summary>
        /// Creates the scalable HTTP web request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static HttpWebRequest CreateScalableHttpWebRequest(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.MaximumAutomaticRedirections = 2;
                request.ReadWriteTimeout = 5000;
                request.Timeout = 3000;
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "en-US");
                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.6) Gecko/20070725 Firefox/2.0.0.6";
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
        /// <summary>
        /// Does the not cache response.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void DoNotCacheResponse(HttpContext context)
        {
            context.Response.Cache.SetNoServerCaching();
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetMaxAge(TimeSpan.Zero);
            context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            context.Response.Cache.SetExpires(DateTime.Now.AddYears(-1));
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
        private static readonly ILog m_log = LogManager.GetLogger(typeof(Logger));
        /// <summary>
        /// Writes the entry.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void WriteEntry(string msg)
        {
            if (m_log.IsDebugEnabled)
            {
                m_log.Debug(msg);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimedLog : IDisposable
    {
        private readonly string m_Message;
        private readonly DateTime m_Start;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedLog"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public TimedLog(string msg)
        {
            m_Message = msg;
            m_Start = DateTime.Now;
        }
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DateTime end = DateTime.Now;
            TimeSpan duration = end  - m_Start;
            Logger.WriteEntry(duration.TotalMilliseconds + "\t" + m_Message + "\n");
        }

        #endregion
    }
}
