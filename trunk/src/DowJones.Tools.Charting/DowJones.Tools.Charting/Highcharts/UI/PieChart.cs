using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class PieChart : Chart
    {
        private PlotOptionsPie _plotOptions;

        public PieChart(string clientId) : base(clientId, RenderType.pie)
        {
        }

        public PlotOptionsPie PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new PlotOptionsPie()); }
            set { _plotOptions = value; }
        }

        public override string ToJson()
        {
            var tScript = base.ToJson();
            tScript = tScript.Replace("[@PlotOptions]", PlotOptions.ToString());
            return tScript;
        }
    }
}