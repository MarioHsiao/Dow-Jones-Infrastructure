using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class BarChart : Chart
    {
        private PlotOptionsBar _plotOptions;

        public BarChart(string clientId) : base(clientId, RenderType.bar)
        {
        }

        public PlotOptionsBar PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new PlotOptionsBar()); }
            set { _plotOptions = value; }
        }

        public sealed override string ToJson()
        {
            var tScript = base.ToJson();
            tScript = tScript.Replace("[@PlotOptions]", PlotOptions.ToString());
            return tScript;
        }
    }
}