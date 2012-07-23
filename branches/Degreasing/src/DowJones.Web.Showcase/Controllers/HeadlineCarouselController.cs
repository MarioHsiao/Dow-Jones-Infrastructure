using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.HeadlineCarousel;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class HeadlineCarouselController : ControllerBase
    {
        public ActionResult Index()
        {
            return View("Index", GetHeadlineCarousel());
        }

        private IViewComponentModel GetHeadlineCarousel()
        {
            var tickerCarousel = new HeadlineCarouselModel { Orientation = CarouselOrientation.Horizontal, Mode = CarouselMode.Ticker, Display = 1, OnHeadlineClick = "HeadlineClick", ID = "TickerCarousel" };
            var videoCarouselX = new HeadlineCarouselModel { ID = "VideoCarouselX", Orientation = CarouselOrientation.Horizontal, Mode = CarouselMode.Video, Display = 1 };
            var videoCarouselY = new HeadlineCarouselModel { ID = "VideoCarouselY", Orientation = CarouselOrientation.Vertical, Mode = CarouselMode.Video, Display = 1 };
            return new ContentContainerModel(new IViewComponentModel[] { tickerCarousel, videoCarouselX, videoCarouselY });
        }
    }
}
