using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    [DataContract(Name = "clipCollection", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "clipCollection")]
    public class ClipCollection : List<Clip>
    {
        public ClipCollection(IEnumerable<Clip> collection) : base(collection) {}

        public ClipCollection() {}
    }
}