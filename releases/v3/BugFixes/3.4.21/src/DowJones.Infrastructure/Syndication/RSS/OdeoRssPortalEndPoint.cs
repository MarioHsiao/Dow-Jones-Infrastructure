using System.Web.UI;
using DowJones.Syndication.RSS;
using DowJones.Syndication.RSS.Core;
using DowJones.Url;

[assembly: WebResource(OdeoRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]

namespace DowJones.Syndication.RSS
{
    internal struct OdeoRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.odeobadge.gif";
    }

    public class OdeoRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://odeo.com/listen/subscribe?feed={feed}
        private static readonly string m_BaseUrl = "http://odeo.com/listen/subscribe";
        private readonly string m_RssFeed;
        private static string m_BadgeEmbeddedResourceUrl;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public override string RssFeed
        {
            get { return m_RssFeed; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsgatorRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public OdeoRssPortalEndPoint(string rssFeed)
        {
            m_RssFeed = rssFeed;
        }

        /// <summary>
        /// Gets the RSS integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
            UrlBuilder urlBuilder = new UrlBuilder(m_BaseUrl);
            urlBuilder.OutputType = UrlBuilder.UrlOutputType.Absolute;
            //urlBuilder.BaseUrl = m_BaseUrl;
            urlBuilder.Append("feed", m_RssFeed);
            return urlBuilder.ToString();
        }

        #region IEmbeddedBadgeResource Members

        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), OdeoRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }

        #endregion
    }
}
