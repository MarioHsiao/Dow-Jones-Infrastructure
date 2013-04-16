using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class DateTimeLineChart : Chart<PlotOptionsDateTimeLine>
    {
        public DateTimeLineChart(string clientId) : base(clientId, RenderType.line)
        {
        }
    }
}