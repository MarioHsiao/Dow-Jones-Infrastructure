using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class AreaChart : Chart<PlotOptionsArea>
    {
        public AreaChart(string clientId) : base(clientId, RenderType.area)
        {
        }
    }
}