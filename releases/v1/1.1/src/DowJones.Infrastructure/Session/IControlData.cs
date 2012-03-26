// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlData.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Session
{
    public interface IControlData
    {
        /// <summary>
        /// Gets or sets the access point code.
        /// </summary>
        /// <value>
        /// The access point code.
        /// </value>
        string AccessPointCode { get; set; }

        /// <summary>
        /// Gets or sets the access point code for usage.
        /// </summary>
        /// <value>
        /// The access point code usage.
        /// </value>
        string AccessPointCodeUsage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [Bypass Client Billing].
        /// </summary>
        /// <value>
        /// <c>true</c> if [Bypass Client Billing]; otherwise, <c>false</c>.
        /// </value>
        string BypassClientBilling { get; set; }

        /// <summary>
        /// Gets or sets the cache key.
        /// </summary>
        /// <value>
        /// The cache key.
        /// </value>
        string CacheKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the client code.
        /// </summary>
        /// <value>
        /// The type of the client code.
        /// </value>
        string ClientCodeType { get; set; }

        /// <summary>
        /// Gets or sets "Content Server Address" associated with the setting.
        /// </summary>
        string ContentServerAddress { get; set; }

        bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the encrypted token.
        /// </summary>
        /// <value>
        /// The encrypted token.
        /// </value>
        string EncryptedToken { get; set; }

        // = DEFAULT_CONTENT_SERVER_ADDRESS;

        /// <summary>
        /// Gets or sets "IP Address" associated with the setting.
        /// </summary>
        string IpAddress { get; set; } // = DEFAULT_IP_ADDRESS;

        /// <summary>
        /// Gets or sets the product ID [i.e. namespace].
        /// </summary>
        /// <value>
        /// The product ID.
        /// </value>
        string ProductID { get; set; }

        string ProxyUserId { get; set; }

        string ProxyProductId { get; set; }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        /// <value>
        /// The session ID.
        /// </value>
        string SessionID { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        string UserID { get; set; }

        /// <summary>
        /// Gets or sets User password associated with the setting.
        /// </summary>
        string UserPassword { get; set; }
    }
}