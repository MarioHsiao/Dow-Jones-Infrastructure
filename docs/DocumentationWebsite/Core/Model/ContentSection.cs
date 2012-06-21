using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation
{
    [DebuggerDisplay("{Name}")]
    public class ContentSection
    {
        public Name Name { get; private set; }

        public int? Ordinal { get; private set; }

        public ContentSection Parent { get; private set; }

        public IEnumerable<ContentSection> Children
        {
            get { return _children; }
        }
        private readonly IList<ContentSection> _children;


        public ContentSection(Name name = null, ContentSection parent = null, IEnumerable<ContentSection> children = null, int? ordinal = null)
        {
            Name = name;
            Ordinal = ordinal;
            Parent = parent;
            _children = new List<ContentSection>(children ?? Enumerable.Empty<ContentSection>());

            foreach (var child in _children)
            {
                child.Parent = this;
            }
        }


        public void Add(ContentSection child)
        {
            Contract.Requires(child != null);
            child.Parent = this;
            _children.Add(child);
        }

        public ContentSection Find(Name name)
        {
            if (name == null || name.Value == null || Children == null)
                return null;

            return Children.FirstOrDefault(s => name.Value.Equals(s.Name.Value, StringComparison.OrdinalIgnoreCase));
        }
    }
}