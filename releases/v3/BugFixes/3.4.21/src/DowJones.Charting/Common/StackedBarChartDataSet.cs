using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Common
{
    public class StackedBarChartDataSet
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
