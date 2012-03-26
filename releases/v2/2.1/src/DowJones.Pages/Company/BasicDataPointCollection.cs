using System.Collections.Generic;

namespace DowJones.Pages.Company
{
    public class BasicDataPointCollection : List<BasicDataPoint>
    {
        public BasicDataPointCollection()
        {
        }

        public BasicDataPointCollection(IEnumerable<StockDataPoint> collection)
            : base(collection)
        {
        }
    }
}