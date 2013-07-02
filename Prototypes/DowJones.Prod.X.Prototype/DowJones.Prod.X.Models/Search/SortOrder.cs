using System;
using System.Xml.Serialization;

namespace DowJones.Prod.X.Models.Search
{
    [Serializable]
    public enum SortOrder
    {
        [XmlEnum(Name = "PublicationDateChronological")]
        PublicationDateChronological,
        [XmlEnum(Name = "PublicationDateReverseChronological")]
        PublicationDateReverseChronological,
        [XmlEnum(Name = "Relevance")]
        Relevance,
        [XmlEnum(Name = "RelevanceMediumFreshness")]
        RelevanceMediumFreshness,
        [XmlEnum(Name = "RelevanceHighFreshness")]
        RelevanceHighFreshness,
        [XmlEnum(Name = "ArrivalTime")]
        ArrivalTime,
        [XmlEnum(Name = "FreshnessDateChronological")]
        FreshnessDateChronological,
        [XmlEnum(Name = "FreshnessDateReverseChronological")]
        FreshnessDateReverseChronological,
        [XmlEnum(Name = "FIFO")]
        FIFO,
        [XmlEnum(Name = "LIFO")]
        LIFO,
        [XmlEnum(Name = "BestDateChronological")]
        BestDateChronological,
        [XmlEnum(Name = "BestDateReverseChronological")]
        BestDateReverseChronological,
    }
}
