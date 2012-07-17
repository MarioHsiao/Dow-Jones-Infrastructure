using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.VideoPlayer
{
    [DataContract(Name = "clipCollection", Namespace = "")]
    public class ClipCollection : List<Clip>
    {
        public ClipCollection(IEnumerable<Clip> collection) : base(collection) {}

        public ClipCollection() {}
    }
}