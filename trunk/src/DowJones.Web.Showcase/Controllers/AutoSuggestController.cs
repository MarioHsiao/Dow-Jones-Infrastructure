using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Web.Mvc;
using DowJones.Extensions;
using DowJones.Web.Mvc.UI.Components.AutoSuggest;

namespace DowJones.Web.Showcase.Controllers
{
    public class AutoSuggestController : Controller
    {
        //
        // GET: /AutoSuggest/

        public ActionResult Index()
        {
            var autoSuggestModel = GetDJXSearchBoxModel();
            return View("Index", autoSuggestModel);
        }

        public ActionResult DJX()
        {
            return View("DJX");
        }


        public AutoSuggestModel GetDJXSearchBoxModel()
        {
           /* var authTypeToken = '@ConfigurationManager.AppSettings["SuggestAuthTypeToken"]';
    var suggestServiceURl = '@ConfigurationManager.AppSettings["SuggestServiceUrl"]';

    DJ.add("AutoSuggest", {
        container: "autoSuggestContainer",
        options: { 
            suggestServiceUrl: suggestServiceURl,
            autocompletionType: "Categories",
            controlId: "djKeywordAutoSuggest",
            authType: "SuggestContext",
            authTypeValue: 'YPC0P9uW1Y0hWvnUcNuqBlWzIuCTZcappPV5OMlQo_2BS8eTDYW_2FsmBm_2B2gksFHYz6ftM1lV0GYtXwrgTuz3ne6Wa8oxr3ma7C|2',
            selectFirst: false,
            showViewAll: false,
            fillInputOnKeyUpDown: true,
            eraseInputOnItemSelect: true, // need to expose
            showHelp: false,
            showSearchText: true, // need to expose
            serviceOptions: {
                maxResults: 3,
                interfaceLanguage: 'en',
                categories: 'company|executive|industry|newssubject|region_all|keyword|',
                it: 'stock',
                companyFilterSet: 'newsCodedAbt|newsCodedOccr|noADR',
                executiveFilterSet: 'newsCoded',
                showMatchingWord: 'true'
            }
        }*/
            ServiceOptions serviceOptions = new ServiceOptions();
            serviceOptions.MaxResults = 3;
            serviceOptions.InterfaceLanguage = "en";
            serviceOptions.Categories = "company|executive|industry|newssubject|region_all|keyword";
            serviceOptions.It = "stock";
            serviceOptions.CompanyFilterSet = "newsCodedAbt|newsCodedOccr|noADR";
            serviceOptions.ExecutiveFilterSet = "newsCoded";
            serviceOptions.ShowMatchingWord = true;
            
            return new AutoSuggestModel()
            {
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = ConfigurationManager.AppSettings["SuggestAuthTypeToken"],
                AutocompletionType = AutoCompletionType.Categories,
                ControlId = "djKeywordAutoSuggest",
                SelectFirst = false,
                ShowViewAll = true,
                FillInputOnKeyUpDown = true,
                EraseInputOnItemSelect = true,
                ShowHelp = false,
                ShowSearchText = true,
                ServiceOptions = serviceOptions
            };
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
                                              //ServiceOptions = "{\"types\":\"blog\"}",
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
