using System.Linq;
using System.Web.Mvc;
using DowJones.Documentation.Website.Extensions;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly DocumentationPages _pages;

        public DocumentationController()
            : this(MvcApplication.DocumentationPages)
        {
        }

        public DocumentationController(DocumentationPages pages)
        {
            _pages = pages;
        }

        public ActionResult Homepage()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Navigation(string category, string page)
        {
            var siteNavigation = new SiteNavigation(_pages);

            if (!string.IsNullOrWhiteSpace(category))
                siteNavigation.CurrentCategoryKey = category;
            if(!string.IsNullOrWhiteSpace(page))
                siteNavigation.CurrentPageName = page;

            return PartialView("_Navigation", siteNavigation);
        }


        public ActionResult Page(string category, string page)
        {
            var documentationCategory = _pages.Category(category) ?? DocumentationCategory.Default;

            if(string.IsNullOrWhiteSpace(page))
            {
                var defaultPage = documentationCategory.DefaultPage;
                if (defaultPage != null)
                {
                    var routeData = new {category = defaultPage.Category.Name, page = defaultPage.Name};
                    return RedirectToAction("Page", routeData);
                }
            }

            var documentationPage = documentationCategory.Page(page);

        	if (documentationPage == null)
                return HttpNotFound();

            ViewData["category"] = documentationCategory.Name;
            ViewData["page"] = documentationPage.Name;

            return View("DocumentationPage", documentationPage);
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.DocumentationPages = _pages;
        }
    }
}