using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages
{
    [CollectionDataContract(Name = "newsDataPoints", Namespace = "")]
    public class NewsDataPointCollection : List<NewsDataPoint>
    {

    }
}