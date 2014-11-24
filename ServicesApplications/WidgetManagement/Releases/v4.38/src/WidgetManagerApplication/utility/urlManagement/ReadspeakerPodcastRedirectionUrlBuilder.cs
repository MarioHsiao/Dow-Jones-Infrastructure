using EMG.Utility.Uri;
using EMG.widgets.ui.Properties;
using EMG.widgets.ui.utility.urlManagement;

namespace EMG.widgets.ui.utility.urlManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadspeakerPodcastRedirectionUrlBuilder : UrlBuilder
    {
        private static readonly string m_ReadSpeaker_Base_Url = Settings.Default.ReadSpeaker_Base_Redirection_Url;
        private static readonly int m_ReadSpeaker_CustomerId = Settings.Default.ReadSpeaker_CustomerId;
        private static readonly int m_ReadSpeaker_Type = Settings.Default.ReadSpeaker_Type;

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
            Append("lang", contentlanguage);
            Append("customerid", m_ReadSpeaker_CustomerId);
            Append("url", new ReadspeakerPodcastContentUrlBuilder(token).ToString());
            Append("type", m_ReadSpeaker_Type);
            Append("voice",voice);
        }
    }}