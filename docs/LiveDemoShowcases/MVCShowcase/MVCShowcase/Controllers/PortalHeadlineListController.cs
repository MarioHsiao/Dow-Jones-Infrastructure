using System.Web.Mvc;
using DowJones.Web.Mvc.Routing;
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
			};
            return View(model);
        }

		[Route("PortalHeadlineList/data/{mode}")]
		public ActionResult Data(string mode = "js")
		{
			var view = mode == "cs" ? "_cSharpData" : "_jsData";

			return PartialView(view);
		}
    }
}
