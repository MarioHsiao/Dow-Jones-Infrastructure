using System.Configuration;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.AutoSuggest;

namespace DowJones.MvcShowcase.Controllers
{
    public class AutoSuggestController : BaseController
    {
        public ActionResult Index()
        {
            var authTypeToken = ConfigurationManager.AppSettings["SuggestAuthTypeToken"];
            var suggestServiceURl = ConfigurationManager.AppSettings["SuggestServiceUrl"];
            var model = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.Keyword,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                ControlId = "djKeywordAutoSuggest",
                FillInputOnKeyUpDown = true,
                SelectFirst = true
            };
            return View("Index", model);
        }
    }
}
