using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class AreaSplineChart : Chart<PlotOptionsAreaSpline>
    {
        public AreaSplineChart(string clientId)
            : base(clientId, RenderType.areaspline)
        {
        }
    }
}