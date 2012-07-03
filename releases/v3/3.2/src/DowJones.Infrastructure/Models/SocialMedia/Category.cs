// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Category.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    /// <summary>
    /// The category.
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{Id}: {Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Category
    {
        #region Properties

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        [DataMember]
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets Path.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets RetirementMessage.
        /// </summary>
        [DataMember]
        public string RetirementMessage { get; set; }

        /// <summary>
        /// Gets or sets Slug.
        /// </summary>
        [DataMember]
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets SuperlistName.
        /// </summary>
        [DataMember]
        public string SuperlistName { get; set; }

        /// <summary>
        /// Gets or sets SuperlistName.
        /// </summary>
        [DataMember]
        public int ListCount { get; set; }

        #endregion
    }
}