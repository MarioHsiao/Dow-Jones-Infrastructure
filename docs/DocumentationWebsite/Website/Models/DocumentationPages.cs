using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    public class DocumentationPages : IEnumerable<DocumentationPage>
    {
        private readonly IList<DocumentationCategory> _categories;

        public IEnumerable<DocumentationCategory> Categories
        {
            get { return _categories; }
        }


        public DocumentationPages(IEnumerable<DocumentationPage> pages = null)
        {
            _categories = new List<DocumentationCategory>();

            foreach (var page in pages ?? Enumerable.Empty<DocumentationPage>())
            {
                AddPage(page);
            }
        }

        public DocumentationPages(IEnumerable<DocumentationCategory> categories)
        {
            _categories = new List<DocumentationCategory>(categories ?? Enumerable.Empty<DocumentationCategory>());
        }


        public void AddPage(DocumentationPage page)
        {
            if (page == null)
                return;

            string categoryName = (page.Category == null) ? null : page.Category.Name;

            AddPage(categoryName, page);
        }

        public void AddPage(string categoryName, DocumentationPage page)
        {
            if (page == null)
                return;

            var existingCategory = Category(categoryName);

            AddPage(existingCategory ?? page.Category, page);
        }

        public void AddPage(DocumentationCategory category, DocumentationPage page)
        {
            if(page == null)
                return;

            category = category ?? DocumentationCategory.Default;

            if(!_categories.Contains(category))
                _categories.Add(category);
            
            category.Add(page);
        }

        public DocumentationCategory Category(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                return DocumentationCategory.Default;

            return _categories.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerator<DocumentationPage> GetEnumerator()
        {
            return _categories.SelectMany(x => x.Pages).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DocumentationPage Page(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            IEnumerable<DocumentationPage> pages = this;

            var split = name.Split(':');
            if(split.Length == 2)
            {
                var category = Category(split[0]);
                if(category != null)
                {
                    name = split[1];
                    pages = category.Pages;
                }
            }

            return pages.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}