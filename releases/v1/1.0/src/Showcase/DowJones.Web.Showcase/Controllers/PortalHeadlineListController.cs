using System.Web.Mvc;
using DowJones.Tools.Ajax.Converters;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Showcase.Models.PortalHeadlineList;

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
            var model = new PortalHeadlineListDemoPage{
                PhilHaack = GetFeedResults("http://feeds.haacked.com/haacked"),
                ScottGu = GetFeedResults("http://weblogs.asp.net/scottgu/atom.aspx"),
                Hanselman = GetFeedResults("http://feeds.feedburner.com/ScottHanselman"),
            };
            
            return View("Index", model);
        }


        private PortalHeadlineListDemoSection GetFeedResults(string feedUrl)
        {
            var feed = _headlineListManager.ProcessFeed(feedUrl);

            return new PortalHeadlineListDemoSection {
                Title = feed.feedTitle,
                HeadlineList = new PortalHeadlineListModel(feed.result) { MaxNumHeadlinesToShow = 5, },
            };
        }
    }
}
