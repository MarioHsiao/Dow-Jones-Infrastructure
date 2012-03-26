using System.Web.UI;
using DowJones.Utilities.Syndication.RSS;
using DowJones.Utilities.Uri;

[assembly: WebResource(MyAolRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]

namespace DowJones.Utilities.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    internal struct MyAolRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.myaol.gif";
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyAolRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        //"http://feeds.my.aol.com/add.jsp?url={feed}"
        private static readonly string m_BaseUrl = "http://feeds.my.aol.com/add.jsp";
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
        /// Initializes a new instance of the <see cref="MyAolRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public MyAolRssPortalEndPoint(string rssFeed)
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
            return GetEmbeddedBadgetUrl(GetType(), MyAolRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }
        #endregion
    }
}
