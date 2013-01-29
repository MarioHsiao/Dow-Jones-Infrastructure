using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Charting.HistoricalNewsData
{
    [CollectionDataContract(Name = "newsDataPoints", Namespace = "")]
    public class NewsDataPointCollection : List<NewsDataPoint>
    {

    }
}