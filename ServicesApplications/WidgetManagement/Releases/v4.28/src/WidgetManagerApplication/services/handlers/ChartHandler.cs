using System;
using System.Web;
using EMG.Tools.Charting.Discovery;
using EMG.Tools.Managers.Charting;
using EMG.Utility.Handlers;
using log4net;

namespace EMG.widgets.services.handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartHandler : BaseHttpHandler
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(ChartHandler));
        /// <summary>
        /// 
        /// </summary>
        private const bool m_RequiresAuthentication = false;
        /// <summary>
        /// 
        /// </summary>
        private const string m_ContentMimeType = "application/x-shockwave-flash";

        /// <summary>
        /// 
        /// </summary>
        public string m_ChartBarColor = "%23000000";
        /// <summary>
        /// 
        /// </summary>
        public string m_ChartType = "co";
        
        /// <summary>
        /// Gets or sets the type of the chart.
        /// </summary>
        /// <value>The type of the chart.</value>
        public string ChartType
        {
            get { return m_ChartType; }
            set { m_ChartType = value; }
        }

        /// <summary>
        /// Gets or sets the color of the chart bar.
        /// </summary>
        /// <value>The color of the chart bar.</value>
        public string ChartBarColor
        {
            get { return m_ChartBarColor; }
            set { m_ChartBarColor = value; }
        }

        public override void HandleRequest(HttpContext context)
        {
            try
            {
                var request = context.Request;
                ChartBarColor = request["chartBarColor"] ?? "%23000000";
                ChartType = request["chartType"] ?? "co"; 
                
                var generator = new DiscoveryChartGenerator
                                    {
                                        BarColor = System.Drawing.ColorTranslator.FromHtml(ChartBarColor), 
                                        OutputChartType = OutputChartType.FLASH, 
                                        UseCache = false, 
                                        IsSampleChart = true
                                    };
                switch (ChartType)
                {
                    case "in":
                        generator.ChartType = DiscoveryChartType.Industries;
                        break;
                    case "co":
                        generator.ChartType = DiscoveryChartType.Companies;
                        break;
                    case "ex":
                        generator.ChartType = DiscoveryChartType.Executives;
                        break;
                    case "ns":
                        generator.ChartType = DiscoveryChartType.NewsSubjects;
                        break;
                    case "re":
                        generator.ChartType = DiscoveryChartType.Regions;
                        break;
                    default:
                        break;
                }

                var response = context.Response;
                response.BinaryWrite(generator.GetBytes().Bytes);
                response.End();
            }
            catch (Utility.Exceptions.EmgUtilitiesException exUtil)
            {
                m_Log.Error(exUtil);
            }
            catch (Exception ex)
            {
                m_Log.Error(ex);
            }
        }

        /// <summary>
        /// Validates the parameters.  Inheriting classes must
        /// implement this and return true if the parameters are
        /// valid, otherwise false.
        /// </summary>
        /// <param name='context'>Context.</param>
        /// <returns>
        /// 	<c>true</c> if the parameters are valid,
        /// otherwise <c>false</c>
        /// </returns>
        public override bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this handler
        /// requires users to be authenticated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if authentication is required
        /// otherwise, <c>false</c>.
        /// </value>
        public override bool RequiresAuthentication
        {
            get { return m_RequiresAuthentication; }
        }

        /// <summary>
        /// Gets the content MIME type.
        /// </summary>
        /// <value></value>
        public override string ContentMimeType
        {
            get { return m_ContentMimeType; }
        }
    }

}