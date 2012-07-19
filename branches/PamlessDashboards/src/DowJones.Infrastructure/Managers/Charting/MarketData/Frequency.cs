using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Managers.Charting.MarketData
{
    [DataContract(Namespace = "")]
    public enum SymbolType
    {
        [EnumMember]
        FCode,
        [EnumMember]
        Ticker,
        [EnumMember]
        Cusip,
        [EnumMember]
        Sedol,
        [EnumMember]
        Isin,
    }

    [DataContract(Namespace = "")]
    public enum Frequency
    {
        [XmlEnum("1mi")]
        [EnumMember]
        OneMinute = 1,
        [XmlEnum("5mi")]
        [EnumMember]
        FiveMinutes = 5,
        /// <summary>
        /// fifteen minutes.
        /// </summary>
        [XmlEnum("15mi")]
        [EnumMember]
        FifteenMinutes = 15,
        [XmlEnum("1hr")]
        [EnumMember]
        OneHour = 4,
        /*[XmlEnum("1mo")]
        OneMonth = 5,
        [XmlEnum("3mo")]
        ThreeMonths = 6,
        [XmlEnum("1yr")]
        OneYear = 7,*/
    }
}