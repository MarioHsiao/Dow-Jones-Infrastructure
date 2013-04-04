// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Dow Jones" file="ComboTimeLineBarChartGenerator.cs">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ComboTimeLineBarChartGenerator type.
//   Author: Infosys
//   Date: 8/21/09
//   Purpose: Generate Itxml for Combo Time Line Graph.
//
//  
// Mod Log
// -----------------------------------------------------------------------------
//   Modified by                          Date                    Purpose
// -----------------------------------------------------------------------------
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections;
using System.Drawing;
using System.Text;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Fcp;
using DowJones.Charting.Manager;
using DowJones.Managers.Core;

namespace DowJones.Charting.Common
{
    public class ComboTimeLineBarChartGenerator : AbstractChartGenerator
    {
        private const string BaseApperanceFile = "common/m_chart_line.itxml";
        private const string BaseApperanceFileHvolume = "common/m_chart_line_hvolume.itxml";
        private const int Height = 400;
        private const int Width = 500;
        private readonly TextBox attribution = new TextBox("CopyRight");
        private readonly Color colorOuterlines = Color.Black;
        private Color backgroundColor = Color.White;
        private BaseLine baseLine;
        private string m_copyright = string.Empty;
        private TimeLineChartDataSet m_DataSet;
        private Color m_GridColor = ColorTranslator.FromHtml("#CCCCCC");
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;

        private Line m_PlotLine;
        private string m_priceLabel = string.Empty;
        private Color m_ScaleColor = ColorTranslator.FromHtml("#999999");
        private bool m_UseCache = true;
        private string m_volumeLabel = string.Empty;
        private double maxValue; //maximum value on the y-axis scale 
        private double minValue; //minimum value on the y-axis scale
        private readonly string timeline_xml = string.Empty;

        #region Constructors

        public ComboTimeLineBarChartGenerator()
        {
        }

        public ComboTimeLineBarChartGenerator(TimeLineChartDataSet DataSet)
        {
            m_DataSet = DataSet;
        }

        #endregion

        #region Properties

        public TextBox Attribution
        {
            get { return attribution; }
        }

        public ArrayList Arr_TextBox { get; set; }

        public bool ShowVolume { get; set; }

        protected bool UseCache
        {
            get { return m_UseCache; }
            set { m_UseCache = value; }
        }

        public TimeLineChartDataSet DataSet
        {
            get
            {
                if (m_DataSet == null)
                    m_DataSet = new TimeLineChartDataSet();
                return m_DataSet;
            }
            set { m_DataSet = value; }
        }


        public BarChartDataSet BarChartDataSet { get; set; }

        public OutputChartType OutputChartType
        {
            get { return m_OutputChartType; }
            set { m_OutputChartType = value; }
        }

        public BaseLine BaseLine
        {
            get
            {
                if (baseLine == null)
                    baseLine = new BaseLine();
                return baseLine;
            }
            set { baseLine = value; }
        }

        public Line PlotLine
        {
            get
            {
                if (m_PlotLine == null)
                    m_PlotLine = new Line();
                return m_PlotLine;
            }
            set { m_PlotLine = value; }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
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

        public string TimeLineItXml
        {
            get { return timeline_xml; }
        }


        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }


        public string CopyrightLabel
        {
            get { return m_copyright; }
            set { m_copyright = value; }
        }

        public bool ShowLegend { get; set; }

        public string PriceLabel
        {
            get { return m_priceLabel; }
            set { m_priceLabel = value; }
        }

        public string VolumeLabel
        {
            get { return m_volumeLabel; }
            set { m_volumeLabel = value; }
        }

        #endregion

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
            var tUri =
                ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml,
                                                                                                                 "",
                                                                                                                 template,
                                                                                                                 m_UseCache));

            var response = new ChartUriResponse(template.Height, template.Width, tUri, original);
            return response;
        }

        /// <summary>
        /// To generate HTML. No formatting changes made to the itxml. Only data is populated.
        /// </summary>
        /// <returns></returns>
        public override IEmbeddedHTMLResponse GetHTML()
        {
            var itxml = (ShowVolume) ? ToITXMLComboTimeLine() + ToITXMLBarGraphTimeLine() : ToITXMLComboTimeLine();
            //itxml = this.ToITXMLComboTimeLine() + this.ToITXMLBarGraphTimeLine();
            var pcscript = GetPCScript();
            var template = GetChartTemplate();
            var tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, pcscript, GetChartTemplate(),
                                                                             m_UseCache);
            tEmbededHTML = tEmbededHTML.Replace("/>", "border=\"0\"/>"); //remove the border.
            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML,
                                                         template.OutputChartType);
            return response;
        }

        /// <summary>
        /// Processes the ITXML for the Bar Graph section in the Combo Graph
        /// </summary>
        /// <returns></returns>
        public string ToITXMLBarGraphTimeLine()
        {
            var sb = new StringBuilder();


            sb.Append("<cit:bar-graph name=\"BarGraph\" method=\"replace\">");
            //sb.Append(SetScales());

            // Add Data
            sb.Append(ProcessBarGraphData());

            // Add Legend

            sb.Append("</cit:bar-graph>");
            sb.Append(ProcessComboAttribution());
            return sb.ToString();
        }


        private string ProcessBarGraphData()
        {
            // Get the Bar-Series nodes
            //<cit:bar-series name="Series 1" number="1" />
            var mainSB = new StringBuilder();
            var outerSB = new StringBuilder();

            if (BarChartDataSet.Series != null)
            {
                mainSB.Append("<cit:data series-data-in-columns=\"true\">");
                for (var i = 1; i <= BarChartDataSet.Series.CategoryNames.Count; i++)
                {
                    var name = BarChartDataSet.Series.CategoryNames[i - 1];
                    outerSB.AppendFormat("<cit:bar-series name=\"{0}\" number=\"{1}\" />", name, i);
                    mainSB.AppendFormat("<cit:column name=\"{0}\"/>", name);
                }
                mainSB.Insert(0, outerSB.ToString());
                foreach (var si in BarChartDataSet.Series.Items)
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\">", StringUtilitiesManager.XmlEncode(si.Name));
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

        internal override ChartTemplate GetChartTemplate()
        {
            var template = new ChartTemplate
            {
                Width = Width,
                Height = Height,
                OutputChartType = m_OutputChartType,
                AppearanceFile = (ShowVolume) ? BaseApperanceFile : BaseApperanceFileHvolume
            };
            return template;
        }

        /// <summary>
        /// Generates ITXML to be updated in the timeline.itxml template
        /// </summary>
        /// <returns></returns>
        internal override string ToITXML()
        {
            var sb = new StringBuilder();
            //Include any formatiing changes to ITXML here.

            //Set Scale
            sb.Append("<cit:time-graph name=\"graph\" method=\"replace\">");
            sb.Append(SetScales());
            sb.Append("</cit:time-graph>");

            //Populate the data in ITXML
            //sb.Append(ProcessData()); //Commented because Corda has an issue with itxml format for data. PCScript used for this.

            //Set Legend
            sb.Append(ProcessAttribution());

            return sb.ToString();
        }

        public string ToITXMLComboTimeLine()
        {
            var sb = new StringBuilder();
            //Include any formatiing changes to ITXML here.
            //<cit:time-graph name="graph" common="top:33;left:50;width:470;height:158">
            //Set Scale 
            if (ShowLegend)
            {
                if (ShowVolume)
                {
                    sb.Append("<cit:time-graph name=\"graph\" common=\"height:158\" method=\"replace\">");
                }
                else
                {
                    sb.Append("<cit:time-graph name=\"graph\" common=\"height:275\" method=\"replace\">");
                }
            }
            else
            {
                sb.Append("<cit:time-graph name=\"graph\" method=\"replace\">");
            }
            //sb.Append("<cit:time-graph name=\"graph\" method=\"replace\">");
            sb.AppendFormat(
                "<cit:outer-lines max-line=\"color:{0}\" min-line=\"color:{0}\" low-outer-line=\"color:{0}\" high-outer-line=\"color:{0}\"/>",
                colorOuterlines);
            sb.Append(SetScales());
            if (ShowLegend)
            {
                if (ShowVolume)
                {
                    sb.Append("<cit:graph-legend common=\"visible:true\">");
                }
                else
                {
                    //Infosys:03/17/2010: fixed PTG defect for legend overlapping with x-axis in hide 
                    //volumen scenario, increasing the top margin for legend
                    sb.Append("<cit:graph-legend common=\"top:358;visible:true\">");
                }
                sb.Append("</cit:graph-legend>");
            }
            sb.Append("</cit:time-graph>");

            if (!ShowVolume)
            {
                sb.Append(ProcessComboAttributionWV());
            }

            return sb.ToString();
        }


        private string ProcessComboAttributionWV()
        {
            var sb = new StringBuilder();

            sb.Append("<cit:textbox name=\"StockEx\" positioned-in-builder=\"true\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[1].ToString()));

            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Price\" positioned-in-builder=\"true\" common=\"width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(PriceLabel));
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Date\" positioned-in-builder=\"true\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[2].ToString()));

            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Title\" positioned-in-builder=\"true\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[0].ToString()));

            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"CopyRight\" positioned-in-builder=\"true\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(CopyrightLabel));

            sb.Append("</cit:textbox>");
            return sb.ToString();
        }


        /// <summary>
        /// Scale settings for Time Line
        /// </summary>
        /// <returns></returns>
        private string SetScales()
        {
            var sb = new StringBuilder();
            sb.Append("<cit:value-scale position=\"left\">");
            sb.AppendFormat(
                "<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;color:{0}\"/>",
                ColorTranslator.ToHtml(ScaleColor));
            if (minValue == 0 && maxValue == 0)
            {
                sb.Append(
                    "<cit:value-scale-divisions set-scale-values=\"automatically\" use-max-value-for-automatic=\"false\" include-zero=\"false\" maximum-major-divisions=\"10\"/>");
            }
            else
            {
                sb.AppendFormat(
                    "<cit:value-scale-divisions set-scale-values=\"automatically\" use-max-value-for-automatic=\"true\" use-min-value-for-automatic=\"true\" include-zero=\"false\" minimum-value=\"{0}\" maximum-value=\"{1}\" maximum-major-divisions=\"10\"/>",
                    minValue, maxValue);
            }

            sb.Append("</cit:value-scale>");
            sb.Append(
                "<cit:time-value-scale set-scales-values=\"manually\" eliminate-white-space=\"true\" set-tick-increment=\"automatically\">");
            sb.AppendFormat(
                "<cit:time-value-scale-labels rotate-labels=\"false\" font=\"name:Arial Unicode MS;color:{0}\" minor-font=\"name:Arial Unicode MS;color:{0}\"/>",
                ColorTranslator.ToHtml(ScaleColor));
            sb.Append("</cit:time-value-scale>");
            return sb.ToString();
        }



        /// <summary>
        /// PCScript used for timeseries. Corda dll from vendor has a bug with itxml.
        /// </summary>
        /// <returns></returns> 
        private string GetPCScript()
        {
            var sb = new StringBuilder();
            foreach (var timeSeries in DataSet.TimeSeries)
            {
                if (timeSeries.TimeItems.Count <= 0) continue;
                var series = timeSeries.Name;
                //sb.Append("graph.setseries(series1;");
                sb.Append(string.Concat("graph.setseries(", ChartingManager.EncodeSpecialCharsForPcScript(series), ";"));

                foreach (var item in timeSeries.TimeItems)
                {
                    sb.Append(
                        string.Concat(
                            item.Date.ToString("MM'/'dd'/'yyyy"),
                            ",",
                            ChartingManager.EncodeSpecialCharsForPcScript(item.Value),
                            ";"
                            )
                        );
                }
                sb.Append(")");
            }

            //// HoverText Addition
            var seriesIndex = 1;
            foreach (var timeSeries in DataSet.TimeSeries)
            {
                if (timeSeries.TimeItems.Count > 0)
                {
                    sb.Append("graph.addHoverText(");
                    var dataItemPosition = 1;
                    foreach (var item in timeSeries.TimeItems)
                    {
                        sb.Append(string.Concat(ChartingManager.EncodeSpecialCharsForPcScript(dataItemPosition), ",", ChartingManager.EncodeSpecialCharsForPcScript(seriesIndex), ",", ChartingManager.EncodeSpecialCharsForPcScript(item.Hover), ";"));
                        dataItemPosition++;
                    }

                    sb.Append(")");
                }

                seriesIndex++;
            }

            //// Drilldown Addition
            seriesIndex = 1;
            foreach (var timeSeries in DataSet.TimeSeries)
            {
                if (timeSeries.TimeItems.Count > 0)
                {
                    sb.Append("main.paramdelimiter(|)graph.ddEnable(");
                    var dataItemPosition = 1;
                    foreach (var item in timeSeries.TimeItems)
                    {
                        sb.Append(item.Drilldown != string.Empty ? string.Concat(dataItemPosition, "|", seriesIndex, "|", ChartingManager.EncodeSpecialCharsForPcScript(item.Drilldown.Substring(0, item.Drilldown.Length - 1)), ";") : string.Concat(dataItemPosition, "|", seriesIndex, "|", ";"));
                        dataItemPosition++;
                    }

                    sb.Append(")");
                }

                seriesIndex++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Process Attribution
        /// </summary>
        /// <returns></returns> 
        /// 
        private string ProcessComboAttribution()
        {
            var sb = new StringBuilder();

            sb.Append(
                "<cit:textbox name=\"StockEx\" positioned-in-builder=\"true\" common=\"top:20;left:220;width:300;height:11\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[1].ToString()));
            sb.Append(
                "<cit:textbox-settings horiz-justification=\"right\" show-border=\"false\" fill-color=\"white\" font=\"size:10\">");
            sb.Append(
                "<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"0\" right-margin=\"0\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Price\" positioned-in-builder=\"true\" common=\"width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(PriceLabel));
            sb.Append(
                "<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-color=\"#E6EFBA\" font=\"size:10\">");
            sb.Append(
                "<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" left-margin=\"5\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Volume\" positioned-in-builder=\"true\" common=\"width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(VolumeLabel));
            sb.Append(
                "<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-color=\"#E6EFBA\" font=\"size:10\">");
            sb.Append(
                "<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" left-margin=\"5\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Date\" positioned-in-builder=\"true\" common=\"width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[2].ToString()));
            sb.Append(
                "<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-color=\"#D8D8D8\" font=\"size:10\">");
            sb.Append(
                "<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" left-margin=\"5\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append(
                "<cit:textbox name=\"Title\" positioned-in-builder=\"true\" common=\"top:20;left:50;width:540;height:11\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[0].ToString()));
            sb.Append(
                "<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled= \"false\" font=\"size:10\">");
            sb.Append(
                "<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"0\" right-margin=\"0\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append(
                "<cit:textbox name=\"CopyRight\" positioned-in-builder=\"true\" common=\"top:385;width:145;height:22\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(CopyrightLabel));
            sb.Append(
                "<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"size:10;color:#727272\">");
            sb.Append(
                "<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-width=\"true\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"5\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            return sb.ToString();
        }

        private string ProcessAttribution()
        {
            return Attribution.ToITXMLTimeLine();
        }
    }
}