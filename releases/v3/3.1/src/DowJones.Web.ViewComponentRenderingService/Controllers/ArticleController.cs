using System.Web.Mvc;
using DowJones.Ajax.Article;
using DowJones.Web.Mvc.UI.Components.Models.Article;
using Newtonsoft.Json;
using System.IO;
using DowJones.Web.Mvc.ModelBinders;

namespace DowJones.Web.Article.Website.Controllers
{
    public class ArticleController : DowJones.Web.Mvc.ControllerBase
    {
        public ActionResult Index()
        {
            return Render();
        }

        public ActionResult Render()
        {
            var articleJson = new StreamReader(Request.InputStream).ReadToEnd();
            var article = JsonConvert.DeserializeObject<ArticleResultset>(articleJson);

            var model = new ArticleModel { ArticleDataSet = article };

            return ViewComponent(model);
        }
    }
}
