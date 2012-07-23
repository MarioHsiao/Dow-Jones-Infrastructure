using System.Web;
using System.Web.UI;
using DowJones.Syndication.RSS;
using DowJones.Syndication.RSS.Core;
using DowJones.Url;

[assembly: WebResource(ZuneRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]

namespace DowJones.Syndication.RSS
{
    internal struct ZuneRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.zuneBadge.gif";
    }

    public class ZuneRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // zune://subscribe/?{title}={feed} note: title is httpencoded encoded
        private readonly string m_BaseUrl = "zune://subscribe/";
        private readonly string m_RssFeed;
        private readonly string m_TitleToFeed;
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
        /// <param name="titleToFeed">The title to feed.</param>
        public ZuneRssPortalEndPoint(string rssFeed, string titleToFeed)
        {
            m_RssFeed = rssFeed;
            m_TitleToFeed = titleToFeed;
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
            urlBuilder.Append(HttpUtility.UrlEncode(m_TitleToFeed), m_RssFeed);
            return urlBuilder.ToString();
        }

        #region IEmbeddedBadgeResource Members

        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), ZuneRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }

        #endregion
    }
}
