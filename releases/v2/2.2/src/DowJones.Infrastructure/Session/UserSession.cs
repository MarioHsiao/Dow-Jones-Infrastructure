using System;
using System.Collections.Generic;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Properties;

namespace DowJones.Session
{
    public class UserSession : IUserSession
    {
        /// <summary>
        /// Gets the access point code.
        /// </summary>
        /// <value>The access point code.</value>
        public string AccessPointCode { get; set; }

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

        public IEnumerable<KeyValuePair<string, object>> Entitlements { get; set; }

        /// <summary>
        /// Gets the interface language.
        /// </summary>
        /// <value>The interface language.</value>
        public InterfaceLanguage InterfaceLanguage { get; set; }

        public virtual bool IsDebug { get; set; }

        public virtual bool IsProxySession
        {
            get { return ProxyUserId.HasValue() && ProxyNamespace.HasValue(); }
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


        public UserSession()
        {
            AccessPointCode = Settings.Default.DefaultAccessPointCode;
            ClientTypeCode = Settings.Default.DefaultClientCodeType;
            Entitlements = new Dictionary<string, object>();
            InterfaceLanguage = default(InterfaceLanguage);
            ProductPrefix = Settings.Default.DefaultProductPrefix;
        }


        public void SetInterfaceLanguage(string languageCode)
        {
            InterfaceLanguage = (InterfaceLanguage) Enum.Parse(typeof (InterfaceLanguage), languageCode, true);
        }

        public void SetProxyUser(string proxyUserId, string proxyNamespace, string proxyAccountId)
        {
            ProxyUserId = proxyUserId;
            ProxyNamespace = proxyNamespace;
        }

        public virtual bool Validate()
        {
            return IsProxySession || SessionId.HasValue();
        }
    }
}