using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class ColumnChart : Chart<PlotOptionsColumn>
    {
        public ColumnChart(string clientId) : base(clientId, RenderType.column)
        {
        }
    }
}