// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformCategoriesSearchResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Defines the PerformCategoriesSearchResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.SocialMedia.Responses
{
    /// <summary>
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class PerformCategoriesSearchResponse : AbstractSocialMediaResponse
    {
        /// <summary>
        ///   Gets or sets the The number of lists per page.
        /// </summary>
        /// <value>
        ///   The lists per page.
        /// </value>
        [JsonProperty("rpp")]
        [DataMember]
        public int CategoriesPerPage { get; set; }

        /// <summary>
        ///   Gets or sets the current page number.
        /// </summary>
        /// <value>
        ///   The page.
        /// </value>
        [DataMember]
        public int Page { get; set; }

        /// <summary>
        ///   Gets or sets the total number of pages.
        /// </summary>
        /// <value>
        ///   The page count.
        /// </value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        ///   Gets or sets the total number of lists matching.
        /// </summary>
        /// <value>
        ///   The total count.
        /// </value>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets Categories.
        /// </summary>
        [DataMember]
        public List<Category> Categories { get; set; }
    }
}
