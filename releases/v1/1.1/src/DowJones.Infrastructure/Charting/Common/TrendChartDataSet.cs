using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Tools.Charting.Data;

namespace DowJones.Tools.Charting.Common
{
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
