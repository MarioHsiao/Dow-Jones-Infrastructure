using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Tools.Charting.Data;

namespace DowJones.Tools.Charting.Common
{
    public class BubbleMapChartDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private BubbleMapSeries bubbleMapSeries;

        /// <remarks/>
        [XmlElement("series")]
        public BubbleMapSeries Series
        {
            get
            {
                if (bubbleMapSeries == null)
                {
                    bubbleMapSeries = new BubbleMapSeries();
                }
                return bubbleMapSeries;
            }
            set { bubbleMapSeries = value; }
        }
    }
}
