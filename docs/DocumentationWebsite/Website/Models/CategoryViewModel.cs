using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    public class CategoryViewModel : ContentSectionViewModel
    {
        private readonly string _currentPage;

        public IEnumerable<PageViewModel> Pages
        {
            get { return _pages.Value; }
        }
        private readonly Lazy<IEnumerable<PageViewModel>> _pages;

        public CategoryViewModel(ContentSection section, string currentPage = null)
            : base(section)
        {
            _currentPage = currentPage;
            _pages = new Lazy<IEnumerable<PageViewModel>>(MapPages);
        }

        public IEnumerable<IEnumerable<PageViewModel>> GetPageGroups(int groupSize = 5)
        {
            if (Pages == null)
                yield break;

            IEnumerable<PageViewModel> group;
            int skip = 0;

            do
            {
                @group = Pages.Skip(skip).Take(groupSize).ToArray();
                skip += groupSize;
                yield return @group;
            } while (@group.Any());
        }

        private IEnumerable<PageViewModel> MapPages()
        {
            var pages =
                from child in ContentSection.Children
                let selected = string.Equals(child.Name.Key, _currentPage, StringComparison.OrdinalIgnoreCase)
                select new PageViewModel(child, this) { Selected = selected };

            return pages;
        }
    }
}