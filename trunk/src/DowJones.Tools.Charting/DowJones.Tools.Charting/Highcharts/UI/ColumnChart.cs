using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class ColumnChart : Chart
    {
        private PlotOptionsColumn _plotOptions;

        public ColumnChart(string clientId) : base(clientId, RenderType.column)
        {
        }

        public PlotOptionsColumn PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new PlotOptionsColumn()); }
            set { _plotOptions = value; }
        }

        public override string ToJson()
        {
            var tScript = base.ToJson();
            tScript = tScript.Replace("[@PlotOptions]", PlotOptions.ToString());
            return tScript;
        }
    }
}