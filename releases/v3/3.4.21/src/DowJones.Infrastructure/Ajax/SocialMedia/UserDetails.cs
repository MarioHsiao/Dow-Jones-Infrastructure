using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Ajax.SocialMedia
{
    [DataContract(Name = "userDetails", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn)]
    public class UserDetails : User
    {
        [DataMember(Name = "following")]
		[JsonProperty("following")]
        public int FollowingCount { get; set; }

        [DataMember(Name = "followers")]
		[JsonProperty("followers")]
        public int FollowersCount { get; set; }

        [DataMember(Name = "listed")]
		[JsonProperty("listed")]
        public int ListedCount { get; set; }

        [DataMember(Name = "tweets")]
		[JsonProperty("tweets")]
        public int TweetsCount { get; set; }

        [DataMember(Name = "tweetsPerDay")]
		[JsonProperty("tweetsPerDay")]
        public int TweetsPerDay { get; set; }

        [DataMember(Name = "lastTweet")]
		[JsonProperty("lastTweet")]
        public DateTime LastTweet { get; set; }

        [DataMember(Name = "location")]
		[JsonProperty("location")]
        public string Location { get; set; }

        [DataMember(Name = "url")]
		[JsonProperty("url")]
        public string Url { get; set; }

        [DataMember(Name = "description")]
		[JsonProperty("description")]
        public string Description { get; set; }

        [DataMember(Name = "talksAbout")]
		[JsonProperty("talksAbout")]
        public string TalksAbout { get; set; }



    }
}
