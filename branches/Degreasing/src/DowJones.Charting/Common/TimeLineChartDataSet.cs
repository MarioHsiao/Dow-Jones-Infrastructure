/* 
 * Author: Infosys
 * Date: 8/3/09
 * Purpose: Dataset for Time Line Graph.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */
using System.Collections.Generic;
using System.ComponentModel;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Common
{
    public class TimeLineChartDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<TimeSeries> timeSeries;

        public List<TimeSeries> TimeSeries
        {
            get
            {
                if (timeSeries == null)
                {
                    timeSeries = new List<TimeSeries>();
                }
                return timeSeries;
            }
            set { timeSeries = value; }
        }
    }
}
