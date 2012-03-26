using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [CollectionDataContract(Name = "fcodes", ItemName = "fcode", Namespace = "")]
    public class FcodeCollection : List<string>
    {
        public FcodeCollection()
        {
        }

        public FcodeCollection(IEnumerable<string> codes)
            : base(codes)
        {
        }
    }
}