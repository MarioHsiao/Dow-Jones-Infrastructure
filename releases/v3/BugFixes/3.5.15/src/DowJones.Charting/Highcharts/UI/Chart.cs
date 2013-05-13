using System.Text.RegularExpressions;
using DowJones.Charting.Highcharts.Core;
using DowJones.Charting.Highcharts.Core.Appearance;
using DowJones.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Charting.Highcharts.UI
{
    public class Chart<T> : GenericChart, IJsonObject where T : PlotOptionsSeries, new()
    {
        private readonly Regex _functionRegex = new Regex(@"\""(function\(event\)\{.*?\})\""", RegexOptions.Multiline);
        private readonly Regex _dateRegex = new Regex(@"(new Date\((.*?)\))", RegexOptions.Multiline);

        protected readonly string Script = "{[@Appearance][@Credits][@Colors][@PlotOptions][@Title][@Subtitle]" +
                                                    "[@Legend][@Exporting][@XAxis][@YAxis][@ToolTip][@Series]}";

        private T _plotOptions;

        protected Chart(string clientId, RenderType renderType)
        {
            ClientId = clientId;
            RenderType = renderType;
            Appearance = new Appearance {RenderTo = clientId};
        }

        public T PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new T()); }
            set { _plotOptions = value; }
        }

        public string ClientId { get; set; }
        

        public string ToJson()
        {
            var tScript = Script.Replace("[@Id]", ClientId);
            Appearance.RenderTo = ClientId;
            Appearance.DefaultSeriesType = RenderType.ToString();

            tScript = tScript.Replace("[@Colors]", Colors.ToString());
            tScript = tScript.Replace("[@Credits]", Credits.ToString());
            tScript = tScript.Replace("[@Appearance]", Appearance.ToString());
            tScript = tScript.Replace("[@PlotOptions]", PlotOptions.ToString());
            tScript = tScript.Replace("[@RenderType]", RenderType.ToString());
            tScript = tScript.Replace("[@Legend]", Legend.ToString());
            tScript = tScript.Replace("[@Exporting]", Exporting.ToString());
            tScript = tScript.Replace("[@ShowCredits]", ShowCredits.ToString().ToLower());
            tScript = tScript.Replace("[@Title]", Title.ToString());
            tScript = tScript.Replace("[@Subtitle]", SubTitle.ToString());
            tScript = tScript.Replace("[@ToolTip]", Tooltip.ToString());
            tScript = tScript.Replace("[@YAxis]", YAxis.ToString());
            tScript = tScript.Replace("[@XAxis]", XAxis.ToString());
            tScript = tScript.Replace("[@Series]", Series.ToString());

            // handle special case for events, such as point click, mouseover etc
            // see PointEvents.cs for examples
            tScript = _functionRegex.Replace(tScript, "$1");
            tScript = _dateRegex.Replace(tScript, "$2"); // or t Script = _dateRegex.Replace(tScript, "$1.ToDate()")

            return tScript;
        }
    }
}