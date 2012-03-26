using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DowJones.Web.Mvc.UI.Controls.CompanyProfileGraph
{
    public class CompanyProfileGraphModel : ViewComponentModel
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

      

 

        //[ClientData]
        //public CompanyChartPackage Result { get; set; }


        public CompanyProfileGraphModel()
        {
            IsFullChart = true;
            GraphHeight = 350;
            NewsSeriesColor = "#ff9300";
            StockSeriesColor = "#41a4ce";
            SummaryGraphDateFormat = "%m/%e/%y";
            FullChartDisplayDateFormat = "%b.%e.%Y";
        }

       

        [ClientEventHandler("dj_CompanyProfileGraphControl.LineSeriesClick")]
        public string onPointClick { get; set; }
        [ClientEventHandler("dj_CompanyProfileGraphControl.CompanyProfilecControlZoom")]
        public string  OnZoomSelect {get;set;}
        [ClientEventHandler("dj_CompanyProfileGraphControl.CompanyProfileControlZoomReset")]
        public string OnZoomReset { get; set; }


    }
}
