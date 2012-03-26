using System.Web.Mvc;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Uri;
using DowJones.Web.Mvc.UI.Components.HeadlineListCarousel;
using DowJones.Web.Mvc;
using Factiva.Gateway.Messages.Search.V2_0;
using DowJones.Web.Showcase.Extensions;

namespace DowJones.Web.Showcase.Controllers
{
    public class HeadlineListCarouselController : DowJonesControllerBase
    {
        private readonly HeadlineListConversionManager _headlineListManager;
        
        private readonly string[] _imageUrls = new[]
                                             {
                                                 "http://iconpacks.mozdev.org/images/thunderbird-icon.png",
                                                 "http://i3.ytimg.com/vi/jhaAiZfG38Q/default.jpg"
                                             };


        private const string ImageHandlerUrl = "http://arch.dev.us.factiva.com/Showcase/EMG.Utility.Handlers.Video.ImageHandler.ashx";

        protected int ImageRequestCounter
        {
            get
            {
                return (TempData["imageRequestCounter"] as int?).GetValueOrDefault();
            }
            set { TempData["imageRequestCounter"] = value; }
        }

        public HeadlineListCarouselController(HeadlineListConversionManager headlineListManager)
        {
            _headlineListManager = headlineListManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Carousel");
        }

        public ActionResult Carousel()
        {
            var dataResult = _headlineListManager.ProcessFeed("http://feeds.haacked.com/haacked").result;
            var headlines = dataResult.resultSet.headlines;
            for (int i = 0; i < headlines.Count; i++)
                GenerateExternalUrlForImageHandler(headlines[i].thumbnailImage);

            var headlineListCarouselState = new HeadlineListCarouselModel 
                {
                    Result = dataResult
                };

            return View("Carousel", headlineListCarouselState);
        }


        private void GenerateExternalUrlForImageHandler(ThumbnailImage thumbnailImage)
        {
            ImageRequestCounter = (ImageRequestCounter + 1) % 2;

            var ub = new UrlBuilder(ImageHandlerUrl) { OutputType = UrlBuilder.UrlOutputType.Absolute };
            ub.QueryString.Add("url", _imageUrls[ImageRequestCounter]);
            ub.QueryString.Add("height", "77");
            ub.QueryString.Add("ar", "locked");

            thumbnailImage.SRC = ub.ToString();
            thumbnailImage.URI = "http://www.youtube.com/watch?v=jhaAiZfG38Q";
        }


    }
}
