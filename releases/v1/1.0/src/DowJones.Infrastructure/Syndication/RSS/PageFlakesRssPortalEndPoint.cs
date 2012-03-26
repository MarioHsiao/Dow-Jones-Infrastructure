using System.Web.UI;
using DowJones.Utilities.Syndication.RSS;
using DowJones.Utilities.Uri;

[assembly: WebResource(PageFlakesRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]
[assembly: WebResource(PageFlakesRssPortalEndPointResource.IconResourceName, MimeTypes.PNG)]


namespace DowJones.Utilities.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    internal struct PageFlakesRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.rss.pageflakes.gif";
        /// <summary>
        /// 
        /// </summary>
        public const string IconResourceName = "DowJones.Utilities.Syndication.RSS.pageflakes.png";
    }

    /// <summary>
    /// 
    /// </summary>
    public class PageFlakesRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://www.pageflakes.com/subscribe.aspx?url={feed}
        private readonly static string m_BaseUrl = "http://www.pageflakes.com/subscribe.aspx";
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
        /// Initializes a new instance of the <see cref="PageFlakesRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public PageFlakesRssPortalEndPoint(string rssFeed)
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
            urlBuilder.Append("url", m_RssFeed);
            return urlBuilder.ToString();
        }

        /// <summary>
        /// Gets the embedded support script.
        /// </summary>
        /// <returns></returns>
        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), PageFlakesRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }

        /// <summary>
        /// Gets the embedded icon URL.
        /// </summary>
        /// <returns></returns>
        public override string GetEmbeddedIconUrl()
        {
            return GetEmbeddedIconUrl(GetType(), PageFlakesRssPortalEndPointResource.IconResourceName, ref m_IconEmbeddedResourceUrl);
        }
    }
}
