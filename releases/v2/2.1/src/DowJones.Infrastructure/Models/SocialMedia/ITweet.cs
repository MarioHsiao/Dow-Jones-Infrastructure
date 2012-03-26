using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    public interface ITweet
    {
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_at")]
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Favorited.
        /// </summary>
        [DataMember]
        bool Favorited { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        long Id { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        [DataMember]
        string Source { get; set; }

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        [DataMember]
        string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Truncated.
        /// </summary>
        [DataMember]
        bool Truncated { get; set; }

        /// <summary>
        /// Gets or sets TwitterUser.
        /// </summary>
        [DataMember]
        TwitterUser User { get; set; }
    }
}
