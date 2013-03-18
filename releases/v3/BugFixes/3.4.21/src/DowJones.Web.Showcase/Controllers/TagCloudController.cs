using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.TagCloud;
using DowJones.Web.Showcase.Models;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class TagCloudController : ControllerBase
    {
        public ActionResult Index()
        {
            var viewModel = new TagCloudDemoViewModel {
                TagCloud1 = new TagCloudModel { OnTagItemClientClick = "OnTagItemClick" },
                TagCloud2 = new TagCloudModel { OnTagItemClientClick = "OnTagItemClick1" },
            };

            return View("Index", viewModel);
        }
    }
}
