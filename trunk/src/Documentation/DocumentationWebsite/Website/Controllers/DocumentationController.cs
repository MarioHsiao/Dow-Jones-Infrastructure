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

        [OutputCache(Duration = 3600)]
        public ActionResult Homepage()
        {
            return View();
        }

		[OutputCache(Duration = 3600)]
		public ActionResult Forum()
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


        [OutputCache(Duration = 3600, VaryByParam = "category;page")]
        public ActionResult Page(string category, string page)
        {
            var documentationPage = _repository.GetPage(page ?? string.Empty, category);

            if (documentationPage == null)
                return RedirectToDefaultCategoryPage(category);

            var documentationCategory = documentationPage.Parent 
                                            ?? _repository.GetCategory(category) 
                                            ?? new ContentSection(category);

            ViewData["category"] = documentationCategory.Name;
            ViewData["page"] = documentationPage.Name;

            var categoryViewModel = new CategoryViewModel(documentationCategory, page);
            var viewModel = new PageViewModel(documentationPage, categoryViewModel);

            return View("DocumentationPage", viewModel);
        }


        private ActionResult RedirectToDefaultCategoryPage(string category)
        {
            var documentationCategory = _repository.GetCategory(category) ?? new ContentSection(new Name(string.Empty));

            if (documentationCategory.Children.Any())
            {
                var routeData = new LinkExtensions.DocumentationBrowserRouteData
                {
                    category = documentationCategory.Name.DisplayKey,
                    page = documentationCategory.Children.First().Name.DisplayKey,
                };

                return RedirectToRoute("DocumentationBrowser", routeData);
            }

            return HttpNotFound();
        }
    }
}