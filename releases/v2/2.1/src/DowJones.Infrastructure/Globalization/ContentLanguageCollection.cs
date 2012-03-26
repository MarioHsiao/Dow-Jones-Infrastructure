using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Globalization
{
    [CollectionDataContract(Name = "contentLanguages", ItemName = "contentLanguage", Namespace = "")]
    public class ContentLanguageCollection : SortedSet<string>
    {
        public ContentLanguageCollection()
        {
        }

        public ContentLanguageCollection(IEnumerable<string> range)
            : base(range)
        {
        }
    }
}