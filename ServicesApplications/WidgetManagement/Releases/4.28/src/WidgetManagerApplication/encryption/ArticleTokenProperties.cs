using System;
using System.Collections.Specialized;
using Factiva.BusinessLayerLogic;
using EMG.widgets.ui.encryption;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.encryption
{
    /// <summary>
    /// 
    /// </summary>
    public class ArticleTokenProperties : AbstractTokenProperties
    {
        private readonly string m_NameSpace;
        private readonly string m_UserId;
        private readonly string m_AccountId;
        private readonly string m_AccessionNumber;
        private readonly ContentLanguage m_ContentLanguage = ContentLanguage.en;
        private readonly ContentCategory m_ContentCategory = ContentCategory.Publications;
        private readonly DateTime m_TimeToLive = DateTime.MinValue;
        private readonly string m_OriginalToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleTokenProperties"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ArticleTokenProperties(string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(token.Trim()))
                throw new ArgumentNullException("token");

            NameValueCollection collection = GetNameValueCollection(token);
            m_OriginalToken = token;
            if (collection.Count > 0)
            {
                m_Collection = collection;
                m_UserId = GetValueFromCollection(RenderWidgetEncryptionConfiguration.USER_ID);
                m_AccountId = GetValueFromCollection(RenderWidgetEncryptionConfiguration.ACCOUNT_ID);
                m_NameSpace = GetValueFromCollection(RenderWidgetEncryptionConfiguration.NAMESPACE);
                
                m_AccessionNumber = GetValueFromCollection(RenderWidgetEncryptionConfiguration.ACCESSION_NUMBER);
                m_ContentLanguage = GetContentLanguage(GetValueFromCollection(RenderWidgetEncryptionConfiguration.CONTENT_LANGUAGE));
                m_ContentCategory = GetContentCategory(GetValueFromCollection(RenderWidgetEncryptionConfiguration.CONTENT_CATEGORY));
                m_TimeToLive = GetTimeToLive(GetValueFromCollection(RenderWidgetEncryptionConfiguration.TIME_TO_LIVE));
            }
        }

        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <param name="lang">The lang.</param>
        /// <returns></returns>
        private static ContentLanguage GetContentLanguage(string lang)
        {
            return (ContentLanguage) Enum.Parse(typeof (ContentLanguage), lang);
        }

        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <param name="cat">The cat.</param>
        /// <returns></returns>
        private static ContentCategory GetContentCategory(string cat)
        {
            return (ContentCategory)Enum.Parse(typeof(ContentCategory), cat);
        }

        private static DateTime GetTimeToLive(string dateTime)
        {
            DateTime temp;
            if (DateTime.TryParse(dateTime, out temp))
                return temp;
            return DateTime.MinValue;
        }

        /// <summary>
        /// Gets the accession number.
        /// </summary>
        /// <value>The accession number.</value>
        public string AccessionNumber
        {
            get { return m_AccessionNumber; }
        }

        /// <summary>
        /// Gets the name space.
        /// </summary>
        /// <value>The name space.</value>
        public string NameSpace
        {
            get { return m_NameSpace; }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId
        {
            get { return m_UserId; }
        }

        /// <summary>
        /// Gets the widget id.
        /// </summary>
        /// <value>The widget id.</value>
        public string AccountId
        {
            get { return m_AccountId; }
        }

        /// <summary>
        /// Gets the original token.
        /// </summary>
        /// <value>The original token.</value>
        public string OriginalToken
        {
            get { return m_OriginalToken; }
        }

        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <value>The content language.</value>
        public ContentLanguage ContentLanguage
        {
            get { return m_ContentLanguage; }
        }


        /// <summary>
        /// Gets the content language.
        /// </summary>
        /// <value>The content language.</value>
        public ContentCategory ContentCategory
        {
            get { return m_ContentCategory; }
        }

        /// <summary>
        /// Gets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        public DateTime TimeToLive
        {
            get { return m_TimeToLive; }
        }

    }
}
