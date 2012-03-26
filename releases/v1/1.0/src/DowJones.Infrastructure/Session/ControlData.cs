// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlData.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Attributes;

namespace DowJones.Session
{
    public class ControlData : IControlData
    {
        private const string DefaultContentServerAddress = "0";
        private const string DefaultIpAddress = "127.0.0.1";

        private string contentServerAddress = DefaultContentServerAddress;
        private string ipAddress = DefaultIpAddress;

        /// <summary>
        /// Gets or sets the access point code.
        /// </summary>
        /// <value>
        /// The access point code.
        /// </value>
        [ParameterName("apc")]
        public string AccessPointCode { get; set; }

        /// <summary>
        /// Gets or sets the access point code for usage.
        /// </summary>
        /// <value>
        /// The access point code usage.
        /// </value>
        [ParameterName("apcu")]
        public string AccessPointCodeUsage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [Bypass Client Billing].
        /// </summary>
        /// <value>
        /// <c>true</c> if [Bypass Client Billing]; otherwise, <c>false</c>.
        /// </value>
        [ParameterName("bpcb")]        
        public string BypassClientBilling { get; set; }

        /// <summary>
        /// Gets or sets the cache key.
        /// </summary>
        /// <value>
        /// The cache key.
        /// </value>
        [ParameterName("ckey")] 
        public string CacheKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the client code.
        /// </summary>
        /// <value>
        /// The type of the client code.
        /// </value>
        [ParameterName("cct")]
        public string ClientCodeType { get; set; }

        /// <summary>
        /// Gets or sets the "Content Server Address" associated with the setting.
        /// </summary>
        [ParameterName("csa")]        
        public string ContentServerAddress
        {
            get
            {
                if (contentServerAddress.IsNullOrEmpty())
                {
                    contentServerAddress = DefaultContentServerAddress;
                }

                return contentServerAddress;
            }

            set
            {
                contentServerAddress = value;
            }
        }

        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the encrypted token.
        /// </summary>
        /// <value>
        /// The encrypted token.
        /// </value>
        [ParameterName("etoken")]  
        public string EncryptedToken { get; set; }

        /// <summary>
        /// Gets or sets "IP Address" associated with the setting.
        /// </summary>
        [ParameterName("ipa")]
        public string IpAddress
        {
            get
            {
                if (ipAddress.IsNullOrEmpty())
                {
                    ipAddress = DefaultIpAddress;
                }

                return ipAddress;
            }

            set
            {
                ipAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the product ID [i.e. namespace].
        /// </summary>
        /// <value>
        /// The product ID.
        /// </value>
        [ParameterName("ns")]                
        public string ProductID { get; set; }

        /// <summary>
        /// Gets or sets the proxy user id.
        /// </summary>
        /// <value>
        /// The proxy user id.
        /// </value>
        [ParameterName("puid")]        
        public string ProxyUserId { get; set; }

        /// <summary>
        /// Gets or sets the proxy product id.
        /// </summary>
        /// <value>
        /// The proxy product id.
        /// </value>
        [ParameterName("ppid")]
        public string ProxyProductId { get; set; }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        /// <value>
        /// The session ID.
        /// </value>
        [ParameterName("sid")]        
        public string SessionID { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        [ParameterName("uid")]                
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets User password associated with the setting.
        /// </summary>
        public string UserPassword { get; set; }
    }
}
