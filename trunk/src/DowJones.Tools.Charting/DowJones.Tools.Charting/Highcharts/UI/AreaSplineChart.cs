﻿using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class AreaSplineChart : Chart, IChart
    {
        private PlotOptionsAreaSpline _plotOptions;

        public AreaSplineChart(string clientId) : base(clientId, RenderType.areaspline)
        {
        }

        public PlotOptionsAreaSpline PlotOptions
        {
            get { return _plotOptions ?? (_plotOptions = new PlotOptionsAreaSpline()); }
            set { _plotOptions = value; }
        }

        public void Render()
        {
            Script =
                @"
var chart[@Id];
$(document).ready(function() {
    [@Localization]
    
    chart[@Id] = new Highcharts.Chart({
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
	});

    [@JSONSource]

});";

            Script = Script.Replace("[@Localization]", Lang.ToString());

            Script = Script.Replace("[@JSONSource]", AjaxDataSource.ToString());
            Script = Script.Replace("[@PlotOptions]", PlotOptions.ToString());
            Script = Script.Replace("[@Id]", ClientId);

            // sobreescrece as propriedades obrigatórias
            Appearance.RenderTo = ClientId;
            Appearance.DefaultSeriesType = RenderType.areaspline.ToString();

            Script = Script.Replace("[@Colors]", Colors.ToString());
            Script = Script.Replace("[@Theme]", Appearance.ToString());
            Script = Script.Replace("[@RenderType]", RenderType.ToString());
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
            var reg = new Regex(@"\""(function\(event\)\{.*?\})\""", RegexOptions.Multiline);
            Script = reg.Replace(Script, "$1");
        }
    }
}