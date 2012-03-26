using System;
using DowJones.Utilities.TokenEncryption;

namespace DowJones.Utilities.Url
{
    public class TimeToLiveProxyTokenGenerator : ITokenParser
    {
        private readonly EncryptedTokenParser m_TokenParser;
        private string m_LatestDecryptedToken = string.Empty;

        public string LatestDecryptedToken
        {
            get { return m_LatestDecryptedToken; }
        }

        public TimeToLiveProxyTokenGenerator()
        {
            m_TokenParser = new EncryptedTokenParser(KeyConfiguration.EID4_TTL_PROXY_ENCRYPTION_KEY);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastArticleToken"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public TimeToLiveProxyTokenGenerator(string token)
            : this()
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
            get { return m_TokenParser.GetItem(EID4Configuration.ACCESSION_NUMBER); }
            set { m_TokenParser.UpdateItem(EID4Configuration.ACCESSION_NUMBER, value); }
        }

        public QueryStringDictionary GetQueryStringDictionary()
        {
            var dict = new QueryStringDictionary();
            dict.Append("eid4", Encrypt());
            return dict;
        }

        /// <summary>
        /// Gets the accession number.
        /// </summary>
        /// <value>The accession number.</value>
        public string TTL_Token
        {
            get { return m_TokenParser.GetItem(EID4Configuration.PROXY_TOKEN); }
            set { m_TokenParser.UpdateItem(EID4Configuration.PROXY_TOKEN, value); }
        }

        #region ITokenParser Members

        public string Encrypt()
        {
            return m_TokenParser.Encrypt();
        }

        public void Decrypt(string str)
        {
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str.Trim()))
            {
                m_LatestDecryptedToken = str;
            }
            m_TokenParser.Decrypt(str);
        }

        #endregion
    }
}
