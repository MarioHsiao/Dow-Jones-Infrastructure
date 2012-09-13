using System;
using System.Xml.Serialization;

namespace DowJones.Pages.Common
{
    [Serializable]
    public enum SortOrder
    {
        [XmlEnum(Name = "Ascending")]
        Ascending,
        [XmlEnum(Name = "Descending")]
        Descending,
    }
}