using System;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Handlers.Syndication.Podcast.Core;

namespace DowJones.Utilities.TokenEncryption
{

    /// <summary>
    /// 
    /// </summary>
    internal class PodcastArticleToken : ITokenParser
    {
       
        private readonly EncryptedTokenParser _tokenParser;
        private string _latestDecryptedToken = string.Empty;

        public string LatestDecryptedToken
        {
            get { return _latestDecryptedToken; }
        }

        public PodcastArticleToken()
        {
            // Set Defaults
            _tokenParser = new EncryptedTokenParser(KeyConfiguration.DEFAULT_ENCRYPTION_KEY);
            ContentCategory = "Publications";
            ContentLanguage = "en";
            TimeToLive = DateTime.MaxValue;
            MediaRedirectionType = MediaRedirectionType.StandardPlayer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastArticleToken"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public PodcastArticleToken(string token) : this()
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(token.Trim()))
                throw new ArgumentNullException("token");

            Decrypt(token);
        }

        /// <summary>
        /// Gets the accession number.
        /// </summary>
        /// <value>The accession number.</value>
        public string AccessionNumber
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.ACCESSION_NUMBER); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.ACCESSION_NUMBER, value); }
        }

        /// <summary>
        /// Gets the name space.
        /// </summary>
        /// <value>The name space.</value>
        public string NameSpace
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.NAMESPACE); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.NAMESPACE, value); }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.USER_ID); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.USER_ID, value); }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string Device
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.DEVICE); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.DEVICE, value); }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string ClientType
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.CLIENT_TYPE); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.CLIENT_TYPE, value); }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string ProductType
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.PRODUCT_TYPE); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.PRODUCT_TYPE, value); }
        }


        /// <summary>
        /// Gets the widget id.
        /// </summary>
        /// <value>The widget id.</value>
        public string AccountId
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.ACCOUNT_ID); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.ACCOUNT_ID, value); }
        }

        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <value>The content language.</value>
        public string ContentLanguage
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.CONTENT_LANGUAGE); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.CONTENT_LANGUAGE, value); }
        }


        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <value>The content language.</value>
        public string ContentCategory
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.CONTENT_CATEGORY); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.CONTENT_CATEGORY, value); }
        }


       /* /// <summary>
        /// Gets or sets the operational data string.
        /// </summary>
        /// <value>The operational data string.</value>
        public string OperationalDataMemento
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.OPERATIONAL_DATA_STRING); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.OPERATIONAL_DATA_STRING, value); }
        }
        * 
*/
        /// <summary>
        /// Gets or sets the type of the media redirection.
        /// </summary>
        /// <value>The type of the media redirection.</value>
        public MediaRedirectionType MediaRedirectionType
        {
            get { return GetEnum<MediaRedirectionType>(_tokenParser.GetItem(EncryptionConfiguration.MEDIA_REDIRECTION_TYPE)); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.MEDIA_REDIRECTION_TYPE, value.ToString()); }

        }
        
        /// <summary>
        /// Gets or sets the access point code.
        /// </summary>
        /// <value>The access point code.</value>
        public string AccessPointCode
        {
            get { return _tokenParser.GetItem(EncryptionConfiguration.ACCESS_POINT_CODE); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.ACCESS_POINT_CODE, value); }
        }

        /// <summary>
        /// Gets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        public DateTime TimeToLive
        {
            get { return GetDate(_tokenParser.GetItem(EncryptionConfiguration.TIME_TO_LIVE)); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.TIME_TO_LIVE, value.ToString("")); }
        }

        public bool IncludeMarketingMessage
        {
            get { return GetBool(_tokenParser.GetItem(EncryptionConfiguration.INCLUDE_MARKETING_MESSAGE)); }
            set { _tokenParser.UpdateItem(EncryptionConfiguration.INCLUDE_MARKETING_MESSAGE,value.ToString()); }
        }

        private static DateTime GetDate(string s)
        {
            DateTime temp;
            if (DateTimeFormatter.ParseDate(s, out temp))
                return temp;
            return DateTime.MaxValue;
        }

        private static T GetEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private static bool GetBool(string s)
        {
            bool temp;
            if (bool.TryParse(s, out temp))
                return temp;
            return false;
        }

        #region ITokenParser Members

        protected void UpdateItem(string key, string value)
        {
            _tokenParser.UpdateItem(key,value);
        }

        protected string GetItem(string key)
        {
            return _tokenParser.GetItem(key);
        }

        public string Encrypt()
        {
            return _tokenParser.Encrypt();
        }

        public void Decrypt(string str)
        {
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str.Trim()))
            {
                _latestDecryptedToken = str;
            }
            _tokenParser.Decrypt(str);
        }

        #endregion
    }
}