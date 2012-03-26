using EMG.Utility.Properties;
using EMG.Utility.Uri;

namespace EMG.Utility.Handlers.Syndication.Podcast.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadspeakerPodcastContentUrlBuilder : UrlBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string m_ReadSpeaker_ContentHandler = Settings.Default.Podcast_ContentHandler;
        internal const string QS_PARAM_TOKEN = "tkn";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadspeakerPodcastContentUrlBuilder"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReadspeakerPodcastContentUrlBuilder(string token)
        {
            OutputType = UrlOutputType.Absolute;
            BaseUrl = m_ReadSpeaker_ContentHandler;
            Append(QS_PARAM_TOKEN, token);
        }
    }

}
