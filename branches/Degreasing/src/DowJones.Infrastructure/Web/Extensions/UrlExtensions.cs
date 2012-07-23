using System;
using System.Web;

namespace DowJones.Extensions.Web
{
    public static class UrlExtensions
    {
        internal static Func<string, string> ApplicationUrlThunk = VirtualPathUtility.ToAbsolute;

        public static string ToAbsoluteUrl(this string url, HttpRequestBase request = null)
        {
            if (url.StartsWith("http"))
                return url;

            request = request ?? new HttpRequestWrapper(HttpContext.Current.Request);

            string rootUrl =
                string.Format(@"{0}://{1}",
                         request.Url.Scheme,
                         request.Url.Authority
                        );

            return rootUrl + ApplicationUrlThunk(url);
        }
    }
}
