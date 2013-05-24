using System.Text.RegularExpressions;
using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class DateTimeLineChart : Chart<PlotOptionsDateTimeLine>
    {
        public DateTimeLineChart(string clientId) : base(clientId, RenderType.line)
        {
        }
    }
}