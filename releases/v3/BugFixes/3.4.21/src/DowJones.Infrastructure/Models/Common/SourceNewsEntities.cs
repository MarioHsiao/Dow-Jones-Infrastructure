using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [CollectionDataContract(Name = "sourceNewsEntities", ItemName = "sourceNewsEntity", Namespace = "")]
    public class SourceNewsEntities : List<SourceNewsEntity>
    {
    }
}
