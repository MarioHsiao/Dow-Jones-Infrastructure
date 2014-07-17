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
        [ClientData]
        public NewsletterListDataResult Result { get; set; }

        public NewsletterListModel(NewsletterListDataResult dataResult = null)
        {
            Result = dataResult;
        }
    }

}
