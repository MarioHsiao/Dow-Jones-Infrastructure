using System;

namespace DowJones.Tools.Charting.Highcharts.Core.Appearance
{
    [Serializable]
    public class DateTimeLabelFormats
    {
        public string Second { get; set; }
        public string Minute { get; set; }
        public string Hour { get; set; }
        public string Day { get; set; }
        public string Week { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        //public DateTimeLabelFormats()
        //{
        //    second  = string .Empty;
        //    minute  = string .Empty;
        //    hour  = string .Empty;
        //    day  = string .Empty;
        //    week = string .Empty;
        //    month = string .Empty;
        //    year = string.Empty;
        //}
    }
}