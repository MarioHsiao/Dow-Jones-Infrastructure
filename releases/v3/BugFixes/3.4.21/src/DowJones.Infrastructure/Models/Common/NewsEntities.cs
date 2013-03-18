using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [CollectionDataContract(Name = "newsEntities", ItemName = "newsEntity", Namespace = "")]
    public class NewsEntities : List<NewsEntity>
    {
    }
}