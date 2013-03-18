using System;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.Data.Chart;
using DowJones.Tools.Charting.Highcharts.UI;
using NUnit.Framework;

namespace DowJones.Tools.Charting.Tests
{
    [TestFixture]
    public class ChartUnitTest
    {
        [Test]
        public void ColumnChart()
        {
            var chart = new ColumnChart("container")
                            {
                                Title = new Title {Text = "Stacked Column Chart"},
                                XAxis = new XAxis(new[] {new XAxisItem {Categories = new object[] {"One", "Two", "Three", "Four", "Five"}}}),
                                PlotOptions = {Stacking = Stacking.normal},
                                Tooltip = {Formatter = null},
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

            Console.Write(chart.ToJson());
            Assert.AreEqual(chart.ToJson(), "{chart: {\"renderTo\":\"container\",\"defaultSeriesType\":\"column\"},credits: { enabled: false },plotOptions: { series: {\"stacking\":\"normal\"} },title: {\"text\":\"Stacked Column Chart\"},exporting: {\"enabled\":false},xAxis: [{\"categories\":[\"One\",\"Two\",\"Three\",\"Four\",\"Five\"],\"title\":{\"text\":\"\"}}],series: [{\"data\":[29.9,71.5,106.4,29.2,144.0],\"stack\":0},{\"data\":[30.0,176.5,135.4,148.2,216.4],\"stack\":0},{\"data\":[29.9,71.5,106.4,29.2,144.0],\"stack\":1},{\"data\":[29.9,71.5,106.4,29.2,144.0],\"stack\":1}]}");
        }

        [Test]
        public void LineChart()
        {
            var chart = new LineChart("container")
                            {
                                Appearance =
                                    {
                                        MarginRight = 130,
                                        MarginBottom = 25
                                    },
                                Title = new Title("Monthly Average Temperature")
                                            {
                                                X = -20,
                                            },
                                Tooltip = {Formatter = null},
                            };

            Console.Write(chart.ToJson());
            Assert.AreEqual(chart.ToJson(),"{chart: {\"renderTo\":\"container\",\"defaultSeriesType\":\"line\",\"marginRight\":130,\"marginBottom\":25},credits: { enabled: false },plotOptions: { series: {} },title: {\"text\":\"Monthly Average Temperature\",\"x\":-20},exporting: {\"enabled\":false},series: []}");
        }
    }
}