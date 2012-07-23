using System.Drawing;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Palettes;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;

namespace DowJones.Charting.Common
{
    public class TrendChartGenerator : AbstractChartGenerator
    {
        #region Constants

        private const int LARGE_HEIGHT = 400;
        private const string LARGE_SIZED_APPERANCE_FILE = "common/trend_lg.itxml";
        private const int LARGE_WIDTH = 600;
        private const int SMALL_HEIGHT = 300;
        private const string SMALL_SIZED_APPERANCE_FILE = "common/trend_sm.itxml";
        private const int SMALL_WIDTH = 300;
        private const int MAX_ROTAION = 90;
        private const int MIN_ROTAION = 0;
        private readonly TextBox m_Title = new TextBox("textbox");

        #endregion

        #region Private Variables
        private Color m_BackgroundColor = Color.White;
        private int m_CategoryScaleLabelRotation = MAX_ROTAION;
        private ChartSize m_ChartSize = ChartSize.Small;
        private AbstractColorPalette m_ColorPalette = new LineChartColorPalette();
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private Color m_GridColor = ColorTranslator.FromHtml("#CCCCCC");
        private Color m_ScaleColor = ColorTranslator.FromHtml("#999999");
        private Color m_DataLabelFillColor = ColorTranslator.FromHtml("#FFFFFF");
        private Color m_DataLabelFontColor = ColorTranslator.FromHtml("#000000");
        private LegendPosition m_LegendPosition = LegendPosition.Top;
        private readonly Legend m_Legend = new Legend("legend");
        private bool m_UseCache = true;
        #endregion

        #region Properties

        public Color BackgroundColor
        {
            get { return m_BackgroundColor; }
            set { m_BackgroundColor = value; }
        }

        public TextBox Title
        {
            get { return m_Title; }
        }

        public ChartSize ChartSize
        {
            get { return m_ChartSize; }
            set { m_ChartSize = value; }
        }

        public AbstractColorPalette ColorPalette
        {
            get { return m_ColorPalette; }
            set { m_ColorPalette = value; }
        }

        public TrendChartDataSet DataSet { get; set; }

        public OutputChartType OutputChartType
        {
            get { return m_OutputChartType; }
            set { m_OutputChartType = value; }
        }

        public int CategoryScaleLabelRotation
        {
            get { return m_CategoryScaleLabelRotation; }
            set
            {
                if (value >= MIN_ROTAION && value <= MAX_ROTAION)
                {
                    m_CategoryScaleLabelRotation = value;
                }
            }
        }

        protected bool UseCache
        {
            get { return m_UseCache; }
            set { m_UseCache = value; }
        }

        public Legend Legend
        {
            get { return m_Legend; }
        }

        public Color ScaleColor
        {
            get { return m_ScaleColor; }
            set { m_ScaleColor = value; }
        }

        public Color GridColor
        {
            get { return m_GridColor; }
            set { m_GridColor = value; }
        }

        public Color DataLabelFillColor
        {
            get { return m_DataLabelFillColor; }
            set { m_DataLabelFillColor = value; }
        }

        public Color DataLabelFontColor
        {
            get { return m_DataLabelFontColor; }
            set { m_DataLabelFontColor = value; }
        }

        public LegendPosition LegendPosition
        {
            get { return m_LegendPosition; }
            set { m_LegendPosition = value; }
        }

        #endregion

        #region Overrides of AbstractChartGenerator

        public override IBytesResponse GetBytes()
        {
            var itxml = ToITXML();
            var template = GetChartTemplate();
            var bytes = ChartingManager.GetChartBytes(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            var response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        public override IUriResponse GetUri()
        {
            var itxml = ToITXML();
            var original = m_OutputChartType;
            switch (m_OutputChartType)
            {
                case OutputChartType.FLASH_WITH_ACTIVEX_FIX:
                    m_OutputChartType = OutputChartType.FLASH;
                    break;
                case OutputChartType.GIF_WITH_JAVASCRIPT_INTERACTIVITY:
                    m_OutputChartType = OutputChartType.GIF;
                    break;
                case OutputChartType.JPEG_WITH_JAVASCRIPT_INTERACTIVITY:
                    m_OutputChartType = OutputChartType.JPEG;
                    break;
                case OutputChartType.PNG_WITH_JAVASCRIPT_INTERACTIVITY:
                    m_OutputChartType = OutputChartType.PNG;
                    break;
            }

            var template = GetChartTemplate();
            m_OutputChartType = original;
            var tUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, template, m_UseCache));

            var response = new ChartUriResponse(template.Height, template.Width, tUri, original);
            return response;
        }

        public override IEmbeddedHTMLResponse GetHTML()
        {
            var itxml = ToITXML();
            var template = GetChartTemplate();
            var tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
            return response;
        }

        internal override ChartTemplate GetChartTemplate()
        {
            switch (ChartSize)
            {
                case ChartSize.Large:
                    return new ChartTemplate(LARGE_SIZED_APPERANCE_FILE, LARGE_WIDTH, LARGE_HEIGHT, OutputChartType);
                default:
                    return new ChartTemplate(SMALL_SIZED_APPERANCE_FILE, SMALL_WIDTH, SMALL_HEIGHT, OutputChartType);
            }
        }

        internal override string ToITXML()
        {
            var sb = new StringBuilder();
            sb.Append(SetBackgroundColor());

            sb.Append("<cit:line-graph name=\"graph\" method=\"replace\">");

            // Add the color palette
            sb.Append("<cit:graph-settings>");
            sb.Append(ColorPalette.ToITXML());
            sb.Append("</cit:graph-settings>");

            // Set the Data Label Colors
            sb.Append(SetDataLabelColors());

            // Set the Grid Colors
            sb.Append(SetGridColors());

            // Set the Scales
            sb.Append(SetScales());
            
            // Add Data
            sb.Append(ProcessData());

            // Add Legend
            sb.Append(ProcessLegend());
            sb.Append("</cit:line-graph>");

            // Add Title
            sb.Append(ProcessTitle());
            return sb.ToString();
        }

        #endregion

        private string SetBackgroundColor()
        {
            return m_BackgroundColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor)) : string.Empty;
        }

        private string SetScales()
        {
            var sb = new StringBuilder();
            sb.Append("<cit:value-scale position=\"left\" sync-with-primary-scale=\"false\">");
            sb.AppendFormat("<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;size:11;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("<cit:value-scale-title text=\"\" reverse-title-direction=\"false\" font=\"name:Arial Unicode MS;size:10;color:black\"/>");
            sb.Append("<cit:value-scale-divisions set-scale-values=\"automatically\" include-zero=\"true\" maximum-major-divisions=\"6\" use-log-scale=\"false\"/>");
            sb.Append("</cit:value-scale>");
            sb.AppendFormat("<cit:category-scale position=\"below\" display-scale-labels=\"true\" limit-label-length=\"false\" enclose-labels=\"false\" line-between-labels=\"false\" manual-tick-location=\"false\" tick-location=\"edges\" first-label-to-be-displayed=\"1\" font=\"name:Arial Unicode MS;size:12;color:{0}\">", ColorTranslator.ToHtml(ScaleColor));
            sb.AppendFormat("<cit:adjust-labels perform-adjustments=\"always\" rotate-labels=\"true\" rotation=\"{0}\" rotate-wrap-width=\"75\" skip-labels=\"false\" show-one-label-for-every=\"2\"/>",CategoryScaleLabelRotation);
		    sb.Append("</cit:category-scale>");
            return sb.ToString();
        }

        private string SetDataLabelColors()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<cit:data-label enable-background=\"true\" shadow-settings=\"show:true;style:fade;color:#00000064;width:4\" show=\"on-hover\" show-border=\"true\" fill-color=\"{0}\" font=\"name:Arial Unicode MS;color:{1}\" />", ColorTranslator.ToHtml(DataLabelFillColor), ColorTranslator.ToHtml(DataLabelFontColor));
            return sb.ToString();
        }

        private string SetGridColors()
        {
            var sb = new StringBuilder();
            sb.Append("<cit:grid>");
            sb.AppendFormat("<cit:outer-lines max-line=\"color:{0}\" min-line=\"color:{0}\" low-outer-line=\"color:{0}\" high-outer-line=\"color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
            sb.Append("<cit:inner-lines>");
            sb.AppendFormat("<cit:vertical-lines major-grid-line=\"show-line:false;color:{0}\" minor-grid-line=\"show-line:false;color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
            sb.AppendFormat("<cit:horizontal-lines zero-line=\"color:{0}\" major-grid-line=\"color:{0}\" minor-grid-line=\"show-line:false;color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
            sb.Append("</cit:inner-lines>");
            sb.Append("<cit:ticks>");
            sb.AppendFormat("<cit:vertical-ticks major-tick=\"color:{0};length:4\" minor-tick=\"position:inside;color:{0};length:2\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.AppendFormat("<cit:horizontal-ticks major-tick=\"color:{0};length:4\" minor-tick=\"position:inside;color:{0};length:2\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("</cit:ticks>");
            sb.AppendFormat("<cit:background color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor));
            sb.Append("</cit:grid>");
            return sb.ToString();
        }

        private string ProcessLegend()
        {
            MapLegendPosition(LegendPosition);
            return Legend.ToITXML();
        }

        private void MapLegendPosition(LegendPosition position)
        {
            switch (ChartSize)
            {
                case ChartSize.Small:
                    switch (position)
                    {
                        case LegendPosition.Right:
                            Legend.Top = 7;
                            Legend.Left = 212;
                            Legend.Width = 90;
                            Legend.Height = 252;
                            break;
                        case LegendPosition.Bottom:
                            Legend.Top = 207;
                            Legend.Left = 24;
                            Legend.Width = 252;
                            Legend.Height = 86;
                            break;
                        case LegendPosition.Top:
                            Legend.Top = 20;
                            Legend.Left = 24;
                            Legend.Width = 252;
                            Legend.Height = 86;
                            break;
                    }
                    break;
                case ChartSize.Large:
                    switch (position)
                    {
                        case LegendPosition.Right:
                            Legend.Top = 7;
                            Legend.Left = 520;
                            Legend.Width = 90;
                            Legend.Height = 400;
                            break;
                        case LegendPosition.Bottom:
                            Legend.Top = 454;
                            Legend.Left = 19;
                            Legend.Width = 561;
                            Legend.Height = 133;
                            break;
                        case LegendPosition.Top:
                            Legend.Top = 15;
                            Legend.Left = 19;
                            Legend.Width = 561;
                            Legend.Height = 133;
                            break;
                    }
                    break;
            }
        }

        private string ProcessTitle()
        {
            return Title.ToITXML();
        }

        private string ProcessData()
        {
            // Get the Bar-Series nodes
            //<cit:bar-series name="Series 1" number="1" />
            var mainSB = new StringBuilder();
            var outerSB = new StringBuilder();

            if (DataSet.Series != null)
            {
                mainSB.Append("<cit:data series-data-in-columns=\"true\">");
                for (var i = 1; i <= DataSet.Series.CategoryNames.Count; i++)
                {
                    var name = DataSet.Series.CategoryNames[i - 1];
                    outerSB.AppendFormat("<cit:line-series show=\"both\" name=\"{0}\" number=\"{1}\" override-line-width=\"true\" line-width=\"3\" override-symbol-settings=\"true\" symbol-settings=\"type:round;width:6;height:6;border-width:0;color:{2}\" />", name, i, ColorTranslator.ToHtml(m_ColorPalette.Palette[(i-1)%16]));
                    mainSB.AppendFormat("<cit:column name=\"{0}\" />", name);
                }
                mainSB.Insert(0, outerSB.ToString());
                foreach (var si in DataSet.Series.Items)
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\">", si.Name);
                    // Get the dict obj
                    foreach (var item in si.DataItems)
                    {
                        mainSB.Append(item.ToITXML());
                    }
                    mainSB.Append("</cit:row>");
                }

                mainSB.Append("</cit:data>");
                return mainSB.ToString();
            }
            return "<cit:data />";
        }
    }
}
