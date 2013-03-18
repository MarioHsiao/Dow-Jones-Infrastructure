using System;
using System.Collections.Generic;
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
        public void TestMethod1()
        {
            var areaChart = new AreaChart("test");
            Console.Write(areaChart.ToJson());
            Assert.Fail();
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
                                            }
                            };

            Console.Write(chart.ToJson());
        }

        [Test]
        public void ColumnChart()
        {
            var chart = new ColumnChart("container")
            {
                Title = new Title("Chart Title"),
                XAxis = new XAxis(new [] { new XAxisItem {Categories = new object[] {"One", "Two", "Three", "Four", "Five" }}}),
                PlotOptions = {Stacking =  Stacking.normal},
                Tooltip = {Formatter = null},
                Series = new SerieCollection(new Serie[]
                                                 {
                                                     new Serie
                                                         {
                                                             Data = new object[] {29.9,71.5,106.4,29.2,144.0},
                                                             Stack = 0,
                                                         },
                                                     new Serie
                                                         {
                                                             Data = new object[] {30.0,176.5,135.4,148.2,216.4},
                                                             Stack = 0,
                                                         },
                                                     new Serie
                                                         {
                                                             Data = new object[] {29.9,71.5,106.4,29.2,144.0},
                                                             Stack = 1,
                                                         },
                                                    new Serie
                                                         {
                                                             Data = new object[] {29.9,71.5,106.4,29.2,144.0},
                                                             Stack = 1,
                                                         },
                                                 })
            };

            Console.Write(chart.ToJson());
        }
    }
}
