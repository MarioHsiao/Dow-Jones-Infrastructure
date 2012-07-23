using System.Web.UI;
using DowJones.Syndication.RSS;
using DowJones.Syndication.RSS.Core;
using DowJones.Url;

[assembly: WebResource(LiveDotComRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]
[assembly: WebResource(LiveDotComRssPortalEndPointResource.IconResourceName, MimeTypes.GIF)]


namespace DowJones.Syndication.RSS
{

    internal struct LiveDotComRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.livedotcombadge.gif";

        /// <summary>
        /// 
        /// </summary>
        public const string IconResourceName = "DowJones.Utilities.Syndication.RSS.livedotcom.gif";
    }

    public class LiveDotComRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://www.live.com/?add={feed}
        private static readonly string m_BaseUrl = "http://my.live.com/";
        private readonly string m_RssFeed;
        private static string m_IconEmbeddedResourceUrl;
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
        /// Initializes a new instance of the <see cref="GoogleRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public LiveDotComRssPortalEndPoint(string rssFeed)
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
            urlBuilder.Append("add", m_RssFeed);
            return urlBuilder.ToString();
        }
        
        #region IEmbeddedIconResource Members

        public override string GetEmbeddedIconUrl()
        {
            return GetEmbeddedIconUrl(GetType(), LiveDotComRssPortalEndPointResource.IconResourceName, ref m_IconEmbeddedResourceUrl);
        }

        #endregion

        #region IEmbeddedBadgeResource Members

        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), LiveDotComRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }

        #endregion
    }
}
