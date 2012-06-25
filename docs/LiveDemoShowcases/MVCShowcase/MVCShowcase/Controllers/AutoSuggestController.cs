using System.Collections.Generic;
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
            var model = GetAutoSuggestModels();
            return View("Index", model);
        }

        public List<AutoSuggestModel> GetAutoSuggestModels()
        {
            var authTypeToken = ConfigurationManager.AppSettings["SuggestAuthTypeToken"];
            var suggestServiceURl = ConfigurationManager.AppSettings["SuggestServiceUrl"];

            var autoSuggestModels = new List<AutoSuggestModel>();
            var sourceSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "Source",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ServiceOptions = "{\"types\":\"blog\"}",
                Tokens = "{\"blogTkn\":\"Blog\"}",
                ControlId = "djSourceAutoSuggest"
            };
            autoSuggestModels.Add(sourceSuggestModel);

            var keywordSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "Keyword",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ControlId = "djKeywordAutoSuggest",
                FillInputOnKeyUpDown = true,
                SelectFirst = true
            };
            autoSuggestModels.Add(keywordSuggestModel);

            var companySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "Company",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                Columns = "value|code|ticker",
                ControlId = "djCompanyAutoSuggest"
            };
            autoSuggestModels.Add(companySuggestModel);

            var executiveSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "Executive",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ControlId = "djExecutiveAutoSuggest"
            };
            autoSuggestModels.Add(executiveSuggestModel);

            var authorSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "Author",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ControlId = "djAuthorAutoSuggest"
            };
            autoSuggestModels.Add(authorSuggestModel);

            var outletSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "Outlet",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ControlId = "djOutletAutoSuggest"
            };
            autoSuggestModels.Add(outletSuggestModel);

            var publisherCitySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "publisherCity",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ControlId = "djPublisherCityAutoSuggest"
            };
            autoSuggestModels.Add(publisherCitySuggestModel);

            var publisherMetaDataSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = "publisherMetaData",
                AuthType = "SuggestContext",
                AuthTypeValue = authTypeToken,
                ControlId = "djPublisherMetaDataAutoSuggest"
            };
            autoSuggestModels.Add(publisherMetaDataSuggestModel);

            return autoSuggestModels;
        }
    }
}
