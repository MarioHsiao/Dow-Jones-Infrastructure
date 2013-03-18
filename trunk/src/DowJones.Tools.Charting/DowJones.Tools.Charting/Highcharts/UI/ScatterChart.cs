using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class ScatterChart : Chart
    {
        private PlotOptionsScatter _plotOptions;

        public ScatterChart(string clientId) : base(clientId, RenderType.scatter)
        {
        }

        public PlotOptionsScatter PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new PlotOptionsScatter()); }
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