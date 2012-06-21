using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    public class PageViewModel : ContentSectionViewModel
    {
        public CategoryViewModel Category { get; private set; }

        public bool HasSections
        {
            get { return Sections != null && Sections.Any(); }
        }

        public IEnumerable<ContentSectionViewModel> Sections
        {
            get { return _sections.Value; }
        }
        private readonly Lazy<IEnumerable<ContentSectionViewModel>> _sections;


        public PageViewModel(ContentSection section, CategoryViewModel category)
            : base(section)
        {
            Category = category;
            _sections = new Lazy<IEnumerable<ContentSectionViewModel>>(MapSections);
        }


        public bool HasSection(Name name)
        {
            if (name == null || !name.IsValid())
                return false;

            return HasSections && Sections.Any();
        }

        private IEnumerable<ContentSectionViewModel> MapSections()
        {
            var sections =
                from section in ContentSection.Children ?? Enumerable.Empty<ContentSection>()
                where section != null && section.Name != null
                let mode = section.Name.Prefix
                where string.IsNullOrWhiteSpace(mode)
                      || mode.Equals(Mode, StringComparison.OrdinalIgnoreCase)
                select new ContentSectionViewModel(section);

            return sections;
        }
    }
}