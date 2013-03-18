using System;
using System.Web;
using DowJones.Session;
using DowJones.Web;

namespace DowJones.Assemblers.Session
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("Use UserSessionFactory instead (class renamed)")]
    public class CookieBasedUserSessionFactory : UserSessionFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CookieBasedUserSessionFactory" /> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="cookieManager">The cookie manager.</param>
        /// <param name="referringProduct">The referring product.</param>
        public CookieBasedUserSessionFactory(HttpContextBase httpContext, HttpCookieManager cookieManager, ReferringProduct referringProduct) 
            : base(httpContext, cookieManager, referringProduct)
        {
        }
    }
}