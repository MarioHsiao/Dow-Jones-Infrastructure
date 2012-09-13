using DowJones.Properties;
using DowJones.Url;
using DowJones.Web.Handlers.Syndication.Podcast;

namespace DowJones.Web.Handlers.Syndication.ReadSpeaker.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class MediaRedirectionUrlBuilder : UrlBuilder
    {
        private static readonly bool m_Use_ReadSpeaker_Enterprise = Settings.Default.UseReadSpeakerEnterprise;
        private static readonly string m_ReadSpeaker_Enterprise_Base_Url = Settings.Default.ReadSpeakerEnterprise_Base_Redireciton_Url;
        private static readonly int m_ReadSpeaker_Enterprise_CustomerId = Settings.Default.ReadSpeakerEnterprise_CustomerId;
        private static readonly string m_ReadSpeaker_Base_Url = Settings.Default.ReadSpeaker_Base_Redirection_Url;
        private static readonly int m_ReadSpeaker_CustomerId = Settings.Default.ReadSpeaker_CustomerId;


        // private readspeaker key consts
        private const string LANGUAGE = "lang";
        private const string CUSTOMER_ID = "customerid";
        private const string URL = "url";
        private const string TYPE = "type";
        private const string VOICE = "voice";
        private const string SAVE = "save";
        private const string AUDIO_FILE_NAME = "audiofilename";
        private const string STAT_TYPE = "stattype";

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRedirectionUrlBuilder"/> class.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="token">The token.</param>
        /// <param name="contentlanguage">The content language.</param>
        /// <param name="voice">The voice.</param>
        public MediaRedirectionUrlBuilder(string accessionNumber, string token, string contentlanguage, string voice)
            : this(MediaRedirectionType.StandardPlayer, accessionNumber, token, contentlanguage, voice, string.Empty)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRedirectionUrlBuilder"/> class.
        /// </summary>
        /// <param name="redirectionType">Type of the redirection.</param>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="token">The token.</param>
        /// <param name="contentlanguage">The content language.</param>
        /// <param name="voice">The voice.</param>
        /// <param name="accessPointCode">The access point code.</param>
        public MediaRedirectionUrlBuilder(MediaRedirectionType redirectionType, string accessionNumber, string token, string contentlanguage, string voice, string accessPointCode)
            : base(m_Use_ReadSpeaker_Enterprise ? m_ReadSpeaker_Enterprise_Base_Url : string.Format(m_ReadSpeaker_Base_Url, accessionNumber))
        {
            OutputType = UrlOutputType.Absolute;
            Append(LANGUAGE, contentlanguage);
            Append(CUSTOMER_ID, m_ReadSpeaker_CustomerId);
            Append(URL, new ContentUrlBuilder(token).ToString());
            Append(TYPE, 0);
            Append(VOICE, voice);
            if (!string.IsNullOrEmpty(accessPointCode) && !string.IsNullOrEmpty(accessPointCode.Trim()))
            {
                switch (accessPointCode.ToUpper())
                {
                    default:
                        break;
                    case "AA":
                        Append(STAT_TYPE, IntegrationType.ListenToArticleView.ToString());
                        break;
                    case "PN":
                        Append(STAT_TYPE, IntegrationType.PodcastNewsletter.ToString());
                        break;
                    case "PW":
                        Append(STAT_TYPE, IntegrationType.PodcastWorkspace.ToString());
                        break;
                }
                
            }
            if (m_Use_ReadSpeaker_Enterprise)
            {
                Append(AUDIO_FILE_NAME, accessionNumber);
                Append(CUSTOMER_ID, m_ReadSpeaker_Enterprise_CustomerId);
            }
            switch (redirectionType)
            {
                 case MediaRedirectionType.StandardPlayer:
                    break;
                 default:
                    Append(SAVE, "1");
                    break;
            }
        }
    }
}