using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.PersonalizationFilters;
using DowJones.Web.Mvc.UI.Components.Search;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class PersonalizationFiltersController : ControllerBase
    {
        public ActionResult Index()
        {
            var model = new PersonalizationFiltersModel
                            {
                                //SessionId = "27137ZzZKJHEQ3CCAAAGUAIAAAAAEDMMAAAAAABSGAYTCMJQGEYDAOJTGU2DQNJX",
                                SuggestServiceUrl = "http://suggest.dev.factiva.com/Search/1.0",
                                DataServiceUrl = "http://api.int.dowjones.com/api/Public/2.0/Taxonomy.svc",
                                FIICodeServiceUrl = "http://api.int.dowjones.com/api/Public/2.0/Taxonomy.svc/fiicode/json",
                                FilterLevel = FilterLevel.Page, 
                                LensType = LensType.Industry, 
                                ParentCodes = new [] { "iacc","iadv" },
                                IndustryFilter = new CodeDesc { Code = "i836", Desc = "" },
                                RegionFilter = new CodeDesc { Code = "india", Desc = "" },
                                CompanyFilter = new CodeDesc { Code = "MCROST", Desc = "" },
                                Disabled = true
                            };

            var model1 = new PersonalizationFiltersModel
            {
                //SessionId = "27137ZzZKJHEQ3CCAAAGUAIAAAAAEDMMAAAAAABSGAYTCMJQGEYDAOJTGU2DQNJX",
                SuggestServiceUrl = "http://suggest.dev.factiva.com/Search/1.0",
                DataServiceUrl = "http://api.int.dowjones.com/api/Public/2.0/Taxonomy.svc",
                FilterLevel = FilterLevel.Page,
                LensType = LensType.Industry,
                ParentCodes = new[] { "iacc", "iadv" }
            };

            return View("index", new List<PersonalizationFiltersModel>{model,model1});

            //var model = new TaxonomySearchBrowse();
            //model.TaxonomyType = TaxonomyType.Industry;
            //model.SessionId = "27137ZzZKJAUQ3CCAAAGUAIAAAAADQ4RAAAAAABSGAYTCMBZGE4TCMBTGE2DSMZX";
            //model.ProductId = "GL";
            //model.TaxonomyServiceUrl = "http://api.int.dowjones.com/api/Public/2.0/Taxonomy.svc";
            //model.ParentCodes = new List<string>() {"a","b"};
            //var model = new PersonalizationFilters { SessionId = "27137ZzZKJHEQ3CCAAAGUAIAAAAAEDMMAAAAAABSGAYTCMJQGEYDAOJTGU2DQNJX", SuggestServiceUrl = "http://suggest.dev.factiva.com/Search/1.0", FilterLevel = FilterLevel.Page, Data = new PersonlizationFiltersData { LensType = LensType.Industry, ParentCodes = new List<string>() { "iacc", "iadv" }, IndustryFilter = new CodeDesc { Code = "i836", Desc = "Accounting" } }, ProductId = "GL", TaxonomyServiceUrl = "http://fdevweb3/api/Public/2.0/Taxonomy.svc", InterfaceLanguage = "en" };
            //"http://api.int.dowjones.com/api/Public/2.0/Taxonomy.svc";
            //return View("index", model);
        }
    }
}
