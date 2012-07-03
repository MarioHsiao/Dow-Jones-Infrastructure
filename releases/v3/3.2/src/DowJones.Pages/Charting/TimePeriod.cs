using System.Xml.Serialization;

namespace DowJones.Pages.Charting
{
    public enum TimePeriod
    {
        /// <summary>
        /// One day time period.
        /// </summary>
        [XmlEnum("1dy")]
        OneDay,
    }
}