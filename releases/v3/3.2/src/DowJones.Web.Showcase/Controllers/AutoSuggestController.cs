using System.Collections.Generic;
using System.Configuration;
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
            var autoSuggestModels = GetAutoSuggestModels();
            return View("Index", autoSuggestModels);
        }

        public List<AutoSuggestModel> GetAutoSuggestModels()
        {
            var authTypeToken = ConfigurationManager.AppSettings["SuggestAuthTypeToken"];
            var suggestServiceURl = ConfigurationManager.AppSettings["SuggestServiceUrl"];

            var autoSuggestModels = new List<AutoSuggestModel>();
            var sourceSuggestModel = new AutoSuggestModel
                                          {
                                              SuggestServiceUrl = suggestServiceURl,
                                              AutocompletionType = AutoCompletionType.Source,
                                              AuthType = AuthType.SuggestContext,
                                              AuthTypeValue = authTypeToken,
                                              ServiceOptions = "{\"types\":\"blog\"}",
                                              Tokens = "{\"blogTkn\":\"Blog\"}",
                                              ControlId = "djSourceAutoSuggest"
                                          };
            autoSuggestModels.Add(sourceSuggestModel);

            var keywordSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.Keyword,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                ControlId = "djKeywordAutoSuggest",
                FillInputOnKeyUpDown = true,
                SelectFirst = true
            };
            autoSuggestModels.Add(keywordSuggestModel);

            var companySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.Company,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                Columns = "value|code|ticker",
                ControlId = "djCompanyAutoSuggest"
            };
            autoSuggestModels.Add(companySuggestModel);

            var executiveSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.Executive,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                ControlId = "djExecutiveAutoSuggest"
            };
            autoSuggestModels.Add(executiveSuggestModel);

            var authorSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.Author,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                ControlId = "djAuthorAutoSuggest"
            };
            autoSuggestModels.Add(authorSuggestModel);

            var outletSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.Outlet,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                ControlId = "djOutletAutoSuggest"
            };
            autoSuggestModels.Add(outletSuggestModel);

            var publisherCitySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.PublisherCity,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                ControlId = "djPublisherCityAutoSuggest"
            };
            autoSuggestModels.Add(publisherCitySuggestModel);

            var publisherMetaDataSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = suggestServiceURl,
                AutocompletionType = AutoCompletionType.PublisherMetaData,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = authTypeToken,
                ControlId = "djPublisherMetaDataAutoSuggest"
            };
            autoSuggestModels.Add(publisherMetaDataSuggestModel);

            return autoSuggestModels;
        }

        public ActionResult ComponentExplorerDemo()
        {
            var autoSuggestModels = GetAutoSuggestModels();
            return View("Index", "_Layout_ComponentExplorer", autoSuggestModels);
        }

    }
}
