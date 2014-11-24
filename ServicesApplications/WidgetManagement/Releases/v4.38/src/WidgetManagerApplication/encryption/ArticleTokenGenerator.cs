using System;
using System.Collections.Specialized;
using Factiva.BusinessLayerLogic;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.encryption
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ArticleTokenGenerator : ITokenGenerator
    {
        private static readonly ArticleTokenGenerator instance = new ArticleTokenGenerator();
        private static readonly Encryption m_Encryption = new Encryption();
 
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleTokenGenerator"/> class.
        /// </summary>
        private ArticleTokenGenerator()
        {
        }

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>The get instance.</value>
        public static ArticleTokenGenerator GetInstance
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
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="contentLanguage">The content language.</param>
        /// <param name="contentCategory">The category.</param>
        /// <returns></returns>
        public static string GenerateToken(string userId, string accountId, string productId, string accessionNumber, ContentLanguage contentLanguage, ContentCategory contentCategory)
        {
           
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.USER_ID, userId);
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.ACCOUNT_ID, accountId);
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.NAMESPACE, productId);

            // Pieces of necessary information to get the article
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.ACCESSION_NUMBER, accessionNumber);
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.CONTENT_LANGUAGE, contentLanguage.ToString());
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.CONTENT_CATEGORY, contentCategory.ToString());
            nameValueCollection.Add(RenderWidgetEncryptionConfiguration.TIME_TO_LIVE, DateTime.Now.ToString());

            // Set the easy parameters
            return m_Encryption.encrypt(nameValueCollection, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
        }

        #endregion

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="productId">The product id.</param>
        /// <param name="accessionNumber">The accession number.</param>
        /// <returns></returns>
        public string GenerateToken(string userId, string accountId, string productId, string accessionNumber)
        {
            return GenerateToken(userId, accountId, productId, accessionNumber, ContentLanguage.en, ContentCategory.Publications);
        }
    }
}
