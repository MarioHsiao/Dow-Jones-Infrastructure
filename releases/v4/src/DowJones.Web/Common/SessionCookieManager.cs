using System;
using System.Text;
using System.Web;

namespace DowJones.Session
{
    
    /// <summary>
    /// Summary description for HttpCookieUtility.
    /// Used to manage cookies for Login server specifically, as password saving was adding Q2 2007 bucket
    /// </summary>
    public class SessionCookieManager
    {


        /// <summary>
        /// Checks to see if the login server's Permanent Cookie (GPLogin) has an encrypted cookie saved.
        /// First checks the FP_{} cookie and then IF_{} and returns true...
        /// </summary>
        /// <returns>True if saved cookie, else false</returns>
        public static bool IsPersistentPasswordCookieSaved()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.Cookies[SessionData.GLOBAL_PERM_COOKIE_KEY] == null)
                return false;

            var retVal = false;
            var data = SessionData.Instance();
            try
            {
                if (HttpContext.Current.Request.Cookies[SessionData.GLOBAL_PERM_COOKIE_KEY] == null)
                {
                    var cookie = HttpContext.Current.Request.Cookies[SessionData.GLOBAL_PERM_COOKIE_KEY];
                    if (cookie != null)
                    {
                        retVal = ((data.UserId == cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FU"]
                                   && data.ProductId == cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FN"]
                                   && data.AccountId == cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FA"]
                                   && !string.IsNullOrEmpty(cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FX"])
                                  )
                                  ||
                                  (data.UserId == cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FU"]
                                   && data.ProductId == cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FN"]
                                   && data.AccountId == cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FA"]
                                   && !string.IsNullOrEmpty(cookie.Values[SessionData.DEFAULT_FACTIVA_PREFIX + "%5FX"])
                                  )
                                 );
                        if (!retVal)
                            retVal = ((data.UserId == cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FU"]
                                       && data.ProductId == cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FN"]
                                       && data.AccountId == cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FA"]
                                       && !string.IsNullOrEmpty(cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FX"])
                                      )
                                      ||
                                      (data.UserId == cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FU"]
                                       && data.ProductId == cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FN"]
                                       && data.AccountId == cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FA"]
                                       && !string.IsNullOrEmpty(cookie.Values[SessionData.DEFAULT_PRODUCT_PREFIX + "%5FX"])
                                      )
                                     );
                    }
                }
                

            }
            catch
            {
                return false;
            }
            return retVal;
        }

/*
        /// <summary>
        /// Writes the Persisten Saved Password cookie so that the user can relogin seamlessly.
        /// </summary>
        /// <param name="token">EncryptedId Token (aka MODAUTOLOG) from BLL's ProfileManager</param>
        public static HttpCookie CreatePersistentPasswordCookie(string token)
        {
            HttpCookie loginServerPeristentCookie = HttpContext.Current.Request.Cookies[SessionData.GLOBAL_PERM_COOKIE_KEY];
            if (loginServerPeristentCookie != null)
            {
                loginServerPeristentCookie.Values.Add(RetrieveLoginServerPrdPrefixForPersistentCookie() + "%5FX", token);
            }
            return loginServerPeristentCookie;
        }
*/
        /// <summary>
        /// Checks if the current sessions's userid/namespace/accountid match the FP's userid/namespace/accountid.
        /// If there's a match returns FP as the prefix else return SessionData.DEFAULT_PRODUCT_PREFIX as Prefix.
        /// This is inline with with Login server does on its end. 
        /// </summary>
        /// <returns>Returns the Prefix for saving the user's persistent cookie.</returns>
        public static string RetrieveLoginServerPrdPrefixForPersistentCookie()
        {
            var retPrefix = SessionData.DEFAULT_PRODUCT_PREFIX;
            var data = SessionData.Instance();

            var cookie = HttpContext.Current.Request.Cookies[SessionData.GLOBAL_PERM_COOKIE_KEY];
            if (cookie != null)
            {
                var retVal = (data.UserId == cookie.Values["FP%5FU"]
                               && data.ProductId == cookie.Values["FP%5FN"]
                               && data.AccountId == cookie.Values["FP%5FA"]
                              );
                if (retVal)
                    retPrefix = SessionData.DEFAULT_FACTIVA_PREFIX;
            }
            return retPrefix;
        }

        
        
        /// <summary>
        /// WriteGlobalCookies - from lgin server code..base
        /// Creates the global cookie GPLOgin sub cookie and adds it.
        /// </summary>
        /// <param name="key">Main cookie Key</param>
        /// <param name="subKey">Sub cookie key</param>
        /// <param name="value">Value which goes into it.</param>
        public static void WritePersistentFactivaCookie(string key, string subKey, string value)
        {
            var request = HttpContext.Current.Request;

            if (key == null || subKey == null)

                return;

            var cookie = request.Cookies[key] ?? new HttpCookie(key);

            if (cookie.HasKeys && subKey.IndexOf("_") != -1 && cookie.Values[subKey.Replace("_", "%5F")] != null)
            {
                subKey = subKey.Replace("_","%5F");
            }

            cookie.Values[subKey] = value;
            cookie.Path = "/";

            if (key == SessionData.GLOBAL_PERM_COOKIE_KEY)
            {
                var dt = DateTime.Now;
                var ts = new TimeSpan(3650, 0, 0, 0, 0);
                cookie.Expires = dt.Add(ts);
            }

            cookie.Domain = ".factiva.com";

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Deletes the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void DeleteCookie(string key)
        {

            var request = HttpContext.Current.Request;

            if (key == null)
            {
                return;
            }

            if (request.Cookies[key] == null)
            {
                return;
            }

            var cookie = new HttpCookie(key)
                             {
                                 Path = "/", 
                                 Domain = ".factiva.com", 
                                 Expires = DateTime.Now.AddYears(-2)
                             };


            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Deletes the cookies.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="subKey">The sub key.</param>
        /// <param name="domain">The domain.</param>
        public static void DeletePersistentCookies(string key, string subKey, string domain)
        {
            var request = HttpContext.Current.Request;

            if (key == null || subKey == null)
            {
                return;
            }

            var cookie = request.Cookies[key];

            if (cookie == null)
            {
                return;
            }

            if (cookie.HasKeys && subKey.IndexOf("_") != -1 && cookie.Values[subKey.Replace("_", "%5F")] != null)
            {
                subKey = subKey.Replace("_", "%5F");
            }
            cookie.Values.Remove(subKey);
            cookie.Path = "/";

            if (key == SessionData.GLOBAL_PERM_COOKIE_KEY)
            {
                var dt = DateTime.Now;
                var ts = new TimeSpan(3650, 0, 0, 0, 0);
                cookie.Expires = dt.Add(ts);
            }

            cookie.Domain = domain;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Gets the cookie Value for the cookie and sub cookie key that was passed in
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subKey"></param>
        /// <returns></returns>
        public static string GetCookieValue(string key, string subKey)
        {
            if (HttpContext.Current == null) return string.Empty; 
            var cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                if (cookie.Values[subKey] != null)
                {
                    return cookie.Values[subKey];
                }

                if (cookie.Values[subKey.Replace("_","%5F")] != null) //Read iis5.0 cookies!
                {
                    var value = cookie.Values[subKey.Replace("_","%5F")];
                    return HttpUtility.UrlDecode(value,Encoding.Default);
                }
            }
            return null;
        }
    }
}