using System;
using System.Text;
using System.Drawing;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;
using DowJones.Managers.Core;

namespace DowJones.Charting.Discovery
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public sealed class KeywordsNewsClustersChartGenerator : AbstractChartGenerator
    {
        
        #region Constants

        // keywords appearance settings
        private const string EXPORT_APPERANCE_FILE = "/discovery/keywords_newsclusters_export.itxml";
        private const int HEIGHT = 270;
        private const int WIDTH = 370;
        private const int LEFT = 200;

        private static readonly int[] TOP = new [] { 30, 52, 74, 96, 118, 140, 162, 184, 206, 228 };

        #endregion

        #region Constructors


        public KeywordsNewsClustersChartGenerator()
        {
            m_DateTimeFormatter = new DateTimeFormatter(InterfaceLanguage.ToString(), DateTimeFormatingPreference);
        }

        #endregion

        #region Private Variables

        private Color m_BackgroundColor = Color.White;
        private OutputChartType m_OutputChartType = OutputChartType.PNG;
        private bool m_UseCache = true;
        private bool m_IsKeywordsChart = true;
        private TextBoxDataSet m_DataSet;
        private readonly TextBox m_Title = new TextBox("textbox");
        private DateTime? m_DateTimeAttribution;
        private DateTimeFormatter m_DateTimeFormatter;
        private string m_DateTimeFormatingPreference;
        private InterfaceLanguage m_InterfaceLanguage = InterfaceLanguage.en;
        internal readonly TextBox m_Copyright = new TextBox("copyright");
        
        #endregion

        #region Properties

        public TextBoxDataSet DataSet
        {
            get
            {
               if (m_DataSet == null)
               {
                   m_DataSet = new TextBoxDataSet();
               }
               return m_DataSet;
            }
            set { m_DataSet = value; }
        }

        public bool IsKeywordsChart
        {
            get { return m_IsKeywordsChart; }
            set { m_IsKeywordsChart = value; }
        }

        public DateTime DateTimeAttribution
        {
            get { return (DateTime)m_DateTimeAttribution; }
            set { m_DateTimeAttribution = value; }
        }

        public InterfaceLanguage InterfaceLanguage
        {
            get { return m_InterfaceLanguage; }
            set
            {
                m_InterfaceLanguage = value;
                m_DateTimeFormatter = new DateTimeFormatter(value.ToString(), DateTimeFormatingPreference);
            }
        }

        public string DateTimeFormatingPreference
        {
            get { return m_DateTimeFormatingPreference; }
            set
            {
                m_DateTimeFormatingPreference = value;
                m_DateTimeFormatter = new DateTimeFormatter(InterfaceLanguage.ToString(), value);
            }
        }

        public TextBox Title
        {
            get { return m_Title; }
        }

        public Color BackgroundColor
        {
            get { return m_BackgroundColor; }
            set { m_BackgroundColor = value; }
        }

        public OutputChartType OutputChartType
        {
            get { return m_OutputChartType; }
            set { m_OutputChartType = value; }
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
            return new ChartTemplate(EXPORT_APPERANCE_FILE, WIDTH, HEIGHT, OutputChartType);
        }

        internal override string ToITXML()
        {
            var sb = new StringBuilder();
            
            // Set the background color
            sb.Append(SetBackgroundColor());

            // Add Title
            sb.Append(ProcessTitle());

            // Add Data
            sb.Append(ProcessData());

            // Add Attribution
            sb.Append(ProcessCopyright());

            if (m_DateTimeAttribution != null)
                sb.Append(ProcessDateTimeAttribution());

            return sb.ToString();
        }

        #endregion

        private string ProcessData()
        {
            var chartIdentifier = 1;
            if (m_DataSet != null &&
                m_DataSet.Items != null &&
                m_DataSet.Items.Count > 0)
            {
                if (IsKeywordsChart)
                {
                    chartIdentifier = (m_DataSet.Items.Count > 10) ? 10 : m_DataSet.Items.Count;
                }
                else
                {
                    chartIdentifier = (m_DataSet.Items.Count > 5) ? 5 : m_DataSet.Items.Count;
                }
            }

            var sb = new StringBuilder();

            for (var i = 0; i < chartIdentifier; i++ )
            {
                if (string.IsNullOrEmpty(DataSet.Items[i]) || string.IsNullOrEmpty(DataSet.Items[i].Trim()))
                    continue;
                // if it is an even line add gray fill
                if (i%2 == 0)
                {
                    sb.AppendFormat("<cit:textbox name=\"{0}\" positioned-in-builder=\"true\" common=\"top:{1};width:370;height:22\"><cit:text content=\"{2}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"true\" font=\"name:Arial Unicode MS;color:black;size:12;\" fill-color=\"#F2F2F2\" /></cit:textbox>", "Text_" + i, TOP[i], StringUtilitiesManager.XmlEncode(DataSet.Items[i]));
                }
                    // otherwise add a white fill
                else 
                {
                    sb.AppendFormat("<cit:textbox name=\"{0}\" positioned-in-builder=\"true\" common=\"top:{1};width:370;height:22\"><cit:text content=\"{2}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;color:black;size:12;\"/></cit:textbox>", "Text_" + i, TOP[i], StringUtilitiesManager.XmlEncode(DataSet.Items[i]));
                }
            }

            return sb.ToString();

        }

        private string SetBackgroundColor()
        {
            return m_BackgroundColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor)) : string.Empty;
        }

        private string ProcessTitle()
        {
            return string.Format("<cit:textbox name=\"{0}\"><cit:text content=\"{1}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"true\" font=\"name:Arial Unicode MS;color:{2}\" fill-color=\"#E4E4ED\" /></cit:textbox>", Title.Name, StringUtilitiesManager.XmlEncode(Title.Text), ColorTranslator.ToHtml(Title.Color));
        }

        private string ProcessCopyright()
        {
            m_Copyright.Text = string.Format("© {0} Factiva", DateTime.Now.Year);
            return m_Copyright.ToITXML();
        }

        private string ProcessDateTimeAttribution()
        {
            var sb = new StringBuilder();
            //infosys:02/24/2010: increased left atribute and made it right align.
            sb.Append("<cit:shape name=\"datetime\" common=\"width:" + (WIDTH - 156) + ";height:8;top:" + (HEIGHT + 18) + ";left:"+ LEFT +";keep-proportions:true;\">");
            sb.Append("<cit:shape-settings type=\"polygon\" fill-enabled=\"false\" show-line=\"false\"/>");
            sb.AppendFormat("<cit:label text=\"as of {0}\" alignment=\"right\" font=\"name:Arial Unicode MS;size:10;color:#404040\" >", m_DateTimeFormatter.FormatStandardDateTime((DateTime)m_DateTimeAttribution));
            sb.Append("<cit:label-layout label-width=\"use-shape-width\" />");
            sb.Append("</cit:label>");
            sb.Append("<cit:path>");
            sb.Append("<cit:polygon-path data=\"m 0,0; l 195,0; l 195,16; l 0,16; z ; \"/>");
            sb.Append("</cit:path>");
            sb.Append("</cit:shape>");

            return sb.ToString();
        }

    } 
}
