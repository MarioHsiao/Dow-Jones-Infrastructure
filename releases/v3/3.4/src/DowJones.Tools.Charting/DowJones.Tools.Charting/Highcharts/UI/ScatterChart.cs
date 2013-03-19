using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class ScatterChart : Chart<PlotOptionsScatter>
    {
        public ScatterChart(string clientId) : base(clientId, RenderType.scatter)
        {
        }
    }
}