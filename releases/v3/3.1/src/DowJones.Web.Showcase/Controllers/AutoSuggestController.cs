using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.AutoSuggest;

namespace DowJones.Web.Showcase.Controllers
{
    public class AutoSuggestController : Controller
    {
        //
        // GET: /AutoSuggest/

        public ActionResult Index()
        {
            var keywordSuggestModel = new AutoSuggestModel {AutocompletionType = "keyword", SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0"};
            return View("Index", keywordSuggestModel);
        }

    }
}
