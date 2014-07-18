using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using DowJones.Ajax.Newsletter;

namespace DowJones.Web.Mvc.UI.Components.NewsletterSectionList
{
    [DataContract(Name = "newsletterSectionListModel", Namespace = "")]
    public class NewsletterSectionListModel : ViewComponentModel
    {
        [JsonProperty("nlid")]
        public long NewsletterId { get; set; }

        [ClientData]
        [DataMember(Name="result")]
        [JsonProperty("result")]
        public NewsletterSectionListDataResult Result { get; set; }
    }
}
