using System.Web;

namespace DowJones.Extensions.Web
{
    public static class UrlExtensions
    {
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

            return rootUrl + VirtualPathUtility.ToAbsolute(url);
        }
    }
}
