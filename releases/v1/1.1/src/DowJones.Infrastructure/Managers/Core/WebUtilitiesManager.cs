using System.Net;
using System.Web;

namespace DowJones.Utilities.Managers.Core
{
    public class WebUtilitiesManager
    {
        /// <summary>
        /// Makes the relative URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string MakeRelativeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            if (url[0] != '~')
                return url;
            var applicationPath = HttpContext.Current.Request.ApplicationPath;
            if (url.Length == 1)
                return applicationPath;
            var indexOfUrl = 1;
            var midPath = (applicationPath.Length > 1) ? "/" : string.Empty;
            if (url[1] == '/' || url[1] == '\\')
                indexOfUrl = 2;
            return string.Concat(applicationPath, midPath, url.Substring(indexOfUrl));
        }

        /// <summary>
        /// Makes the absolute URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string MakeAbsoluteUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            if (url[0] != '~')
                return url;
            var applicationPath = HttpContext.Current.Request.ApplicationPath;
            if (url.Length == 1)
                return applicationPath;
            var indexOfUrl = 1;
            if (url[1] == '/' || url[1] == '\\')
                indexOfUrl = 2;
            return string.Concat(GetApplicationUrl(), "/", url.Substring(indexOfUrl));
        }

        /// <summary>
        /// Gets the application URL.
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationUrl()
        {
            var context = HttpContext.Current;
            var applicationUrl = string.Concat(context.Request.Url.Scheme, "://", context.Request.Url.Host);
            var applicationPath = context.Request.ApplicationPath;

            if (applicationPath.Length > 1)
            {
                applicationUrl = string.Concat(applicationUrl, applicationPath);
            }

            return applicationUrl;
        }


        /// <summary>
        /// Determines whether [is local user].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is local user]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLocalUser()
        {
            if (HttpContext.Current == null)
            {
                return false;
            }
            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            var ipAddress = hostEntry.AddressList[0].ToString();
            return ipAddress.Equals(HttpContext.Current.Request.ServerVariables["remote_addr"]);
        }

        public static bool IsHttps()
        {
            return HttpContext.Current.Request["HTTPS"] != "off";
        }

        public static string GetLocalPath()
        {
            var request = HttpContext.Current.Request;
            var localPath = request.Url.LocalPath.ToLower();
            var appLength = request.ApplicationPath.Length;
            if (appLength > 1)
            {
                localPath = request.Url.LocalPath.Replace(request.ApplicationPath, "").ToLower();
            }
            return localPath;
        }
    }
}
