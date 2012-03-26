// --------------------------------------------------------------------------------------------------------------------
// <copyright file="List.cs" company="Dow Jones">
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
    [DataContract]
    [DebuggerDisplay("{Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class TwitterList
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the full name.
        /// </summary>
        /// <value>
        ///   The full name.
        /// </value>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        /// <value>
        ///   The id.
        /// </value>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        ///   Gets or sets Mode.
        /// </summary>
        [DataMember]
        public PublicationStatus Mode { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>
        ///   The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the slug.
        /// </summary>
        /// <value>
        ///   The slug.
        /// </value>
        [DataMember]
        public string Slug { get; set; }

        /// <summary>
        ///   Gets or sets the URI.
        /// </summary>
        /// <value>
        ///   The URI.
        /// </value>
        [DataMember]
        public string Uri { get; set; }

        /// <summary>
        ///   Gets or sets the user.
        /// </summary>
        /// <value>
        ///   The URI.
        /// </value>
        [DataMember]
        public TwitterUser User { get; set; }

        /// <summary>
        /// Gets or sets MemberCount.
        /// </summary>
        [DataMember]
        public int MemberCount { get; set; }

        /// <summary>
        /// Gets or sets SubscriberCount.
        /// </summary>
        [DataMember]
        public int SubscriberCount { get; set; }

        /// <summary>
        ///   Gets or sets Language.
        /// </summary>
        [JsonProperty("lang")]
        [DataMember]
        public string Language
        {
            get;
            set;
        }

        #endregion

        #region Sulia enhanced data

        /// <summary>
        /// Gets or sets IconUrl.
        /// </summary>
        [DataMember]
        public string IconUrl { get; set; }
                       
        /// <summary>
        /// Gets or sets IconUrl.
        /// </summary>
        [DataMember]
        public string BackgroundImageUrl { get; set; }

        /// <summary>
        /// Gets or sets Landing Page Url.
        /// </summary>
        [DataMember]
        public string LandingPageUrl { get; set; }    

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Tweets per day.
        /// </summary>
        [DataMember]
        public double TweetsPerDay { get; set; }

        /// <summary>
        /// Gets or sets whether the curator has claimed his/her list.
        /// </summary>
        [DataMember]
        public bool Claimed { get; set; }

        /// <summary>
        /// Gets or sets data extracted from recent tweets from participants.
        ///  Suitable for displaying as a graphical tag cloud.
        /// </summary>
        [DataMember]
        [JsonProperty("wordcloud")]
        public WordCloud WordCloud { get; set; }

        /// <summary>
        /// Gets or sets a list of distinctive terms extracted from recent tweets from participants.
        /// </summary>
        [DataMember]
        public WordCloud TalksAbout { get; set; }

        #endregion
    }
}