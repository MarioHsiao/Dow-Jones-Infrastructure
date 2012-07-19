using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class NewsFeedController : ControllerBase
    {
        public ActionResult Index( string id = null )
        {
            return View( "Index" );
        }

        public ActionResult POC()
        {
            var model = new PortalHeadlineListModel
                        {
                            MaxNumHeadlinesToShow = 25,
                            ShowAuthor = true,
                            ShowSource = true,
                            ShowPublicationDateTime = true,
                            ShowTruncatedTitle = false,
                            AuthorClickable = true,
                            SourceClickable = true,
                            DisplaySnippets = SnippetDisplayType.Hover,
                            Layout = PortalHeadlineListLayout.TimelineLayout,
                        };

            return View( "POC", model );
        }
    }
}