using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class AreaSplineChart : Chart<PlotOptionsAreaSpline>
    {
        public AreaSplineChart(string clientId)
            : base(clientId, RenderType.areaspline)
        {
        }
    }
}