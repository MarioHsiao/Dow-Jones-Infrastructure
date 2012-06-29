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

        public string AnchorId
        {
            get
            {
                return ContentSection.Name.Key.Replace(".", "");
            }
        }

        public bool Collapsible
        {
            get { return Key != "livedemo" && Key != "configuration"; }
        }

        public string DisplayName
        {
            get { return ContentSection.Name.DisplayName; }
        }

        public bool HasChildren
        {
            get { return Children != null && Children.Any(); }
        }

        public bool IsView
        {
            get { return !string.IsNullOrWhiteSpace(View); }
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
        
        public bool ShowHeader
        {
            get { return _showHeader.GetValueOrDefault(Key != "overview"); }
            set { _showHeader = value; }
        }   
        private bool? _showHeader;

        public IEnumerable<ContentSectionViewModel> Children
        {
            get { return _children.Value; }
        }
        private readonly Lazy<IEnumerable<ContentSectionViewModel>> _children;

        public virtual string View
        {
            get
            {
                if(_view == null)
                {
                    string view = null;

                    if (!HasChildren && ContentSection.Parent != null)
                        view = ContentSection.Parent.Name.Key;

                    _view = new Lazy<string>(() => view);
                }

                return _view.Value;
            }
            protected set
            {
                _view = new Lazy<string>(() => value);
            }
        }
        private Lazy<string> _view;



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
                (
                    from child in ContentSection.Children ?? Enumerable.Empty<ContentSection>()
                    where child != null && child.Name != null
                    select MapChild(child)
                ).ToArray();

            SetDefaultSelection(children);

            return children;
        }

        private static void SetDefaultSelection(IEnumerable<ContentSectionViewModel> children)
        {
			var childrenList = children.ToList();		// avoid multiple enumerations
			var hasSelected = childrenList.Any(x => x.Selected);

			if (!hasSelected && childrenList.Any())
				childrenList.First().Selected = true;
        }

        protected virtual ContentSectionViewModel MapChild(ContentSection child)
        {
            return new ContentSectionViewModel(child);
        }
    }
}