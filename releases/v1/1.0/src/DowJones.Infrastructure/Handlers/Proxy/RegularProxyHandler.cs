// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegularProxyHandler.cs" company="Dow Jones">
//    © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   Regular Proxy Handler class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using DowJones.Utilities.Handlers.Proxy.Core;
using System.Linq;
using System.Collections.Specialized;
using System.Text;

namespace DowJones.Utilities.Handlers.Proxy
{
    /// <summary>
    /// Regular Proxy Handler class
    /// </summary>
    public class RegularProxyHandler : IHttpHandler
    {
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
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            var url = context.Request["url"];
            var cacheDuration = Convert.ToInt32(context.Request["cache"] ?? "0");
            var contentType = context.Request["type"];

            // We don't want to buffer because we want to save memory
            context.Response.Buffer = false;

            // Serve from cache if available
            if (cacheDuration > 0)
            {
                if (context.Cache[url] != null)
                {
                    var temp = context.Cache[url] as byte[];
                    if (temp != null)
                    {
                        context.Response.BinaryWrite(temp);
                        context.Response.Flush();
                    }

                    return;
                }
            }


            StringBuilder urlBuilder = new StringBuilder();

            urlBuilder.AppendFormat("{0}?", url);

            // copy query string params
            // skip the first one as it's the target URL itself
            for (int i = 1; i < context.Request.QueryString.Keys.Count; i++)
            {
                urlBuilder.AppendFormat("{0}={1}&", context.Request.QueryString.Keys[i], context.Request.QueryString[i]);
            }

            url = urlBuilder.ToString().TrimEnd("?&".ToCharArray());

            using (new TimedLog("RegularProxy\t" + url))
            {
                using (var client = new WebClient())
                {
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        client.Headers["Content-Type"] = contentType;
                    }

                    foreach (var key in context.Request.Headers.Keys.OfType<string>().Where(key => key != "Keep-Alive" && key != "Close" && key != "Connection"))
                    {
                        client.Headers[key] = context.Request.Headers[key];
                    }

                    client.Headers["Accept-Encoding"] = "gzip";
                    client.Headers["Accept"] = "*/*";
                    client.Headers["Accept-Language"] = "en-US";
                    client.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.6) Gecko/20070725 Firefox/2.0.0.6";

                    byte[] data = null;
                    var responseCode = HttpStatusCode.OK;
                    using (new TimedLog("RegularProxy\tDownload Sync"))
                    {
                        try
                        {
                            data = client.DownloadData(url);
                            responseCode = HttpStatusCode.OK;
                        }
                        catch (WebException wex)
                        //catch (WebException)
                        {

                            client.ResponseHeaders.Add(wex.Response.Headers);
                            data = wex.Response.GetResponseStream().ReadAllBytes();
                            responseCode = ((HttpWebResponse)wex.Response).StatusCode;
                            wex.Response.Close();
                        }
                    }

                    if (cacheDuration > 0)
                    {
                        context.Cache.Insert(
                            url,
                            data,
                            null,
                            Cache.NoAbsoluteExpiration,
                            TimeSpan.FromMinutes(cacheDuration),
                            CacheItemPriority.Normal,
                            null);
                    }

                    if (!context.Response.IsClientConnected)
                    {
                        return;
                    }

                    // Deliver content type, encoding and length as it is received from the external URL
                    context.Response.ContentType = client.ResponseHeaders["Content-Type"];
                    var contentEncoding = client.ResponseHeaders["Content-Encoding"];
                    var contentLength = client.ResponseHeaders["Content-Length"];

                    context.Response.TrySkipIisCustomErrors = true;
                    context.Response.StatusCode = (int)responseCode;

                    if (!string.IsNullOrEmpty(contentEncoding))
                    {
                        context.Response.AppendHeader("Content-Encoding", contentEncoding);
                    }

                    if (!string.IsNullOrEmpty(contentLength))
                    {
                        context.Response.AppendHeader("Content-Length", contentLength);
                    }

                    if (cacheDuration > 0)
                    {
                        context.Response.SetHeaderCacheDuration(cacheDuration);
                    }
                    else
                    {
                        context.Response.DoNotCache();
                    }

                    // Transmit the exact bytes downloaded
                    using (new TimedLog("RegularProxy\tResponse Write " + data.Length))
                    {
                        context.Response.OutputStream.Write(data, 0, data.Length);
                        context.Response.Flush();
                    }
                }
            }
        }
    }
}