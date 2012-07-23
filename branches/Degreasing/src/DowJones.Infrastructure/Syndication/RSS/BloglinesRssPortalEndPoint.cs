using System.Web.UI;
using DowJones.Syndication.RSS;
using DowJones.Syndication.RSS.Core;

[assembly: WebResource(BloglinesRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]
[assembly: WebResource(BloglinesRssPortalEndPointResource.IconResourceName, MimeTypes.GIF)]

namespace DowJones.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    internal struct BloglinesRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.bloglinesbadge.gif";
       
        /// <summary>
        /// 
        /// </summary>
        public const string IconResourceName = "DowJones.Utilities.Syndication.RSS.bloglinesicon.gif";
    }

    /// <summary>
    /// 
    /// </summary>
    public class BloglinesRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://www.bloglines.com/sub/{feed}
        // Do not encode and return the feed as is
        private static readonly string m_BaseUrl = "http://www.bloglines.com/sub/";
        private readonly string m_RssFeed;
        private static string m_BadgeEmbeddedResourceUrl;
        private static string m_IconEmbeddedResourceUrl;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public override string RssFeed
        {
            get { return m_RssFeed; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BloglinesRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public BloglinesRssPortalEndPoint(string rssFeed)
        {
            m_RssFeed = rssFeed;
        }

        /// <summary>
        /// Gets the RSS integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
            return string.Concat(m_BaseUrl, m_RssFeed);
        }

        #region IEmbeddedBadgeResource Members
        /// <summary>
        /// Gets the embedded badget URL.
        /// </summary>
        /// <returns></returns>
        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), BloglinesRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }

        public override string GetEmbeddedIconUrl()
        {
            return GetEmbeddedIconUrl(GetType(), BloglinesRssPortalEndPointResource.IconResourceName, ref m_IconEmbeddedResourceUrl);
        }
        #endregion
    }
}