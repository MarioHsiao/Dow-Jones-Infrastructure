using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages.Company
{
    [CollectionDataContract(Name = "fcodes", ItemName = "fcode", Namespace = "")]
    public class FCodeCollection : List<string>
    {
    }
}