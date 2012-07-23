using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Charting.HistoricalStockData
{
    [CollectionDataContract(Name = "stockDataPoints", Namespace = "")]
    public class StockDataPointCollection : List<StockDataPoint>
    {
        //public StockDataPointCollection()
        //{
        //}

        //public StockDataPointCollection(IEnumerable<StockDataPoint>collection)
        //    : base(collection)
        //{
        //}
    }
}