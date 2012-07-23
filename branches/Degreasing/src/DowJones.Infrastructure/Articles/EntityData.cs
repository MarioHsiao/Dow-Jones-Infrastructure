using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Ajax.Article
{
    public class EntityLinkData
    {
        [DataMember(Name = "fcode")]
        [JsonProperty("fcode")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember(Name = "category")]
        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
