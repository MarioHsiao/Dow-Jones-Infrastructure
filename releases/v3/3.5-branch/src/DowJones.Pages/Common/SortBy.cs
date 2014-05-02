using System;
using System.Xml.Serialization;

namespace DowJones.Pages.Common
{
    [Serializable]
    public enum SortBy
    {
        [XmlEnum(Name = "Position")]
        Position,
        [XmlEnum(Name = "Name")]
        Name,
        [XmlEnum(Name = "LastModifiedDate")]
        LastModifiedDate,
        [XmlEnum(Name = "CreatedDate")]
        CreatedDate,
    }
}