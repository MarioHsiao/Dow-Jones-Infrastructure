using System;
using System.Xml.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// The result sort order.
    /// </summary>
    [Serializable]
    public enum ResultSortOrder
    {
        /// <summary>
        /// The unspecified.
        /// </summary>
        [XmlEnum(Name = "Unspecified")] Unspecified, 

        /// <summary>
        /// The publication date chronological.
        /// </summary>
        [XmlEnum(Name = "PublicationDateChronological")] PublicationDateChronological, 

        /// <summary>
        /// The publication date reverse chronological.
        /// </summary>
        [XmlEnum(Name = "PublicationDateReverseChronological")] PublicationDateReverseChronological, 

        /// <summary>
        /// The relevance.
        /// </summary>
        [XmlEnum(Name = "Relevance")] Relevance, 

        /// <summary>
        /// The relevance medium freshness.
        /// </summary>
        [XmlEnum(Name = "RelevanceMediumFreshness")] RelevanceMediumFreshness, 

        /// <summary>
        /// The relevance high freshness.
        /// </summary>
        [XmlEnum(Name = "RelevanceHighFreshness")] RelevanceHighFreshness, 

        /// <summary>
        /// The arrival time.
        /// </summary>
        [XmlEnum(Name = "ArrivalTime")] ArrivalTime, 

        /// <summary>
        /// The freshness date chronological.
        /// </summary>
        [XmlEnum(Name = "FreshnessDateChronological")] FreshnessDateChronological, 

        /// <summary>
        /// The freshness date reverse chronological.
        /// </summary>
        [XmlEnum(Name = "FreshnessDateReverseChronological")] FreshnessDateReverseChronological, 

        /// <summary>
        /// The "FIFO" de-duplication type.
        /// </summary>
        [XmlEnum(Name = "FIFO")] FIFO, 

        /// <summary>
        /// The "LIFO" de-duplication type.
        /// </summary>
        [XmlEnum(Name = "LIFO")] LIFO, 
    }
}