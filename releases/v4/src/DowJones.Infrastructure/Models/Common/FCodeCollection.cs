using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [CollectionDataContract(Name = "fcodes", ItemName = "fcode", Namespace = "")]
    public class FCodeCollection : List<string>
    {
    }
}