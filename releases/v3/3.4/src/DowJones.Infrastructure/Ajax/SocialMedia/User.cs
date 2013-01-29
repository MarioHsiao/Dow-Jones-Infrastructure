using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Ajax.SocialMedia
{
    [DataContract(Name = "user", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        [DataMember(Name = "fullName")]
		[JsonProperty("fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "screenName")]
		[JsonProperty("screenName")]
        public string ScreenName { get; set; }

        [DataMember(Name = "profileImageUrl")]
		[JsonProperty("profileImageUrl")]
        public string ProfileImageUrl { get; set; }

        [DataMember(Name = "verified")]
		[JsonProperty("verified")]
        public  bool IsVerified { get; set; }

        [DataMember(Name = "id")]
		[JsonProperty("id")]
        public int Id { get; set; }

        [DataMember(Name = "profileHashUrl")]
		[JsonProperty("profileHashUrl")]
        public string ProfileHashUrl { get; set; }

        [DataMember(Name = "profileUrl")]
		[JsonProperty("profileUrl")]
        public string ProfileUrl { get; set; }
    }
}
