using System;
using DowJones.Utilities.Encoders;
using DowJones.Utilities.TokenEncryption;
using DowJones.Utilities.Uri;

namespace DowJones.Utilities.Handlers.Syndication.Podcast.Core
{
    public enum MediaRedirectionType
    {
        StandardPlayer = 0,
        UrlToSoundFile = 1,
    }

    public enum IntegrationType
    {
        ListenToArticleView,
        PodcastNewsletter,
        PodcastWorkspace
    }


    /// <summary>
    /// 
    /// </summary>
    public class AudioMediaUrlBuilder
    {
        // private static readonly int m_SegmentSize = 20;
        // private const string m_StartSegment = "_SOT";
        // private const string m_EndSegment = "_EOT";
        // private const string DEFAULT_FOLDER_PATH = "~/{0}.mp3"; OLD STRING

        private const string DEFAULT_FOLDER_PATH = "~/{1}.mp3?" + BaseMediaRedirectionHandler.TOKEN_NAME_VALUE_PAIR + "={0}";
        private readonly string _path;
        private readonly PodcastArticleToken _token = new PodcastArticleToken();


        /// <summary>
        /// Initializes a new instance of the <see cref="AudioMediaUrlBuilder"/> class.
        /// </summary>
        /// <param name="folderPath">The accession number.</param>
        /// <param name="mediaRedirectionType">Type of the media redirection.</param>
        /// <param name="integartaionType">Type of the integartaion.</param>
        public AudioMediaUrlBuilder(string folderPath, MediaRedirectionType mediaRedirectionType, IntegrationType integartaionType)
        {
            _path = DEFAULT_FOLDER_PATH;
            MediaRedirectionType = mediaRedirectionType;
            IntegrationType = integartaionType;
            if (!string.IsNullOrEmpty(folderPath) && !string.IsNullOrEmpty(folderPath.Trim()))
            {
                _path = folderPath.EndsWith("/") ? string.Concat(folderPath, "{0}.mp3") : string.Concat(folderPath, "/{0}.mp3");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioMediaUrlBuilder"/> class.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="mediaRedirectionType">Type of the media redirection.</param>
        public AudioMediaUrlBuilder(string folderPath, MediaRedirectionType mediaRedirectionType)
            : this(folderPath, mediaRedirectionType, IntegrationType.ListenToArticleView)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioMediaUrlBuilder"/> class.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        public AudioMediaUrlBuilder(string folderPath) 
            : this(folderPath, MediaRedirectionType.StandardPlayer, IntegrationType.ListenToArticleView)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioMediaUrlBuilder"/> class.
        /// </summary>
        /// <param name="mediaRedirectionType">Type of the media redirection.</param>
        /// <param name="integrationType">Type of the integration.</param>
        public AudioMediaUrlBuilder(MediaRedirectionType mediaRedirectionType, IntegrationType integrationType) 
            : this(null, mediaRedirectionType, integrationType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioMediaUrlBuilder"/> class.
        /// </summary>
        /// <param name="mediaRedirectionType">Type of the media redirection.</param>
        public AudioMediaUrlBuilder(MediaRedirectionType mediaRedirectionType) 
            : this(null, mediaRedirectionType, IntegrationType.ListenToArticleView)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioMediaUrlBuilder"/> class.
        /// </summary>
        public AudioMediaUrlBuilder() 
            : this(null, MediaRedirectionType.StandardPlayer, IntegrationType.ListenToArticleView)
        {
        }


        /// <summary>
        /// Gets the accession number.
        /// </summary>
        /// <value>The accession number.</value>
        public string AccessionNumber
        {
            get { return _token.AccessionNumber; }
            set { _token.AccessionNumber = value; }
        }

        /// <summary>
        /// Gets the name space.
        /// </summary>
        /// <value>The name space.</value>
        public string NameSpace
        {
            get { return _token.NameSpace; }
            set { _token.NameSpace = value; }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId
        {
            get { return _token.UserId; }
            set { _token.UserId = value; }
        }

        /// <summary>
        /// Gets the widget id.
        /// </summary>
        /// <value>The widget id.</value>
        public string AccountId
        {
            get { return _token.AccountId; }
            set { _token.AccountId = value; }
        }

        public IntegrationType IntegrationType
        {
            get
            {
                if (!string.IsNullOrEmpty(_token.AccessPointCode) && !string.IsNullOrEmpty(_token.AccessPointCode.Trim()))
                {
                    switch (_token.AccessPointCode.ToUpper())
                    {
                        case "PN":
                            return IntegrationType.PodcastNewsletter;
                        case "PW":
                            return IntegrationType.PodcastWorkspace;
                    }
                }
                return IntegrationType.ListenToArticleView;
            }
            set
            {
                switch (value)
                {
                    //case IntegrationType.ListenToArticleView:
                    default:
                        _token.AccessPointCode = "AA";
                        break;
                    case IntegrationType.PodcastNewsletter:
                        _token.AccessPointCode = "PN";
                        break;
                    case IntegrationType.PodcastWorkspace:
                        _token.AccessPointCode = "PW";
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <value>The content language.</value>
        public string ContentLanguage
        {
            get { return _token.ContentLanguage; }
            set { _token.ContentLanguage = value; }
        }

        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <value>The content language.</value>
        public string ContentCategory
        {
            get { return _token.ContentCategory; }
            set { _token.ContentLanguage = value; }
        }

        /// <summary>
        /// Gets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        public DateTime TimeToLive
        {
            get { return _token.TimeToLive; }
            set { _token.TimeToLive = value; }
        }

        public bool IncludeMarketingMessage
        {
            get { return _token.IncludeMarketingMessage; }
            set { _token.IncludeMarketingMessage = value; }
        }

        public MediaRedirectionType MediaRedirectionType
        {
            get { return _token.MediaRedirectionType; }
            set { _token.MediaRedirectionType = value; }
        }

        /*public string OperationalDataMemento
        {
            get { return _token.OperationalDataMemento; }
            set { _token.OperationalDataMemento = value; }
        }*/

        public string Device
        {
            get { return _token.Device; }
            set { _token.Device = value; }
        }

        public string ProductType
        {
            get { return _token.ProductType; }
            set { _token.ProductType = value; }
        }

        public string ClientType
        {
            get { return _token.ClientType; }
            set { _token.ClientType = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        new public string ToString()
        {
            CustomCharacterEncoder encoder = new CustomCharacterEncoder();
            UrlBuilder ub = new UrlBuilder(string.Format(_path, encoder.Encode(_token.Encrypt()), AccessionNumber));
            ub.OutputType = UrlBuilder.UrlOutputType.Absolute;

            return ub.ToString();
        }


        public static string Decode(string token)
        {
            CustomCharacterEncoder encoder = new CustomCharacterEncoder();
            return encoder.Decode(token);
        }
    }
}