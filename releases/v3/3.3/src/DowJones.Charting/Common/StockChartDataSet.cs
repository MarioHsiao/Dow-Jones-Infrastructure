using System.Collections.Generic;
using System.ComponentModel;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Common
{
   public class StockChartDataSet
   {
       [EditorBrowsable(EditorBrowsableState.Never)]
       private List<StockSeries> stockSeries;

       public List<StockSeries> StockSeries
       {
           get
           {
               if (stockSeries == null)
               {
                   stockSeries = new List<StockSeries>();
               }
               return stockSeries;
           }
           set { stockSeries = value; }
       }
   }
}
