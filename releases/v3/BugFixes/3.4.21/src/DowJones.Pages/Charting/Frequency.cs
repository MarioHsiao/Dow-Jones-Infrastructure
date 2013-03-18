using System.Xml.Serialization;

namespace DowJones.Pages.Charting
{
    public enum Frequency
    {
        /// <summary>
        /// fifteen minutes.
        /// </summary>
        [XmlEnum("15mi")]
        FifteenMinutes,
    }
}