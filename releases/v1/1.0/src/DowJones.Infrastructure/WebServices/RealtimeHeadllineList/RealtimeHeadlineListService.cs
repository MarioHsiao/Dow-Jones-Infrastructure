// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RealtimeHeadlineListService.cs" company="">
//   
// </copyright>
// <summary>
//   Family Tree Web Service
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Utilities.Ajax.Converters;
using DowJones.Utilities.Ajax.RealtimeHeadlineList;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using Factiva.Gateway.Managers;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace DowJones.Tools.WebServices
{
    /// <summary>
    /// RealtimeHeadlineList Web Service
    /// </summary>
    [WebService(Namespace = "DowJones.Tools.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class RealtimeHeadlineListService : BaseWebService
    {
        /// <summary>
        /// The m log.
        /// </summary>
        private static readonly ILog MLog = LogManager.GetLogger(typeof(FamilyTreeService));

        /// <summary>
        /// The _c data.
        /// </summary>
        private readonly ControlData _cData = ControlDataManager.GetLightWeightUserControlData("joyful", "joyful", "16");

        /// <summary>
        /// The process family tree.
        /// </summary>
        /// <param name="requestDelegate"></param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="productPrefix">The product prefix.</param>
        /// <returns>The FamilyTreeResponseDelegate Object</returns>
        [WebMethod]
        //[ScriptMethod(UseHttpGet = true)]
        public RealtimeHeadlineResponseDelegate ProcessRealtimeHeadline(RealtimeHeadlineRequestDelegate requestDelegate, string interfaceLanguage, string accessPointCode, string productPrefix)
        {
            string alertContext = requestDelegate.AlertContext;
            int maxHeadlinesToReturn = requestDelegate.MaxHeadlinesToReturn;

            using (var transactionLogger = new TransactionLogger(MLog, MethodBase.GetCurrentMethod()))
            {
                var responseDelegate = new RealtimeHeadlineResponseDelegate();
                try
                {
                    if (!string.IsNullOrEmpty(alertContext))
                    {
                        // Creating request
                        var getSharedAlertContentRequest = new GetSharedAlertContentRequest
                        {
                            AlertContext = alertContext,
                            MaxHeadlinesToReturn = maxHeadlinesToReturn
                        };
                        // Getting Response
                        GetSharedAlertContentResponse response =
                            RTQueueAggregationService.GetSharedAlertContent(ControlDataManager.Clone(_cData),
                                                                            getSharedAlertContentRequest);
                        HandleResponse(response);
                        //var manager = new RealtimeHeadlinelistConversionManager("en", "on,-05:00|1,on", ClockType.TwelveHours);
                        var manager = new RealtimeHeadlinelistConversionManager(interfaceLanguage, requestDelegate.DateTimeFormatingPreference, requestDelegate.ClockType);
                        responseDelegate.headlineListDataResult = manager.Process(response, null, null);
                        responseDelegate.AlertContext = alertContext;
                        responseDelegate.MaxHeadlinesToReturn = maxHeadlinesToReturn;
                        responseDelegate.ElapsedTime = transactionLogger.ElapsedTimeSinceInvocation;
                    }
                }
                catch (DowJonesUtilitiesException rEx)
                {
                    UpdateAjaxDelegate(rEx, responseDelegate, transactionLogger);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), responseDelegate, transactionLogger);
                }

                return responseDelegate;
            }
        }

        private static void HandleResponse(GetSharedAlertContentResponse r)
        {
            if (r.rc != 0)
            {
                throw new DowJonesUtilitiesException(r.rc);
            }
        }
    }
}