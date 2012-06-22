using System.Collections.Generic;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    public class PageViewModel : ContentSectionViewModel
    {
        public CategoryViewModel Category { get; private set; }

        public IEnumerable<ContentSectionViewModel> FilteredChildren
        {
            get { return Children.Where(x => string.IsNullOrWhiteSpace(x.Mode) || x.Mode == Mode); }
        }

        public PageViewModel(ContentSection section, CategoryViewModel category, string mode)
            : base(section)
        {
            Category = category;
            Mode = mode;
        }
    }
}