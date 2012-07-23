// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlData.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Session
{
    [DataContract]
    public enum CacheStatus
    {
        /// <summary>
        /// 'NotSpecified' means the value found is null or "" from backend
        /// </summary>
        [EnumMember]
        NotSpecified,
        /// <summary>
        /// 'NotSpecified' means the value found is null or "" from backend
        /// </summary>
        [EnumMember]
        Unknown,
        /// <summary>
        /// 'Hit' means item has been found in the cache
        /// </summary>
        [EnumMember]
        Hit,
        /// <summary>
        /// 'Miss' means items was not found in the cache.
        /// </summary>
        [EnumMember]
        Miss,
    }

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

        string AccountId { get; set; }

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

        string CacheExpirationPolicy { get; set; }
        string CacheExpirationTime { get; set; }
        string CacheRefreshInterval { get; set; }
        CacheStatus CacheStatus { get; set; }
        string CacheWait { get; set; }
        string CacheApplication { get; set; }
        string CacheScope { get; set; }
        string ForceCacheRefresh { get; set; }

        /// <summary>
        /// Gets or sets the type of the client code.
        /// </summary>
        /// <value>
        /// The type of the client code.
        /// </value>
        string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the client type.
        /// </summary>
        /// <value>
        /// The type of the client type.
        /// </value>
        string ClientType { get; set; }

        /// <summary>
        /// Gets or sets "Content Server Address" associated with the setting.
        /// </summary>
        ushort ContentServerAddress { get; set; }

        bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the encrypted token.
        /// </summary>
        /// <value>
        /// The encrypted token.
        /// </value>
        string EncryptedToken { get; set; }

        /// <summary>
        /// Gets or sets "IP Address" associated with the setting.
        /// </summary>
        string IpAddress { get; set; }

        IDictionary<string, string> Metrics { get; set; }

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

        bool IsValid();
    }
}