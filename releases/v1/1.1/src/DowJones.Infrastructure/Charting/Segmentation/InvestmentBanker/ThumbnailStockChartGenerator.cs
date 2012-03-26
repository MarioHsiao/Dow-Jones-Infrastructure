using System.Drawing;
using System.Text;
using DowJones.Tools.Managers.Charting;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Numerical;
using DowJones.Utilities.Managers.Charting;
using DowJones.Utilities.Managers.Core;
using DowJones.Tools.Charting.Data;

namespace DowJones.Tools.Charting.Segmentation.InvestmentBanker
{
    public class ThumbnailStockChartGenerator : AbstractChartGenerator
    {
        private const string BASE_APPERANCE_FILE = "investmentbanker/stockthumbnail.itxml";
        private const int HEIGHT = 150;
        private const int WIDTH = 225;
        private readonly TextBox m_Attribution;
        private ThumbnailStockChartDataSet m_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private BaseLine m_BaseLine;
        private Color m_BackgroundColor = ColorTranslator.FromHtml("#727272");
        private Color m_GridColor = ColorTranslator.FromHtml("#E0E0E0");
        private Color m_ScaleColor = ColorTranslator.FromHtml("#CCCCCC");
        private Color m_SecondaryFillColor = ColorTranslator.FromHtml("#639CCF");
        private Line m_PlotLine;
        private bool m_UseCache = true;

        #region Constructors

        public ThumbnailStockChartGenerator()
        {
            m_Attribution = new TextBox("CopyRight")
                                {
                                    Color = ColorTranslator.FromHtml("#E0E0E0")
                                };
        }

        public ThumbnailStockChartGenerator(ThumbnailStockChartDataSet DataSet)
        {
            m_DataSet = DataSet;
            m_Attribution = new TextBox("CopyRight")
                                {
                                    Color = ColorTranslator.FromHtml("#E0E0E0")
                                };
        }

        #endregion

        #region Properties

        public TextBox Attribution
        {
            get { return m_Attribution; }
        }

        protected bool UseCache
        {
            get { return m_UseCache; }
            set { m_UseCache = value; }
        }

        public ThumbnailStockChartDataSet DataSet
        {
            get
            {
                if (m_DataSet == null)
                    m_DataSet = new ThumbnailStockChartDataSet();
                return m_DataSet;
            }
            set { m_DataSet = value; }
        }

        public OutputChartType OutputChartType
        {
            get { return m_OutputChartType; }
            set { m_OutputChartType = value; }
        }

        public BaseLine BaseLine
        {
            get
            {
                if (m_BaseLine == null)
                {
                    m_BaseLine = new BaseLine {LineColor = ColorTranslator.FromHtml("#EB1313")};
                }
                return m_BaseLine;
            }
            set { m_BaseLine = value; }
        }

        public Line PlotLine
        {
            get
            {
                if (m_PlotLine == null)
                {
                    m_PlotLine = new Line
                                     {
                                         FillColor = ColorTranslator.FromHtml("#E0E6FF"), 
                                         LineColor = ColorTranslator.FromHtml("#E0E6FF")
                                     };
                }
                return m_PlotLine;
            }
            set { m_PlotLine = value; }
        }

        public Color BackgroundColor
        {
            get { return m_BackgroundColor; }
            set { m_BackgroundColor = value; }
        }

        public Color GridColor
        {
            get { return m_GridColor; }
            set { m_GridColor = value; }
        }

        public Color ScaleColor
        {
            get { return m_ScaleColor; }
            set { m_ScaleColor = value; }
        }

        public Color SecondaryFillColor
        {
            get { return m_SecondaryFillColor; }
            set { m_SecondaryFillColor = value; }
        }

        #endregion

        public override IBytesResponse GetBytes()
        {
            string itxml = ToITXML();
            string pcscript = GetPCScript();
            ChartTemplate template = GetChartTemplate();
            byte[] bytes = ChartingManager.GetChartBytes(itxml, pcscript, GetChartTemplate(), m_UseCache);

            var response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        public override IUriResponse GetUri()
        {
            var itxml = ToITXML();
            var pcscript = GetPCScript();
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
            var tUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, pcscript, template, m_UseCache));

            var response = new ChartUriResponse(template.Height, template.Width, tUri, original);
            return response;
        }

        public override IEmbeddedHTMLResponse GetHTML()
        {
            var itxml = ToITXML();
            var pcscript = GetPCScript();
            var template = GetChartTemplate();
            var tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, pcscript, GetChartTemplate(), m_UseCache);

            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
            return response;
        }

        internal override ChartTemplate GetChartTemplate()
        {
            var template = new ChartTemplate
                               {
                                   Width = WIDTH, 
                                   Height = HEIGHT, 
                                   OutputChartType = m_OutputChartType, 
                                   AppearanceFile = BASE_APPERANCE_FILE
                               };
            return template;
        }

        internal override string ToITXML()
        {
            var sb = new StringBuilder();
            sb.Append(SetBackgroundColor());
            sb.Append("<cit:time-graph name=\"graph\" method=\"replace\">");

            // Add the color palette
            sb.Append("<cit:graph-settings>");
            // Add Data portion of Chart
            sb.AppendFormat("<cit:color-palette name=\"User Custom\" color-1=\"{0}\" color-2=\"{1}\"/>", 
                            ColorTranslator.ToHtml(PlotLine.LineColor),
                            ColorTranslator.ToHtml(BaseLine.LineColor)
                );
            sb.Append("</cit:graph-settings>");

            sb.Append(SetGridColors());
            sb.Append(SetScales());

            // set main line style
            sb.AppendFormat("<cit:filled-line-series show=\"line\" name=\"series1\" number=\"1\" style=\"{0}\" override-line-width=\"true\" line-width=\"{1}\" show-fill=\"{2}\" fill-type=\"gradient\" fill-color=\"{3}\" fill-color2=\"{4}\" gradient-type=\"top-bottom\" gradient-x-offset-percent=\"35\" gradient-y-offset-percent=\"35\" override-label=\"false\" data-label=\"%_YVALUE\" add-to-legend=\"true\"/>",
                            StringUtilitiesManager.GetXmlEnumName<LineStyle>(PlotLine.LineStyle.ToString()), // style attribute
                            PlotLine.LineThickness,
                            (PlotLine.FillType == FillType.Mountain).ToString().ToLower(),
                            ColorTranslator.ToHtml(PlotLine.FillColor),
                            ColorTranslator.ToHtml(SecondaryFillColor)
                );

            // Set the trendline style
            if (BaseLine.IsEnabled)
            {
                sb.AppendFormat("<cit:filled-line-series show=\"line\" name=\"trendline\" number=\"2\" style=\"{0}\" override-line-width=\"true\" line-width=\"{1}\" line-color=\"{2}\" show-fill=\"{3}\" fill-type=\"gradient\" fill-color=\"{4}\" fill-color2=\"{5}\" gradient-type=\"bottom-top\" gradient-x-offset-percent=\"35\" gradient-y-offset-percent=\"35\" override-label=\"false\" data-label=\"%_YVALUE\" add-to-legend=\"true\"/>",
                                StringUtilitiesManager.GetXmlEnumName<LineStyle>(BaseLine.LineStyle.ToString()), // style attribute
                                BaseLine.LineThickness,
                                ColorTranslator.ToHtml(BaseLine.LineColor),
                                (BaseLine.FillType == FillType.Mountain).ToString().ToLower(),
                                ColorTranslator.ToHtml(BaseLine.FillColor),
                                ColorTranslator.ToHtml(SecondaryFillColor)
                    );
            }


            sb.Append("</cit:time-graph>");
            // Add the attribution
            sb.Append(ProcessAttribution());
            return sb.ToString();
        }

        private string GetPCScript()
        {
            var formatter = new NumberFormatter();
           
            var sb = new StringBuilder();
            if (DataSet.Items.Count > 0)
            {
                sb.Append("graph.setseries(series1;");
                foreach (var item in DataSet.Items)
                {
                    /*
                   var val = new PrecisionNumber(item.Value) { precision = 6 };
                   formatter.Format(val);
                    
                   sb.Append(
                       string.Concat(
                           item.Date.ToString("MM'/'dd'/'yyyy"),
                           ",",
                           val.text.Value,
                           ";"
                           )
                       );*/

                   sb.Append(
                       string.Concat(
                           item.Date.ToString("MM'/'dd'/'yyyy"),
                           ",",
                           string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", item.Value),
                           ";")
                       );

                }
                sb.Append(")");
            }
            if (BaseLine.IsEnabled)
            {
                sb.Append("graph.setseries(trendline;");
                foreach (var item in DataSet.Items)
                {
                    /*
                    var val = new PrecisionNumber(BaseLine.Value) { precision = 6 };
                    formatter.Format(val);

                    sb.Append(
                        string.Concat(
                            item.Date.ToString("MM'/'dd'/'yyyy"),
                            ",",
                            val.text.Value,
                            ";"
                            )
                        );*/
                    sb.Append(
                       string.Concat(
                           item.Date.ToString("MM'/'dd'/'yyyy"),
                           ",",
                           string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", BaseLine.Value),
                           ";")
                       );
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        private string SetBackgroundColor()
        {
            return m_BackgroundColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor)) : string.Empty;
        }

        private string SetScales()
        {
            var sb = new StringBuilder();
            sb.Append("<cit:value-scale position=\"right\">");
            sb.AppendFormat("<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("<cit:value-scale-divisions set-scale-values=\"custom\" use-max-value-for-automatic=\"false\" include-zero=\"false\" use-min-value-for-automatic=\"false\" percent-under-min-value=\"0\"/>");
            sb.Append("</cit:value-scale>");
            sb.Append("<cit:time-value-scale set-scales-values=\"manually\" eliminate-white-space=\"true\" set-tick-increment=\"automatically\" show-quarters=\"true\">");
            sb.AppendFormat("<cit:time-value-scale-labels rotate-labels=\"false\" display-minor-label=\"false\" font=\"name:Arial Unicode MS;color:{0}\" minor-font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("</cit:time-value-scale>");
            return sb.ToString();
        }

        private string SetGridColors()
        {
            var sb = new StringBuilder();
            sb.Append("<cit:grid>");
            sb.AppendFormat("<cit:outer-lines max-line=\"color:{0}\" min-line=\"color:#656069;line-width:4\" low-outer-line=\"color:{0}\" high-outer-line=\"color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
            sb.Append("<cit:inner-lines>");
            sb.AppendFormat("<cit:vertical-lines major-grid-line=\"color:{0};line-width:4\" minor-grid-line=\"color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
            sb.AppendFormat("<cit:horizontal-lines zero-line=\"color:{0}\" major-grid-line=\"color:{0}\" minor-grid-line=\"show-line:false;color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
            sb.Append("</cit:inner-lines>");
            sb.Append("<cit:ticks>");
            sb.AppendFormat("<cit:vertical-ticks major-tick=\"visible:true;position:outside;thickness:4;color:{0};length:4\" minor-tick=\"position:inside;color:{0};length:2\" />", ColorTranslator.ToHtml(ScaleColor));
            sb.AppendFormat("<cit:horizontal-ticks major-tick=\"color:{0};length:4\" minor-tick=\"visible:true;position:outside;thickness:1;color:{0};length:2\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("</cit:ticks>");
            sb.AppendFormat("<cit:background color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor));
            sb.Append("</cit:grid>");
            return sb.ToString();
        }

        private string ProcessAttribution()
        {
            return Attribution.ToITXML();
        }
    }
}