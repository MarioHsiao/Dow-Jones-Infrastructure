using System.Web.Mvc;
using DowJones.Tools.Ajax.Converters;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Showcase.Controllers
{
    public class PortalHeadlineListController : DowJonesControllerBase
    {
        private readonly HeadlineListConversionManager _headlineListManager;

        public PortalHeadlineListController(HeadlineListConversionManager headlineListManager)
        {
            _headlineListManager = headlineListManager;
        }

        public ActionResult Index()
        {
            var model = GetPortalHeadlineListSection();
            return View("Index", model);
        }

        public ActionResult PartialResult(string url, string callback)
        {
            var model = GetPortalHeadlineListSection(url);
            return ViewComponent(model, callback);
        }


        private PortalHeadlineListModel GetPortalHeadlineListSection(string url = null)
        {
            url = url ?? "http://feeds.haacked.com/haacked";
            var feed = _headlineListManager.ProcessFeed(url);

            return new PortalHeadlineListModel(feed.result) { MaxNumHeadlinesToShow = 5 };
        }

        public ActionResult SectionSample()
        {
            return View("SectionSample");
        }
    }
}
