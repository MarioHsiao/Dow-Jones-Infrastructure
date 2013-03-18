using System;
using System.Web;
using System.Web.UI;
using DowJones.Syndication.RSS.Core;

[assembly: WebResource(AbstractRssPortalEndPointResource.IconResourceName, MimeTypes.GIF)]


namespace DowJones.Syndication.RSS.Core
{
    public struct AbstractRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string IconResourceName = "DowJones.Utilities.Syndication.RSS.addto.gif";
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractRssPortalEndPoint : IRssPortalEndPoint, IEmbeddedIconResource
    {
        private readonly static object m_BadgeSyncRoot = new object();
        private readonly static object m_IconSyncRoot = new object();
        private static string m_IconEmbeddedResourceUrl;

        #region IRssPortalEndPoint Members

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public abstract string RssFeed { get; }

        /// <summary>
        /// Gets the RSS integration URL.
        /// </summary>
        /// <returns></returns>
        public abstract string GetIntegrationUrl();

        #endregion

        /// <summary>
        /// Gets the web resource URL.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        protected static string GetWebResourceUrl(Type type, string resourceName)
        {
            Page pageHandler = new Page();
            string tempUrl = pageHandler.ClientScript.GetWebResourceUrl(type, resourceName);
            return string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Host, tempUrl);
        }



        /// <summary>
        /// Gets the embedded badget URL.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="badgeEmbeddedResourceUrl">The badge embedded resource URL.</param>
        /// <returns></returns>
        protected static string GetEmbeddedBadgetUrl(Type type, string resourceName, ref string badgeEmbeddedResourceUrl)
        {
            if (string.IsNullOrEmpty(badgeEmbeddedResourceUrl))
            {
                lock (m_BadgeSyncRoot)
                {
                    if (string.IsNullOrEmpty(badgeEmbeddedResourceUrl))
                    {
                        badgeEmbeddedResourceUrl = GetWebResourceUrl(type, resourceName);
                    }
                }
            }
            return badgeEmbeddedResourceUrl;
        }

        /// <summary>
        /// Gets the embedded icon URL.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="iconEmbeddedResourceUrl">The icon embedded resource URL.</param>
        /// <returns></returns>
        protected virtual string GetEmbeddedIconUrl(Type type, string resourceName, ref string iconEmbeddedResourceUrl)
        {
            if (string.IsNullOrEmpty(iconEmbeddedResourceUrl))
            {
                lock (m_IconSyncRoot)
                {
                    if (string.IsNullOrEmpty(iconEmbeddedResourceUrl))
                    {
                        iconEmbeddedResourceUrl = GetWebResourceUrl(type, resourceName);
                    }
                }
            }
            return iconEmbeddedResourceUrl;
        }

        #region IEmbeddedIconResource Members

        public virtual string GetEmbeddedIconUrl()
        {
            return GetEmbeddedIconUrl(GetType(), AbstractRssPortalEndPointResource.IconResourceName, ref m_IconEmbeddedResourceUrl);
        }

        #endregion
    }
}