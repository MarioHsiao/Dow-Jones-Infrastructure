using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Ajax.Newsletter;
using Factiva.Gateway.Messages.Assets.SourceList.V1_0;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.NewsletterList
{
    public class NewsletterListModel: ViewComponentModel
    {
        [JsonProperty("newsletters")]
        public IEnumerable<Newsletter> Newsletters { get; set; }

        [ClientData]
        public NewsletterListDataResult Result { get; set; }

        public NewsletterListModel(NewsletterListDataResult dataResult = null)
        {
            Result = dataResult;
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
