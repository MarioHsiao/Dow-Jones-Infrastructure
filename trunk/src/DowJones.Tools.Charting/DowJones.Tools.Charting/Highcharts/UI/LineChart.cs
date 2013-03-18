using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class LineChart : Chart
    {
        private PlotOptionsLine _plotOptionsLine;

        public LineChart(string clientId) : base(clientId, RenderType.line)
        {
        }

        public PlotOptionsLine PlotOptions
        {
            get { return _plotOptionsLine ?? (_plotOptionsLine = new PlotOptionsLine()); }
            set { _plotOptionsLine = value; }
        }

        public override string ToJson()
        {
            var tScript = base.ToJson();
            tScript = tScript.Replace("[@PlotOptions]", PlotOptions.ToString());
            return tScript;
        }
    }
}