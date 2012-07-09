using System.Web.Mvc;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.MvcShowcase.Controllers
{
	public class PortalHeadlineListController : BaseController
    {
        public ActionResult Index()
        {
			var model = new PortalHeadlineListModel
			{
				MaxNumHeadlinesToShow = 5,
				ShowAuthor = true,
				ShowSource = true,
				ShowPublicationDateTime = true,
				ShowTruncatedTitle = false,
				AuthorClickable = true,
				SourceClickable = true,
				DisplaySnippets = SnippetDisplayType.Hover,
				Layout = PortalHeadlineListLayout.HeadlineLayout,
				Result = GetData()
			};
            return View(model);
        }


		private PortalHeadlineListDataResult GetData()
		{
			// random feed url
			const string url = "http://feeds.haacked.com/haacked";

			var headlineListManager = new HeadlineListConversionManager(new DateTimeFormatter("en"));

			// process the feed a get a HeadlineListDataResult
			var feed = headlineListManager.ProcessFeed(url);

			// map relevant fields from response to portalHeadlineListDataResult
			var portalHeadlineListDataResult = PortalHeadlineConversionManager.Convert(feed.result);

			// return data
			return portalHeadlineListDataResult;
		}
    }
}
