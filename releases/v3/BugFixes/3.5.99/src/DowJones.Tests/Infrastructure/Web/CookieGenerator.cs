using System;
using System.Web;
using DowJones.Properties;

namespace DowJones.Web
{
    public class CookieGenerator
    {
        public string ProductPrefix
        {
            get { return _productPrefix ?? Settings.Default.DefaultProductPrefix; }
            set { _productPrefix = value; }
        }
        private string _productPrefix;

        public HttpCookie GeneratePermanentCookie(string username = null, string sessionId = null)
        {
            var cookie = new HttpCookie(HttpCookieManager.GLOBAL_PERM_COOKIE_KEY)
                             {
                                 Domain = ".factiva.com",
                                 Path = "/",
                                 Secure = false,
                                 Expires = DateTime.Now.AddYears(3)
                             };

            if (username != null)
                cookie.Values[string.Format("{0}%5FU", ProductPrefix)] = username;

            if (sessionId != null)
                cookie.Values[string.Format("{0}%5FS", ProductPrefix)] = sessionId;

            return cookie;
        }

        public HttpCookie GenerateSessionCookie(string username = null, string sessionId = null)
        {
            var cookie = new HttpCookie(HttpCookieManager.GLOBAL_SESS_COOKIE_KEY)
            {
                Domain = ".factiva.com",
                Path = "/",
                Secure = false,
            };

            if (sessionId != null)
                HttpCookieManager.SetSubValue(cookie, ProductPrefix + "_S", sessionId);

            if (username != null)
                HttpCookieManager.SetSubValue(cookie, ProductPrefix + "_U", username);

            return cookie;
        }

        public void SetValue(HttpCookie cookie, string key, string value)
        {
            HttpCookieManager.SetSubValue(cookie, ProductPrefix + key, value);
        }
    }
}
