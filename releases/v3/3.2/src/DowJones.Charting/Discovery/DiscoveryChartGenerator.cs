// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscoveryChartGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the DiscoveryChartGenerator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;
using DowJones.Managers.Core;

namespace DowJones.Charting.Discovery
{
    public class DiscoveryChartGenerator : AbstractChartGenerator
    {
        protected const string BASELINE_CHART_APPEARANCE_ITXML_FILE = "discovery/horizontalBar_{0}.itxml";
        protected const string BASELINE_CJK_CHART_APPEARANCE_ITXML_FILE = "discovery/horizontalBar_{0}_for_cjk.itxml";
        protected const string SAMPLE_CHART_APPEARANCE_ITXML_FILE = "discovery/sample_horizontalBar_10.itxml";
        protected const string SAMPLE_CJK_CHART_APPEARANCE_ITXML_FILE = "discovery/sample_horizontalBar_10_for_cjk.itxml";
        protected const string SAMPLE_DATA = "<cit:data><cit:column name=\"{0} A\"/><cit:column name=\"{0} B\"/><cit:column name=\"{0} C\"/><cit:column name=\"{0} D\"/><cit:column name=\"{0} E\"/><cit:column name=\"{0} F\"/><cit:column name=\"{0} G\"/><cit:column name=\"{0} H\"/><cit:column name=\"{0} I\"/><cit:column name=\"{0} J\"/><cit:row name=\"{0}\"><cit:data-item value=\"434\"/><cit:data-item value=\"339\"/><cit:data-item value=\"331\"/><cit:data-item value=\"320\"/><cit:data-item value=\"279\"/><cit:data-item value=\"247\"/><cit:data-item value=\"247\"/><cit:data-item value=\"242\"/><cit:data-item value=\"226\"/><cit:data-item value=\"172\"/></cit:row></cit:data>";
        protected const int WIDTH = 200;
        protected const int EXPORT_LAYOUT_WIDTH = 370;
        protected const int LEFT = 120; // Infosys;01/03/2010: increased the left margin.
        private static readonly int[] Height = new[] { 30, 30, 38, 52, 66, 80, 94, 108, 122, 136, 150 };
        private Color barColor = Color.Red;
        private OutputChartType outputChartType = OutputChartType.FLASH;
        private DiscoveryChartDataSet dataSet;
        private bool hasCJK;
        private bool isSampleChart;
        private bool isExportLayout;
        private DateTime? dateTimeAttribution;
        private DateTimeFormatter dateTimeFormatter;
        private string dateTimeFormatingPreference;
        private InterfaceLanguage interfaceLanguage;
        private Color titleFillColor = ColorTranslator.FromHtml("#E5E5E5");
        private Color titleFontColor = Color.Black;
        private string titleText = string.Empty;
        private bool useCache = true;
        private DiscoveryChartType chartType = DiscoveryChartType.Companies;
        private bool useGradient = true;
        private readonly IResourceTextManager _resources;

        #region Constructors

        public DiscoveryChartGenerator() 
            : this(new DiscoveryChartDataSet())
        {
        }

        public DiscoveryChartGenerator(DiscoveryChartDataSet dataSet)
            : this(dataSet, null)
        {
        }

        public DiscoveryChartGenerator(DiscoveryChartDataSet dataSet, DateTimeFormatter dateTimeFormatter, IResourceTextManager resources = null)
        {
            this.interfaceLanguage = InterfaceLanguage.en;
            this.dataSet = dataSet;
            this.dateTimeFormatter = dateTimeFormatter;
            this._resources = resources;
        }

        #endregion

        #region Properties

        public DiscoveryChartType ChartType
        {
            get { return chartType; }
            set { chartType = value; }
        }

        public OutputChartType OutputChartType
        {
            get { return outputChartType; }
            set { outputChartType = value; }
        }

        public bool IsSampleChart
        {
            get { return isSampleChart; }
            set { isSampleChart = value; }
        }

        public bool IsExportLayout
        {
            get { return isExportLayout; }
            set { isExportLayout = value; }
        }

        public bool IsTransparent { get; set; }

        public DateTime DateTimeAttribution
        {
            get
            {
                if (dateTimeAttribution != null)
                {
                    return (DateTime) dateTimeAttribution;
                }

                return DateTime.MinValue;
            }

            set
            {
                dateTimeAttribution = value;
            }
        }

        public InterfaceLanguage InterfaceLanguage
        {
            get
            {
                return interfaceLanguage;
            }

            set
            {
                interfaceLanguage = value;
                dateTimeFormatter = new DateTimeFormatter(value.ToString(), DateTimeFormatingPreference);
            }
        }

        public string DateTimeFormatingPreference
        {
            get
            {
                return dateTimeFormatingPreference;
            }
            
            set
            {
                dateTimeFormatingPreference = value;
                dateTimeFormatter = new DateTimeFormatter(InterfaceLanguage.ToString(), value);
            }
        }

        public Color BarColor
        {
            set
            {
                barColor = value;
            }
        }

        public Color TitleFillColor
        {
            get { return titleFillColor; }
            set { titleFillColor = value; }
        }

        public Color TitleFontColor
        {
            get { return titleFontColor; }
            set { titleFontColor = value; }
        }

        /// <summary>
        /// Gets or sets the title text.
        /// </summary>
        /// <value>The title text.</value>
        public string TitleText
        {
            get { return titleText; }
            set { titleText = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use cache].
        /// </summary>
        /// <value><c>true</c> if [use cache]; otherwise, <c>false</c>.</value>
        public bool UseCache
        {
            get { return useCache; }
            set { useCache = value; }
        }

        /// <summary>
        /// Gets or sets the data set.
        /// </summary>
        /// <value>The data set.</value>
        public DiscoveryChartDataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use gradient].
        /// </summary>
        /// <value><c>true</c> if [use gradient]; otherwise, <c>false</c>.</value>
        public bool UseGradient
        {
            get { return useGradient; }
            set { useGradient = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is secure.
        /// </summary>
        /// <value><c>true</c> if this instance is secure; otherwise, <c>false</c>.</value>
        public bool IsSecure { get; set; }

        #endregion

        #region AbstractChartGenerator Members

        public override IBytesResponse GetBytes()
        {
            var itxml = ToITXML(isSampleChart);
            hasCJK = StringUtilitiesManager.HasCJK(itxml) | StringUtilitiesManager.HasArabicSlashHebreCharacters(itxml);
            var template = GetChartTemplate();
            var bytes = ChartingManager.GetChartBytes(itxml, string.Empty, GetChartTemplate(), useCache);
            var response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        public override IUriResponse GetUri()
        {
            var itxml = ToITXML(isSampleChart);
            hasCJK = StringUtilitiesManager.HasCJK(itxml) | StringUtilitiesManager.HasArabicSlashHebreCharacters(itxml);
            var original = outputChartType;
            switch (outputChartType)
            {
                case OutputChartType.FLASH_WITH_ACTIVEX_FIX:
                    outputChartType = OutputChartType.FLASH;
                    break;
                case OutputChartType.GIF_WITH_JAVASCRIPT_INTERACTIVITY:
                    outputChartType = OutputChartType.GIF; 
                    break;
                case OutputChartType.JPEG_WITH_JAVASCRIPT_INTERACTIVITY:
                    outputChartType = OutputChartType.JPEG; 
                    break;
                case OutputChartType.PNG_WITH_JAVASCRIPT_INTERACTIVITY:
                    outputChartType = OutputChartType.PNG;
                    break;
            }
            
            var template = GetChartTemplate();
            outputChartType = original;
            var tempUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, template, useCache, IsSecure));
            var response = new ChartUriResponse(template.Height, template.Width, tempUri, original);
            return response;
        }

        public override IEmbeddedHTMLResponse GetHTML()
        {
            var itxml = ToITXML(isSampleChart);
            hasCJK = StringUtilitiesManager.HasCJK(itxml) | StringUtilitiesManager.HasArabicSlashHebreCharacters(itxml);
            var template = GetChartTemplate();
            var tempEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, GetChartTemplate(), useCache, IsSecure);
            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tempEmbededHTML, template.OutputChartType);
            return response;
        }

        internal override string ToITXML()
        {
            return ToITXML(false);
        }

        internal override ChartTemplate GetChartTemplate()
        {
            var template = new ChartTemplate();
            if (IsExportLayout && !IsSampleChart)
            {
                template.Width = EXPORT_LAYOUT_WIDTH;
            }
            else
            {
                template.Width = WIDTH;
            }
            
            template.OutputChartType = outputChartType;
            if (isSampleChart)
            {
                template.AppearanceFile = SAMPLE_CHART_APPEARANCE_ITXML_FILE;
                template.Height = Height[10];
                if (hasCJK)
                {
                    template.AppearanceFile = SAMPLE_CJK_CHART_APPEARANCE_ITXML_FILE;
                }
            }
            else
            {
                var chartIdentifier = 1;
                if (dataSet != null &&
                    dataSet.Items != null &&
                    dataSet.Items.Count > 0)
                {
                    chartIdentifier = (dataSet.Items.Count > 10) ? 10 : dataSet.Items.Count;
                }

                template.AppearanceFile = string.Format(BASELINE_CHART_APPEARANCE_ITXML_FILE, chartIdentifier);
                template.Height = (isExportLayout && !IsSampleChart) ? 180 : Height[chartIdentifier];
                if (hasCJK)
                {
                    template.AppearanceFile = string.Format(BASELINE_CJK_CHART_APPEARANCE_ITXML_FILE, chartIdentifier);
                }
            }

            return template;
        }
        #endregion

        internal string ToITXML(bool includeSampleData)
        {
            var mainSB = new StringBuilder();
            var discoveryChartTypeStr = StringUtilitiesManager.GetXmlEnumName<DiscoveryChartType>(chartType.ToString());
           
            // If export layout add a border around the chart
            if (IsExportLayout && !IsSampleChart)
            {
                if (IsTransparent)
                {
                    mainSB.AppendFormat("<cit:canvas show-border=\"true\" border-type=\"thin\" border-color=\"black\" fill-color=\"{0}\"/>", ColorTranslator.FromHtml("#FFFFFF00"));
                } 
                else
                {
                    mainSB.Append("<cit:canvas show-border=\"true\" border-type=\"thin\" border-color=\"black\" />");
                }

                mainSB.Append("<cit:image-template-layout width=\"200\" height=\"180\" auto-resize=\"false\" fit-in-bounds=\"true\" collision-protection=\"false\"/>");
                mainSB.Append(!UseGradient ? "<cit:bar-graph name=\"graph\" method=\"replace\" common =\"top:24;left:200;\" >" : "<cit:bar-graph name=\"graph\" method=\"replace\" common =\"top:24;left:200;width:80;\" >");
            }
            else
            {
                if (IsTransparent)
                {
                    mainSB.Append("<cit:canvas show-border=\"false\" transparency-percent=\"0\" />");
                } 

                if (!UseGradient)
                {
                    mainSB.Append("<cit:bar-graph name=\"graph\" method=\"replace\">");
                }
                else
                {
                    // Infosys:01/03/2010: increase left margin to accomodate longer names
                    mainSB.Append("<cit:bar-graph name=\"graph\" method=\"replace\" common =\"left:" + LEFT + ";width:50;\" >");
                }
            }

            // Add the custom Chart color picked by the user
            mainSB.Append("<cit:graph-settings>");
            
            // Add Data portion of Chart
            mainSB.AppendFormat("<cit:color-palette name=\"User Custom\" color-1=\"{0}\"/>", ColorTranslator.ToHtml(barColor));
            mainSB.Append("</cit:graph-settings>");

            var tempHSL = ColorUtilities.RGB_to_HSL(barColor);
            tempHSL.L = 300;

            if (UseGradient)
            {
                mainSB.AppendFormat("<cit:bar-settings gradient-color=\"{0}\"/>", ColorTranslator.ToHtml(ColorUtilities.HSL_to_RGB(tempHSL)));  // Currently set it to light gray
            }
            else
            {
                mainSB.AppendFormat("<cit:bar-settings bar-style=\"rectangle\" />");
                mainSB.Append("<cit:data-label position=\"outside-right\" />");
            }
            
            if (!includeSampleData &&
                dataSet != null &&
                dataSet.Items != null &&
                dataSet.Items.Count > 0)
            {
                mainSB.Append("<cit:data>");
                var columnSB = new StringBuilder();
                var dataItemSB = new StringBuilder();
                Column column;
                var i = 0;
                foreach (var item in dataSet.Items)
                {
                    if (!isSampleChart)
                    {
                        column = new Column
                                     {
                                         Name = dataSet.Items[i].Hover, 
                                         Hover = dataSet.Items[i].Hover, 
                                         Drilldown = dataSet.Items[i].Drilldown
                                     };
                        columnSB.AppendFormat(column.ToITXML());
                        dataItemSB.AppendFormat(item.ToITXML());
                        i++;
                    } 
                    else
                    {
                        column = new Column
                                     {
                                         Name = string.Concat(discoveryChartTypeStr, i)
                                     };
                        columnSB.AppendFormat(column.ToITXML());
                        dataItemSB.AppendFormat(item.ToITXML());
                        i++;
                    }
                }

                mainSB.Append(columnSB);
                mainSB.AppendFormat("<cit:row name=\"{0}\">", discoveryChartTypeStr);
                mainSB.Append(dataItemSB);
                mainSB.Append("</cit:row>");
                mainSB.Append("</cit:data>");
            }
            else if (includeSampleData)
            {
                // Generate Sample Data
                mainSB.AppendFormat(SAMPLE_DATA, _resources.GetString(discoveryChartTypeStr));
            }

            // If export layout increase the text width 
            if (IsExportLayout && !IsSampleChart)
            {
                mainSB.Append("<cit:category-scale position=\"left\" limit-label-length=\"true\" max-text-length=\"24\" font=\"name:Arial Unicode MS;size:11\"/>");
            }

            mainSB.Append("</cit:bar-graph>");
            
            // If export layout add a title bar at the top
            if (IsExportLayout && !IsSampleChart)
            {
                mainSB.Append("<cit:shape name=\"titlebox\" common=\"width:370;height:20;keep-proportions:false\">");
                mainSB.AppendFormat("<cit:shape-settings type=\"polygon\" fill-color=\"{0}\" shadow-settings=\"show:true;style:glow;width:3\" show-line=\"false\"/>", ColorTranslator.ToHtml(TitleFillColor));
                mainSB.AppendFormat("<cit:label text=\"{0}\" alignment=\"left\" font=\"name:Arial Unicode MS;size:11;color:{1}\" >", TitleText, ColorTranslator.ToHtml(TitleFontColor));
                mainSB.Append("<cit:label-layout label-width=\"use-shape-width\" />");
                mainSB.Append("</cit:label>");
                mainSB.Append("<cit:drilldown fill-color=\"#CDCDE3\"/>");
                mainSB.Append("<cit:path>");
                mainSB.Append("<cit:polygon-path data=\"m 0,0; l 195,0; l 195,16; l 0,16; z ; \"/>");
                mainSB.Append("</cit:path>");
                mainSB.Append("</cit:shape>");

                mainSB.Append("<cit:shape name=\"copyright\" common=\"width:200;height:20;top:160;keep-proportions:false;\">");
                mainSB.Append("<cit:shape-settings type=\"polygon\" fill-enabled=\"false\" show-line=\"false\"/>");
                mainSB.AppendFormat("<cit:label text=\"© {0} Factiva\" alignment=\"left\" font=\"name:Arial Unicode MS;size:9;color:#404040\" >", DateTime.Now.Year);
                mainSB.Append("<cit:label-layout label-width=\"use-shape-width\" />");
                mainSB.Append("</cit:label>");
                mainSB.Append("<cit:path>");
                mainSB.Append("<cit:polygon-path data=\"m 0,0; l 195,0; l 195,16; l 0,16; z ; \"/>");
                mainSB.Append("</cit:path>");
                mainSB.Append("</cit:shape>");

                if (dateTimeAttribution != null)
                {
                    mainSB.Append("<cit:shape name=\"datetime\" common=\"width:200;height:20;top:160;left:200;keep-proportions:false;\">");
                    mainSB.Append("<cit:shape-settings type=\"polygon\" fill-enabled=\"false\" show-line=\"false\"/>");
                    mainSB.AppendFormat("<cit:label text=\"as of {0}\" alignment=\"right\" font=\"name:Arial Unicode MS;size:9;color:#404040\" >", dateTimeFormatter.FormatStandardDateTime((DateTime) dateTimeAttribution));
                    mainSB.Append("<cit:label-layout label-width=\"use-shape-width\" />");
                    mainSB.Append("</cit:label>");
                    mainSB.Append("<cit:path>");
                    mainSB.Append("<cit:polygon-path data=\"m 0,0; l 195,0; l 195,16; l 0,16; z ; \"/>");
                    mainSB.Append("</cit:path>");
                    mainSB.Append("</cit:shape>");
                }
            }

            return mainSB.ToString();
        }
    }
}