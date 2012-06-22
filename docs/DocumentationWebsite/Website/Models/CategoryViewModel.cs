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
            get { return Children.OfType<PageViewModel>(); }
        }


        public CategoryViewModel(ContentSection section, string currentPage = null)
            : base(section)
        {
            _currentPage = currentPage;
        }


        public IEnumerable<IEnumerable<PageViewModel>> GetPageGroups(int groupSize = 5)
        {
            var pages = Pages.ToArray();

            IEnumerable<PageViewModel> group;
            int skip = 0;

            do
            {
                @group = pages.Skip(skip).Take(groupSize).ToArray();
                skip += groupSize;
                yield return @group;
            } while (@group.Any());
        }

        protected override IEnumerable<ContentSectionViewModel> MapChildren()
        {
            var children = base.MapChildren().ToArray();

            var selectedPage =
                children.FirstOrDefault(child => string.Equals(child.Key, _currentPage, StringComparison.OrdinalIgnoreCase));

            if(selectedPage != null)
                selectedPage.Selected = true;

            return children;
        }

        protected override ContentSectionViewModel MapChild(ContentSection child)
        {
            return new PageViewModel(child, this, null);
        }
    }
}