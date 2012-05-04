using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.SocialMedia
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tweet
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("isoTimeStamp")]
        public string IsoTimeStamp
        {
            get { return CreatedAt.ToUniversalTime().ToString("o"); }
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Favorited.
        /// </summary>
        [JsonProperty("favorited")]
        public bool Favorited { get; set; }

        [JsonProperty("twitterIconUrl")]
        public string TwitterIconUrl { get; set; }


        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }

        /// <summary>
        /// Gets or sets Text with hashtags, entities etc mapped as HTML links.
        /// </summary>
        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("sourceText")]
        public string SourceText
        {
            get
            {
                var match = Regex.Match(Source, @">([^<]+)<");
                return match.Success ? match.Groups[1].Value : string.Empty;
            }
        }

        [JsonProperty("reTweetId")]
        public string ReTweetId { get; set; }
    }
}
