using System.Web.UI;
using DowJones.Syndication.RSS;
using DowJones.Syndication.RSS.Core;
using DowJones.Url;

[assembly: WebResource(GoogleRssPortalEndPointResource.BadgeResourceName, MimeTypes.GIF)]
[assembly: WebResource(GoogleRssPortalEndPointResource.IconResourceName, MimeTypes.PNG)]

namespace DowJones.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    internal struct GoogleRssPortalEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BadgeResourceName = "DowJones.Utilities.Syndication.RSS.googleBadge.gif";

        /// <summary>
        /// 
        /// </summary>
        public const string IconResourceName = "DowJones.Utilities.Syndication.RSS.googleIcon.png";
    }

    /// <summary>
    /// 
    /// </summary>
    public class GoogleRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedBadgeResource
    {
        // http://fusion.google.com/add?feedurl={feed}
        private static readonly string m_BaseUrl = "http://fusion.google.com/add";
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
        public GoogleRssPortalEndPoint(string rssFeed)
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
            urlBuilder.Append("feedurl", m_RssFeed);
            return urlBuilder.ToString();
        }

        #region IEmbeddedBadgeResource Members
        /// <summary>
        /// Gets the embedded badget URL.
        /// </summary>
        /// <returns></returns>
        public string GetEmbeddedBadgetUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), GoogleRssPortalEndPointResource.BadgeResourceName, ref m_BadgeEmbeddedResourceUrl);
        }
        #endregion

        #region IEmbeddedIconResource Members
        /// <summary>
        /// Gets the embedded icon URL.
        /// </summary>
        /// <returns></returns>
        public override string GetEmbeddedIconUrl()
        {
            return GetEmbeddedBadgetUrl(GetType(), GoogleRssPortalEndPointResource.IconResourceName, ref m_IconEmbeddedResourceUrl);            
        }

        #endregion
    }
}