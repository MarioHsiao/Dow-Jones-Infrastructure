using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class LineChart : Chart<PlotOptionsLine>
    {
        public LineChart(string clientId) : base(clientId, RenderType.line)
        {
        }
    }
}