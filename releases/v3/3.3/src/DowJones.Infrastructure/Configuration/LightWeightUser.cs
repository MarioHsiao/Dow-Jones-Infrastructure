using System;
using System.Xml.Serialization;
using DowJones.Extensions;

namespace DowJones.Configuration
{
    /// <summary>
    /// Summary description for LightWeightUserSettings.
    /// </summary>
    [XmlType(Namespace = "")]
    [XmlRoot("lightWeightUser", Namespace = "", IsNullable = false)]
    [Serializable]
    public class LightWeightUser
    {
        private const string DefaultContentServerAddress = "0";

        private const string DefaultIpAddress = "127.0.0.1";

        private string _ipAddress = DefaultIpAddress;
        private string _contentServerAddress = DefaultContentServerAddress;

        /// <summary>
        /// Gets or sets the user id associated with the setting.
        /// </summary>
        [XmlElement("userId", Namespace = "")]
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
        public string contentServerAddress
        {
            get
            {
                return _contentServerAddress;
            }

            set { 
                if (value.IsNullOrEmpty())
                {
                    return;
                }
                _contentServerAddress = value;
            }
        }


        /// <summary>
        /// IP address associated with the setting.
        /// </summary>
        public string ipAddress {
            get
            {
                return _ipAddress;
            }

            set { 
                if (value.IsNullOrEmpty())
                {
                    return;
                }
                _ipAddress = value;
            }
        }



        /// <summary>
        /// ByPass Client billing flag associated with the setting.
        /// </summary>
        public bool byPassClientBilling { get; set; }
    }
}