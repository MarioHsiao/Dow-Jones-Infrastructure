using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    [DebuggerDisplay("{Name}")]
    public class DocumentationPageSection
    {
        public string DisplayName { get; set; }

        public bool HasSections
        {
            get { return Sections != null && Sections.Any(); }
        }

        public string Name { get; set; }

        public IEnumerable<DocumentationPageSection> Sections { get; set; }


        public DocumentationPageSection(Name name = null, IEnumerable<DocumentationPageSection> sections = null)
        {
            Sections = sections ?? Enumerable.Empty<DocumentationPageSection>();

            if (name == null)
                return;

            DisplayName = name.DisplayName;
            Name = name.Value.ToLower();
        }


        public bool HasSection(string name)
        {
            return HasSections && Sections.Any(s => name.Equals(s.Name, StringComparison.OrdinalIgnoreCase));
        }

        public DocumentationPageSection Section(string name)
        {
            if(string.IsNullOrWhiteSpace(name) || !HasSections)
                return null;

            return Sections.FirstOrDefault(s => name.Equals(s.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}