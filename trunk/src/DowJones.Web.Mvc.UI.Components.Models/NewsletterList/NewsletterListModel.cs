using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.NewsletterList
{
    public class NewsletterListModel: ViewComponentModel
    {
        [JsonProperty("newsletters")]
        public IEnumerable<Newsletter> Newsletters { get; set; }

        public NewsletterListModel()
        {
            Newsletters = Enumerable.Empty<Newsletter>();
        }
    }

    public class Newsletter
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [JsonProperty("lastModifiedDateDescriptor")]
        public string LastModifiedDateDescriptor { get; set; }
    }
}
