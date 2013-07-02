using System;
using System.Xml.Serialization;

namespace DowJones.Prod.X.Models.Search
{
    [Serializable]
    public enum DeduplicationMode
    {
        [XmlEnum(Name = "Off")]
        Off,
        [XmlEnum(Name = "Similar")]
        Similar,
        [XmlEnum(Name = "NearExact")]
        NearExact,
    }
}
