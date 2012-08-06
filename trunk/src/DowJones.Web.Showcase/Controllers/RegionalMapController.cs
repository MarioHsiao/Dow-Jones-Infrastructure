using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.RegionalMap;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class RegionalMapController : ControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Regional Map";
            return Components("Index", GetRegionalMapModel());
        }

        private IViewComponentModel GetRegionalMapModel()
        {
            var regionalMapModel = new RegionalMapModel{
                                       OnRegionClick = "RegionClicked",
                                       ShowTextLabels = true,
                                       Size = MapSize.Large       
                                   };
            return new ContentContainerModel(new IViewComponentModel[] { regionalMapModel });
        }

    }
}
