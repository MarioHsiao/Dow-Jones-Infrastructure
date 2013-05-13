using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI.Generators
{
    public class HighchartsScriptGenerator<T> : IScriptGenerator where T : PlotOptionsSeries, new()
    {
        private readonly Chart<T> _chart;
        private const string HighchartScript = @"var chart[@Id] = new Highcharts.Chart([@Json]);";

        public HighchartsScriptGenerator(Chart<T> chart)
        {
            _chart = chart;
        }

        public string RenderScript()
        {
            var tmp = HighchartScript.Replace("[@Id]", string.Concat("_", _chart.ClientId));
            return tmp.Replace("[@Json]", _chart.ToJson());
        }
    }
}