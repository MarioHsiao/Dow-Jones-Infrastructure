using DowJones.Managers.Abstract;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using log4net;

namespace DowJones.Session
{
    public class SessionCacheManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (SessionCacheManager));


        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCacheManager"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLangugage">The interface langugage.</param>
        public SessionCacheManager(string sessionId, string clientTypeCode, string accessPointCode, string interfaceLangugage)
            : base(sessionId, clientTypeCode, accessPointCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCacheManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public SessionCacheManager(IControlData controlData, string interfaceLanguage)
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

        public GetItemInfoResponse GetItemInfo(string key, Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope scope)
        {
            var request = new GetItemInfoRequest
                              {
                                  Key = key,
                                  Scope = scope
                              };
            return Process<GetItemInfoResponse>(request);
        }
    }
}