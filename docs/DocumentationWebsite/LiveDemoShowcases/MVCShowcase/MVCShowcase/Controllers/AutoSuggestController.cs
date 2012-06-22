using System;
using System.Collections.Generic;
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
            var autoSuggestModels = new List<AutoSuggestModel>();
            var sourceSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Source",
                ServiceOptions = "{\"types\":\"blog\"}",
                Tokens = "{\"blogTkn\":\"Blog\"}",
                ControlId = "djSourceAutoSuggest"
            };
            autoSuggestModels.Add(sourceSuggestModel);

            var keywordSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Keyword",
                ControlId = "djKeywordAutoSuggest",
                FillInputOnKeyUpDown = true,
                SelectFirst = true
            };
            autoSuggestModels.Add(keywordSuggestModel);

            var companySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Company",
                Columns = "value|code|ticker",
                ControlId = "djCompanyAutoSuggest"
            };
            autoSuggestModels.Add(companySuggestModel);

            var executiveSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Executive",
                ControlId = "djExecutiveAutoSuggest"
            };
            autoSuggestModels.Add(executiveSuggestModel);

            var authorSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Author",
                ControlId = "djAuthorAutoSuggest"
            };
            autoSuggestModels.Add(authorSuggestModel);

            var outletSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Outlet",
                ControlId = "djOutletAutoSuggest"
            };
            autoSuggestModels.Add(outletSuggestModel);

            var publisherCitySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "publisherCity",
                ControlId = "djPublisherCityAutoSuggest"
            };
            autoSuggestModels.Add(publisherCitySuggestModel);

            var publisherMetaDataSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "publisherMetaData",
                ControlId = "djPublisherMetaDataAutoSuggest"
            };
            autoSuggestModels.Add(publisherMetaDataSuggestModel);

            return autoSuggestModels;
        }
    }
}
