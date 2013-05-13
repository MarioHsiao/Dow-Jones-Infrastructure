/*
 * Author: Jagadish - JR
 * Date: 7/22/09
 * Purpose: Generate Itxml for Pie Chart.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 * Rajendra G. Kulkarni - RGK           7/28/2009               Changes after code review on 7/27/2009
 *                                                              - Renamed DJCE as Screening, ScreeingPie as Pie
 */
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Fcp
{
    public class ScreeningPieChartDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private PieSeries pieSeries;

        /// <remarks/>
        [XmlElement("series")]
        public PieSeries Series
        {
            get
            {
                if (pieSeries == null)
                {
                    pieSeries = new PieSeries();
                }
                return pieSeries;
            }
            set { pieSeries = value; }
        }
    }
}
