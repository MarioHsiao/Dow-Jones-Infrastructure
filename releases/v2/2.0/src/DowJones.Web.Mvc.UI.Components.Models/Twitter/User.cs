using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Twitter
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("screenName")]
        public string ScreenName { get; set; }

        [JsonProperty("profileImageUrl")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("verified")]
        public  bool IsVerified { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
