using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DowJones.Ajax.Newsletter;
using Factiva.Gateway.Messages.Assets.SourceList.V1_0;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.NewsletterList
{
    [DataContract(Name = "newsletterListModel", Namespace = "")]
    public class NewsletterListModel: ViewComponentModel
    {
        [ClientProperty("type")]
        public string Type { get; set; }

        [ClientProperty("showAddAction")]
        public bool ShowAddAction { get; set; }

        [ClientProperty("showEditAction")]
        public bool ShowEditAction { get; set; }

        [ClientProperty("showClearAction")]
        public bool ShowClearAction { get; set; }

        [ClientProperty("showGotoAction")]
        public bool ShowGotoNewsletter { get; set; }

        [ClientData]
        [DataMember(Name="result")]
        [JsonProperty("result")]
        public NewsletterListDataResult Result { get; set; }

        public NewsletterListModel(NewsletterListDataResult dataResult = null)
        {
            Result = dataResult;
        }
    }

}
