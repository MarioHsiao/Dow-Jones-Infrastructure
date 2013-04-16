// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryEntity.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace DowJones.Pages
{
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
}