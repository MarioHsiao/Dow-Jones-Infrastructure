using System;
using System.ComponentModel;

namespace DowJones.Web.Mvc.UI.Components.NewsChart
{
    public class NewsChartModel : ViewComponentModel
    {
        [ClientProperty("newsSeriesTitle")]
        public string NewsSeriesTitle { get; set; }

        [ClientProperty("stockSeriesTitle")]
        public string StockSeriesTitle { get; set; }

        [DefaultValue(true)]
        [ClientProperty("isFullChart")]
        public Boolean IsFullChart {get;set;}

        [DefaultValue(220)]
        [ClientProperty("graphHeight")]
        public int GraphHeight { get; set; }

        [DefaultValue(300)]
        [ClientProperty("graphWidth")]
        public int GraphWidth { get; set; }
        
        [ClientProperty("summaryGraphDateFormat")]
        public string SummaryGraphDateFormat { get; set; }

        [ClientProperty("fullChartDisplayDateFormat")]
        public string FullChartDisplayDateFormat { get; set; }
       
        [ClientProperty("newsSeriesColor")]
        public string NewsSeriesColor { get; set; }
        [ClientProperty("stockSeriesColor")]
        public string StockSeriesColor { get; set; }

        [ClientProperty("newsSeriesStep")]
        public int NewsSeriesStep { get; set; }


        public NewsChartModel()
        {
            IsFullChart = true;
            GraphHeight = 300;
            NewsSeriesColor = "#FFAD33";
            StockSeriesColor = "#5EA9C3";
            NewsSeriesStep = 10;
            NewsSeriesTitle = "News Volume";
            StockSeriesTitle = "Stock Price";
            SummaryGraphDateFormat = "%m/%e";
            FullChartDisplayDateFormat = "%b. %d";
        }



        [ClientEventHandler("dj.NewsChart.pointClick")]
        public string onPointClick { get; set; }

        [ClientEventHandler("dj.NewsChart.zoom")]
        public string  OnZoomSelect {get;set;}

        [ClientEventHandler("dj.NewsChart.resetZoom")]
        public string OnZoomReset { get; set; }
    }
}
