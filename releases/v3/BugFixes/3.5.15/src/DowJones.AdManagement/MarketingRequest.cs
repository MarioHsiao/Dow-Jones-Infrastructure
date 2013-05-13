// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketingRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using DowJones.Mapping.IpAddress;

namespace DowJones.AdManagement
{
    #region Public Enum's

    /// <summary>
    /// The product sub type.
    /// </summary>
    public enum ProductSubType
    {
        /// <summary>
        /// The global productid = GL.
        /// </summary>
        Global,

        /// <summary>
        /// The salesworks productid = FC.
        /// </summary>
        Salesworks,

        /// <summary>
        /// The fce productid = FC.
        /// </summary> 
        Fce,

        /// <summary>
        /// The preview productid = IF.
        /// </summary>
        Preview,

        /// <summary>
        /// The iworks productid = IF.
        /// </summary>
        Iworks,

        /// <summary>
        /// The reader productid = IF.
        /// </summary>
        Reader,

        /// <summary>
        /// The customer productid = CP.
        /// </summary>
        Customer,

        /// <summary>
        /// The insight productid = IN.
        /// </summary>
        Insight,

        /// <summary>
        /// The radar productid = IF.
        /// </summary>
        Radar,

        /// <summary>
        /// The financial services.
        /// </summary>
        FinancialServices, 

        /// <summary>
        /// The Wall Street Journal Professional.
        /// </summary>
        WSJE
    }

    /// <summary>
    /// The image size.
    /// </summary>
    public enum ImageSize
    {
        /// <summary>
        /// Mini = 20H * 250W
        /// </summary>
        Mini, 

        /// <summary>
        /// Small = 120H * 300W
        /// </summary>
        Small, 

        /// <summary>
        /// Larget = 80H * 700W
        /// </summary>
        Large, 

        /// <summary>
        /// Custom = ????
        /// </summary>
        Custom, 

        /// <summary>
        /// SmallVertical = 250H * 135W 
        /// </summary>
        SmallVertical, 

        /// <summary>
        /// SmallLowHorizontal = 80H * 360W   
        /// </summary>
        SmallLowHorizontal, 
    }

    /// <summary>
    /// The elements to return.
    /// </summary>
    public enum ElementsToReturn
    {
        /// <summary>
        /// To Return only ad container, new for project visibility.
        /// </summary>
        Ad,                     
        
        /// <summary>
        /// To Return image and whats new for login page, default value.
        /// </summary>
        Image, 

        /// <summary>
        /// To Return image, whats new and ad container.
        /// </summary>
        All 
    }

    /// <summary>
    /// The response type.
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// The ad response type.
        /// </summary>
        Ad, 

        /// <summary>
        /// The image response type.
        /// </summary>
        Image, 

        /// <summary>
        /// The text response type.
        /// </summary>
        Text, 

        /// <summary>
        /// Returns everything for project visibility – Dump tables!.
        /// </summary>
        All    
    }

    #endregion

    /// <summary>
    /// The marketing request.
    /// </summary>
    [XmlRoot("MarketingRequest", Namespace = "", IsNullable = false)]
    public class MarketingRequest
    {
        #region private class variables

        /// <summary>
        /// The _interface language.
        /// </summary>
        private string _interfaceLanguage = "en";

        /// <summary>
        /// The _source country code.
        /// </summary>
        private string _sourceCountryCode = "us";

        /// <summary>
        /// The _source ip address.
        /// </summary>
        private string _sourceIpAddress;

        #endregion

        /// <summary>
        /// Gets or sets ProductID.
        /// </summary>
        [XmlElement("ProductID")]
        public string ProductID { get; set; }

        /// <summary>
        /// Gets or sets ProductSubType.
        /// </summary>
        [XmlElement("ProductSubType")]
        public ProductSubType ProductSubType { get; set; }

        /// <summary>
        /// Gets or sets GroupType.
        /// </summary>
        [XmlElement("GroupType")]
        public string GroupType { get; set; }

        /// <summary>
        /// Gets or sets InterfaceLanguage.
        /// </summary>
        [XmlElement("InterfaceLanguage")]
        public string InterfaceLanguage
        {
            get { return _interfaceLanguage; }
            set { _interfaceLanguage = value; }
        }

        /// <summary>
        /// Gets or sets Size.
        /// </summary>
        [XmlElement("Size")]
        public ImageSize Size { get; set; }

        /// <summary>
        /// Gets or sets ElementsToReturn.
        /// </summary>
        [XmlElement("ElementsToReturn")]
        public ElementsToReturn ElementsToReturn { get; set; }

        /// <summary>
        /// Gets or sets ResponseType.
        /// </summary>
        [XmlElement("ResponseType")]
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// Gets or sets SourceIpAddress.
        /// </summary>
        /// <remarks>
        /// Added new user Ip address element for Q2-2009-regional ads
        /// </remarks>
        [XmlElement("SourceIpAddress")]
        public string SourceIpAddress
        {
            get
            {
                return _sourceIpAddress;
            }

            set
            {
                _sourceIpAddress = value;

                var ipAddressMapper = new IpAddressMapper();
                _sourceCountryCode = ipAddressMapper.GetCountryCode(_sourceIpAddress);
            }
        }

        /// <summary>
        /// Gets SourceCountryCode.
        /// </summary>
        /// <remarks>
        /// Added new Country code element for Q2-2009-regional ads
        /// </remarks>
        [XmlElement("SourceCountryCode")]
        public string SourceCountryCode
        {
            get { return _sourceCountryCode; }
        }
    }
}