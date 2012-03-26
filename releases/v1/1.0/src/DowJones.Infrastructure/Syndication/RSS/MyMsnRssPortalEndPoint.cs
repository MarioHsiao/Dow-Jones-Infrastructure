using System.Web.UI;
using DowJones.Utilities.Syndication.RSS;
using DowJones.Utilities.Uri;

[assembly: WebResource(MyMsnRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]

namespace DowJones.Utilities.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    internal struct MyMsnRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.mymsn.gif";
    }
    
    public class MyMsnRssPortalEndPoint: AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://my.msn.com/addtomymsn.armx?id=rss&ut={feed}&ru=returnlink
        private static readonly string m_BaseUrl = "http://my.msn.com/addtomymsn.armx";
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
        public MyMsnRssPortalEndPoint(string rssFeed)
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
            urlBuilder.Append("id", "rss");
            urlBuilder.Append("ut", m_RssFeed);
            return urlBuilder.ToString();
        }

        /// <summary>
        /// Gets the RSS integration URL.
        /// </summary>
        /// <returns></returns>
        public string GetIntegrationUrl(string returnUrl)
        {
            UrlBuilder urlBuilder = new UrlBuilder(m_BaseUrl);
            urlBuilder.OutputType = UrlBuilder.UrlOutputType.Absolute;
            //urlBuilder.BaseUrl = m_BaseUrl;
            urlBuilder.Append("id", "rss");
            urlBuilder.Append("ut", m_RssFeed);
            urlBuilder.Append("ru", returnUrl);
            return urlBuilder.ToString();
        }

        #region IEmbeddedBadgeResource Members
        /// <summary>
        /// Gets the embedded badget URL.
        /// </summary>
        /// <returns></returns>
        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), MyMsnRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }
        #endregion
    }
}
