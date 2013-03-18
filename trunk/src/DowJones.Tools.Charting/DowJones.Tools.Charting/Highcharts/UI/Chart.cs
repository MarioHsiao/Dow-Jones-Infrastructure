using System.Text.RegularExpressions;
using DowJones.Tools.Charting.Highcharts.Core;
using DowJones.Tools.Charting.Highcharts.Core.Appearance;

namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class Chart : GenericChart, IJsonObject
    {
        private readonly Regex _reg = new Regex(@"\""(function\(event\)\{.*?\})\""", RegexOptions.Multiline);

        protected readonly static string Script = "{[@Appearance] credits: { enabled: [@ShowCredits] }, [@Colors] [@PlotOptions] [@Title] [@Subtitle]" +
                                                    "[@Legend] [@Exporting] [@XAxis] [@YAxis] [@ToolTip] [@Series]}";


        public Chart(string clientId, RenderType renderType)
        {
            ClientId = clientId;
            RenderType = renderType;
            Appearance = new Appearance {RenderTo = clientId};
        }

        public string ClientId { get; set; }

        public virtual string ToJson()
        {
            var tScript = Script.Replace("[@Id]", ClientId);
            Appearance.RenderTo = ClientId;
            Appearance.DefaultSeriesType = RenderType.ToString();

            tScript = tScript.Replace("[@Colors]", Colors.ToString());
            tScript = tScript.Replace("[@Appearance]", Appearance.ToString());
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
            tScript = _reg.Replace(tScript, "$1");

            return tScript;
        }
    }
}