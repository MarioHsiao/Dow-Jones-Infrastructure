using System.Collections.Generic;
using System.Diagnostics;

namespace DowJones.Documentation.Website.Models
{
    [DebuggerDisplay("{Name}")]
    public class DocumentationPage : DocumentationPageSection
    {
        public DocumentationCategory Category { get; set; }

        public DocumentationPage(Name name = null, IEnumerable<DocumentationPageSection> sections = null)
            : base(name, sections)
        {
            Category = new DocumentationCategory();
        }
}
}