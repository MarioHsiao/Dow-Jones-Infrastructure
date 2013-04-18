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

using System;
using System.Drawing;
using System.Text;
using DowJones.Charting.Common;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;
using DowJones.Formatters;
using DowJones.Formatters.Numerical;

namespace DowJones.Charting.Fcp
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class TimeLineChartGenerator : AbstractChartGenerator
    {
        private const string BaseApperanceFile = "/fcp/fcp_timeline.itxml";
        //private const int HEIGHT = 150;//Infosys - 1/5/2010 - Thumbnail chart -commented
        //private const int WIDTH = 225;//Infosys - 1/5/2010 - Thumbnail chart -commented
        private int _height = 150;//Infosys - 1/5/2010 - Thumbnail chart - added
        private int _width = 225;//Infosys - 1/5/2010 - Thumbnail chart - added
        private readonly TextBox _attribution = new TextBox("CopyRight");
        private TimeLineChartDataSet _dataSet;
        private OutputChartType _outputChartType = OutputChartType.FLASH;
        private BaseLine _baseLine;
        private Color _backgroundColor = Color.White;
        private Color _gridColor = ColorTranslator.FromHtml("#CCCCCC");
        private Color _scaleColor = ColorTranslator.FromHtml("#999999");
        private Line _plotLine;
        private bool _useCache = true;
        private double _minValue = double.MinValue; //minimum value on the y-axis scale
        private double _maxValue = double.MaxValue; //maximum value on the y-axis scale
        private readonly NumberFormatter _formatter = new NumberFormatter();
        
        #region Constructors

        public TimeLineChartGenerator()
        {
        }

        public TimeLineChartGenerator(TimeLineChartDataSet DataSet)
        {
            _dataSet = DataSet;
        }

        #endregion

        #region Properties
        //Infosys - 1/5/2010 - Thumbnail chart - Begin
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }


        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        //Infosys - 1/5/2010 - Thumbnail chart -End

        public TextBox Attribution
        {
            get { return _attribution; }
        }

        protected bool UseCache
        {
            get { return _useCache; }
            set { _useCache = value; }
        }

        public TimeLineChartDataSet DataSet
        {
            get
            {
                if (_dataSet == null)
                    _dataSet = new TimeLineChartDataSet();
                return _dataSet;
            }
            set { _dataSet = value; }
        }

        public OutputChartType OutputChartType
        {
            get { return _outputChartType; }
            set { _outputChartType = value; }
        }

        public BaseLine BaseLine
        {
            get
            {
                if (_baseLine == null)
                    _baseLine = new BaseLine();
                return _baseLine;
            }
            set { _baseLine = value; }
        }

        public Line PlotLine
        {
            get
            {
                if (_plotLine == null)
                    _plotLine = new Line();
                return _plotLine;
            }
            set { _plotLine = value; }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public Color GridColor
        {
            get { return _gridColor; }
            set { _gridColor = value; }
        }

        public Color ScaleColor
        {
            get { return _scaleColor; }
            set { _scaleColor = value; }
        }

        public double MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        public double MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        #endregion

        public override IBytesResponse GetBytes()
        {
            var itxml = ToITXML();
            var pcscript = GetPCScript();
            var template = GetChartTemplate();
            var bytes = ChartingManager.GetChartBytes(itxml, pcscript, GetChartTemplate(), _useCache);
            var response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }

        ///This method is not used and not tested.
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
            var tUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, "", template, _useCache));

            var response = new ChartUriResponse(template.Height, template.Width, tUri, original);
            return response;
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
            var tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, pcscript, GetChartTemplate(), _useCache);

            var response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
            return response;
        }

        internal override ChartTemplate GetChartTemplate()
        {
            var template = new ChartTemplate
                               {
                                   Width = Width, //Infosys - 1/5/2010 - Thumbnail chart - changed to use Width property
                                   Height = Height, //Infosys - 1/5/2010 - Thumbnail chart - changed to use Height property
                                   OutputChartType = _outputChartType, 
                                   AppearanceFile = BaseApperanceFile
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
            if (_minValue == double.MinValue && _maxValue == double.MaxValue)
            {
                sb.Append("<cit:value-scale-divisions set-scale-values=\"custom\" use-max-value-for-automatic=\"false\" include-zero=\"false\" maximum-major-divisions=\"10\"/>");
            }
            else
            {
                var maxPrecision = new PrecisionNumber(_maxValue) { Precision = 6 };
                var minPrecision = new PrecisionNumber(_minValue) { Precision = 6 };
                _formatter.Format(maxPrecision);
                _formatter.Format(minPrecision);
                sb.AppendFormat("<cit:value-scale-divisions set-scale-values=\"custom\" use-max-value-for-automatic=\"true\" use-min-value-for-automatic=\"true\" include-zero=\"false\" minimum-value=\"{0}\" maximum-value=\"{1}\" maximum-major-divisions=\"10\"/>", minPrecision.Text.Value.Replace(",", string.Empty), maxPrecision.Text.Value.Replace(",", string.Empty));
            }
            
            sb.Append("</cit:value-scale>");
            sb.Append("<cit:time-value-scale set-scales-values=\"manually\" eliminate-white-space=\"true\" set-tick-increment=\"automatically\" show-quarters=\"true\">");
            sb.AppendFormat("<cit:time-value-scale-labels rotate-labels=\"false\" font=\"name:Arial Unicode MS;color:{0}\" minor-font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            sb.Append("</cit:time-value-scale>");
            return sb.ToString();
        }
        
        /// <summary>
        /// PCScript used for timeseries. Corda dll from vendor has a issue with itxml.
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
                    foreach (var item in timeSeries.TimeItems)
                    {
                        var val = new PrecisionNumber(item.Value) {Precision = 6};
                        _formatter.Format(val);
                        sb.Append(
                            string.Concat(
                                item.Date.ToString("MM'/'dd'/'yyyy"),
                                ",",
                                ChartingManager.EncodeSpecialCharsForPcScript(val.Text.Value),
                                ";"));
                    }
                    sb.Append(")");
                }
                if (!BaseLine.IsEnabled)
                    continue;
                sb.Append("graph.setseries(trendline;");
                foreach (var item in timeSeries.TimeItems)
                {
                    var val = new PrecisionNumber(BaseLine.Value) { Precision = 6 };
                    _formatter.Format(val);
                    sb.Append(
                        string.Concat(
                            item.Date.ToString("MM'/'dd'/'yyyy"),
                            ",",
                            ChartingManager.EncodeSpecialCharsForPcScript(val.Text.Value),
                            ";"
                            )
                        );
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Uses ITXML format to populate data. Corda dll has an issue with populating data in itmxl format. 
        /// Hence this function is not used. To be tested and used once Corda provides a fix for the issue.
        /// </summary>
        /// <returns></returns>
        protected string ProcessData()
        {
            var sb = new StringBuilder();
            if (DataSet.TimeSeries != null)
            {
                //sb.Append(
                //    "<cit:date-time-format override-input-format=\"true\" input-format=\"%m/%d/%Y\" override-month-names=\"true\" month-names=\"${sJan},${sFeb},${sMar},${sApr},${sMay},${sJun},${sJul},${Aug},${Sep},${Oct},${Nov},${Dec}\" override-day-names=\"true\" day-names=\"${sSun},${Mon},${Tue},${Wed},${Thu},${Fri},${Sat}\"/>");
                for (int timeSeriesCount = 0; timeSeriesCount < DataSet.TimeSeries.Count; timeSeriesCount++)
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