using System;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.Data.Chart;
using DowJones.Tools.Charting.Highcharts.Core.Events;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;
using DowJones.Tools.Charting.Highcharts.UI;
using NUnit.Framework;

namespace DowJones.Tools.Charting.Tests
{
    [TestFixture]
    public class GeneralUiChartsUnitTest
    {
        [Test]
        public void ColumnChart()
        {
            var chart = new ColumnChart("container")
                            {
                                Title = new Title {Text = "Stacked Column Chart"},
                                XAxis = new XAxis(new[] {new XAxisItem {Categories = new object[] {"One", "Two", "Three", "Four", "Five"}}}),
                                PlotOptions =
                                    {
                                        Stacking = Stacking.normal,
                                        Events = new PlotOptionEvents {Click = "alert('hi dave');"}
                                    },
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
            Assert.AreEqual(chart.ToJson(), "{chart: {\"renderTo\":\"container\",\"defaultSeriesType\":\"column\"},credits: { enabled: false },plotOptions: { series: {\"events\":{\"click\":function(event){ alert('hi dave'); }},\"stacking\":\"normal\"} },title: {\"text\":\"Stacked Column Chart\"},exporting: {\"enabled\":false},xAxis: [{\"categories\":[\"One\",\"Two\",\"Three\",\"Four\",\"Five\"],\"title\":{\"text\":\"\"}}],series: [{\"data\":[29.9,71.5,106.4,29.2,144.0],\"stack\":0},{\"data\":[30.0,176.5,135.4,148.2,216.4],\"stack\":0},{\"data\":[29.9,71.5,106.4,29.2,144.0],\"stack\":1},{\"data\":[29.9,71.5,106.4,29.2,144.0],\"stack\":1}]}");
        }

        [Test]
        public void DateTimeLineChart()
        {
            var chart = new DateTimeLineChart("container")
                                    {
                                        Appearance =
                                            {
                                                Height = 150,
                                                Width = 325,
                                            },
                                        PlotOptions =
                                            {
                                                Cursor = "pointer",
                                                ShowInLegend = true,
                                                PointStart = new DateTime(2013,1,1),
                                                PointInterval = 24*3600*1000*7,
                                                DataLabels = new DataLabels
                                                    {
                                                        Enabled = false
                                                    },
                                                Point = new PlotPointEvents
                                                {
                                                   Events =  new PointEvents()
                                                       {
                                                           Click = "triggerCount(event.point.id);return false",
                                                           LegendItemClick = "triggerCount(event.target.options.id);return false;"
                                                       }
                                                }
                                            },
                                        Title =
                                            {
                                                Text = ""
                                            },
                                        Legend =
                                            {
                                                Enabled = true,
                                                UseHtml = true,
                                            },
                                        XAxis = new XAxis(new[]
                                                              {
                                                                  new XAxisItem
                                                                      {
                                                                          Type = AxisDataType.datetime,
                                                                          GridLineWidth = 1,
                                                                          TickPixelInterval = 25,
                                                                          Title = new Title(""),
                                                                      }
                                                              }),

                                        Series = new SerieCollection(new[]
                                                                 {
                                                                     new Serie
                                                                         {
                                                                             Type = RenderType.line,
                                                                             Data = new object[] {715.63,700.01,737.97,739.99,704.51,753.67,775.6,785.37,792.89,799.71,806.19,831.52,814.302,807.79},
                                                                         }
                                                                 }),
                                    };
            Console.Write(chart.ToJson());
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
            Assert.AreEqual(chart.ToJson(), "{chart: {\"renderTo\":\"container\",\"defaultSeriesType\":\"line\",\"marginRight\":130,\"marginBottom\":25},credits: { enabled: false },plotOptions: { series: {} },title: {\"text\":\"Monthly Average Temperature\",\"x\":-20},exporting: {\"enabled\":false},series: []}");
        }
    }
}