using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.DegreasedDashboards.Website.Controllers
{
    public class PagesController : DowJones.Web.Mvc.UI.Canvas.Controllers.PagesControllerBase
    {
        private readonly IPageRepository _pageRepository;

        public PagesController(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Page", new {id = "1234"});
        }

        [Route("pages/{pageId}/reset")]
        public ActionResult Reset(string pageId)
        {
            _pageRepository.DeletePage(pageId);

            TempData["SuccessMessage"] = "Page data reset";

            return RedirectToAction("Page", new { id = pageId });
        }

        public override ActionResult PageNotFound(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                return base.PageNotFound(id);

            var page = new Page
                           {
                               ID = id,
                               Layout = new ZonePageLayout(),
                               Title = "Test Page " + id, 
                           };

            var pageId = _pageRepository.CreatePage(page);
            
            return Page(pageId);
        }
    }
}
