/* 
 * Author: Infosys
 * Date: 8/3/09
 * Purpose: Generate Itxml for Time Line Graph.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */

using System.Drawing;
using System.Text;
using DowJones.Tools.Charting.Common;
using DowJones.Tools.Managers.Charting;
using DowJones.Tools.Charting;
using DowJones.Utilities.Managers.Charting;
using DowJones.Tools.Charting.Data;

namespace DowJones.Utilities.Charting.Fcp
{
    public class TimeLineStockChartGenerator : AbstractChartGenerator
    {
        private const string BASE_APPERANCE_FILE = "/fcp/thumbnail_news.itxml";
        private const int HEIGHT = 100;
        private const int WIDTH = 200;
        private readonly TextBox m_Attribution = new TextBox("CopyRight");
        private TimeLineChartDataSet m_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.PNG_WITH_JAVASCRIPT_INTERACTIVITY;
        private BaseLine m_BaseLine;
        private Color m_BackgroundColor = Color.White;
        private Color m_GridColor = ColorTranslator.FromHtml("#CCCCCC");
        private Color m_ScaleColor = ColorTranslator.FromHtml("#999999");
        private Line m_PlotLine;
        private bool m_UseCache = true;
        private double minValue; //minimum value on the y-axis scale
        private double maxValue; //maximum value on the y-axis scale

        #region Constructors

        public TimeLineStockChartGenerator()
        {
        }

        public TimeLineStockChartGenerator(TimeLineChartDataSet DataSet)
        {
            m_DataSet = DataSet;
        }

        #endregion

        #region Properties

        public TextBox Attribution
        {
            get { return m_Attribution; }
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
            var tUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, "", template, m_UseCache));
            return new ChartUriResponse(template.Height, template.Width, tUri, original);
        }

        /// <summary>
        /// To generate HTML. No formatting changes made to the itxml. Only data is populated.
        /// </summary>
        /// <returns></returns>
        public override IEmbeddedHTMLResponse GetHTML()
        {
            var itxml = ToITXML();
            var pcscript = GetPCScript();
            var template = GetChartTemplate();
            var tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, pcscript, GetChartTemplate(), m_UseCache);

            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
            return response;
        }

        internal override ChartTemplate GetChartTemplate()
        {
            var template = new ChartTemplate
                               {
                                   Width = WIDTH, Height = HEIGHT, OutputChartType = m_OutputChartType, AppearanceFile = BASE_APPERANCE_FILE
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


        /// <summary>
        /// Scale settings for Time Line
        /// </summary>
        /// <returns></returns>
        private string SetScales()
        {
            var sb = new StringBuilder();
            sb.Append("<cit:value-scale position=\"right\">");
            sb.AppendFormat("<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            if (minValue == 0 && maxValue == 0)
            {
                sb.Append("<cit:value-scale-divisions set-scale-values=\"custom\" use-max-value-for-automatic=\"false\" include-zero=\"false\" maximum-major-divisions=\"6\"/>");
            }
            else
            {
                sb.AppendFormat("<cit:value-scale-divisions set-scale-values=\"custom\" use-max-value-for-automatic=\"true\" use-min-value-for-automatic=\"true\" include-zero=\"false\" minimum-value=\"{0}\" maximum-value=\"{1}\" maximum-major-divisions=\"10\"/>", minValue, maxValue);
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
            foreach (var timeSeries in DataSet.TimeSeries)
            {
                if (timeSeries.TimeItems.Count > 0)
                {
                    sb.Append("graph.setseries(series1;");
                    //foreach (TimeItem item in timeSeries.TimeItems)
                    //{
                    //    sb.Append(
                    //        string.Concat(
                    //            item.Date.ToString("MM'/'dd'/'yyyy"),
                    //            ",",
                    //            double.Parse(item.Value.ToString(), System.Globalization.CultureInfo.CurrentCulture),
                    //            ";"
                    //            )
                    //        );
                    //}
                    foreach (var item in timeSeries.TimeItems)
                    {
                        sb.Append(
                            string.Concat(
                                item.Date.ToString("MM'/'dd'/'yyyy"),
                                ",",
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", item.Value),
                                ";")
                            );
                    }
                    sb.Append(")");
                }
                if (!BaseLine.IsEnabled)
                    continue;
                sb.Append("graph.setseries(trendline;");
                foreach (var item in timeSeries.TimeItems)
                {
                    sb.Append(
                        string.Concat(
                            item.Date.ToString("MM'/'dd'/'yyyy"),
                            ",",
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", BaseLine.Value),
                            ";")
                        );
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

/*
        /// <summary>
        /// Uses ITXML format to populate data. Corda dll has an issue with populating data in itmxl format. 
        /// Hence this function is not used. To be tested and used once Corda provides a fix for the issue.
        /// </summary>
        /// <returns></returns>
        private string ProcessData()
        {
            var sb = new StringBuilder();
            if (DataSet.TimeSeries != null)
            {
                //sb.Append(
                //    "<cit:date-time-format override-input-format=\"true\" input-format=\"%m/%d/%Y\" override-month-names=\"true\" month-names=\"${sJan},${sFeb},${sMar},${sApr},${sMay},${sJun},${sJul},${Aug},${Sep},${Oct},${Nov},${Dec}\" override-day-names=\"true\" day-names=\"${sSun},${Mon},${Tue},${Wed},${Thu},${Fri},${Sat}\"/>");
                for (var timeSeriesCount = 0; timeSeriesCount < DataSet.TimeSeries.Count; timeSeriesCount++)
                {
                    sb.AppendFormat("<cit:filled-line-series name=\"{0}\" number=\"{1}\" fill-color=\"{2}\" />", DataSet.TimeSeries[timeSeriesCount].Name, timeSeriesCount + 1, "#54559B");
                }
                sb.Append("<cit:time-data>");
                for (var timeSeriesCount = 0; timeSeriesCount < DataSet.TimeSeries.Count; timeSeriesCount++)
                {
                    sb.AppendFormat("<cit:time name=\"{0}\">", DataSet.TimeSeries[timeSeriesCount].Name);
                    if (DataSet.TimeSeries[timeSeriesCount].TimeItems != null)
                    {
                        for (var dataItemCount = 0;
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
*/

        /// <summary>
        /// Set Legend
        /// </summary>
        /// <returns></returns>
        private string ProcessAttribution()
        {
            return Attribution.ToITXMLTimeLine();
        }
    }
}