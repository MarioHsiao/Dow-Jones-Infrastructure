using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Tools.Charting.Data;

namespace DowJones.Utilities.Charting.Discovery
{
    public class DiscoveryBarChartDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private StackedBarSeries stackedBarSeries;
        
        /// <remarks/>
        [XmlElement("series")]
        public StackedBarSeries Series
        {
            get
            {
                if (stackedBarSeries == null)
                {
                    stackedBarSeries = new StackedBarSeries();
                }
                return stackedBarSeries;
            }
            set { stackedBarSeries = value; }
        }
    }
}
