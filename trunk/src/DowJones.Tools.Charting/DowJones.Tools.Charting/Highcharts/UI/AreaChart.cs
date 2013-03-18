using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class AreaChart : Chart
    {
        private PlotOptionsArea _plotOptions;

        public AreaChart(string clientId) : base(clientId, RenderType.area)
        {
        }

        public PlotOptionsArea PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new PlotOptionsArea()); }
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