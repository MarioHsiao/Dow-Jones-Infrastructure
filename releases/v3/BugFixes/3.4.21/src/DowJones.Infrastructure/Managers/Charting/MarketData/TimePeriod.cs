using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Managers.Charting.MarketData
{
    [DataContract(Namespace = "")]
    public enum TimePeriod
    {
        [XmlEnum("1hr")]
        [EnumMember]
        OneHour = 1,
        /// <summary>
        /// One day time period.
        /// </summary>
        [XmlEnum("1dy")]
        [EnumMember]
        OneDay = 24,
        [XmlEnum("2dy")]
        [EnumMember]
        TwoDays = 48,
        [XmlEnum("5dy")]
        [EnumMember]
        FiveDays = 120,
        [XmlEnum("10dy")]
        [EnumMember]
        TenDays = 240,
        /*  [XmlEnum("1mo")]
          OneMonth = 5,
         [XmlEnum("3mo")]
         ThreeMonths = 6,
         [XmlEnum("6mo")]
         SixMonths = 7,
         [XmlEnum("1yr")]
         OneYear = 8,
         [XmlEnum("2yr")]
         TwoYears = 9,
         [XmlEnum("3yr")]
         ThreeYears = 10,
         [XmlEnum("4yr")]
         FourYears = 11,
         [XmlEnum("5yr")]
         FiveYears = 12,
         [XmlEnum("10yr")]
         TenYears = 13,
         [XmlEnum("Ytd")]
         YearToDate = 14,
         [XmlEnum("All")]
         All = 15*/
    }
}