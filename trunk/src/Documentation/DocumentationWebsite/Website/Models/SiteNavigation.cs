using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    public class SiteNavigation
    {
        public IEnumerable<CategoryViewModel> Categories { get; private set; }

		public CategoryViewModel CurrentCategory { get; private set; }
		
        public PageViewModel CurrentPage
        {
            get { return _currentPage.Value; }
        }
        private readonly Lazy<PageViewModel> _currentPage;



        public SiteNavigation(IEnumerable<ContentSection> categories, string currentCategory = null, string currentPage = null)
        {
            Categories = (categories ?? Enumerable.Empty<ContentSection>()).Select(x => new CategoryViewModel(x, currentPage)).ToArray();
            _currentPage = new Lazy<PageViewModel>(() => GetCurrentPage(currentPage));
			CurrentCategory = GetCurrentCategory(currentCategory);
        }


        private CategoryViewModel GetCurrentCategory(string key = null)
        {
            var category =
                Categories.FirstOrDefault(
                    x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));

            if (category != null)
                category.Selected = true;
            
            return category;
        }

        private PageViewModel GetCurrentPage(string key = null)
        {
            var categoryPages = CurrentCategory.Pages.ToArray();

            var page = categoryPages.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
            page = page ?? categoryPages.FirstOrDefault();

            if(page != null)
                page.Selected = true;

            return page;
        }
    }
}