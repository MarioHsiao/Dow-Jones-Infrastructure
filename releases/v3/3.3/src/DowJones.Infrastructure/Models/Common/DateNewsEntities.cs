using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [CollectionDataContract(Name = "dateNewsEntities", ItemName = "dateNewsEntity", Namespace = "")]
    public class DateNewsEntities : List<DateNewsEntity>
    {
    }
}
