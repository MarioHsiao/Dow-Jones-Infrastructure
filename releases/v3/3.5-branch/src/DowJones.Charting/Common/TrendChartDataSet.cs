using System;
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Common
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class TrendChartDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private TrendSeries TrendSeries;


        /// <remarks/>
        [XmlElement("series")]
        public TrendSeries Series
        {
            get
            {
                if (TrendSeries == null)
                {
                    TrendSeries = new TrendSeries();
                }
                return TrendSeries;
            }
            set { TrendSeries = value; }
        }
    }
}
