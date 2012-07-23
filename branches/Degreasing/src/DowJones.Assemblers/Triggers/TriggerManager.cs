using System;
using DowJones.Exceptions;
using DowJones.Managers.Abstract;
using DowJones.Session;
using Factiva.Gateway.Messages.Trigger.Definition.V1_1;
using Factiva.Gateway.Messages.Trigger.V1_1;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Assemblers.Triggers
{
    /// <summary>
    /// Trigger Management Class implementing the base aggregation manager.
    /// </summary>
    public class TriggerManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TriggerManager));

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerManager"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLangugage">The interface langugage.</param>
        public TriggerManager(string sessionId, string clientTypeCode, string accessPointCode, string interfaceLangugage)
            : base(sessionId, clientTypeCode, accessPointCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public TriggerManager(IControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return _log; }
        }

        /// <summary>
        /// Gets the type of the trigger.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetTriggerTypeResponse GetTriggerType(GetTriggerTypeRequest request)
        {
            ServiceResponse<GetTriggerTypeResponse> sr;
            try
            {
                sr = Invoke<GetTriggerTypeResponse>(request);
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
        /// Performs the trigger search.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public PerformTriggerSearchResponse PerformTriggerSearch(PerformTriggerSearchRequest request)
        {
            ServiceResponse<PerformTriggerSearchResponse> sr;
            try
            {
                sr = Invoke<PerformTriggerSearchResponse>(request);
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
        /// Gets the detail.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns the GetDetailsResponse object</returns>
        public GetDetailResponse GetDetail(GetDetailRequest request)
        {
            ServiceResponse<GetDetailResponse> sr;
            try
            {
                sr = Invoke<GetDetailResponse>(request);
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

    }
}
