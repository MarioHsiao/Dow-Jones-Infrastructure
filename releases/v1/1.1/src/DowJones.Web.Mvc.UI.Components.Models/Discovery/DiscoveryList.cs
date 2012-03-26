using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;

namespace DowJones.Web.Mvc.UI.Components.Models.Discovery
{
    /// <summary>
    /// Contains discovery buckets, allow for transoforming buckets.
    /// </summary>
    public class DiscoveryList 
    {
        public List<DiscoveryItem> DiscoveryItems { get; set; }
        public DiscoveryChartTypes ChartType { get; set; }
        public string Title { get; set; }
        public double Top { get; set; }
        public int Total { get; set; }
        public string DataTypeCode { get; set; }
        
        internal DiscoveryList()
        { 
            //for internal transformations.
            this.DiscoveryItems = new List<DiscoveryItem>();
        }

        public DiscoveryList(string title, DiscoveryChartTypes chartType) : this()
        {
            this.Title = title;
            this.ChartType = chartType;
        }
        
        public virtual void CalculateShares()
        {
            this.Top = (from s in this.DiscoveryItems orderby s.Count descending select s).FirstOrDefault().ShareCount;
            dynamic total = 0;

            foreach (var item in DiscoveryItems)
            {
                try
                {
                    dynamic a = item.ShareCount;
                    dynamic b = this.Top;
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
                this.Total = total;
            }
            catch
            {

            }
        }

        
        


    }
}
