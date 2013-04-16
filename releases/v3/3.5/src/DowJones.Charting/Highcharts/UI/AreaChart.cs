using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class AreaChart : Chart<PlotOptionsArea>
    {
        public AreaChart(string clientId) : base(clientId, RenderType.area)
        {
        }
    }
}