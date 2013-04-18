using System;
using System.Drawing;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Palettes;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;
using DowJones.Managers.Core;

namespace DowJones.Charting.Common
{
    [Obsolete("This class should no longer be used due to Corda replacement project")]
    public class PieLegendChartGenerator : AbstractChartGenerator
    {
        #region Constants

        //Constants for Large pie chart
        private const int LARGE_HEIGHT = 600;
        private const string LARGE_SIZED_APPERANCE_FILE = "common/pielegend_lg.itxml";
        private const int LARGE_WIDTH = 600;
        //START: Infosys 04/23/2010 - Renamed small chart to medium and added new small chart
        //Small chart supports both online and print chart format
        //Constants for Medium pie chart
        private const int MEDIUM_HEIGHT = 400;
        private const string MEDIUM_SIZED_APPERANCE_FILE = "common/pielegend_med.itxml";
        private const int MEDIUM_WIDTH = 300;
        //Constants for Small pie chart
        private const int SMALL_HEIGHT = 400;
        private const string SMALL_SIZED_LEG_TOP_APPERANCE_FILE = "common/pielegendtop_sm.itxml";
        private const string SMALL_SIZED_LEG_TOP_APPERANCE_FILE_PRINT_VERSION = "common/pielegendtop_prnt_sm.itxml";
        private const string SMALL_SIZED_LEG_BOTTOM_APPERANCE_FILE = "common/pielegendbottom_sm.itxml";
        private const string SMALL_SIZED_LEG_BOTTOM_APPERANCE_FILE_PRINT_VERSION = "common/pielegendbottom_prnt_sm.itxml";
        //START: Infosys 05/24/2010 - Modified small chart size to 200 from 225
        private const int SMALL_WIDTH = 200;
        //END: Infosys 05/24/2010 - Modified small chart size to 200 from 225
        //END: Infosys 04/23/2010 - Renamed small chart to medium and added new small chart- end

        #endregion

        #region Private Variables

        private Color m_BackgroundColor = Color.White;
        private ChartSize m_ChartSize = ChartSize.Small;
        private AbstractColorPalette m_ColorPalette = new PieChartColorPalette();
        private PieLegendChartDataSet m_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private Color m_DataLabelFillColor = ColorTranslator.FromHtml("#FFFFFF");
        private Color m_DataLabelFontColor = ColorTranslator.FromHtml("#000000");
        private PieLegendPosition m_LegendPosition = PieLegendPosition.Bottom;
        private readonly Legend m_Legend = new Legend("legend");
        private bool m_UseCache = true;
        //START: Infosys 05/11/2010 - Chart Format property exposed. Default will be Online
        private ChartFormatType m_ChartFormatType = ChartFormatType.Online;
        //END: Infosys 05/11/2010 - Chart Format property exposed. Default will be Online

        #endregion

        #region Properties

        public PieLegendChartDataSet DataSet
        {
            get
            {
                if (m_DataSet == null)
                {
                    m_DataSet = new PieLegendChartDataSet();
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

        public OutputChartType OutputChartType
        {
            get { return m_OutputChartType; }
            set { m_OutputChartType = value; }
        }

        public AbstractColorPalette ColorPalette
        {
            get { return m_ColorPalette; }
            set { m_ColorPalette = value; }
        }

        public PieLegendPosition LegendPosition
        {
            get { return m_LegendPosition; }
            set { m_LegendPosition = value; }
        }

        //START: Infosys 05/11/2010 - Chart Format property exposed. Default will be Online
        public ChartFormatType ChartFormatType
        {
            get { return m_ChartFormatType; }
            set { m_ChartFormatType = value; }
        }
        //END: Infosys 05/11/2010 - Chart Format property exposed. Default will be Online

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
            //START: Infosys 05/11/2010 - Select Appearance file based on chart format
            string itxmlName;

            switch (ChartSize)
            {
                case ChartSize.Large:
                    return new ChartTemplate(LARGE_SIZED_APPERANCE_FILE, LARGE_WIDTH, LARGE_HEIGHT, OutputChartType);
                //Infosys-04/23/2010: creating option for small size and medium size chart will be default.
                case ChartSize.Small:
                    //Infosys-04/23/21010:check legend position and return appropriate chart template.
                    if (LegendPosition == PieLegendPosition.Top)
                    {
                        itxmlName = (ChartFormatType.Print == ChartFormatType) ? SMALL_SIZED_LEG_TOP_APPERANCE_FILE_PRINT_VERSION : SMALL_SIZED_LEG_TOP_APPERANCE_FILE;
                        return new ChartTemplate(itxmlName, SMALL_WIDTH, SMALL_HEIGHT, OutputChartType);
                    }
                    else
                    {
                        itxmlName = (ChartFormatType.Print == ChartFormatType) ? SMALL_SIZED_LEG_BOTTOM_APPERANCE_FILE_PRINT_VERSION : SMALL_SIZED_LEG_BOTTOM_APPERANCE_FILE;
                        return new ChartTemplate(itxmlName, SMALL_WIDTH, SMALL_HEIGHT, OutputChartType);
                    }
                default:
                    //infosys:04/23/2010:default is medium sized pie chart
                    return new ChartTemplate(MEDIUM_SIZED_APPERANCE_FILE, MEDIUM_WIDTH, MEDIUM_HEIGHT, OutputChartType);
            }
            //END: Infosys 05/11/2010 - Select Appearance file based on chart format
        }

        internal override string ToITXML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SetBackgroundColor());

            sb.Append("<cit:image-template-layout auto-resize=\"false\" fit-in-bounds=\"true\" collision-protection=\"true\" />");

            //START: Infosys 05/12/2010 - Override graph template only for Large and Medium chart
            if (ProcessTemplateRequired())
                sb.AppendFormat("<cit:pie-graph name=\"graph\" method=\"replace\" common=\"top:{0};\">", (LegendPosition == PieLegendPosition.Top) ? 96 : 5);
            else
                sb.Append("<cit:pie-graph name=\"graph\" method=\"replace\">");
            //END: Infosys 05/12/2010 - Override graph template only for Large and Medium chart

            // Add the color palette
            sb.Append("<cit:graph-settings>");
            sb.Append(ColorPalette.ToITXML());
            sb.Append("</cit:graph-settings>");

            // Set the Pie Settings
            sb.Append(SetDataLabelColors());

            // Add Data
            sb.Append(ProcessData());

            //START: Infosys 05/11/2010 - Override legend template only for Large and Medium chart
            // Add Legend
            if (ProcessTemplateRequired())
                sb.Append(ProcessLegend());
            //END: Infosys 05/11/2010 - Override legend template only for Large and Medium chart

            sb.Append("</cit:pie-graph>");

            return sb.ToString();
        }

        #endregion

        private string SetBackgroundColor()
        {
            // return "<cit:canvas show-border=\"false\" border-type=\"thin\" border-color=\"black\" fill-type=\"solid\" fill-color=\"gray\" fill-color2=\"#CDCDE3\" pattern-type=\"checker\" gradient-type=\"left-right\" gradient-x-offset-percent=\"35\" gradient-y-offset-percent=\"35\" image-name=\"\" image-reference=\"\" image-style=\"stretch\" transparency-percent=\"0\" override-system-font-mode=\"false\" use-system-fonts=\"basic-3-fonts\"/>";
            return m_BackgroundColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" image-style=\"stretch\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor)) : string.Empty;
        }

        private string SetDataLabelColors()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<cit:data-label fill-color=\"{0}\" font=\"name:Arial Unicode MS;color:{1}\" />", ColorTranslator.ToHtml(DataLabelFillColor), ColorTranslator.ToHtml(DataLabelFontColor));
            return sb.ToString();
        }

        private string ProcessData()
        {
            var mainSB = new StringBuilder();
            var outerSB = new StringBuilder();

            if (DataSet.Series != null)
            {
                mainSB.Append("<cit:data>");
                //// mainSB.AppendFormat("<cit:column name=\"{0}\" />", "Group");
                
                for (var i = 1; i <= DataSet.Series.Items.Count; i++)
                {
                    var name = DataSet.Series.Items[i - 1].Name;

                    outerSB.AppendFormat("<cit:pie-series name=\"{0}\" number=\"{1}\"/>", StringUtilitiesManager.XmlAttributeEncode(name), i);
                    mainSB.AppendFormat("<cit:column name=\"{0}\"/>", StringUtilitiesManager.XmlAttributeEncode(name));
                }
                mainSB.Insert(0, outerSB.ToString());
                foreach (var si in DataSet.Series.Items)
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\" drilldown=\"{1}\">", StringUtilitiesManager.XmlAttributeEncode(si.Name), StringUtilitiesManager.XmlAttributeEncode(si.Drilldown));
                    
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

        private string ProcessLegend()
        {
            MapLegendPosition(LegendPosition);
            return Legend.ToITXML();
        }

        //START: Infosys 05/11/2010 - Function to allow overriding legend template
        private bool ProcessTemplateRequired()
        {
            if (ChartFormatType.Print == ChartFormatType)
                return false;

            switch (ChartSize)
            {
                case ChartSize.Medium:
                case ChartSize.Large:
                    return true;
            }

            return false;
        }
        //END: Infosys 05/11/2010 - Function to allow overriding legend template

        private void MapLegendPosition(PieLegendPosition position)
        {
            switch (ChartSize)
            {
                case ChartSize.Medium:
                    switch (position)
                    {
                        case PieLegendPosition.Bottom:
                            Legend.Top = 310;
                            Legend.Left = 5;
                            Legend.Width = 290;
                            Legend.Height = 100;
                            break;
                        case PieLegendPosition.Top:
                            Legend.Top = 5;
                            Legend.Left = 5;
                            Legend.Width = 290;
                            Legend.Height = 100;
                            break;
                    }
                    break;
                case ChartSize.Large:
                    switch (position)
                    {
                        case PieLegendPosition.Bottom:
                            Legend.Top = 454;
                            Legend.Left = 19;
                            Legend.Width = 561;
                            Legend.Height = 133;
                            break;
                        case PieLegendPosition.Top:
                            Legend.Top = 7;
                            Legend.Left = 19;
                            Legend.Width = 561;
                            Legend.Height = 133;
                            break;
                    }
                    break;
            }
        }

    }
}
