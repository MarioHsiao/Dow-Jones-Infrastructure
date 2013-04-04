using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class BarChart : Chart<PlotOptionsBar>
    {
        public BarChart(string clientId) : base(clientId, RenderType.bar)
        {
        }
    }
}