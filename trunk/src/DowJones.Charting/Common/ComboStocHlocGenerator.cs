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

using System;
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
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class ComboStockHlocGenerator : AbstractChartGenerator
    {
        private const string BASE_APPERANCE_FILE = "common/chart_hloc.itxml";
        private const string BASE_APPERANCE_FILE_HVOLUME = "common/chart_hloc_hvolume.itxml";
        private const int HEIGHT = 400;
        private const int WIDTH = 500;
        private readonly TextBox m_Attribution = new TextBox("CopyRight");
        private Color COLOR_OUTERLINES = Color.Black;
        private string combo_xml = string.Empty;
        private Color m_BackgroundColor = Color.White;
        private BaseLine m_BaseLine;
        private StockChartDataSet m_DataSet;
        private Color m_GridColor = ColorTranslator.FromHtml("#CCCCCC");
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;

        private Line m_PlotLine;
        private Color m_ScaleColor = ColorTranslator.FromHtml("#999999");
        private bool m_UseCache = true;
        private string m_copyright = string.Empty;
        private string m_priceLabel = string.Empty;
        private string m_volumeLabel = string.Empty;
        private double maxValue; //maximum value on the y-axis scale 
        private double minValue; //minimum value on the y-axis scale
        private string timeline_xml = string.Empty;

        #region Constructors

        public ComboStockHlocGenerator()
        {
        }

        public ComboStockHlocGenerator(StockChartDataSet DataSet)
        {
            m_DataSet = DataSet;
        }

        #endregion

        #region Properties

        public TextBox Attribution
        {
            get { return m_Attribution; }
        }

        public ArrayList Arr_TextBox { get; set; }

        protected bool UseCache
        {
            get { return m_UseCache; }
            set { m_UseCache = value; }
        }

        public bool ShowVolume { get; set; }

        public StockChartDataSet DataSet
        {
            get
            {
                if (m_DataSet == null)
                    m_DataSet = new StockChartDataSet();
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

            var response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
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
            //itxml = this.ToITXMLBarGraphTimeLine();
            var pcscript = GetPCScript();
            var template = GetChartTemplate();
            //string tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, GetChartTemplate(), m_UseCache);
            var tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, pcscript, GetChartTemplate(), m_UseCache);

            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
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
                // mainSB.Append("<cit:data series-data-in-columns=\"true\">");
                mainSB.Append("<cit:data>");
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
            var template = new ChartTemplate();
            template.Width = WIDTH;
            template.Height = HEIGHT;
            template.OutputChartType = m_OutputChartType;
            template.AppearanceFile = (ShowVolume) ? BASE_APPERANCE_FILE : BASE_APPERANCE_FILE_HVOLUME;
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

            //Set Scale
            sb.Append("<cit:stock-graph name=\"graph\" method=\"replace\">");
            //sb.AppendFormat("<cit:outer-lines max-line=\"color:{0}\" min-line=\"color:{0}\" low-outer-line=\"color:{0}\" high-outer-line=\"color:{0}\"/>", COLOR_OUTERLINES);
            sb.Append(SetScales());
            //sb.Append("</cit:time-graph>");

            //Populate the data in ITXML
            // sb.Append(ProcessData()); //Commented because Corda has an issue with itxml format for data. PCScript used for this.

            sb.Append("</cit:stock-graph>");
            //For No Volume 
            if (!ShowVolume)
            {
                sb.Append(ProcessComboAttributionWV());
            }

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
            var sb = new StringBuilder();
            string series;
            foreach (StockSeries stockSeries in DataSet.StockSeries)
            {
                if (stockSeries.StockItems.Count > 0)
                {
                    series = stockSeries.Name;
                    //sb.Append("graph.setseries(series1;");
                    sb.Append(string.Concat("graph.setseries(", series, ";"));

                    foreach (StockItem item in stockSeries.StockItems)
                    {
                        sb.Append(
                            string.Concat(
                                //item.Date.ToString("MM'/'dd'/'yyyy"),
                                //item.Date.ToString("dd'-'MMM'-'yyyy"),
                                item.Target,
                                ",",
                                item.High,
                                ",",
                                item.Low,
                                ",",
                                item.Open,
                                ",",
                                item.Close,
                                ";"
                                )
                            );
                    }
                    sb.Append(")");
                }
            }
            //HoverText addition
            int seriesIndex = 1;
            foreach (StockSeries stockSeries in DataSet.StockSeries)
            {
                if (stockSeries.StockItems.Count > 0)
                {
                    sb.Append("graph.addHoverText(");
                    int dataItemPosition = 1;
                    foreach (StockItem item in stockSeries.StockItems)
                    {
                        sb.Append(
                            string.Concat(dataItemPosition, ",", seriesIndex, ",", item.Hover, ";")
                            );
                        dataItemPosition++;
                    }
                    sb.Append(")");
                }
                seriesIndex++;
            }

            //drilldown addition
            seriesIndex = 1;
            foreach (StockSeries stockSeries in DataSet.StockSeries)
            {
                if (stockSeries.StockItems.Count > 0)
                {
                    sb.Append("main.paramdelimiter(|)graph.ddEnable(");
                    int dataItemPosition = 1;
                    foreach (StockItem item in stockSeries.StockItems)
                    {
                        if (item.Drilldown != "")
                        {
                            sb.Append(
                                string.Concat(dataItemPosition, "|", seriesIndex, "|", item.Drilldown)
                                );
                        }
                        else
                        {
                            sb.Append(
                                string.Concat(dataItemPosition, "|", seriesIndex, "|", ";")
                                );
                        }
                        dataItemPosition++;
                    }
                    sb.Append(")");
                }
                seriesIndex++;
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
            var sb = new StringBuilder();
            if (DataSet.StockSeries != null)
            {
                //sb.Append(
                //    "<cit:date-time-format override-input-format=\"true\" input-format=\"%m/%d/%Y\" override-month-names=\"true\" month-names=\"${sJan},${sFeb},${sMar},${sApr},${sMay},${sJun},${sJul},${Aug},${Sep},${Oct},${Nov},${Dec}\" override-day-names=\"true\" day-names=\"${sSun},${Mon},${Tue},${Wed},${Thu},${Fri},${Sat}\"/>");
                //for (int timeSeriesCount = 0; timeSeriesCount < DataSet.StockSeries.Count; timeSeriesCount++)
                //{
                //    sb.AppendFormat("<cit:filled-line-series name=\"{0}\" number=\"{1}\" fill-color=\"{2}\" />", DataSet.TimeSeries[timeSeriesCount].Name, timeSeriesCount + 1, "#54559B");
                //}
                sb.Append("<cit:stock-data>");
                for (int stockSeriesCount = 0; stockSeriesCount < DataSet.StockSeries.Count; stockSeriesCount++)
                {
                    //sb.AppendFormat("<cit:time name=\"{0}\">", DataSet.StockSeries[timeSeriesCount].Name);
                    if (DataSet.StockSeries[stockSeriesCount].StockItems != null)
                    {
                        for (int dataItemCount = 0;
                             dataItemCount < DataSet.StockSeries[stockSeriesCount].StockItems.Count;
                             ++dataItemCount)
                        {
                            //item.Date = DataSet.StockSeries[stockSeriesCount].StockItems[dataItemCount].Date;
                            sb.Append(DataSet.StockSeries[stockSeriesCount].StockItems[dataItemCount].ToITXML());
                        }
                    }
                    //sb.Append("</cit:time>");
                }
                sb.Append("</cit:stock-data>");
                return sb.ToString();
            }
            return "<cit:stock-data/>";
        }

        /// <summary>
        /// Process Attribution
        /// </summary>
        /// <returns></returns> 
        /// 
        private string ProcessComboAttribution()
        {
            var sb = new StringBuilder();

            sb.Append("<cit:textbox name=\"StockEx\" positioned-in-builder=\"true\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(Arr_TextBox[1].ToString()));

            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Price\" positioned-in-builder=\"true\" common=\"width:540;height:15\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(PriceLabel));

            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"Volume\" positioned-in-builder=\"true\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", StringUtilitiesManager.XmlEncode(VolumeLabel));

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


        private string ProcessAttribution()
        {
            return Attribution.ToITXMLTimeLine();
        }
    }
}