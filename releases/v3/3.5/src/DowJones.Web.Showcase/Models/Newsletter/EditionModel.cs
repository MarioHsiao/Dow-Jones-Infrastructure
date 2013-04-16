using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Web.Mvc.UI.Components.Article;

namespace DowJones.Web.Showcase.Models.Newsletter
{
    public class EditionModel
    {
        public Meta Meta { get; set; }

        public Mvc.UI.Components.Article.ArticleModel Article { get; set; }
    }

    public class Meta
    {
        public string NewsletterName { get; set; }

        public string EditionName { get; set; }

        public DateTime Timestamp { get; set; }

        public string TimestampDescripter { get; set; }

        public List<string> Companies { get; set; }

        public List<string> Sources{ get; set; }
    }

    public enum LinkType
    {

    }

    public class NewsLetterEntity
    {
        public LinkType LinkType { get; set; }
    }
}