using DowJones.Charting.Highcharts.Core.Appearance;
using DowJones.Charting.Highcharts.UI;
using DowJones.Charting.Highcharts.UI.Generators;

namespace DowJones.Charting.Highcharts.Core.PlotOptions
{
    public class ChartOptions
    {
        public ChartOptions()
        {
            XAxis = new XAxis();
            YAxis = new YAxis();
            Series = new SerieCollection();
            Appearance = new Appearance.Appearance();
            Lang = new LocalizationScriptGenerator();
            Exporting = new Exporting();
            Colors = new ColorSet();
            Legend = new Legend();
            Tooltip = new ToolTip();
            ClientId = "";
        }

        public string ClientId { get; set; }

        public LocalizationScriptGenerator Lang { get; set; }

        public Appearance.Appearance Appearance { get; set; }

        public ColorSet Colors { get; set; }

        public RenderType RenderType { get; set; }

        public Legend Legend { get; set; }

        public Exporting Exporting { get; set; }

        public bool ShowCredits { get; set; }

        public Title Title { get; set; }

        public SubTitle SubTitle { get; set; }

        public ToolTip Tooltip { get; set; }

        public YAxis YAxis { get; set; }

        public XAxis XAxis { get; set; }

        public SerieCollection Series { get; set; }

        public string Theme { get; set; }
        
        public PlotOptionsSeries PlotOptions { get; set; }
    }
}