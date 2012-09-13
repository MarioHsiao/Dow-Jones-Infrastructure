using DowJones.Syndication.RSS.Core;
using DowJones.Url;

namespace DowJones.Syndication.RSS
{
    public class SpringWidgetsRssPortalEndPoint : AbstractRssPortalEndPoint
    {
         //http://www.springwidgets.com/widgets/view/23/false/true?param={feed}
        private readonly static string m_BaseUrl =  "http://www.springwidgets.com/widgets/view/23/false/true";
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
        /// Initializes a new instance of the <see cref="NewsgatorRssPortalEndPoint"/> class.
        /// </summary>
        /// <param name="rssFeed">The RSS feed.</param>
        public SpringWidgetsRssPortalEndPoint(string rssFeed)
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
            urlBuilder.Append("param", m_RssFeed);
            return urlBuilder.ToString();
        }
    }
}
