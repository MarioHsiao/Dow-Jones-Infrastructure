using System.Xml.Serialization;

namespace DowJones.Prod.X.Models.Search
{
    public enum DeduplicationLevel
    {
        [XmlEnum("None")]
        None,
        [XmlEnum("NearExact")]
        NearExact,
        [XmlEnum("Similar")]
        Similar,
    }
}
