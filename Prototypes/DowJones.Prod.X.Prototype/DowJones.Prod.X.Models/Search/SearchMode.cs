using System;
using System.Xml.Serialization;

namespace DowJones.Prod.X.Models.Search
{
    [Serializable]
    public enum SearchMode
    {
        [XmlEnum(Name = "all")]
        All,
        [XmlEnum(Name = "any")]
        Any,
        [XmlEnum(Name = "none")]
        None,
        [XmlEnum(Name = "phrase")]
        Phrase,
        [XmlEnum(Name = "simple")]
        Simple,
        [XmlEnum(Name = "advanced")]
        Advanced,
        [XmlEnum(Name = "traditional")]
        Traditional,
        [XmlEnum(Name = "unified")]
        Unified,
    }
}
