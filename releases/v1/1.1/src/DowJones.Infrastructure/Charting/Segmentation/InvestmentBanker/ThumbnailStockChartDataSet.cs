using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Tools.Charting.Data;

namespace DowJones.Tools.Charting.Segmentation.InvestmentBanker
{
    public class ThumbnailStockChartDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<TimeItem> items;


        /// <remarks/>
        [XmlElement("items")]
        public List<TimeItem> Items
        {
            get
            {
                if ((items == null))
                {
                    items = new List<TimeItem>();
                }
                return items;
            }
            set { items = value; }
        }
    }
}
