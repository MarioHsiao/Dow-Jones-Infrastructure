using System;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.Data.Chart;
using DowJones.Tools.Charting.Highcharts.Core.Events;
using DowJones.Tools.Charting.Highcharts.UI;
using DowJones.Tools.Charting.Highcharts.UI.Util;
using NUnit.Framework;

namespace DowJones.Tools.Charting.Tests
{
    [TestFixture]
    public class GetBinaryUnitTest
    {
        [Test]
        public void CannedChartTestMethod()
        {
            var chartRequest = new ChartRequest
                              {
                                  MimeType = MimeType.Jpeg,
                                  Json = @"
{
	xAxis: {
		categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
	},
	series: [{
		data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]
	}]
};
                                  "
                              };
            var utilities = new ChartUtilities( );
            var bytes = utilities.GetBytes("http://montana.dev.us.factiva.com",
                                      "OldShowCase/dowjones.charting.handler.exporter.ashx",
                                      chartRequest);
            Assert.Less(0, bytes.Length);

        }

        [Test]
        public void GeneratedLineChartTestMethod()
        {
            var chart = new ColumnChart("container")
            {
                Title = new Title { Text = "Stacked Column Chart" },
                XAxis = new XAxis(new[] { new XAxisItem { Categories = new object[] { "One", "Two", "Three", "Four", "Five" } } }),
                PlotOptions =
                {
                    Stacking = Stacking.normal,
                    Events = new PlotOptionEvents { Click = "alert('hi dave');" }
                },
                Tooltip = { Formatter = null },
                Series = new SerieCollection(new[]
                                                                 {
                                                                     new Serie
                                                                         {
                                                                             Data = new object[] {29.9, 71.5, 106.4, 29.2, 144.0},
                                                                             Stack = 0,
                                                                         },
                                                                     new Serie
                                                                         {
                                                                             Data = new object[] {30.0, 176.5, 135.4, 148.2, 216.4},
                                                                             Stack = 0,
                                                                         },
                                                                     new Serie
                                                                         {
                                                                             Data = new object[] {29.9, 71.5, 106.4, 29.2, 144.0},
                                                                             Stack = 1,
                                                                         },
                                                                     new Serie
                                                                         {
                                                                             Data = new object[] {29.9, 71.5, 106.4, 29.2, 144.0},
                                                                             Stack = 1,
                                                                         }
                                                                 })
            };

            var chartRequest = new ChartRequest
            {
                MimeType = MimeType.Jpeg,
                Json = chart.ToJson(),
            };
            var utilities = new ChartUtilities();
            var bytes = utilities.GetBytes("http://montana.dev.us.factiva.com",
                                      "OldShowCase/dowjones.charting.handler.exporter.ashx",
                                      chartRequest);
            Assert.Less(0, bytes.Length);
        }
    }
}
