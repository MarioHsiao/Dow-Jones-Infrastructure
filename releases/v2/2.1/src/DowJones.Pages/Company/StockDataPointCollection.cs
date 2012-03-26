using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages.Company
{
    [CollectionDataContract(Name = "stockDataPoints", Namespace = "")]
    public class StockDataPointCollection : List<StockDataPoint>
    {
        public StockDataPointCollection()
        {
        }

        public StockDataPointCollection(IEnumerable<StockDataPoint>collection)
            : base(collection)
        {
        }
    }
}