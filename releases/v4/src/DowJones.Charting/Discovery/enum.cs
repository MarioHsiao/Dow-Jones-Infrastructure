using System.Xml.Serialization;

namespace DowJones.Charting.Discovery
{
    public enum DiscoveryChartType
    {
        [XmlEnum("companies")]
        Companies,
        [XmlEnum("newsSubjects")]
        NewsSubjects,
        [XmlEnum("sources")]
        Sources,
        [XmlEnum("industries")]
        Industries,
        [XmlEnum("executives")]
        Executives,
        [XmlEnum("authors")]
        Authors,
    }
}
