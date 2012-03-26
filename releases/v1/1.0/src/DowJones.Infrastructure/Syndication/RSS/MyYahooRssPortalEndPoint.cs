using System.Web.UI;
using DowJones.Utilities.Syndication.RSS;
using DowJones.Utilities.Uri;

[assembly: WebResource(MyYahooRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]

namespace DowJones.Utilities.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public struct MyYahooRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.myyahoo.gif";
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyYahooRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://add.my.yahoo.com/rss?url={feed}
        private static readonly string m_BaseUrl = "http://add.my.yahoo.com/rss";
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
        /// Initializes a new instance of the <see cref="MyYahooRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public MyYahooRssPortalEndPoint(string rssFeed)
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

        #region IEmbeddedBadgeResource Members
        /// <summary>
        /// Gets the embedded badget URL.
        /// </summary>
        /// <returns></returns>
        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), MyYahooRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }
        #endregion
    }
}