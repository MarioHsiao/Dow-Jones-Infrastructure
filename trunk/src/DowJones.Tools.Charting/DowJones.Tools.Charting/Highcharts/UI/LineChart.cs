using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class LineChart : Chart, IChart
    {
        private PlotOptionsLine _plotOptionsLine;

        public LineChart(string clientId) : base(clientId, RenderType.line)
        {
        }

        public PlotOptionsLine PlotOptions
        {
            get { return _plotOptionsLine ?? (_plotOptionsLine = new PlotOptionsLine()); }
            set { _plotOptionsLine = value; }
        }

        public void Render()
        {
            Script =
                @"
{    
    [@Theme]
    [@Colors]
    credits: { enabled: [@ShowCredits] },
    [@PlotOptions]
    [@Title]
    [@Subtitle]
    [@Legend]
    [@Exporting]
    [@XAxis]
    [@YAxis]
    [@ToolTip]
    [@Series]
});";

            Script = Script.Replace("[@Localization]", Lang.ToString());

            Script = Script.Replace("[@JSONSource]", AjaxDataSource.ToString());
            Script = Script.Replace("[@PlotOptions]", PlotOptions.ToString());
            Script = Script.Replace("[@Id]", ClientId);

            // sobreescrece as propriedades obrigatórias
            Appearance.RenderTo = ClientId;
            Appearance.DefaultSeriesType = RenderType.line.ToString();

            Script = Script.Replace("[@Colors]", Colors.ToString());
            Script = Script.Replace("[@Theme]", Appearance.ToString());
            Script = Script.Replace("[@Legend]", Legend.ToString());
            Script = Script.Replace("[@Exporting]", Exporting.ToString());
            Script = Script.Replace("[@ShowCredits]", ShowCredits.ToString().ToLower());
            Script = Script.Replace("[@Title]", Title.ToString());
            Script = Script.Replace("[@Subtitle]", SubTitle.ToString());
            Script = Script.Replace("[@ToolTip]", Tooltip.ToString());
            Script = Script.Replace("[@YAxis]", YAxis.ToString());
            Script = Script.Replace("[@XAxis]", XAxis.ToString());
            Script = Script.Replace("[@Series]", Series.ToString());

            // handle special case for events, such as point click, mouseover etc
            // see PointEvents.cs for examples
            var _reg = new Regex(@"\""(function\(event\)\{.*?\})\""", RegexOptions.Multiline);
            Script = _reg.Replace(Script, "$1");
/*
            if (!string.IsNullOrEmpty(Theme))
            {

                var themeAPI = @"
var highchartsOptions = Highcharts.setOptions(themes['{0}']);";
                Script = string.Concat(string.Format(themeAPI, Theme.ToString()), Script);

            }*/
        }
    }
}