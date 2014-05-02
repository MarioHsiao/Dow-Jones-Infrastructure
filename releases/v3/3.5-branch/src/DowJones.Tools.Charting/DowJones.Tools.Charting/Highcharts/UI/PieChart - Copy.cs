using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class PieChart : Chart<PlotOptionsPie>
    {
        public PieChart(string clientId) : base(clientId, RenderType.pie)
        {
        }
    }
}