using System.Web.Mvc;
using DowJones.Search;
using DowJones.Web.Mvc.UI.Components.EntityList;

namespace DowJones.Web.Showcase.Controllers
{
    public class EntityListController : Controller
    {
        public ActionResult Index()
        {
            var model = 
                new EntityList {
                    Groups = new [] {
                        new EntityGroup {
                            Category = NewsFilterCategory.Company.ToString(),
                            Entities = new [] {
                                new Entity { Code = "MSFT", Name = "Microsoft" },
                                new Entity { Code = "GOOG", Name = "Google" },
                            }
                        },
                        new EntityGroup {
                            Category = NewsFilterCategory.Author.ToString(),
                            Entities = new [] {
                                new Entity { Code = "12345", Name = "Fred Flintsone" },
                                new Entity { Code = "9876", Name = "Homer Simpson" },
                            }
                        },
                    }
                };

            return View("Index", model);
        }

    }
}
