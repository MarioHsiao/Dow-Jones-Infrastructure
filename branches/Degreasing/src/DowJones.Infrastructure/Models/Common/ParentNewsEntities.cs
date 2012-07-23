using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [CollectionDataContract(Name = "parentNewsEntities", ItemName = "parentNewsEntity", Namespace = "")]
    public class ParentNewsEntities : List<ParentNewsEntity>
    {
    }
}