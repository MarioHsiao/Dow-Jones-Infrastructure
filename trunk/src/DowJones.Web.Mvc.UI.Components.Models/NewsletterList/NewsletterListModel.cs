using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.NewsletterList
{
    public class NewsletterListModel: ViewComponentModel
    {
        [ClientProperty("newsletters")]
        public IEnumerable<Newsletter> Newsletters { get; set; }

        public NewsletterListModel()
        {
            Newsletters = Enumerable.Empty<Newsletter>();
        }
    }

    public class Newsletter
    {
        [ClientProperty("code")]
        public string Code { get; set; }

        [ClientProperty("id")]
        public long Id { get; set; }

        [ClientProperty("name")]
        public string Name { get; set; }

        [ClientProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [ClientProperty("lastModifiedDateDescriptor")]
        public string LastModifiedDateDescriptor { get; set; }
    }
}
