using System.Linq;
using DowJones.Ajax.Article;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.UI.Components.ArticleTranslator;
using DowJones.Web.Mvc.UI.Components.Models.Article;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using Factiva.Gateway.Messages.Archive.V2_0;
using DowJones.Web.Mvc.UI.Components.Article;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Components.Articles
{
    public class ArticlesModel : ViewComponentModel
    {
        public List<ArticleModel> ArticleModels { get; set; }

        public DisplayOptions ArticleDisplayOptions { get; set; }

        public bool ShowCompanyEntityReference { get; set; }

        public string Keywords { get; set; }

        public ArticlesModel(ArticleResponseSet responseSet, DisplayOptions options, string keywords, bool showCompanyEntityReference)
        {
            ArticleDisplayOptions = options;
            Keywords = keywords;
            ShowCompanyEntityReference = showCompanyEntityReference;

            var models = new List<ArticleModel>();

            foreach (var article in responseSet.article)
            {
                models.Add(new ArticleModel(article) { ArticleDisplayOptions = this.ArticleDisplayOptions, Keywords = this.Keywords, ShowCompanyEntityReference = this.ShowCompanyEntityReference });
            }

            ArticleModels = models;
        }
    }
}