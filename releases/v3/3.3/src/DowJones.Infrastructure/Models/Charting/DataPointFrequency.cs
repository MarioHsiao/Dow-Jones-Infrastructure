using System.Xml.Serialization;

namespace DowJones.Models.Charting
{
    public enum DataPointFrequency
    {
        [XmlEnum(Name = "D")]
        Daily,
        [XmlEnum(Name = "W")]
        Weekly,
        [XmlEnum(Name = "M")]
        Monthly,
        [XmlEnum(Name = "Q")]
        Quarterly,
        [XmlEnum(Name = "Y")]
        Yearly
    }
}
