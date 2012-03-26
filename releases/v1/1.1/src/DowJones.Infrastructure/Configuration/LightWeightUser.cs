using System;
using System.Xml.Serialization;

namespace DowJones.Utilities.Configuration
{
    /// <summary>
    /// Summary description for LightWeightUserSettings.
    /// </summary>
    [XmlType(Namespace = "")]
    [XmlRoot("lightWeightUser", Namespace = "", IsNullable = false)]
    [Serializable]
    public class LightWeightUser
    {
        private const string DEFAULT_CONTENT_SERVER_ADDRESS = "0";
        private const string DEFAULT_IP_ADDRESS = "127.0.0.1";

        /// <summary>
        /// Gets or sets the user id associated with the setting.
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// User password associated with the setting.
        /// </summary>
        public string userPassword { get; set; }

        /// <summary>
        /// Product Id associated with the setting.
        /// </summary>
        public string productId { get; set; }

        /// <summary>
        /// Client code type associated with the setting.
        /// </summary>
        public string clientCodeType { get; set; }

        /// <summary>
        /// Access point code associated with the setting.
        /// </summary>
        public string accessPointCode { get; set; } 

        /// <summary>
        /// Access point code associated with the setting.
        /// </summary>
        public string accessPointCodeUsage { get; set; }

        /// <summary>
        /// Content server address associated with the setting.
        /// </summary>
        public string contentServerAddress = DEFAULT_CONTENT_SERVER_ADDRESS;

        /// <summary>
        /// IP address associated with the setting.
        /// </summary>
        public string ipAddress = DEFAULT_IP_ADDRESS;

        /// <summary>
        /// ByPass Client billing flag associated with the setting.
        /// </summary>
        public bool byPassClientBilling;
    }
}