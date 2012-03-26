using System;
using System.Web.UI;
using DowJones.Utilities.Managers.Core;
using DowJones.Utilities.Syndication.RSS;

[assembly: WebResource(GenericFeedReaderEndPointResource.IconResourceName, MimeTypes.GIF)]

namespace DowJones.Utilities.Syndication.RSS
{
    internal struct GenericFeedReaderEndPointResource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string IconResourceName = "DowJones.Utilities.Syndication.RSS.addto.gif";
    }

    public class GenericFeedReaderRssPortalEndPoint : AbstractRssPortalEndPoint, IEmbeddedIconResource
    {
        private readonly string m_RssFeed;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public override string RssFeed
        {
            get { return m_RssFeed; }
        }

        /// <summary>
        /// Gets the RSS integration URL.
        /// </summary>
        /// <returns></returns>
        public override string GetIntegrationUrl()
        {
           if (StringUtilitiesManager.IsValid(m_RssFeed))
           {
                if (m_RssFeed.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || 
                    m_RssFeed.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                {
                    return string.Concat("feed://", m_RssFeed.Substring(7));
                }
            }
            return m_RssFeed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public GenericFeedReaderRssPortalEndPoint(string rssFeed)
        {
            m_RssFeed = rssFeed;
        }
    }
}
