using System;
using System.Collections.Specialized;
using EMG.widgets.ui.encryption;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.encryption
{
    /// <summary>
    /// 
    /// </summary>
    public class WidgetTokenProperties : AbstractTokenProperties
    {
        private readonly string m_AccountId;
        private readonly string m_NameSpace;
        private readonly string m_UserId;
        private readonly string m_WidgetId;
        private readonly string m_im;
        private readonly string m_OriginalToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetTokenProperties"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public WidgetTokenProperties(string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(token.Trim()))
                throw new ArgumentNullException("token");

            NameValueCollection collection = GetNameValueCollection(token);
            m_OriginalToken = token;
            if (collection.Count > 0)
            {
                m_Collection = collection;
                m_WidgetId = GetValueFromCollection(RenderWidgetEncryptionConfiguration.WIDGET_ID);
                m_UserId = GetValueFromCollection(RenderWidgetEncryptionConfiguration.USER_ID);
                m_AccountId = GetValueFromCollection(RenderWidgetEncryptionConfiguration.ACCOUNT_ID);
                m_NameSpace = GetValueFromCollection(RenderWidgetEncryptionConfiguration.NAMESPACE);
                m_im = GetValueFromCollection(RenderWidgetEncryptionConfiguration.IS_MCT);
            }
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
        /// Gets  the account id.
        /// </summary>
        /// <value>The account id.</value>
        public string AccountId
        {
            get { return m_AccountId; }
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
        /// Gets the token id.
        /// </summary>
        /// <value>The token id.</value>
        public string WidgetId
        {
            get { return m_WidgetId; }
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
        /// Gets the MCT flag.
        /// </summary>
        /// <value>The MCT flag.</value>
        public bool IsMct
        {
            get { return (!string.IsNullOrEmpty(m_im) && "1" == m_im); }
        }
    }
}