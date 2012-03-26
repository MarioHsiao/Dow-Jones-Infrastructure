// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tweet.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;     
using System.Diagnostics;                
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    

    /// <summary>
    /// </summary>
    [Serializable]
    [DataContract(Name = "tweet", Namespace = "")]
    [DebuggerDisplay("{Id}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Tweet : ITweet
    {
        #region Properties

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [DataMember(Name = "createDate")]
        [JsonProperty("created_at")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Favorited.
        /// </summary>
        [DataMember(Name = "favorited")]
        public bool Favorited { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember(Name = "id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        [DataMember(Name = "source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Truncated.
        /// </summary>
        [DataMember(Name="truncated")]
        public bool Truncated { get; set; }

        /// <summary>
        /// Gets or sets TwitterUser.
        /// </summary>
        [DataMember(Name="user")]
        public TwitterUser User { get; set; }

        #endregion
    }
}