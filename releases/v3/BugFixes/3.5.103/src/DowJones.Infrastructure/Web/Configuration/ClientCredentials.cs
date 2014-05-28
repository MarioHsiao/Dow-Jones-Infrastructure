using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Web.Configuration
{
    /// <summary>
    /// Client-Side Credentials for use in the browser
    /// </summary>
    public class ClientCredentials : ICredentials
    {
        /// <summary>The access point code.</summary>
        [JsonProperty("accessPointCode",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AccessPointCode { get; set; }

        /// <summary>The access point code usage.</summary>
        [JsonProperty("accessPointCodeUsage",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AccessPointCodeUsage { get; set; }

        /// <summary>The client type code.</summary>
        [JsonProperty("clientCode",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ClientCode { get; set; }

        /// <summary>The client type code.</summary>
        [JsonProperty("cacheKey",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CacheKey { get; set; }

        /// <summary>The client code.</summary>
        [JsonProperty("clientType",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ClientType { get; set; }

        /// <summary>The Proxy User ID</summary>
        [JsonProperty("proxyUserId",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ProxyUserId { get; set; }

        /// <summary>The Proxy User Namespace</summary>
        [JsonProperty("proxyUserNamespace",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ProxyUserNamespace { get; set; }

        /// <summary>
        /// Gets or sets the type of the credential.
        /// </summary>
        /// <value>
        /// The type of the credential.
        /// </value>
        [JsonProperty("credentialType",
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CredentialType CredentialType { get; set; }

        /// <summary>The remote address.</summary>
        [JsonProperty("remoteAddress",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RemoteAddress { get; set; }

        /// <summary>The Product ID from which the request originated</summary>
        [JsonProperty("SA_FROM",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SeamlessAccessFrom { get; set; }
        
        /// <summary>The user's current encrypted token.</summary>
        [JsonProperty("token",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Token { get; set; }


        public void SetProxyCredentials(string userId, string @namespace)
        {
            ProxyUserId = userId;
            ProxyUserNamespace = @namespace;
        }
    }
}