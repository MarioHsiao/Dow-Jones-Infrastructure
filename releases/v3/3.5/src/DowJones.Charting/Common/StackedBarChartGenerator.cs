using System;
using System.Drawing;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Palettes;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;

namespace DowJones.Charting.Common
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class StackedBarChartGenerator : AbstractChartGenerator
    {
        #region Constants

        private const int LARGE_HEIGHT = 400;
        private const string LARGE_SIZED_APPERANCE_FILE = "common/stackedbar_lg.itxml";
        private const int LARGE_WIDTH = 600;
        private const int MAX_ROTATION = 90;
        private const int MIN_ROTATION = 0;
        private const int SMALL_HEIGHT = 300;
        private const string SMALL_SIZED_APPERANCE_FILE = "common/stackedbar_sm.itxml";
        private const int SMALL_WIDTH = 300;
        private readonly TextBox m_Title = new TextBox("textbox");

        #endregion

        #region Private Variables

        private Color m_BackgroundColor = Color.White;
        private int m_CategoryScaleLabelRotation = MAX_ROTATION;
        private ChartSize m_ChartSize = ChartSize.Small;
        private AbstractColorPalette m_ColorPalette = new StackedBarChartColorPalette();
        private StackedBarChartDataSet m_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private Color m_GridColor = ColorTranslator.FromHtml("#CCCCCC");
        private Color m_ScaleColor = ColorTranslator.FromHtml("#999999");
        private readonly Legend m_Legend = new Legend("legend");
        private bool m_UseCache = true;

        #endregion

        #region Properties

        public StackedBarChartDataSet DataSet
        {
            get
            {
                if (m_DataSet == null)
                {
                    m_DataSet = new StackedBarChartDataSet();
                }
                return m_DataSet;
            }
            set { m_DataSet = value; }
        }

        public Legend Legend
        {
            get { return m_Legend; }
        }

        protected bool UseCache
        {
            get { return m_UseCache; }
            set { m_UseCache = value; }
        }

        public ChartSize ChartSize
        {
            get { return m_ChartSize; }
            set { m_ChartSize = value; }
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
                if (value >= MIN_ROTATION && value <= MAX_ROTATION)
                {
                    m_CategoryScaleLabelRotation = value;
                }
            }
        }

        public TextBox Title
        {
            get { return m_Title; }
        }

        public AbstractColorPalette ColorPalette
        {
            get { return m_ColorPalette; }
            set { m_ColorPalette = value; }
        }

        #endregion

        #region Overrides of AbstractChartGenerator

        public override IBytesResponse GetBytes()
        {
            string itxml = ToITXML();
            ChartTemplate template = GetChartTemplate();
            byte[] bytes = ChartingManager.GetChartBytes(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            ChartBytesResponse response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        public override IUriResponse GetUri()
        {
            string itxml = ToITXML();
            OutputChartType original = m_OutputChartType;
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

            ChartTemplate template = GetChartTemplate();
            m_OutputChartType = original;
            string tUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, template, m_UseCache));

            ChartUriResponse response = new ChartUriResponse(template.Height, template.Width, tUri, original);
            return response;
        }

        public override IEmbeddedHTMLResponse GetHTML()
        {
            string itxml = ToITXML();
            ChartTemplate template = GetChartTemplate();
            string tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            ChartEmbeddedHTMLResponse response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
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
            StringBuilder sb = new StringBuilder();
            sb.Append(SetBackgroundColor());

            sb.Append("<cit:bar-graph name=\"graph\" method=\"replace\">");

            // Add the color palette
            sb.Append("<cit:graph-settings>");
            sb.Append(ColorPalette.ToITXML());
            sb.Append("</cit:graph-settings>");

            // Set the Grid Colors
            sb.Append(SetGridColors());

            // Set the Scales
            sb.Append(SetScales());

            // Add Data
            sb.Append(ProcessData());

            // Add Legend
            sb.Append(ProcessLegend());
            sb.Append("</cit:bar-graph>");

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
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:value-scale position=\"left\" sync-with-primary-scale=\"false\">");
            sb.AppendFormat("<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("<cit:value-scale-title text=\"\" reverse-title-direction=\"false\" font=\"name:Arial Unicode MS;size:10;color:black\"/>");
            sb.Append("<cit:value-scale-divisions set-scale-values=\"automatically\" include-zero=\"true\" maximum-major-divisions=\"6\" use-log-scale=\"false\"/>");
            sb.Append("</cit:value-scale>");
            sb.AppendFormat("<cit:category-scale position=\"below\" display-scale-labels=\"true\" limit-label-length=\"false\" enclose-labels=\"false\" line-between-labels=\"false\" manual-tick-location=\"false\" tick-location=\"edges\" first-label-to-be-displayed=\"1\" font=\"name:Arial Unicode MS;size:12;color:{0}\">", ColorTranslator.ToHtml(ScaleColor));
            if (CategoryScaleLabelRotation == MIN_ROTATION)
                sb.Append("<cit:adjust-labels perform-adjustments=\"always\" rotate-labels=\"false\" rotation=\"0\" rotate-wrap-width=\"75\" skip-labels=\"false\" show-one-label-for-every=\"2\"/>");
            else
                sb.AppendFormat("<cit:adjust-labels perform-adjustments=\"always\" rotate-labels=\"true\" rotation=\"{0}\" rotate-wrap-width=\"75\" skip-labels=\"false\" show-one-label-for-every=\"2\"/>", CategoryScaleLabelRotation);
            sb.Append("</cit:category-scale>");
            return sb.ToString();
        }

        private string SetGridColors()
        {
            StringBuilder sb = new StringBuilder();
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
            return Legend.ToITXML();
        }

        private string ProcessTitle()
        {
            return Title.ToITXML();
        }

        private string ProcessData()
        {
            // Get the Bar-Series nodes
            //<cit:bar-series name="Series 1" number="1" />
            StringBuilder mainSB = new StringBuilder();
            StringBuilder outerSB = new StringBuilder();

            if (DataSet.Series != null)
            {
                mainSB.Append("<cit:data series-data-in-columns=\"true\">");
                for (int i = 1; i <= DataSet.Series.CategoryNames.Count; i++)
                {
                    string name = DataSet.Series.CategoryNames[i - 1];
                    outerSB.AppendFormat("<cit:bar-series name=\"{0}\" number=\"{1}\" />", name, i);
                    mainSB.AppendFormat("<cit:column name=\"{0}\"/>", name);
                }
                mainSB.Insert(0, outerSB.ToString());
                foreach (SeriesItem si in DataSet.Series.Items   )
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\">", si.Name);
                    // Get the dict obj
                    foreach (DataItem item in si.DataItems)
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