/* 
 * Author: Jagadish - JR
 * Date: 7/22/09
 * Purpose: Generate Itxml for Pie Chart.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 * Rajendra G. Kulkarni - RGK           7/28/2009               Changes after code review on 7/27/2009 
 *                                                              - Renamed DJCE as Screening, ScreeingPie as Pie
 */

using System;
using System.Drawing;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;

namespace DowJones.Charting.Fcp
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class ScreeningPieChartGenerator : AbstractChartGenerator
    {
        #region Constants


        private const string DEFAULT_APPERANCE_FILE = "/fcp/fcp_screening_pie.itxml";
        private const string DEFAULT_APPEARANCE_FILE_SMALL_LEGEND = "/fcp/fcp_screening_pie_small_legend.itxml";


        #endregion

        #region Private Variables

        private int HEIGHT = 200;
        private string APPERANCE_FILE = "";
        private int WIDTH = 150;
        private Color m_BackgroundColor = Color.White;
        private ScreeningPieChartDataSet m_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private Color m_DataLabelFillColor = ColorTranslator.FromHtml("#FFFFFF");
        private Color m_DataLabelFontColor = ColorTranslator.FromHtml("#000000");
        private readonly Legend m_Legend = new Legend("legend");
        private bool m_UseCache = true;

        #endregion

        #region Properties
        public static string AppreanceFileDefault
        {
            get { return DEFAULT_APPERANCE_FILE; }
        }
        public static string AppreanceFileSmallLegend
        {
            get { return DEFAULT_APPEARANCE_FILE_SMALL_LEGEND; }
        }
        public string ScreeningAppearanceFile
        {
            get
            {
                if (APPERANCE_FILE == "")
                {
                    APPERANCE_FILE = DEFAULT_APPERANCE_FILE;
                }
                return APPERANCE_FILE;
            }
            set { APPERANCE_FILE = value; }
        }
        public ScreeningPieChartDataSet DataSet
        {
            get
            {
                if (m_DataSet == null)
                {
                    m_DataSet = new ScreeningPieChartDataSet();
                }
                return m_DataSet;
            }
            set { m_DataSet = value; }
        }
        public int Height
        {
            get { return HEIGHT; }
            set { HEIGHT = value; }
        }
        public int Width
        {
            get { return WIDTH; }
            set { WIDTH = value; }
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

        #endregion

        #region Overrides of AbstractChartGenerator

        ///This method is not used and not tested.
        public override IBytesResponse GetBytes()
        {
            var itxml = ToITXML();
            var template = GetChartTemplate();
            var bytes = ChartingManager.GetChartBytes(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            var response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        ///This method is not used and not tested.
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
            return new ChartTemplate(APPERANCE_FILE, WIDTH, HEIGHT, OutputChartType);
        }

        internal override string ToITXML()
        {
            var sb = new StringBuilder();
            sb.Append(SetBackgroundColor());
            sb.Append("<cit:image-template-layout auto-resize=\"false\" fit-in-bounds=\"true\" collision-protection=\"true\" />");

            sb.AppendFormat("<cit:pie-graph name=\"graph\" method=\"replace\" >");

            // Set the Pie Settings
            sb.Append(SetDataLabelColors());

            // Add Data
            sb.Append(ProcessData());

            // Add Legend
            sb.Append(ProcessLegend());

            sb.Append("</cit:pie-graph>");


            return sb.ToString();
        }

        #endregion

        private string SetBackgroundColor()
        {
            return m_BackgroundColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor)) : string.Empty;
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
                for (var i = 1; i <= DataSet.Series.Items.Count; i++)
                {
                    var name = DataSet.Series.Items[i - 1].Name;
                    outerSB.AppendFormat("<cit:pie-series name=\"{0}\" number=\"{1}\" />", name, i);
                    mainSB.AppendFormat("<cit:column name=\"{0}\" />", name);
                }
                mainSB.Insert(0, outerSB.ToString());
                foreach (var si in DataSet.Series.Items)
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\" hover=\"{1}\" drilldown=\"{2}\" >", si.Name, si.Hover, System.Web.HttpUtility.HtmlEncode(si.Drilldown));
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
            return Legend.ToITXML();
        }
    }
}
