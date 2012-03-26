using System.Web.UI;
using DowJones.Syndication.RSS;
using DowJones.Syndication.RSS.Core;
using DowJones.Url;

[assembly: WebResource(NetvibesRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]
[assembly: WebResource(NetvibesRssPortalEndPointResource.IconResourceName, MimeTypes.PNG)]


namespace DowJones.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    internal struct NetvibesRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.netvibes.gif";

        /// <summary>
        /// 
        /// </summary>
        public const string IconResourceName = "DowJones.Utilities.Syndication.RSS.netvibes.png";
    }

    /// <summary>
    /// 
    /// </summary>
    public class NetvibesRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://www.netvibes.com/subscribe.php?url={feed}
        private static readonly string m_BaseUrl = "http://www.netvibes.com/subscribe.php";
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
        /// Initializes a new instance of the <see cref="NetvibesRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public NetvibesRssPortalEndPoint(string rssFeed)
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
        /// Gets the embedded badget URL.
        /// </summary>
        /// <returns></returns>
        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), NetvibesRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }

        #region IEmbeddedIconResource Members

        /// <summary>
        /// Gets the embedded icon URL.
        /// </summary>
        /// <returns></returns>
        public override string GetEmbeddedIconUrl()
        {
            return GetEmbeddedIconUrl(GetType(), NetvibesRssPortalEndPointResource.IconResourceName, ref m_IconEmbeddedResourceUrl);
        }

        #endregion
    }
}