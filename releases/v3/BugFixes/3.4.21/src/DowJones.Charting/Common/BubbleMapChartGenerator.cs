using System.Drawing;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Palettes;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;

namespace DowJones.Charting.Common
{
    public class BubbleMapChartGenerator : AbstractChartGenerator
    {
        #region Constants

        private const int LARGE_HEIGHT = 518;
        private const string LARGE_SIZED_APPERANCE_FILE = "common/bubblemap_lg.itxml";
        private const int LARGE_WIDTH = 983;
        private const int SMALL_HEIGHT = 194;
        private const string SMALL_SIZED_APPERANCE_FILE = "common/bubblemap_sm.itxml";
        private const int SMALL_WIDTH = 300;

        #endregion

        #region Private Variables

        private ChartSize m_ChartSize = ChartSize.Small;
        private AbstractColorPalette m_ColorPalette = new BubbleMapColorPalette();
        private BubbleMapChartDataSet m_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private Color m_LandColor = ColorTranslator.FromHtml("#E5E5E5");
        private Color m_LandOutlineColor = Color.Black;
        private Color m_OceanColor = ColorTranslator.FromHtml("#CCCCFF");
        private Color m_BubbleFillColor = ColorTranslator.FromHtml("#FF6600");
        private Color m_BubbleOutlineColor = ColorTranslator.FromHtml("#FF6600CA");
        private Color m_DataLabelFillColor = ColorTranslator.FromHtml("#FFFFFF");
        private Color m_DataLabelFontColor = ColorTranslator.FromHtml("#000000");
        private bool m_UseCache = true;

        #endregion

        #region Properties

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

        public BubbleMapChartDataSet DataSet
        {
            get { return m_DataSet; }
            set { m_DataSet = value; }
        }

        public OutputChartType OutputChartType
        {
            get { return m_OutputChartType; }
            set { m_OutputChartType = value; }
        }

        public Color LandColor
        {
            get { return m_LandColor; }
            set { m_LandColor = value; }
        }

        public Color LandOutlineColor
        {
            get { return m_LandOutlineColor; }
            set { m_LandOutlineColor = value; }
        }

        public Color OceanColor
        {
            get { return m_OceanColor; }
            set { m_OceanColor = value; }
        }

        public Color BubbleOutlineColor
        {
            get { return m_BubbleOutlineColor; }
            set { m_BubbleOutlineColor = value; }
        }

        public Color BubbleFillColor
        {
            get { return m_BubbleFillColor; }
            set { m_BubbleFillColor = value; }
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

        protected bool UseCache
        {
            get { return m_UseCache; }
            set { m_UseCache = value; }
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
            sb.Append(SetOceanColor());
            sb.Append("<cit:image-template-layout auto-resize=\"true\" />");

            sb.Append("<cit:map name=\"World_Country\" method=\"replace\">");

            // Set the Land Color, Ocean Color, and Bubble Colors
            sb.Append(SetLandColor());

            sb.Append("<cit:layer name=\"CountryPoint\" method=\"replace\">");
            sb.Append(SetBubbleColors());

            // Add Data
            sb.Append(ProcessData());
            sb.Append("</cit:layer>");

            sb.Append("</cit:map>");
            return sb.ToString();
        }

        #endregion

        private string SetOceanColor()
        {
            return m_OceanColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(OceanColor)) : string.Empty;
        }

        private string SetLandColor()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:layer name=\"Country\" type=\"area\" >");
            sb.Append("<cit:default-shape>");
            sb.AppendFormat("<cit:shape-settings type=\"polygon\" fill-color=\"{0}\" line-color=\"{1}\" />", ColorTranslator.ToHtml(LandColor), ColorTranslator.ToHtml(LandOutlineColor));
            sb.Append("</cit:default-shape>");
            sb.Append("</cit:layer>");
            sb.Append("<cit:layer name=\"World\" type=\"area\" >");
            sb.Append("<cit:default-shape>");
            sb.AppendFormat("<cit:shape-settings type=\"polygon\" fill-enabled=\"false\" line-width=\"1.5\" line-color=\"{0}\" />", ColorTranslator.ToHtml(LandOutlineColor));
            sb.Append("</cit:default-shape>");
            sb.Append("</cit:layer>");
            return sb.ToString();
        }

        private string SetBubbleColors()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:layer-settings hide-shapes-with-no-data=\"true\" use-ranges=\"true\" use-original-colors=\"false\"/>");

            // Apply Data Label Color Settings
            sb.Append("<cit:default-shape>");
            sb.AppendFormat("<cit:hover-text fill-color=\"{0}\" font=\"name:Arial Unicode MS;size:14;style:plain;color:{1}\" />", ColorTranslator.ToHtml(DataLabelFillColor), ColorTranslator.ToHtml(DataLabelFontColor));
            sb.Append("</cit:default-shape>");

            // Apply Bubble Point Color Settings
            sb.Append("<cit:map-ranges clean-boundaries=\"true\">");

            switch(ChartSize)
            {
                #region case ChartSize.Large

                case ChartSize.Large:
                    sb.Append("<cit:map-range name=\"Out of Range Low\" add-to-legend=\"false\" minimum=\"no-data\" maximum=\"0\">");
                    sb.AppendFormat("<cit:shape-settings fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,line-color,width,line-width,fill-type,type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"lowest .5\" add-to-legend=\"false\">");
                    sb.AppendFormat("<cit:shape-settings width=\"6\" height=\"6\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\" />", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\".5to1\" add-to-legend=\"false\" minimum=\".5\" maximum=\"1\">");
                    sb.AppendFormat("<cit:shape-settings width=\"8\" height=\"8\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"1to2\" add-to-legend=\"false\" minimum=\"1\" maximum=\"2\">");
                    sb.AppendFormat("<cit:shape-settings width=\"10\" height=\"10\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");
                    
                    sb.Append("<cit:map-range name=\"2to3\" add-to-legend=\"false\" minimum=\"2\" maximum=\"3\">");
                    sb.AppendFormat("<cit:shape-settings width=\"12\" height=\"12\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"3to5\" add-to-legend=\"false\" minimum=\"3\" maximum=\"5\">");
                    sb.AppendFormat("<cit:shape-settings width=\"12\" height=\"12\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");
                    
                    sb.Append("<cit:map-range name=\"5to10\" add-to-legend=\"false\" minimum=\"5\" maximum=\"10\">");
                    sb.AppendFormat("<cit:shape-settings width=\"12\" height=\"12\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"10to25\" add-to-legend=\"false\" minimum=\"10\" maximum=\"25\">");
                    sb.AppendFormat("<cit:shape-settings width=\"14\" height=\"14\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"25to50\" add-to-legend=\"false\" minimum=\"25\" maximum=\"50\">");
                    sb.AppendFormat("<cit:shape-settings width=\"18\" height=\"18\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"50to75\" add-to-legend=\"false\" minimum=\"50\" maximum=\"75\">");
                    sb.AppendFormat("<cit:shape-settings width=\"22\" height=\"22\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"75to100\" add-to-legend=\"false\" minimum=\"75\" maximum=\"100\">");
                    sb.AppendFormat("<cit:shape-settings width=\"26\" height=\"26\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"Out of Range High\" add-to-legend=\"false\" minimum=\"100\" maximum=\"no-data\">");
                    sb.AppendFormat("<cit:shape-settings width=\"26\" height=\"26\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");
                    break;

                #endregion

                #region case ChartSize.Small

                case ChartSize.Small:
                    sb.Append("<cit:map-range name=\"Out of Range Low\" add-to-legend=\"false\" minimum=\"no-data\" maximum=\"0\">");
                    sb.AppendFormat("<cit:shape-settings fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,line-color,width,line-width,fill-type,type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"lowest .5\" add-to-legend=\"false\">");
                    sb.AppendFormat("<cit:shape-settings width=\"4\" height=\"4\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\" />", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\".5to1\" add-to-legend=\"false\" minimum=\".5\" maximum=\"1\">");
                    sb.AppendFormat("<cit:shape-settings width=\"4\" height=\"4\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"1to2\" add-to-legend=\"false\" minimum=\"1\" maximum=\"2\">");
                    sb.AppendFormat("<cit:shape-settings width=\"4\" height=\"4\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"2to3\" add-to-legend=\"false\" minimum=\"2\" maximum=\"3\">");
                    sb.AppendFormat("<cit:shape-settings width=\"4\" height=\"4\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"3to5\" add-to-legend=\"false\" minimum=\"3\" maximum=\"5\">");
                    sb.AppendFormat("<cit:shape-settings width=\"4\" height=\"4\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"5to10\" add-to-legend=\"false\" minimum=\"5\" maximum=\"10\">");
                    sb.AppendFormat("<cit:shape-settings width=\"4\" height=\"4\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"10to25\" add-to-legend=\"false\" minimum=\"10\" maximum=\"25\">");
                    sb.AppendFormat("<cit:shape-settings width=\"4\" height=\"4\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"25to50\" add-to-legend=\"false\" minimum=\"25\" maximum=\"50\">");
                    sb.AppendFormat("<cit:shape-settings width=\"7\" height=\"7\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"50to75\" add-to-legend=\"false\" minimum=\"50\" maximum=\"75\">");
                    sb.AppendFormat("<cit:shape-settings width=\"10\" height=\"10\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"75to100\" add-to-legend=\"false\" minimum=\"75\" maximum=\"100\">");
                    sb.AppendFormat("<cit:shape-settings width=\"12\" height=\"12\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");

                    sb.Append("<cit:map-range name=\"Out of Range High\" add-to-legend=\"false\" minimum=\"100\" maximum=\"no-data\">");
                    sb.AppendFormat("<cit:shape-settings width=\"12\" height=\"12\" fill-color=\"{0}\" line-color=\"{1}\" overrides=\"height,fill-color,width,line-color,line-width,type,fill-type\"/>", ColorTranslator.ToHtml(BubbleFillColor), ColorTranslator.ToHtml(BubbleOutlineColor));
                    sb.Append("</cit:map-range>");
                    break;

                #endregion
            }

            sb.Append("</cit:map-ranges>");
            return sb.ToString();
        }

        private string ProcessData()
        {
            StringBuilder sb = new StringBuilder();
            
            if (DataSet.Series != null)
            {
                foreach (SeriesItem si in DataSet.Series.Items)
                {
                    for (int i = 1; i <= si.DataItems.Count; i++)
                    {
                        string temp = string.Format("<cit:map-point name=\"{0}\" ", si.Name);
                        temp = string.Format(temp + "value=\"{0}\" >", si.DataItems[i-1].Value);
                        sb.Append(temp);
                        sb.Append(si.DataItems[i-1].ToITXML());
                        sb.Append("</cit:map-point>");
                    }
                }
            }
            return sb.ToString();
        }

    }
}
