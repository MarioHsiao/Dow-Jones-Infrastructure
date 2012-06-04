using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    [DebuggerDisplay("{Name}")]
    public class DocumentationCategory
    {
        public static readonly DocumentationCategory Default = new DocumentationCategory();


        public string DisplayName { get; set; }

        public string Name { get; set; }

        public IEnumerable<DocumentationPage> Pages
        {
            get { return _pages; }
        }
        private readonly IList<DocumentationPage> _pages;


        public DocumentationCategory(Name name = null, IEnumerable<DocumentationPage> pages = null)
        {
            _pages = new List<DocumentationPage>();

            foreach (var page in pages ?? Enumerable.Empty<DocumentationPage>())
            {
                Add(page);
            }

            if (name == null)
                return;

            DisplayName = name.DisplayName;
            Name = name.Value.ToLower();
        }


        public void Add(DocumentationPage page)
        {
            Contract.Requires(page != null);
            page.Category = this;
            _pages.Add(page);
        }

        public DocumentationPage Page(string page)
        {
            return Pages.FirstOrDefault(x => string.Equals(x.Name, page, StringComparison.OrdinalIgnoreCase));
        }
    }
}