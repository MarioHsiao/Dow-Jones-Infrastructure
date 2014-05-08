/*
 * Author: Infosys
 * Date: 8/06/09
 * Purpose: Generate Itxml for BAR Chart.
 * 
 */

using System;
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Fcp
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class BarChartDataSet
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
