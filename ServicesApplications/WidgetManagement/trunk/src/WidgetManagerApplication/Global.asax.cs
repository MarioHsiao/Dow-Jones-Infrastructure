// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <author>
//   Kumar Mayank
// </author>
// <lastModified> 
//   <entry type="creation"><date>07/14/2010</date><author>David Da Costa</author></entry>
// </lastModified>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using EMG.widgets.ui.exception;
using Factiva.BusinessLayerLogic.Configuration;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using log4net;
using Factiva.Gateway.Services.V1_0;
using EMG.Utility.Managers.Track;
using Factiva.Gateway.Messages.Track.V1_0;

namespace EMG.widgets.ui
{
    /// <summary>
    /// The global.
    /// </summary>
    public class Global : HttpApplication
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(Global));

        /// <summary>
        /// The application start.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            LoadTrackDeletedFolders();
        }

        /// <summary>
        /// Loads the track deleted folders.
        /// </summary>
        private static void LoadTrackDeletedFolders()
        {
            try
            {
                var deletedFoldersResponse = new DeletedFoldersResponse();
                var controlData = new ControlData
                {
                    UserID = ConfigurationManager.GetLightWeightUser("RssFeed1LightWeightUser").userId,
                    UserPassword = ConfigurationManager.GetLightWeightUser("RssFeed1LightWeightUser").userPassword,
                    ProductID = ConfigurationManager.GetLightWeightUser("RssFeed1LightWeightUser").productId
                };

                
                if (Log.IsDebugEnabled) Log.Debug("Global >> LoadTrackDeletedFolders: Retrieving Deleted Folders.");
                var serviceResponse = TrackService.DeletedFolders(controlData.Clone(), new DeletedFoldersRequest());
                if (serviceResponse.rc != 0)
                {
                    throw new EmgWidgetsUIException(serviceResponse.ReturnCode);
                }
                object responseObj;
                serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);

                deletedFoldersResponse = (DeletedFoldersResponse) responseObj;
                if (deletedFoldersResponse == null || deletedFoldersResponse.DeletedFolders.Count <= 0)
                {
                    return;
                }

                
                var deletedFolderManager = new TrackDeletedFoldersCacheManager(controlData.Clone(), "en");
                deletedFolderManager.Load(deletedFoldersResponse.DeletedFolders);
                
                if (Log.IsDebugEnabled) Log.Debug("Global >> LoadTrackDeletedFolders: Successfully Retreived Deleted Folders.");
            }
            catch (Exception ex)
            {
                throw new EmgWidgetsUIException(ex, -1);
            }
        }

        /// <summary>
        /// The session_ start.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application_ begin request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
             // Logger.ClientEntry("GL");
        }

        /// <summary>
        /// Logs the exit point in the original log4net file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
             // Logger.ClientExit();
        }

        /// <summary>
        /// The application_ authenticate request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application_ error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        protected void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The session_ end.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application_ end.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        protected void Application_End(object sender, EventArgs e)
        {
        }
    }

    /// <summary>
    /// A helper class to create the specified performance counters.
    /// </summary>
    public class PerfmormanceMonitor
    {
        private readonly CounterCreationDataCollection _counters = new CounterCreationDataCollection();
        private readonly string _categoryName = string.Empty;
        private readonly string _categoryHelp = string.Empty;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PerfmormanceMonitor));

        /// <summary>
        /// Initializes a new instance of the <see cref="PerfmormanceMonitor"/> class.
        /// Creates an instance of the class.
        /// </summary>
        /// <param name="categoryName">The name of the performance counter category.</param>
        /// <param name="categoryHelp">The category help.</param>
        public PerfmormanceMonitor(string categoryName, string categoryHelp)
        {
            _categoryName = categoryName;
            _categoryHelp = categoryHelp;
        }

        /// <summary>
        /// Determines whether this instance has counters.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance has counters; otherwise, <c>false</c>.
        /// </returns>
        public bool HasCounters()
        {
            if (!PerformanceCounterCategory.Exists(_categoryName))
            {
                return false;
            }

            if (_counters.Cast<CounterCreationData>().Any(counter => !PerformanceCounterCategory.CounterExists(counter.CounterName, _categoryName)))
            {
                return false;
            }

            if (Logger.IsInfoEnabled)
            {
                Logger.Info("All counters and category exists");
            }

            return true;
        }

        /// <summary>
        /// Creates the performance counters
        /// </summary>
        public void CreateCounters()
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Creating the counters");
            }

            PerformanceCounterCategory.Create(_categoryName, _categoryHelp, PerformanceCounterCategoryType.Unknown, _counters);
        }

        /// <summary>
        /// Adds a performance counter to the category of performance counters.
        /// </summary>
        /// <param name="name">The counter name.</param>
        /// <param name="helpText">The counter help text.</param>
        /// <param name="type">The counter type.</param>
        public void AddCounter(string name, string helpText, PerformanceCounterType type)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.InfoFormat("Adding Counter {0}", name);
            }

            var ccd = new CounterCreationData
                          {
                              CounterName = name, 
                              CounterHelp = helpText, 
                              CounterType = type
                          };

            _counters.Add(ccd);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PerformaceCounterInitializer
    {
        /// <summary>
        /// Category Name
        /// </summary>
        public const string CategoryName = "WidgetPlatform";
              
        /// <summary>
        /// Alert Headline Widget Delegate Fill Counter Name
        /// </summary>
        public const string AlertDelegateFillCounter = "# alert_headline_widget_delegate fill commands executed";
        /// <summary>
        /// 
        /// </summary>
        public const string AlertDelegateFillAverageTime = "average time per alert_headline_widget_delegate fill command";
        
        private const string CategoryHelp = "Consumer Widget Platform Category";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PerformaceCounterInitializer));

        /// <summary>
        /// Creates the counters.
        /// </summary>
        public static void CreateCounters()
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Creating the counters");
            }

            var monitor = new PerfmormanceMonitor(CategoryName, CategoryHelp);
            monitor.AddCounter(AlertDelegateFillCounter, "Total number of AlertHeadlineDelegate's fill commands", PerformanceCounterType.NumberOfItems64);
            monitor.AddCounter(AlertDelegateFillAverageTime, "Average duration per operation of alert_headline_widget_delegate fill command", PerformanceCounterType.NumberOfItems64);
            
            /*monitor.AddCounter("# logfiles parsed", "Total number of logfiles parsed", PerformanceCounterType.NumberOfItems64);
            monitor.AddCounter("# operations / sec", "Number of operations executed per second", PerformanceCounterType.RateOfCountsPerSecond32);
            monitor.AddCounter("average time per operation", "Average duration per operation execution", PerformanceCounterType.AverageTimer32);
            monitor.AddCounter("average time per operation base", "Average duration per operation execution base", PerformanceCounterType.AverageBase);
           */
            if (!monitor.HasCounters())
            {
                monitor.CreateCounters();
            }
            
        }
    }
}