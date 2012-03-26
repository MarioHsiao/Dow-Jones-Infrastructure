using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.NewsStandTicker;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class NewsStandTickerController : ControllerBase
    {
        public ActionResult Index()
        {
            return View("Index", GetNewsStandTicker());
        }

        private IViewComponentModel GetNewsStandTicker()
        {
            var newsStandTicker = new NewsStandTickerModel
                                        {
                                            TickerDirection = TickerDirection.Left,
                                            TickerSpeed = 15,
                                            OnSourceTitleClick = "SourceTitleClick"
                                        };
            return new ContentContainerModel(new IViewComponentModel[] { newsStandTicker });
        }
    }
}
