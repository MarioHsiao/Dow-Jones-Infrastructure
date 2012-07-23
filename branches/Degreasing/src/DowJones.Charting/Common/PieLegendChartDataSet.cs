using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Common
{
    public class PieLegendChartDataSet
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
