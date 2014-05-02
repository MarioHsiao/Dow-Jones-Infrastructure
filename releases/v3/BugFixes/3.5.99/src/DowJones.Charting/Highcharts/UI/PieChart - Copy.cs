using System.Text.RegularExpressions;
using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class PieChart : Chart<PlotOptionsPie>
    {
        public PieChart(string clientId) : base(clientId, RenderType.pie)
        {
        }
    }
}