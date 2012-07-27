using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;
using DowJones.Infrastructure.Common;
using DowJones.Preferences;
using DowJones.Session;
using Newtonsoft.Json;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.UI
{
    [JsonObject("globalHeaders")]
    public class GlobalHeaders
    {
        [JsonProperty("credentials")]
        public ClientCredentials Credentials { get; set; }

        [JsonProperty("preferences")]
        public ClientPreferences Preferences { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }


        public GlobalHeaders(IControlData controlData, IUserSession session, IPreferences preferences, Product product)
        {
            Credentials = new ClientCredentials
                              { 
                                  AccessPointCode = controlData.AccessPointCode,
                                  AccessPointCodeUsage = controlData.AccessPointCodeUsage,
                                  ClientCode = controlData.ClientCode,
                                  CredentialType = controlData.EncryptedToken.IsNullOrEmpty() ? ClientCredentialTokenType.SessionId : ClientCredentialTokenType.EncryptedToken,
                                  SeamlessAccessFrom = session.ProductPrefix,
                                  ProxyUserId = controlData.ProxyUserId,
                                  ProxyUserNamespace = controlData.ProxyProductId,
                                  RemoteAddress = controlData.IpAddress,
                                  Token = controlData.EncryptedToken ?? controlData.SessionID,
                              };

            Preferences = new ClientPreferences
                              {
                                  ClockType = preferences.ClockType,
                                  ContentLanguages = preferences.ContentLanguages,
                                  InterfaceLanguage = preferences.InterfaceLanguage,
                                  TimeZone = preferences.TimeZone
                              };
            
            ProductId = product.Id;
        }

        // Convenience pass-through to Credentials.SetProxyCredentials
        public void SetProxyCredentials(string userId, string @namespace)
        {
            if (Credentials != null)
                Credentials.SetProxyCredentials(userId, @namespace);
        }
    }

    
    /// <summary>
    /// ClientCredentials class
    /// </summary>
     /// <example>
    /// ClientCredentials Serialized to XML:
    /// <code>
    /// <![CDATA[
    ///     <credentials>
    ///         <accessPointCode>NP</accessPointCode>
    ///         <accessPointCodeUsage>NP</accessPointCodeUsage>
    ///         <remoteAddress>172.25.247.244</remoteAddress>
    ///         <token>27137ZzZINAUQT2CAAAGUAIAAAAAAYU2AAAAAABSGAYTCMBXGEZTCMBSGIZTCMJZ</token>
    ///         <tokenType>sessionId</tokenType>
    ///    </credentials>
    /// ]]>
    /// </code>
    /// </example>
    public class ClientCredentials {

        /// <summary>Gets or sets the access point code.</summary>
        /// <value>The access point code.</value>
        [JsonProperty("accessPointCode")]
        public string AccessPointCode { get; set; }

        /// <summary>Gets or sets the access point code usage.</summary>
        /// <value>The access point code usage.</value>
        [JsonProperty("accessPointCodeUsage")]
        public string AccessPointCodeUsage { get; set; }

        /// <summary>Gets or sets the type of the token.</summary>
        /// <value>The type of the token.</value>
        [JsonProperty("clientCode")]
        public string ClientCode { get; set; }

        /// <summary>Gets or sets the type of the token.</summary>
        /// <value>The type of the token.</value>
        [JsonProperty("credentialType")]
        public ClientCredentialTokenType CredentialType { get; set; }

        [JsonProperty("proxyUserId", NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyUserId { get; set; }

        [JsonProperty("proxyUserNamespace", NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyUserNamespace { get; set; }

        /// <summary>Gets or sets the remote address.</summary>
        /// <value>The remote address.</value>
        [JsonProperty("remoteAddress")]
        public string RemoteAddress { get; set; }

        /// <summary>Gets or sets the remote address.</summary>
        /// <value>The remote address.</value>
        [JsonProperty("SA_FROM", NullValueHandling = NullValueHandling.Ignore)]
        public string SeamlessAccessFrom { get; set; }

        /// <summary>Gets or sets the token.</summary>
        /// <value>The token.</value>
        [JsonProperty("token")]
        public string Token { get; set; }


        public void SetProxyCredentials(string userId, string @namespace)
        {
            ProxyUserId = userId;
            ProxyUserNamespace = @namespace;
        }
    }



    /// <summary>
    /// ClientPreferences class
    /// </summary>
    /// <example>
    /// ClientPreferences Serialized to XML:
    /// <code>
    /// <![CDATA[
    ///     <preferences>
    ///         <clockType>TwentyFourHours</clockType>
    ///         <contentLanguages><contentLanguage>en</contentLanguage></contentLanguages>
    ///         <interfaceLanguage>en</interfaceLanguage>
    ///         <timeZone>on, -05:00|1, on</timeZone>
    ///    </preferences>
    /// ]]>
    /// </code>
    /// </example>
    public class ClientPreferences {
        /// <summary>Gets or sets the type of the clock.</summary>
        /// <value>The type of the clock.</value>
        [JsonProperty("clockType")]
        public ClockType ClockType { get; set; }

        /// <summary>Gets or sets the content languages.</summary>
        /// <value>The content languages.</value>
        [JsonProperty("contentLanguages")]
        public ContentLanguageCollection ContentLanguages { get; set; }

        /// <summary>Gets or sets the interface language.</summary>
        /// <value>The interface language.</value>
        [JsonProperty("interfaceLanguage")]
        public string InterfaceLanguage { get; set; }

        /// <summary>Gets or sets the time zone.</summary>
        /// <value>The time zone.</value>
        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
}



   
}
