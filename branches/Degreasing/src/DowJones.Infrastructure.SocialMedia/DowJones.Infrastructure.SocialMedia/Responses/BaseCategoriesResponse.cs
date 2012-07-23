// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCategoriesResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The i base users response.
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
    /// The base categories response.
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{TotalCount}")]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseCategoriesResponse : AbstractSocialMediaResponse, IBaseResultsResponse, IBaseCategoriesResponse
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the categories.
        /// </summary>
        /// <value>
        ///   The categories.
        /// </value>
        [DataMember]
        public List<Category> Categories { get; set; }

        /// <summary>
        ///   Gets or sets the The number of categories per page.
        /// </summary>
        /// <value>
        ///   The lists per page.
        /// </value>
        [DataMember]
        [JsonProperty("rpp")]
        public int ResultsPerPage { get; set; }

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
        ///   Gets or sets the total count.
        /// </summary>
        /// <value>
        ///   The total count.
        /// </value>
        [DataMember]
        public int TotalCount { get; set; }

        #endregion
    }
}