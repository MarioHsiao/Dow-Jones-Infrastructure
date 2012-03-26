/* 
 * Author: Infosys
 * Date: 8/21/09
 * Purpose: Generate Itxml for Combo Time Line Graph.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */

using System.Drawing;
using System.Text;
using EMG.Tools.Charting.Common;
using EMG.Tools.Managers.Charting;
using EMG.Utility.Managers.Charting;
using EMG.Utility.Managers.Core;
using EMG.Tools.Charting.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace EMG.Tools.Charting.Common
{
    public class ComboTimeLineBarChartGenerator : AbstractChartGenerator
    {
        private const string BASE_APPERANCE_FILE = "common/m_chart_line.itxml";
        private Color COLOR_OUTERLINES = Color.Black;
        private const int HEIGHT = 400;
        private const int WIDTH = 500;
        private readonly TextBox m_Attribution = new TextBox("CopyRight");
        private TimeLineChartDataSet m_DataSet; 
        private BarChartDataSet mb_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private BaseLine m_BaseLine;
        private Color m_BackgroundColor = Color.White;
        private Color m_GridColor = ColorTranslator.FromHtml("#CCCCCC");
        private Color m_ScaleColor = ColorTranslator.FromHtml("#999999");
        
        private Line m_PlotLine;
        private bool m_UseCache = true;
        private double minValue = 0; //minimum value on the y-axis scale
        private double maxValue = 0; //maximum value on the y-axis scale 
        private string combo_xml = string.Empty;
        private string timeline_xml = string.Empty;
        private string m_copyright = string.Empty;
        private string m_priceLabel = string.Empty;
        private string m_volumeLabel = string.Empty; 


        private ArrayList arr_textbox = null; 
        


        #region Constructors

        public ComboTimeLineBarChartGenerator()
        {
            timeline_xml = this.ToITXMLComboTimeLine();
            
        }

        public ComboTimeLineBarChartGenerator(TimeLineChartDataSet DataSet)
        {
            m_DataSet = DataSet;
        }

        #endregion

        #region Properties
         
 

        public TextBox Attribution
        {
            get { return m_Attribution; }
        }

        public ArrayList Arr_TextBox
        {
            get { return arr_textbox; }
            set { arr_textbox = value; }

        }

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



        public BarChartDataSet BarChartDataSet 
        {   
            get { return mb_DataSet;} 
            set { mb_DataSet = value;}

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
                    m_BaseLine = new BaseLine();
                return m_BaseLine;
            }
            set { m_BaseLine = value; }
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

        public string TimeLineItXml 
        { 
        get 
            { return timeline_xml; }
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
            string itxml = ToITXML();
            ChartTemplate template = GetChartTemplate();
            byte[] bytes = ChartingManager.GetChartBytes(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            ChartBytesResponse response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        ///This method is not used and not tested.
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
            string tUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, "", template, m_UseCache));

            ChartUriResponse response = new ChartUriResponse(template.Height, template.Width, tUri, original);
            return response;
        }

        /// <summary>
        /// To generate HTML. No formatting changes made to the itxml. Only data is populated.
        /// </summary>
        /// <returns></returns>
        public override IEmbeddedHTMLResponse GetHTML()
        {
            string itxml = string.Empty;

            itxml = this.ToITXMLComboTimeLine() + this.ToITXMLBarGraphTimeLine();
            
            string pcscript = GetPCScript();
            ChartTemplate template = GetChartTemplate();
            string tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, pcscript, GetChartTemplate(), m_UseCache);

            ChartEmbeddedHTMLResponse response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
            return response;
        }

        /// <summary>
        /// Processes the ITXML for the Bar Graph section in the Combo Graph
        /// </summary>
        /// <returns></returns>
        public string ToITXMLBarGraphTimeLine()
        {
            StringBuilder sb = new StringBuilder();


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
            StringBuilder mainSB = new StringBuilder();
            StringBuilder outerSB = new StringBuilder();

            if (BarChartDataSet.Series != null)
            {
                mainSB.Append("<cit:data series-data-in-columns=\"true\">");
                for (int i = 1; i <= BarChartDataSet.Series.CategoryNames.Count; i++)
                {
                    string name = BarChartDataSet.Series.CategoryNames[i - 1];
                    outerSB.AppendFormat("<cit:bar-series name=\"{0}\" number=\"{1}\" />", name, i);
                    mainSB.AppendFormat("<cit:column name=\"{0}\"/>", name);
                }
                mainSB.Insert(0, outerSB.ToString());
                foreach (SeriesItem si in BarChartDataSet.Series.Items)
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\">", StringUtilitiesManager.XmlEncode(si.Name));
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

        internal override ChartTemplate GetChartTemplate()
        {
            ChartTemplate template = new ChartTemplate();
            template.Width = WIDTH;
            template.Height = HEIGHT;
            template.OutputChartType = m_OutputChartType;
            template.AppearanceFile = BASE_APPERANCE_FILE;
            return template;
        }

        /// <summary>
        /// Generates ITXML to be updated in the timeline.itxml template
        /// </summary>
        /// <returns></returns>
        internal override string ToITXML()
        {
            StringBuilder sb = new StringBuilder();
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
            StringBuilder sb = new StringBuilder();
            //Include any formatiing changes to ITXML here.

            //Set Scale
            sb.Append("<cit:time-graph name=\"graph\" method=\"replace\">");
            sb.AppendFormat("<cit:outer-lines max-line=\"color:{0}\" min-line=\"color:{0}\" low-outer-line=\"color:{0}\" high-outer-line=\"color:{0}\"/>", COLOR_OUTERLINES);
            sb.Append(SetScales());
            sb.Append("</cit:time-graph>");

            //Populate the data in ITXML
            //sb.Append(ProcessData()); //Commented because Corda has an issue with itxml format for data. PCScript used for this.

            //Set Legend
            // sb.Append(ProcessAttribution());

            return sb.ToString();


        }


        /// <summary>
        /// Scale settings for Time Line
        /// </summary>
        /// <returns></returns>
        private string SetScales()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:value-scale position=\"left\">");
            sb.AppendFormat("<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            if (minValue == 0 && maxValue == 0)
            {
                sb.Append("<cit:value-scale-divisions set-scale-values=\"automatically\" use-max-value-for-automatic=\"false\" include-zero=\"false\" maximum-major-divisions=\"10\"/>");
            }
            else
            {
                sb.AppendFormat("<cit:value-scale-divisions set-scale-values=\"automatically\" use-max-value-for-automatic=\"true\" use-min-value-for-automatic=\"true\" include-zero=\"false\" minimum-value=\"{0}\" maximum-value=\"{1}\" maximum-major-divisions=\"10\"/>", minValue, maxValue);
            }

            sb.Append("</cit:value-scale>");
            sb.Append("<cit:time-value-scale set-scales-values=\"manually\" eliminate-white-space=\"true\" set-tick-increment=\"automatically\" show-quarters=\"true\">");
            sb.AppendFormat("<cit:time-value-scale-labels rotate-labels=\"false\" font=\"name:Arial Unicode MS;color:{0}\" minor-font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("</cit:time-value-scale>");
            return sb.ToString();
        }


        


        /// <summary>
        /// PCScript used for timeseries. Corda dll from vendor has a bug with itxml.
        /// </summary>
        /// <returns></returns> 
        private string GetPCScript()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TimeSeries timeSeries in DataSet.TimeSeries)
            {
                if (timeSeries.TimeItems.Count > 0)
                {
                    sb.Append("graph.setseries(series1;");
                    foreach (TimeItem item in timeSeries.TimeItems)
                    {
                        sb.Append(
                            string.Concat(
                                item.Date.ToString("MM'/'dd'/'yyyy"),
                                ",",
                                item.Value,
                                ";"
                                )
                            );
                    }
                    sb.Append(")");
                }
                if (BaseLine.IsEnabled)
                {
                    sb.Append("graph.setseries(trendline;");
                    foreach (TimeItem item in timeSeries.TimeItems)
                    {
                        sb.Append(
                            string.Concat(
                                item.Date.ToString("MM'/'dd'/'yyyy"),
                                ",",
                                BaseLine.Value,
                                ";"
                                )
                            );
                    }
                    sb.Append(")");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Uses ITXML format to populate data. Corda dll has an issue with populating data in itmxl format. 
        /// Hence this function is not used. To be tested and used once Corda provides a fix for the issue.
        /// </summary>
        /// <returns></returns>
        private string ProcessData()
        {
            StringBuilder sb = new StringBuilder();
            if (DataSet.TimeSeries != null)
            {
                //sb.Append(
                //    "<cit:date-time-format override-input-format=\"true\" input-format=\"%m/%d/%Y\" override-month-names=\"true\" month-names=\"${sJan},${sFeb},${sMar},${sApr},${sMay},${sJun},${sJul},${Aug},${Sep},${Oct},${Nov},${Dec}\" override-day-names=\"true\" day-names=\"${sSun},${Mon},${Tue},${Wed},${Thu},${Fri},${Sat}\"/>");
                for (int timeSeriesCount = 0; timeSeriesCount < DataSet.TimeSeries.Count; timeSeriesCount++)
                {
                    sb.AppendFormat("<cit:filled-line-series name=\"{0}\" number=\"{1}\" fill-color=\"{2}\" />", DataSet.TimeSeries[timeSeriesCount].Name, timeSeriesCount + 1, "#54559B");
                }
                sb.Append("<cit:time-data>");
                for (int timeSeriesCount = 0; timeSeriesCount < DataSet.TimeSeries.Count; timeSeriesCount++)
                {
                    sb.AppendFormat("<cit:time name=\"{0}\">", DataSet.TimeSeries[timeSeriesCount].Name);
                    if (DataSet.TimeSeries[timeSeriesCount].TimeItems != null)
                    {
                        for (int dataItemCount = 0;
                             dataItemCount < DataSet.TimeSeries[timeSeriesCount].TimeItems.Count;
                             ++dataItemCount)
                        {
                            //sb.AppendFormat("<cit:time-item date=\"{0}\" value=\"{1}\" />", DataSet.Items[timeSeriesCount].TimeItems[dataItemCount].Date, DataSet.Items[timeSeriesCount].TimeItems[dataItemCount].Value);
                            sb.Append(DataSet.TimeSeries[timeSeriesCount].TimeItems[dataItemCount].ToITXML());
                        }
                    }
                    sb.Append("</cit:time>");
                }
                sb.Append("</cit:time-data>");
                return sb.ToString();
            }
            return "<cit:time-data />";
        }

        /// <summary>
        /// Process Attribution
        /// </summary>
        /// <returns></returns> 
        /// 
        private string ProcessComboAttribution()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<cit:textbox name=\"StockEx\" positioned-in-builder=\"true\" common=\"top:20;left:220;width:300;height:11\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[1].ToString()));
            sb.Append("<cit:textbox-settings horiz-justification=\"right\" show-border=\"false\" fill-color=\"white\" font=\"size:10\">");
            sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"0\" right-margin=\"0\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Price\" positioned-in-builder=\"true\" common=\"width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(PriceLabel));
            sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-color=\"#E6EFBA\" font=\"size:10\">");
            sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" left-margin=\"5\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Volume\" positioned-in-builder=\"true\" common=\"top:246;width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(VolumeLabel));
            sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-color=\"#E6EFBA\" font=\"size:10\">");
            sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" left-margin=\"5\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Date\" positioned-in-builder=\"true\" common=\"top:367;width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[2].ToString()));
            sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-color=\"#D8D8D8\" font=\"size:10\">");
            sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" left-margin=\"5\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Title\" positioned-in-builder=\"true\" common=\"top:20;left:50;width:540;height:11\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[0].ToString()));
            sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled= \"false\" font=\"size:10\">");
            sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"0\" right-margin=\"0\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"CopyRight\" positioned-in-builder=\"true\" common=\"top:385;width:145;height:22\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(CopyrightLabel));
            sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"size:10;color:#727272\">");
            sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-width=\"true\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"5\"/>");
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