
using EMG.Utility.Uri;
using EMG.widgets.ui.Properties;

namespace EMG.widgets.ui.utility.urlManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadspeakerPodcastContentUrlBuilder : UrlBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string m_ReadSpeaker_ContentHandler = Settings.Default.ReadSpeaker_ContentHandler;
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadspeakerPodcastContentUrlBuilder"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReadspeakerPodcastContentUrlBuilder(string token)
        {
            OutputType = UrlOutputType.Absolute;
            BaseUrl = m_ReadSpeaker_ContentHandler;
            Append("tkn", token);
        }
    }

}
