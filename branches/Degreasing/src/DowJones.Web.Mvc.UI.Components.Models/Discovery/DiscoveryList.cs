using System.Collections.ObjectModel;
using System.Linq;

namespace DowJones.Web.Mvc.UI.Components.Discovery
{
    /// <summary>
    /// Contains discovery buckets, allow for transoforming buckets.
    /// </summary>
    public class DiscoveryList
    {
        public Collection<DiscoveryItem> DiscoveryItems { get; set; }
        public DiscoveryChartTypes ChartType { get; set; }
        public string Title { get; set; }
        public double Top { get; set; }
        public int Total { get; set; }
        public string DataTypeCode { get; set; }

        internal DiscoveryList()
        {
            //for internal transformations.
            DiscoveryItems = new Collection<DiscoveryItem>();
        }

        public DiscoveryList(string title, DiscoveryChartTypes chartType)
            : this()
        {
            Title = title;
            ChartType = chartType;
        }

        public virtual void CalculateShares()
        {
            Top = (from s in DiscoveryItems orderby s.Count descending select s).FirstOrDefault().ShareCount;
            dynamic total = 0;

            foreach (var item in DiscoveryItems)
            {
                try
                {
                    dynamic a = item.ShareCount;
                    dynamic b = Top;
                    dynamic c = (0.010 * a) / (0.010 * b);
                    total = total + a;
                    item.ShareOfTop = (double)(c * 100);
                }
                catch
                {
                    dynamic d = -1;
                    item.ShareOfTop = d;
                }
            }

            try
            {
                Total = total;
            }
            catch
            {

            }
        }





    }
}
