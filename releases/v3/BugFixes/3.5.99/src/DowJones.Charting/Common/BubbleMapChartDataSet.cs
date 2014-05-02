using System;
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Common
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
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
