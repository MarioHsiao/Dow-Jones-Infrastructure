using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Ajax.TagCloud;

namespace DowJones.Pages
{
    [CollectionDataContract(Name = "tags", ItemName = "tag", Namespace = "")]
    public class TagCollection : List<ITag>
    {
        public TagCollection()
            : base()
        {
        }

        public TagCollection(IEnumerable<ITag> tags)
            : base(tags)
        {
        }
    }
}