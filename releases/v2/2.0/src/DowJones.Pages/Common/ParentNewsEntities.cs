using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [CollectionDataContract(Name = "parentNewsEntities", ItemName = "parentNewsEntity", Namespace = "")]
    public class ParentNewsEntities : List<ParentNewsEntity>
    {
    }
}