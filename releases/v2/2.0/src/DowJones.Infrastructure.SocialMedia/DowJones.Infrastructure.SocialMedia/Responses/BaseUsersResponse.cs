﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseUsersResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The i users response.
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
    /// The base users response.
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{TotalCount}")]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseUsersResponse : AbstractSocialMediaResponse, IBaseResultsResponse, IUsersResponse
    {
        #region Properties

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
        ///   Gets or sets the total number of users matching.
        /// </summary>
        /// <value>
        ///   The total count.
        /// </value>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        ///   Gets or sets Lists.
        /// </summary>
        [DataMember]
        public List<TwitterUser> Users { get; set; }

        /// <summary>
        ///   Gets or sets the The number of users per page.
        /// </summary>
        /// <value>
        ///   The users per page.
        /// </value>
        [DataMember]
        [JsonProperty("rpp")]
        public int ResultsPerPage { get; set; }

        #endregion
    }
}