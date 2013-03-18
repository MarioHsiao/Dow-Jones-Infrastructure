// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySuperList.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Defines the CategorySuperList type.
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
    [DataContract]
    [DebuggerDisplay("{Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class CategorySuperList : TwitterList
    {
        /// <summary>
        /// Gets or sets the category word cloud.
        /// </summary>
        /// <value>
        /// The category word cloud.
        /// </value>
        [JsonProperty("category_wordcloud")]
        [DataMember]
        public WordCloud CategoryWordCloud { get; set; }

        /// <summary>
        /// Gets or sets the related words.
        /// </summary>
        /// <value>
        /// The related words.
        /// </value>
        [DataMember]
        public StringCollection RelatedWords { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        [DataMember]
        public StringCollection Categories { get; set; }

        /// <summary>
        /// Gets or sets the sponsored users.
        /// </summary>
        /// <value>
        /// The sponsored users.
        /// </value>
        [DataMember]
        public SponsoredUsersCollection SponsoredUsers { get; set; }

    }

    /// <summary>
    /// </summary>
    public class StringCollection : System.Collections.Generic.List<string>
    {
    }

    /// <summary>
    /// </summary>
    public class SponsoredUsersCollection : System.Collections.Generic.List<TwitterUser>
    {
    }

}
