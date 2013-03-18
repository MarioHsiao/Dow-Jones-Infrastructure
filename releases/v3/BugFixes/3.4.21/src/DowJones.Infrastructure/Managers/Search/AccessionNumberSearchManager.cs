using EMG.Utility.Managers.CacheService;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace EMG.Utility.Managers.Search
{

    public class AccessionNumberSearchManager : AbstractAggregationManager
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(CacheManager));

        public AccessionNumberSearchManager(string sessionID, string clientTypeCode, string accessPointCode, string interfaceLangugage) : base(sessionID, clientTypeCode, accessPointCode, interfaceLangugage)
        {
        }

        public AccessionNumberSearchManager(ControlData controlData, string interfaceLanguage) : base(controlData, interfaceLanguage)
        {
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return m_Log; }
        }


        public PerformContentSearchResponse PerformContentSearch(AccessionNumberSearchRequest accessionNumberSearchRequest)
        {
            return null
        }

    }
}
