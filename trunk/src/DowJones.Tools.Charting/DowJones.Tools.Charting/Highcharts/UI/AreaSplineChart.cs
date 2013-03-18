using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class AreaSplineChart : Chart
    {
        private PlotOptionsAreaSpline _plotOptions;

        public AreaSplineChart(string clientId) : base(clientId, RenderType.areaspline)
        {
        }

        public PlotOptionsAreaSpline PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new PlotOptionsAreaSpline()); }
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