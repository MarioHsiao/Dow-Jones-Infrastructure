using System.Configuration;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.MvcShowcase.Controllers
{
    public class AutoSuggestController : BaseController
    {
        //
        // GET: /AutoSuggest/

        public ActionResult Index()
        {
            var authTypeToken = ConfigurationManager.AppSettings["SuggestAuthTypeToken"];
            var suggestServiceURl = ConfigurationManager.AppSettings["SuggestServiceUrl"];
            var model = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "Keyword",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ControlId = "djKeywordAutoSuggest",
                FillInputOnKeyUpDown = true,
                SelectFirst = true
            };
            return View("Index", model);
        }
    }
}
