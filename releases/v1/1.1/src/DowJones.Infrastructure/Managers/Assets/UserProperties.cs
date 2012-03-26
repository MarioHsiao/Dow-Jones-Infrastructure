
namespace DowJones.Utilities.Managers.Assets
{
    /// <summary>
    /// User Properties for use in cojuntion with Audience Manager.
    /// </summary>
    public class UserProperties
    {
        private readonly string m_AccountId;
        private readonly string m_NameSpace;
        private readonly string m_UserId;

        /// <summary>
        /// Gets the account id.
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
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId
        {
            get { return m_UserId; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProperties"/> class.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <param name="userId">The user id.</param>
        public UserProperties(string accountId, string nameSpace, string userId)
        {
            m_AccountId = accountId;
            m_NameSpace = nameSpace;
            m_UserId = userId;
        }
    }
}
