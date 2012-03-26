// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryEntity.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Common
{
    /// <summary>
    /// The search mode.
    /// </summary>
    [Serializable]
    public enum SearchMode
    {
        /// <summary>
        /// The simple.
        /// </summary>
        [XmlEnum(Name = "Simple")] Simple, 

        /// <summary>
        /// The traditional.
        /// </summary>
        [XmlEnum(Name = "Traditional")] Traditional, 
    }

    /// <summary>
    /// The de-duplication type.
    /// </summary>
    [Serializable]
    public enum DeduplicationType
    {
        /// <summary>
        /// The not applicable de-duplication type.
        /// </summary>
        [XmlEnum(Name = "NotApplicable")] NotApplicable, 

        /// <summary>
        /// The off de-duplication type.
        /// </summary>
        [XmlEnum(Name = "Off")] Off, 

        /// <summary>
        /// The virtually identical de-duplication type.
        /// </summary>
        [XmlEnum(Name = "VirtuallyIdentical")] VirtuallyIdentical, 

        /// <summary>
        /// The similar de-duplication type.
        /// </summary>
        [XmlEnum(Name = "Similar")] Similar, 
    }

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

    /// <summary>
    /// The query collection.
    /// </summary>
    [Serializable]
    public class QueryCollection : List<Query>
    {
    }

    /// <summary>
    /// The query entity.
    /// </summary>
    [DataContract(Name = "queryEntity", Namespace = "")]
    public class QueryEntity
    {
        private Query query;

        /// <summary>
        /// Gets or sets ResultSortOrder.
        /// </summary>
        [DataMember(Name = "resultSortOrder")]
        public ResultSortOrder ResultSortOrder { get; set; }

        /// <summary>
        /// Gets or sets DeduplicationType.
        /// </summary>
        [DataMember(Name = "deduplicationType")]
        public DeduplicationType DeduplicationType { get; set; }

        [DataMember(Name = "query")]
        public Query Query
        {
            get 
            {
                return query ?? (query = new Query());
            }

            set
            {
                query = value;
            }
        }
    }

    /// <summary>
    /// The query.
    /// </summary>
    [DataContract(Name = "query", Namespace = "")]
    public class Query
    {
        /// <summary>
        /// Gets or sets SearchMode.
        /// </summary>
        [DataMember(Name = "searchMode")]
        public SearchMode SearchMode { get; set; }

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}