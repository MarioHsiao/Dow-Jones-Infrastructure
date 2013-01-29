using System.Runtime.Serialization;
using DowJones.Attributes;
using DowJones.DTO.Web.LOB;
using DowJones.Managers.Core;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.DTO.Web.Request
{
    [DataContract(Name = "sessionRequestDTO", Namespace = "")]
    public class SessionRequestDTO : AbstractSessionRequestDTO
    {
        public SessionRequestDTO()
        {
            InterfaceLanguage = "en";
            ProductPrefix = "GL";
        }

        /// <summary>
        /// Gets or sets Access Point Code
        /// </summary>
        /// <value>
        /// The access point code.
        /// </value>
        [ParameterName("napc")]
        [DataMember(Name = "accessPointCode")]
        public string AccessPointCode { get; set; }

        /// <summary>
        /// Gets or sets Interface Language
        /// </summary>
        /// <value>
        /// The interface language.
        /// </value>
        [ParameterName("stil")]
        [DataMember(Name = "interfaceLanguage")]
        public string InterfaceLanguage { get; set; }

        /// <summary>
        /// Gets or sets Product Prefix
        /// </summary>
        /// <value>
        /// The product prefix.
        /// </value>
        [ParameterName("SA_FROM")]
        [DataMember(Name = "productPrefix")]
        public string ProductPrefix { get; set; }

        /// <summary>
        /// Gets or sets Debug Level
        /// </summary>
        /// <value>
        /// The debug level.
        /// </value>
        [ParameterName("dbg")]
        [DataMember(Name = "debugLevel")]
        public int DebugLevel { get; set; }

        /// <summary>
        /// Gets or sets the access point code for usage.
        /// </summary>
        /// <value>
        /// The access point code usage.
        /// </value>
        [ParameterName("apcu")]
        [DataMember(Name = "accessPointCodeUsage")]
        public string AccessPointCodeUsage { get; set; }

        /// <summary>
        /// Gets or sets the type of the client code.
        /// </summary>
        /// <value>
        /// The type of the client code.
        /// </value>
        [ParameterName("cct")]
        [DataMember(Name = "clientCodeType")]
        public string ClientCodeType { get; set; }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        /// <value>
        /// The session ID.
        /// </value>
        [ParameterName("sid")]
        [DataMember(Name = "sessionID")]
        public string SessionID { get; set; }

        /// <summary>
        /// Gets or sets the product ID [i.e. namespace].
        /// </summary>
        /// <value>
        /// The product ID.
        /// </value>
        [ParameterName("ns")]
        [DataMember(Name = "productID")]
        public string ProductID { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        [ParameterName("uid")]
        [DataMember(Name = "userID")]
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the encrypted token.
        /// </summary>
        /// <value>
        /// The encrypted token.
        /// </value>
        [ParameterName("etoken")]
        [DataMember(Name = "encryptedToken")]
        public string EncryptedToken { get; set; }

        /// <summary>
        /// Gets or sets the cache key.
        /// </summary>
        /// <value>
        /// The cache key.
        /// </value>
        [ParameterName("ckey")]
        [DataMember(Name = "cacheKey")]
        public string CacheKey { get; set; }


        [ParameterName("pwd")]
        [DataMember(Name = "password")]
        public string Password { get; set; }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(AccessPointCode);
        }


        public Preferences.Preferences GetBasePreferences()
        {
            return new Preferences.Preferences(InterfaceLanguage);
        }

        public ControlData GetControlData()
        {
            var temp = new ControlData
            {
                UserID = UserID,
                AccessPointCode = AccessPointCode,
                AccessPointCodeUsage = AccessPointCodeUsage,
                CacheKey = CacheKey,
                ClientCode = ClientCodeType,
                EncryptedLogin = EncryptedToken,
                ProductID = ProductID,
                SessionID = SessionID,
                UserPassword = Password
            };
            return temp;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public override void Load()
        {
            if (StringUtilitiesManager.IsValid(InterfaceLanguage))
            {
                InterfaceLanguage = InterfaceLanguage.ToLower();
            }

            if (!StringUtilitiesManager.IsValid(ProductPrefix))
            {
                return;
            }

            switch (ProductPrefix.ToUpper())
            {
                case "GL":
                case "FC":
                case "IF":
                case "IN":
                    ProductPrefix = ProductPrefix.ToUpper();
                    break;
                default:
                    ProductPrefix = "GL";
                    break;

            }
        }
    }
}