using System;
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Common
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
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
