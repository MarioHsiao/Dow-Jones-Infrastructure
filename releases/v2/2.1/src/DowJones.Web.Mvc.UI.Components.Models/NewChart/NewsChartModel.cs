using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class NewsChartModel : ViewComponentModel
    {

        private string _NewsTitle = "News Volume";
        private string _StockPrice = "Stock Price";

        [ClientProperty("newsSeriesTitle")]
        public string NewsSeriesTitle
        {
            get { return _NewsTitle; }
            set { _NewsTitle = value; }
        }

        [ClientProperty("stockSeriesTitle")]
        public string StockSeriesTitle
        {
            get { return _StockPrice; }
            set { _StockPrice = value; }
        }


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

 

        //[ClientData]
        //public CompanyChartPackage Result { get; set; }


        public NewsChartModel()
        {
            IsFullChart = true;
            GraphHeight = 300;
            NewsSeriesColor = "#FFAD33";
            StockSeriesColor = "#5EA9C3";
            NewsSeriesStep = 10;
            SummaryGraphDateFormat = "%m/%e";
            FullChartDisplayDateFormat = "%b. %d";
        }



        [ClientEventHandler("dj.NewsChartControl.pointClick")]
        public string onPointClick { get; set; }
        [ClientEventHandler("dj.NewsChartControl.zoom")]
        public string  OnZoomSelect {get;set;}
        [ClientEventHandler("dj.NewsChartControl.resetZoom")]
        public string OnZoomReset { get; set; }
    }
}
