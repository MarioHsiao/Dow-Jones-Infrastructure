using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    public class SiteNavigation
    {
        private readonly DocumentationPages _pages;

        public IEnumerable<SiteNavigationGroup> Categories { get; private set; }

        public SiteNavigationGroup CurrentCategory
        {
            get
            {
                var category = Categories.FirstOrDefault(x => string.Equals(x.Name, CurrentCategoryKey, StringComparison.OrdinalIgnoreCase));
                return category ?? new SiteNavigationGroup(DocumentationCategory.Default);
            }
        }

        public string CurrentCategoryKey { get; set; }

		public DocumentationPage CurrentPage
		{
			get
			{
				var page = _pages.FirstOrDefault(x => string.Equals(x.Name, CurrentPageName, StringComparison.OrdinalIgnoreCase));

			    page = page ?? CurrentCategory.Groups.SelectMany(x => x).FirstOrDefault();
                
				return page ?? new DocumentationPage();
			}
		}

		public string CurrentPageName { get; set; }

        public SiteNavigation(DocumentationPages pages)
        {
            _pages = pages;
            Categories = pages.Categories.Select(x => new SiteNavigationGroup(x)).ToArray();
//            CurrentCategoryKey = pages.Categories.Select(x => x.Name).FirstOrDefault();
        }


        [DebuggerDisplay("{Name}")]
        public class SiteNavigationGroup
        {
            private readonly DocumentationCategory _category;

            public string Name
            {
                get { return _category.Name; }
            }

            public string DisplayName
            {
                get { return _category.DisplayName; }
            }

            public IEnumerable<IEnumerable<DocumentationPage>> Groups { get; private set; }


            public SiteNavigationGroup(DocumentationCategory category)
            {
                _category = category;
                Groups = GetPageGroups(category).ToArray();
            }

            private IEnumerable<IEnumerable<DocumentationPage>> GetPageGroups(DocumentationCategory category, int groupSize = 5)
            {
                if(category.Pages == null)
                    yield break;

                IEnumerable<DocumentationPage> group;
                int skip = 0;

                do
                {
                    group = category.Pages.Skip(skip).Take(groupSize).ToArray();
                    skip += groupSize;
                    yield return group;
                } while (group.Any());
            }
        }
    }
}