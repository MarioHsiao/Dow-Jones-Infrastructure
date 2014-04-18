using System;
using System.Collections.Generic;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Properties;

namespace DowJones.Session
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSession : IUserSession
    {
        private bool _isInvalid;

        /// <summary>
        /// Gets the access point code.
        /// </summary>
        /// <value>The access point code.</value>
        public string AccessPointCode { get; set; }

        /// <summary>
        /// Gets the access point code usage.
        /// </summary>
        /// <value>The access point code usage.</value>
        public string AccessPointCodeUsage { get; set; }

        /// <summary>
        /// Gets the account id.
        /// </summary>
        /// <value>The account id.</value>
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the client type code.
        /// </summary>
        /// <value>The client type code.</value>
        public string ClientTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the entitlements.
        /// </summary>
        /// <value>
        /// The entitlements.
        /// </value>
        public IEnumerable<KeyValuePair<string, object>> Entitlements { get; set; }

        /// <summary>
        /// Gets the interface language.
        /// </summary>
        /// <value>The interface language.</value>
        public InterfaceLanguage InterfaceLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is debug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is debug; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsDebug { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is proxy session.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is proxy session; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsProxySession
        {
            get { return ProxyUserId.HasValue() && ProxyNamespace.HasValue() && (SessionId.HasValue() || LightWeightLoginToken.HasValue()); }
        }

        /// <summary>
        /// Gets the product id.
        /// </summary>
        /// <value>The product id.</value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets the product prefix.
        /// </summary>
        /// <value>The product prefix.</value>
        public string ProductPrefix { get; set; }

        /// <summary>
        /// Gets or sets the light weight login token.
        /// </summary>
        /// <value>
        /// The light weight login token.
        /// </value>
        public string LightWeightLoginToken { get; set; }

        /// <summary>
        /// Gets or sets the proxy user id.
        /// </summary>
        /// <value>The proxy user id.</value>
        public string ProxyUserId { get; set; }

        /// <summary>
        /// Gets or sets the proxy namespace.
        /// </summary>
        /// <value>The proxy namespace.</value>
        public string ProxyNamespace { get; set; }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <value>The session id.</value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets the Encrypted user id and Namespace.
        /// </summary>
        /// <value>The user id.</value>
        public string EncryptedUserId { get; set; }

        /// <summary>
        /// Gets Security Hash.
        /// </summary>
        /// <value>Security Hash</value>
        public string SecurityHash { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession" /> class.
        /// </summary>
        public UserSession()
        {
            _isInvalid = false;
            AccessPointCode = Settings.Default.DefaultAccessPointCode;
            ClientTypeCode = Settings.Default.DefaultClientCodeType;
            Entitlements = new Dictionary<string, object>();
            InterfaceLanguage = default(InterfaceLanguage);
            ProductPrefix = Settings.Default.DefaultProductPrefix;
        }


        /// <summary>
        /// Sets the interface language.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        public void SetInterfaceLanguage(string languageCode)
        {
            InterfaceLanguage interfaceLanguage;
            //Default to "en" if it is not a valid supported interface language
            Enum.TryParse(languageCode, true, out interfaceLanguage);
            InterfaceLanguage = interfaceLanguage;
        }

        /// <summary>
        /// Sets the proxy user.
        /// </summary>
        /// <param name="proxyUserId">The proxy user id.</param>
        /// <param name="proxyNamespace">The proxy namespace.</param>
        /// <param name="proxyAccountId">The proxy account id.</param>
        public void SetProxyUser(string proxyUserId, string proxyNamespace, string proxyAccountId)
        {
            ProxyUserId = proxyUserId;
            ProxyNamespace = proxyNamespace;
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsValid()
        {
            return _isInvalid || IsProxySession || SessionId.HasValue() || LightWeightLoginToken.HasValue();
        }

        /// <summary>
        /// Invalidates this instance.
        /// </summary>
        public void Invalidate()
        {
            _isInvalid = true;
        }
    }
}