using System.Diagnostics;
using System.Diagnostics.Contracts;

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

        public string Key
        {
            get { return ContentSection.Name.Key; }
        }

        public string Mode
        {
            get { return ContentSection.Name.Prefix; }
        }

        public int Ordinal
        {
            get { return ContentSection.Ordinal.GetValueOrDefault(); }
        }

        public bool Selected { get; set; }


        public ContentSectionViewModel(ContentSection section)
        {
            Contract.Requires(section != null);
            ContentSection = section;
        }
    }
}