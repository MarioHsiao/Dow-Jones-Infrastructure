using System.Xml.Serialization;

namespace DowJones.Managers.Charting.MarketData
{
    public enum Frequency
    {
        [XmlEnum("1mi")]
        OneMinute = 1,
        [XmlEnum("5mi")]
        FiveMinutes = 5,
        /// <summary>
        /// fifteen minutes.
        /// </summary>
        [XmlEnum("15mi")]
        FifteenMinutes = 15,
        [XmlEnum("1hr")]
        OneHour = 4,
        /*[XmlEnum("1mo")]
        OneMonth = 5,
        [XmlEnum("3mo")]
        ThreeMonths = 6,
        [XmlEnum("1yr")]
        OneYear = 7,*/
    }
}