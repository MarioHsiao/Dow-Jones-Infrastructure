using System;
using DowJones.Managers.Core;
using DowJones.Syndication.RSS.Core;

namespace DowJones.Syndication.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public class ITunesRssPortalEndPoint : AbstractRssPortalEndPoint
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
        /// Initializes a new instance of the <see cref="ITunesRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public ITunesRssPortalEndPoint(string rssFeed)
        {
            m_RssFeed = rssFeed;
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
                    return string.Concat("itpc://", m_RssFeed.Substring(7));
                }
            }
            return m_RssFeed;
        }
    }
}
