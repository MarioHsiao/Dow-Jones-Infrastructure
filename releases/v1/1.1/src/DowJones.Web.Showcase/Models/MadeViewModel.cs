using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Web.Mvc.UI.Components.Models.Article;
using DowJones.Web.Mvc.UI.Components.Models.HeadlineList;
using ControlModels = DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Showcase.Models
{
    public class MadeViewModel
    {
        public ControlModels.HeadlineList.HeadlineListModel HeadlineList {get;set;}
        public ControlModels.Article.ArticleModel FirstArticle {get;set;}
        public ControlModels.Discovery.DiscoveryModel Discovery { get; set; }
    }
}