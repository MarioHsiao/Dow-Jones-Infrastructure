using EMG.Utility.Properties;
using EMG.Utility.Uri;


namespace EMG.Utility.Handlers.Syndication.Podcast.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadspeakerPodcastRedirectionUrlBuilder : UrlBuilder
    {
        private static readonly string m_ReadSpeaker_Base_Url = Settings.Default.ReadSpeaker_Base_Redirection_Url;
        private static readonly int m_ReadSpeaker_CustomerId = Settings.Default.ReadSpeaker_CustomerId;
        private static readonly int m_ReadSpeaker_Type = Settings.Default.ReadSpeaker_Type;

        // private readspeaker key consts
        private const string m_Language = "lang";
        private const string m_CustomerId = "customerid";
        private const string m_Url = "url";
        private const string m_Type = "type";
        private const string m_Voice = "voice";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadspeakerPodcastRedirectionUrlBuilder"/> class.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="token">The token.</param>
        /// <param name="contentlanguage">The contentlanguage.</param>
        /// <param name="voice">The voice.</param>
        public ReadspeakerPodcastRedirectionUrlBuilder(string accessionNumber, string token, string contentlanguage, string voice)
        {
            OutputType = UrlOutputType.Absolute;
            BaseUrl = string.Format(m_ReadSpeaker_Base_Url, accessionNumber);
            Append(m_Language, contentlanguage);
            Append(m_CustomerId, m_ReadSpeaker_CustomerId);
            Append(m_Url, new ReadspeakerPodcastContentUrlBuilder(token).ToString());
            Append(m_Type, m_ReadSpeaker_Type);
            Append(m_Voice, voice);
        }
    }
}