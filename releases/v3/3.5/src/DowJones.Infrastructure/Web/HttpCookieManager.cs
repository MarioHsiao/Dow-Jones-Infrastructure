using System;
using System.Linq;
using System.Text;
using System.Web;
using DowJones.Infrastructure;
using DowJones.Properties;

namespace DowJones.Web
{
    public class HttpCookieManager
    {
        public const string GLOBAL_PERM_COOKIE_KEY = "GPLogin";
        public const string GLOBAL_SESS_COOKIE_KEY = "GSLogin";
        public const string LOCAL_PERM_COOKIE_KEY = "persistent";
        public const string LOGIN_PERM_COOKIE_KEY = "LPLogin";

        private readonly HttpContextBase _httpContext;

        protected internal HttpCookieCollection RequestCookies
        {
            get { return _httpContext.Request.Cookies; }
        }

        protected internal HttpCookieCollection ResponseCookies
        {
            get { return _httpContext.Response.Cookies; }
        }

        public bool HasGlobalSessionCookie
        {
            get { return RequestCookieExists(GLOBAL_SESS_COOKIE_KEY); }
        }

        public bool HasGlobalPermanentCookie
        {
            get { return RequestCookieExists(GLOBAL_PERM_COOKIE_KEY); }
        }

        public string Domain
        {
            get { return _domain ?? Settings.Default.EntitlementsCookieDomain; }
            set { _domain = value; }
        }
        private string _domain;

        public string Path
        {
            get { return _path ?? Settings.Default.EntitlementsCookiePath; }
            set { _path = value; }
        }
        private string _path;


        public HttpCookieManager(HttpContextBase httpContext)
        {
            Guard.IsNotNull(httpContext, "httpContext");

            _httpContext = httpContext;
        }


        public string GetSessionValue(string key)
        {
            return GetCookieValue(GLOBAL_SESS_COOKIE_KEY, key);
        }

        public string GetPermanentValue(string key)
        {
            return GetCookieValue(GLOBAL_PERM_COOKIE_KEY, key);
        }

        public string GetLocalPermanentValue(string key)
        {
            return GetCookieValue(LOCAL_PERM_COOKIE_KEY, key);
        }

        public string GetCookieValue(string key, string subKey)
        {
            if (!RequestCookieExists(key))
            {
                return null;
            }

            var cookie = RequestCookies[key];

            if (cookie != null)
            {
                if (cookie.Values[subKey] != null) {
                    return cookie.Values[subKey];
                }

                if (cookie.Values[subKey.Replace("_", "%5F")] != null) //Read iis5.0 cookies!
                {
                    var value = cookie.Values[subKey.Replace("_", "%5F")];
                    return HttpUtility.UrlDecode(value, Encoding.Default);
                }
            }

           return null;
        }

        public bool RequestCookieExists(string key)
        {
            return RequestCookies.AllKeys.Contains(key);
        }

        public void SetSessionValue(string key, string value)
        {
            SetCookieValue(GLOBAL_SESS_COOKIE_KEY, key, value);
        }

        public void SetLocalPermanentValue(string key, string value)
        {
            SetCookieValue(LOCAL_PERM_COOKIE_KEY, key, value);
        }

        public void SetCookieValue(string key, string subKey, string value)
        {
            if (key == null || subKey == null)
                return;

            HttpCookie cookie = RequestCookies[key] ?? new HttpCookie(key);

            SetSubValue(cookie, subKey, value);

            cookie.Domain = Domain;
            cookie.Path = Path;

            if (key == GLOBAL_PERM_COOKIE_KEY || key == LOCAL_PERM_COOKIE_KEY)
            {
                var dt = DateTime.Now;
                var ts = new TimeSpan(3650, 0, 0, 0, 0);
                cookie.Expires = dt.Add(ts);
            }

            ResponseCookies.Add(cookie);
        }

        public static string DecodeAspValueString(string str)
        {
            str = str.Replace("%5F", "_");
            str = str.Replace("%2F", "/");
            str = str.Replace("%3D", "=");
            str = str.Replace("%7C", "|");
            str = str.Replace("%7E", "~");
            str = str.Replace("%2D", "-");
            return str;
        }

        public static void SetSubValue(HttpCookie cookie, string key, string value)
        {
            if (cookie.HasKeys && key.IndexOf("_", StringComparison.Ordinal) != -1 && cookie.Values[key.Replace("_", "%5F")] != null)
            {
                key = key.Replace("_", "%5F");
            }
            cookie.Values[key] = value;
        }

        public bool IsCrossDomainCookies()
        {
            return (GetCookieValue(LOGIN_PERM_COOKIE_KEY, "cdp") == "y");
        }

        public static void DeleteGlobalCookies(string key, string subKey)
        {
            var request = HttpContext.Current.Request;
            if (key == null || subKey == null)
                return;
            var cookie = request.Cookies[key];
            if (cookie == null)
            {
                return;
            }
            if (cookie.HasKeys && subKey.IndexOf("_", StringComparison.Ordinal) != -1 && cookie.Values[subKey.Replace("_", "%5F")] != null)
            {
                subKey = subKey.Replace("_", "%5F");
            }
            cookie.Values.Remove(subKey);
            cookie.Path = "/";
            if (key == GLOBAL_PERM_COOKIE_KEY)
            {
                var dt = DateTime.Now;
                var ts = new TimeSpan(3650, 0, 0, 0, 0);
                cookie.Expires = dt.Add(ts);
            }
            cookie.Domain = GetDefaultCookieDomain();
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void DeleteGlobalCookies(string key)
        {
            if (key == null)
                return;
            var request = HttpContext.Current.Request;
            var cookie = request.Cookies[key];
            if (cookie == null)
                return;
            cookie.Expires = DateTime.Now.AddYears(-2);
            cookie.Path = "/";
            cookie.Domain = GetDefaultCookieDomain();
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private static string GetDefaultCookieDomain()
        {
            var hostname = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            var parts = hostname.Split('.');
            var length = parts.Length;
            if (length > 2)
            {
                return "." + parts[length - 2] + "." + parts[length - 1];
            }
            return ".factiva.com";
        }
    }
}
