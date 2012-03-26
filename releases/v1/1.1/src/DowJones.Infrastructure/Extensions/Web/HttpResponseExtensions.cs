// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpResponseExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for the HttpResponse class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Web;
using DowJones.Utilities.Uri;
using DowJones.Utilities.Url;

/// <summary>
/// Extension methods for the HttpResponse class
/// </summary>
public static class HttpResponseExtensions
{
    /// <summary>
    /// Reloads the current page / handler by performing a redirect to the current url
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    public static void Reload(this HttpResponse response)
    {
        response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
    }

    /// <summary>
    /// Performs a response redirect and allows the url to be populated with string format parameters.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    /// <param name="urlFormat">The URL including string.Format placeholders.</param>
    /// <param name="values">The values to the populated.</param>
    public static void Redirect(this HttpResponse response, string urlFormat, params object[] values)
    {
        response.Redirect(urlFormat, true, values);
    }

    /// <summary>
    /// Performs a response redirect and allows the url to be populated with string format parameters.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    /// <param name="urlFormat">The URL including string.Format placeholders.</param>
    /// <param name="endResponse">If set to <c>true</c> the response will be terminated.</param>
    /// <param name="values">The values to the populated.</param>
    public static void Redirect(this HttpResponse response, string urlFormat, bool endResponse, params object[] values)
    {
        var url = string.Format(urlFormat, values);
        response.Redirect(url, endResponse);
    }

    /// <summary>
    /// Performs a response redirect and allows the url to be populated with a query string.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    /// <param name="url">The target URL.</param>
    /// <param name="queryString">The query string.</param>
    public static void Redirect(this HttpResponse response, string url, UrlBuilder queryString)
    {
        response.Redirect(url, queryString, true);
    }

    /// <summary>
    /// Performs a response redirect and allows the url to be populated with a query string.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    /// <param name="url">The target URL.</param>
    /// <param name="queryString">The query string.</param>
    /// <param name="endResponse">If set to <c>true</c> the response will be terminated.</param>
    public static void Redirect(this HttpResponse response, string url, QueryStringDictionary queryString, bool endResponse)
    {
        var urlBuilder = new UrlBuilder(url)
                             {
                                 QueryString = queryString
                             };
        response.Redirect(urlBuilder.ToString(), endResponse);
    }

    /// <summary>
    /// Returns a 404 to the client and ends the response.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    public static void SetFileNotFound(this HttpResponse response)
    {
        response.SetFileNotFound(true);
    }

    /// <summary>
    /// Returns a 404 to the client and optionally ends the response.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    /// <param name="endResponse">If set to <c>true</c> the response will be terminated.</param>
    public static void SetFileNotFound(this HttpResponse response, bool endResponse)
    {
        response.SetStatus(404, "Not Found", endResponse);
    }

    /// <summary>
    /// Returns a 500 to the client and ends the response.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    public static void SetInternalServerError(this HttpResponse response)
    {
        response.SetInternalServerError(true);
    }

    /// <summary>
    /// Returns a 500 to the client and optionally ends the response.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    /// <param name="endResponse">If set to <c>true</c> the response will be terminated.</param>
    public static void SetInternalServerError(this HttpResponse response, bool endResponse)
    {
        response.SetStatus(500, "Internal Server Error", endResponse);
    }

    /// <summary>
    /// Set the specified HTTP status code and description and optionally ends the response.
    /// </summary>
    /// <param name="response">The HttpResponse to perform on.</param>
    /// <param name="code">The status code.</param>
    /// <param name="description">The status description.</param>
    /// <param name="endResponse">If set to <c>true</c> the response will be terminated.</param>
    public static void SetStatus(this HttpResponse response, int code, string description, bool endResponse)
    {
        response.StatusCode = code;
        response.StatusDescription = description;

        if (endResponse)
        {
            response.End();
        }
    }

    /// <summary>
    /// Sets the header for client side browser cache duration.
    /// </summary>
    /// <param name="minutes">The minutes.</param>
    public static void SetHeaderCacheDuration(this HttpResponse response, int minutes)
    {
        TimeSpan duration = TimeSpan.FromMinutes(minutes);

        response.Cache.SetCacheability(HttpCacheability.Public);
        response.Cache.SetExpires(DateTime.Now.Add(duration));
        response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        response.Cache.SetMaxAge(duration);
    }

    /// <summary>
    /// Does the not cache response.
    /// </summary>
    public static void DoNotCache(this HttpResponse response)
    {
        response.Cache.SetNoServerCaching();
        response.Cache.SetNoStore();
        response.Cache.SetMaxAge(TimeSpan.Zero);
        response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        response.Cache.SetExpires(DateTime.Now.AddYears(-1));
    }


    public static void WriteStream(this HttpResponse response, Stream contentStream)
    {
        WriteStream(new HttpResponseWrapper(response), contentStream);
    }

    public static void WriteStream(this HttpResponseBase response, Stream contentStream)
    {
        using (var reader = new BinaryReader(contentStream))
        {
            int bytesRead = 0;
            byte[] buffer = new byte[1 << 16]; // 64kb
            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) != 0)
            {
                response.OutputStream.Write(buffer, 0, bytesRead);
            }
        }
    }

}
