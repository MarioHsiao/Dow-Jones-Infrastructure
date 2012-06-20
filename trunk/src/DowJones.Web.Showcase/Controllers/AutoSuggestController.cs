using System.Collections.Generic;
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
            var autoSuggestModels = new List<AutoSuggestModel>();
            var sourceSuggestModel = new AutoSuggestModel
                                          {
                                              SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                                              AutocompletionType = "Source",
                                              ServiceOptions = "{\"types\":\"blog\"}",
                                              Tokens = "{\"blogTkn\":\"Blog\"}",
                                              ID = "djSourceAutoSuggest"
                                          };
            autoSuggestModels.Add(sourceSuggestModel);

            var keywordSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Keyword",
                ID = "djKeywordAutoSuggest"
            };
            autoSuggestModels.Add(keywordSuggestModel);

            var companySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Company",
                Columns = "value|code|ticker",
                ID = "djCompanyAutoSuggest"
            };
            autoSuggestModels.Add(companySuggestModel);

            var executiveSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Executive",
                ID = "djExecutiveAutoSuggest"
            };
            autoSuggestModels.Add(executiveSuggestModel);

            var authorSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Author",
                ID = "djAuthorAutoSuggest"
            };
            autoSuggestModels.Add(authorSuggestModel);

            var outletSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "Outlet",
                ID = "djOutletAutoSuggest"
            };
            autoSuggestModels.Add(outletSuggestModel);

            var publisherCitySuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "publisherCity",
                ID = "djPublisherCityAutoSuggest"
            };
            autoSuggestModels.Add(publisherCitySuggestModel);

            var publisherMetaDataSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = "http://suggest.int.factiva.com/Search/1.0",
                AutocompletionType = "publisherMetaData",
                ID = "djPublisherMetaDataAutoSuggest"
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
