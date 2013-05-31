using System.Text.RegularExpressions;
using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class ScatterChart : Chart<PlotOptionsScatter>
    {
        public ScatterChart(string clientId) : base(clientId, RenderType.scatter)
        {
        }
    }
}