using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    [DebuggerDisplay("{DisplayName}")]
    public class ContentSectionViewModel
    {
        protected ContentSection ContentSection { get; private set; }

        public string DisplayName
        {
            get { return ContentSection.Name.DisplayName; }
        }

        public bool HasChildren
        {
            get { return Children != null && Children.Any(); }
        }

        public string Key
        {
            get { return ContentSection.Name.Key; }
        }

        public int Ordinal
        {
            get { return ContentSection.Ordinal.GetValueOrDefault(); }
        }

        public bool Selected { get; set; }

        public IEnumerable<ContentSectionViewModel> Children
        {
            get { return _children.Value; }
        }
        private readonly Lazy<IEnumerable<ContentSectionViewModel>> _children;


        public ContentSectionViewModel(ContentSection section)
        {
            Contract.Requires(section != null);
            ContentSection = section;
            _children = new Lazy<IEnumerable<ContentSectionViewModel>>(MapChildren);
        }

        public bool HasChild(Name name)
        {
            if (name == null || !name.IsValid())
                return false;

            return HasChildren && Children.Any();
        }

        protected virtual IEnumerable<ContentSectionViewModel> MapChildren()
        {
            var children =
                from child in ContentSection.Children ?? Enumerable.Empty<ContentSection>()
                where child != null && child.Name != null
                select MapChild(child);

            return children;
        }

        protected virtual ContentSectionViewModel MapChild(ContentSection child)
        {
            return new ContentSectionViewModel(child);
        }
    }
}