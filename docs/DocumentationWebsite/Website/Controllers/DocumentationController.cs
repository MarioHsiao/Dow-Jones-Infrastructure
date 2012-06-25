using System.Linq;
using System.Web.Mvc;
using DowJones.Documentation.DataAccess;
using DowJones.Documentation.Website.Extensions;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly IContentRepository _repository;

        public DocumentationController()
            : this(MvcApplication.ContentRepository)
        {
        }

        public DocumentationController(IContentRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Homepage()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Navigation(string category, string page)
        {
            var categories = _repository.GetCategories();
            var siteNavigation = new SiteNavigation(categories, category, page);

            return PartialView("_Navigation", siteNavigation);
        }


        public ActionResult Page(string category, string page)
        {
            var documentationCategory = _repository.GetCategory(category) ?? new ContentSection(new Name(string.Empty));

            if (string.IsNullOrWhiteSpace(page) && documentationCategory.Children.Any())
            {
                var routeData = new LinkExtensions.DocumentationBrowserRouteData
                    {
                        category = category,
                        page = documentationCategory.Children.First().Name.Key,
                    };

                return RedirectToRoute("DocumentationBrowser", routeData);
            }

            var documentationPage = documentationCategory.Find(page) ?? documentationCategory.Children.FirstOrDefault();

        	if (documentationPage == null)
                return HttpNotFound();

            ViewData["category"] = documentationCategory.Name;
            ViewData["page"] = documentationPage.Name;

            var categoryViewModel = new CategoryViewModel(documentationCategory, page);
            var viewModel = new PageViewModel(documentationPage, categoryViewModel);

            return View("DocumentationPage", viewModel);
        }
    }
}