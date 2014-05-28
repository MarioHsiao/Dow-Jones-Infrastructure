using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class ColumnChart : Chart<PlotOptionsColumn>
    {
        public ColumnChart(string clientId) : base(clientId, RenderType.column)
        {
        }
    }
}