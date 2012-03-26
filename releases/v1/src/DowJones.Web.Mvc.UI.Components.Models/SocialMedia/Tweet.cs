using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Twitter
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tweet
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("isoTimestamp")]
        public DateTime IsoTimestamp { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("twitterIconUrl")]
        public string TwitterIconUrl { get; set; }

        [JsonProperty("retweets")]
        public List<Tweet> Retweets { get; set; }

        [JsonProperty("replies")]
        public List<Tweet> Replies { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }
    }
}
