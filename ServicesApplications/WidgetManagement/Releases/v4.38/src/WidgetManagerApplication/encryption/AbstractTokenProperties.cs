using System;
using System.Collections.Specialized;
using System.Web;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.exception;
using factiva.nextgen.ui;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.encryption
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractTokenProperties
    {
        /// <summary>
        /// 
        /// </summary>
        protected NameValueCollection m_Collection;

        /// <summary>
        /// Gets the name value collection.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected static NameValueCollection GetNameValueCollection(string token)
        {
            try
            {
                Encryption encryption = new Encryption();
                NameValueCollection collection = encryption.decrypt(token, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
                if (collection == null || collection.Count == 0)
                {
                    token = HttpUtility.UrlDecode(token);
                    collection = encryption.decrypt(token, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
                }
                return collection;
            }
            catch (EmgWidgetsUIException ex)
            {
                throw new EmgWidgetsUIException("Unable to decrypt token");
            }
            
        }

        /// <summary>
        /// Gets the value from collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected string GetValueFromCollection(string key)
        {
            if (m_Collection != null && m_Collection.Count > 0)
            {
                return m_Collection[key];
            }
            return string.Empty;
        }
    }
}
