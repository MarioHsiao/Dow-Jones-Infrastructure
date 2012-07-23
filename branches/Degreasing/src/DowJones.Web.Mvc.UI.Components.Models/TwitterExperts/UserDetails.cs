using System;
using DowJones.Web.Mvc.UI.Components.SocialMedia;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.SocialMedia
{

    [JsonObject(MemberSerialization.OptIn)]
    public class UserDetails : User
    {
        [JsonProperty("following")]
        public int FollowingCount { get; set; }

        [JsonProperty("followers")]
        public int FollowersCount { get; set; }

        [JsonProperty("listed")]
        public int ListedCount { get; set; }

        [JsonProperty("tweets")]
        public int TweetsCount { get; set; }

        [JsonProperty("tweetsPerDay")]
        public int TweetsPerDay { get; set; }

        [JsonProperty("lastTweet")]
        public DateTime LastTweet { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("talksAbout")]
        public string TalksAbout { get; set; }



    }
}
