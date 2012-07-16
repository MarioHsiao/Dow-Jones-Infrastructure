using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace DowJones.Documentation
{
    [DebuggerDisplay("{Name}")]
    public class ContentSection
    {
        public Name Name { get; private set; }

        public int? Ordinal { get; private set; }

        public virtual ContentSection Parent { get; private set; }

        public virtual IEnumerable<ContentSection> Children
        {
            get { return _children; }
        }
        private readonly IList<ContentSection> _children;

        public virtual IEnumerable<ContentSection> RelatedTopics { get; private set; }

		public ContentSection(Name name = null, ContentSection parent = null, IEnumerable<ContentSection> children = null, IEnumerable<ContentSection> relatedTopics = null, int? ordinal = null)
        {
            Name = name;
            Ordinal = ordinal.HasValue ? ordinal : GetOrdinalFromName(name);
            Parent = parent;
			_children = (children ?? Enumerable.Empty<ContentSection>()).ToList();
			
            foreach (var child in _children)
            {
                child.Parent = this;
            }

			RelatedTopics = (relatedTopics ?? Enumerable.Empty<ContentSection>());
        }

        public virtual void Add(ContentSection child)
        {
            Contract.Requires(child != null);
            child.Parent = this;
            _children.Add(child);
        }

        public virtual ContentSection Find(Name name)
        {
            if (name == null || name.Value == null || Children == null)
                return null;

			return Children.FirstOrDefault(s => name.Key.Equals(s.Name.Key, StringComparison.OrdinalIgnoreCase))
					?? RelatedTopics.FirstOrDefault(s => name.Key.Equals(s.Name.Key, StringComparison.OrdinalIgnoreCase))
					?? Children.FirstOrDefault(s => name.DisplayKey.Equals(s.Name.DisplayKey, StringComparison.OrdinalIgnoreCase))
					?? RelatedTopics.FirstOrDefault(s => name.DisplayKey.Equals(s.Name.DisplayKey, StringComparison.OrdinalIgnoreCase))
					?? Children.FirstOrDefault(s => name.Value.Equals(s.Name.Value, StringComparison.OrdinalIgnoreCase))
					?? RelatedTopics.FirstOrDefault(s => name.Value.Equals(s.Name.Value, StringComparison.OrdinalIgnoreCase));
        }

        private int? GetOrdinalFromName(Name name)
        {
            if(name == null)
                return null;

            var match = Regex.Match(name.Value, "^(?<Ordinal>\\d+)[^\\d]");

            if (match.Success)
                return int.Parse(match.Groups["Ordinal"].Captures[0].Value);

            return null;
        }
    }
}