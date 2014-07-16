using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Ajax.Newsletter
{
    public class NewsletterInfo
    {
        [DataMember(Name = "code")]
        [JsonProperty("code")]
        public string Code { get; set; }

        [DataMember(Name = "id")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "lastModifiedDateDescriptor")]
        [JsonProperty("lastModifiedDateDescriptor")]
        public string LastModifiedDateDescriptor { get; set; }
    }
}
