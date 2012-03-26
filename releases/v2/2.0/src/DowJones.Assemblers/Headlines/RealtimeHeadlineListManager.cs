using System;
using DowJones.Exceptions;
using DowJones.Managers.Abstract;
using DowJones.Session;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using Factiva.Gateway.V1_0;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Assemblers.Headlines
{
    public class RealtimeHeadlineListManager: AbstractAggregationManager
    {
        /// <summary>
        /// The m log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(RealtimeHeadlineListManager));

        /// <summary>
        /// The _c data.
        /// </summary>
        private readonly IControlData _cData = ControlDataManager.GetLightWeightUserControlData("dacostad", "brian", "16");

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="controlData"></param>
        /// <param name="interfaceLanguage"></param>
        public RealtimeHeadlineListManager(IControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
        }

        /// <summary>
        /// /// Initializes a new instance of the class
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="clientTypeCode"></param>
        /// <param name="accessPointCode"></param>
        /// <param name="interfaceLangugage"></param>
        public RealtimeHeadlineListManager(string sessionID, string clientTypeCode, string accessPointCode, string interfaceLangugage)
            : base(sessionID, clientTypeCode, accessPointCode)
        {
        }

        public GetSharedAlertContentResponse Process(GetSharedAlertContentRequest request)
        {
            ServiceResponse<GetSharedAlertContentResponse> sr;
            try
            {
                sr = Invoke<GetSharedAlertContentResponse>(request, ControlData);
                if (sr != null)
                {
                    return sr.ObjectResponse;
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }
            return null;
        }


        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return _log; }
        }
    }
}
