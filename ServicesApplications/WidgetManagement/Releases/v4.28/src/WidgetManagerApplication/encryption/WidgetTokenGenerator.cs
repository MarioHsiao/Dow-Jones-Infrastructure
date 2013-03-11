using System.Collections.Specialized;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.encryption
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WidgetTokenGenerator : ITokenGenerator
    {
        private static readonly WidgetTokenGenerator instance = new WidgetTokenGenerator();
        private static readonly Encryption m_Encryption = new Encryption();
 

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleTokenGenerator"/> class.
        /// </summary>
        private WidgetTokenGenerator()
        {
        }

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>The get instance.</value>
        public static WidgetTokenGenerator GetInstance
        {
            get { return instance; }
        }

        #region ITokenGenerator Members

        /// <summary>
        /// Generates a token.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="productId">The product id.</param>
        /// <param name="widgetId">The widget id.</param>
        /// <returns></returns>
        public string GenerateToken(string userId, string accountId, string productId, string widgetId){
           
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.WIDGET_ID, widgetId);
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.USER_ID, userId);
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.ACCOUNT_ID, accountId);
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.NAMESPACE, productId);

            // Set the easy parameters
            return m_Encryption.encrypt(nameValueCollection, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
        }

        #endregion
    }
}
