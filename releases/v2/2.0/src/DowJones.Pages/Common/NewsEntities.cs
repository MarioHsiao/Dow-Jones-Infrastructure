using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [CollectionDataContract(Name = "newsEntities", ItemName = "newsEntity", Namespace = "")]
    public class NewsEntities : List<NewsEntity>
    {
    }
}