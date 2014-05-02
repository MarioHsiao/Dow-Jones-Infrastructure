// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscoveryBarChartGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the DiscoveryBarChartGenerator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Palettes;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;
using DowJones.Managers.Core;

namespace DowJones.Charting.Discovery
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class DiscoveryBarChartGenerator : AbstractChartGenerator
    {
        #region Constants

        protected const int BASE_HEIGHT = 140;
        protected const string BASE_APPERANCE_FILE = "/discovery/discovery_bar.itxml";
        protected const int BASE_WIDTH = 175;

        protected const int EXPORT_LAYOUT_HEIGHT = 250;
        protected const string EXPORT_LAYOUT_APPERANCE_FILE = "/discovery/discovery_bar_export.itxml";
        protected const int EXPORT_LAYOUT_WIDTH = 370;
        
        #endregion
        
        #region Private Variables

        private readonly TextBox _title = new TextBox("textbox");
        private readonly TextBox _copyright = new TextBox("copyright");
        private readonly TextBox _startDate = new TextBox("StartDate");
        private readonly TextBox _endDate = new TextBox("EndDate");
        private readonly TextBox _distribution = new TextBox("Distribution");
        private Color _backgroundColor = Color.White;
        private AbstractColorPalette _colorPalette = new DiscoveryBarChartColorPalette();
        private OutputChartType _outputChartType = OutputChartType.FLASH;
        private bool _useCache = true;
        private DateTime? _dateTimeAttribution;
        private InterfaceLanguage _interfaceLanguage = InterfaceLanguage.en;
        private DateTimeFormatter _dateTimeFormatter;
        private string _dateTimeFormatingPreference;
       
        private DiscoveryBarChartDataSet _dataSet;
        private bool _useGradient = true;

        #endregion

        #region Constructors

        public DiscoveryBarChartGenerator()
        {
            _dateTimeFormatter = new DateTimeFormatter(InterfaceLanguage.ToString(), DateTimeFormatingPreference);
        }

        #endregion

        #region Properties

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public AbstractColorPalette ColorPalette
        {
            get { return _colorPalette; }
            set { _colorPalette = value; }
        }

        public OutputChartType OutputChartType
        {
            get { return _outputChartType; }
            set { _outputChartType = value; }
        }

        public bool UseCache
        {
            get { return _useCache; }
            set { _useCache = value; }
        }

        public bool IsExportLayout { get; set; }

        public DateTime DateTimeAttribution
        {
            get
            {
                if (_dateTimeAttribution != null)
                {
                    return (DateTime) _dateTimeAttribution;
                }

                return DateTime.MinValue;
            }

            set
            {
                _dateTimeAttribution = value;
            }
        }

        public InterfaceLanguage InterfaceLanguage
        {
            get
            {
                return _interfaceLanguage;
            }

            set 
            {
                _interfaceLanguage = value;
                _dateTimeFormatter = new DateTimeFormatter(value.ToString(), DateTimeFormatingPreference);
            }
        }

        public string DateTimeFormatingPreference
        {
            get
            {
                return _dateTimeFormatingPreference;
            }

            set
            {
                _dateTimeFormatingPreference = value;
                _dateTimeFormatter = new DateTimeFormatter(InterfaceLanguage.ToString(), value);
            }
        }

        public TextBox Title
        {
            get
            {
                return _title;
            }
        }

        public TextBox StartDate
        {
            get
            {
                return _startDate;
            }
        }

        public TextBox EndDate
        {
            get
            {
                return _endDate;
            }
        }

        public TextBox Distribution
        {
            get
            {
                return _distribution;
            }
        }

        public DiscoveryBarChartDataSet DataSet
        {
            get { return _dataSet ?? (_dataSet = new DiscoveryBarChartDataSet()); }
            set { _dataSet = value; }
        }

        public bool UseGradient
        {
            get { return _useGradient; }
            set { _useGradient = value; }
        }

        public bool IsSecure { get; set; }

        #endregion

        #region Overrides of AbstractChartGenerator

        public override IBytesResponse GetBytes()
        {
            var itxml = ToITXML();
            var template = GetChartTemplate();
            var bytes = ChartingManager.GetChartBytes(itxml, string.Empty, GetChartTemplate(), _useCache);
            var response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        public override IUriResponse GetUri()
        {
            var itxml = ToITXML();
            var original = _outputChartType;
            switch (_outputChartType)
            {
                case OutputChartType.FLASH_WITH_ACTIVEX_FIX:
                    _outputChartType = OutputChartType.FLASH;
                    break;
                case OutputChartType.GIF_WITH_JAVASCRIPT_INTERACTIVITY:
                    _outputChartType = OutputChartType.GIF;
                    break;
                case OutputChartType.JPEG_WITH_JAVASCRIPT_INTERACTIVITY:
                    _outputChartType = OutputChartType.JPEG;
                    break;
                case OutputChartType.PNG_WITH_JAVASCRIPT_INTERACTIVITY:
                    _outputChartType = OutputChartType.PNG;
                    break;
            }

            var template = GetChartTemplate();
            _outputChartType = original;
            var tempUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, template, _useCache, IsSecure));

            var response = new ChartUriResponse(template.Height, template.Width, tempUri, original);
            return response;
        }

        public override IEmbeddedHTMLResponse GetHTML()
        {
            var itxml = ToITXML();
            var template = GetChartTemplate();
            var tempEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, GetChartTemplate(), _useCache, IsSecure);
            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tempEmbededHTML, template.OutputChartType);
            return response;
        }

        internal override ChartTemplate GetChartTemplate()
        {
            return !IsExportLayout ? new ChartTemplate(BASE_APPERANCE_FILE, BASE_WIDTH, BASE_HEIGHT, OutputChartType) : new ChartTemplate(EXPORT_LAYOUT_APPERANCE_FILE, EXPORT_LAYOUT_WIDTH, EXPORT_LAYOUT_HEIGHT, OutputChartType);
        }

        internal override string ToITXML()
        {
            var sb = new StringBuilder();

            sb.Append(SetBackgroundColor());
            sb.Append("<cit:bar-graph name=\"graph\" method=\"replace\">");

            // Add the color palette
            sb.Append("<cit:graph-settings>");
            sb.Append(ColorPalette.ToITXML());
            sb.Append("</cit:graph-settings>");

            // Add Data
            sb.Append(ProcessData());
            sb.Append(ProcessBarSettings());  // Infosys - 11/18/2009 - Bar width Configuration - Added
            sb.Append("</cit:bar-graph>");

            // Add Title and Copyright if Export
            if (IsExportLayout)
            {
                sb.Append(ProcessTitle());
                sb.Append(ProcessCopyright());
                if (_dateTimeAttribution != null)
                {
                    sb.Append(ProcessDateTimeAttribution());
                }
            }

            sb.Append(ProcessDistribution());
            sb.Append(ProcessStartDate());
            sb.Append(ProcessEndDate());

            return sb.ToString();
        }

        #endregion
        
        // Infosys - 11/18/2009 - Bar width Configuration - Start
        private string ProcessBarSettings()
        {
            var barSetting = string.Empty;
            var temp = ColorTranslator.ToHtml(_colorPalette.Palette[0]);

            var decGradient = Int32.Parse(temp.Replace("#", string.Empty), NumberStyles.HexNumber) + 4539441;
            var hexGradient = "#" + decGradient.ToString("X");

            if (DataSet.Series != null)
            {
                var barCount = 0;

                foreach (var si in DataSet.Series.Items)
                {
                    barCount = barCount + si.DataItems.Count;
                    if (barCount > 8)
                    {
                        break;
                    }
                }

                barSetting = barCount > 8 ? string.Format("<cit:bar-settings bar-style=\"{0}\" percent-space-between-bars=\"1\" percent-bar-width=\"99\" gradient-direction=\"right-left\" gradient-type=\"color\" gradient-color=\"{1}\"/>", (UseGradient ? "gradient" : "rectangle"), hexGradient) : string.Format("<cit:bar-settings bar-style=\"{0}\" percent-space-between-bars=\"1\" percent-bar-width=\"99\" fixed-width=\"true\" gradient-direction=\"right-left\" gradient-type=\"color\" gradient-color=\"{1}\"/>", (UseGradient ? "gradient" : "rectangle"), hexGradient);
            }

            return barSetting;
        }

        // Infosys - 11/18/2009 - Bar width Configuration - End
        private string SetBackgroundColor()
        {
            return _backgroundColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor)) : string.Empty;
        }

        private string ProcessTitle()
        {
            return string.Format("<cit:textbox name=\"{0}\"><cit:text content=\"{1}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"true\" font=\"name:Arial Unicode MS;color:{2}\" fill-color=\"#E4E4ED\" /></cit:textbox>", Title.Name, StringUtilitiesManager.XmlEncode(Title.Text), ColorTranslator.ToHtml(Title.Color));
        }

        private string ProcessCopyright()
        {
            _copyright.Text = string.Format("© {0} Factiva", DateTime.Now.Year);
            return _copyright.ToITXML();
        }

        private string ProcessDateTimeAttribution()
        {
            if (_dateTimeAttribution == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            sb.Append("<cit:shape name=\"datetime\" common=\"width:" + EXPORT_LAYOUT_WIDTH + ";height:12;top:" + (EXPORT_LAYOUT_HEIGHT - 15) + ";keep-proportions:true;\">");
            sb.Append("<cit:shape-settings type=\"polygon\" fill-enabled=\"false\" show-line=\"false\"/>");
            sb.AppendFormat("<cit:label text=\"as of {0}\" alignment=\"right\" font=\"name:Arial Unicode MS;size:9;color:#404040\" >", _dateTimeFormatter.FormatStandardDateTime((DateTime) _dateTimeAttribution));
            sb.Append("<cit:label-layout label-width=\"use-shape-width\" />");
            sb.Append("</cit:label>");
            sb.Append("<cit:path>");
            sb.Append("<cit:polygon-path data=\"m 0,0; l 195,0; l 195,16; l 0,16; z ; \"/>");
            sb.Append("</cit:path>");
            sb.Append("</cit:shape>");
            return sb.ToString();
        }

        private string ProcessStartDate()
        {
            return _startDate.ToITXML();
        }

        private string ProcessEndDate()
        {
            return _endDate.ToITXML();
        }

        private string ProcessDistribution()
        {
            return _distribution.ToITXML();
        }

        private string ProcessData()
        {
            // Get the Bar-Series nodes
            // <cit:bar-series name="Series 1" number="1" />
            var mainSB = new StringBuilder();
            var outerSB = new StringBuilder();

            if (DataSet.Series != null)
            {
                mainSB.Append("<cit:data series-data-in-columns=\"true\">");
                for (var i = 1; i <= DataSet.Series.CategoryNames.Count; i++)
                {
                    var name = DataSet.Series.CategoryNames[i - 1];
                    outerSB.AppendFormat("<cit:bar-series name=\"{0}\" number=\"{1}\" />", name, i);
                    mainSB.AppendFormat("<cit:column name=\"{0}\"/>", name);
                }

                mainSB.Insert(0, outerSB.ToString());
                foreach (var si in DataSet.Series.Items)
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\">", si.Name);

                    //// Get the dict obj
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
