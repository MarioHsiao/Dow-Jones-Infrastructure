using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class LineChart : Chart<PlotOptionsLine>
    {
        public LineChart(string clientId) : base(clientId, RenderType.line)
        {
        }
    }
}