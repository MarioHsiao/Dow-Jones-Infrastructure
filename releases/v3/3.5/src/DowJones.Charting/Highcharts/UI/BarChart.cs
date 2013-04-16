using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class BarChart : Chart<PlotOptionsBar>
    {
        public BarChart(string clientId) : base(clientId, RenderType.bar)
        {
        }
    }
}