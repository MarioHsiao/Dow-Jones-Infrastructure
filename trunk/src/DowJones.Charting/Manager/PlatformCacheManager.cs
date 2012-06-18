using DowJones.Managers.Abstract;
using DowJones.Session;
using Factiva.Gateway.Messages.Cache.PlatformCache.V1_0;
using log4net;

namespace DowJones.Charting.Manager
{
    public class PlatformCacheManager : AbstractAggregationManager 
    {

        private static readonly ILog _log = LogManager.GetLogger(typeof(PlatformCacheManager));

        public PlatformCacheManager(IControlData controlData) : base(controlData)
        {
        }

        protected override ILog Log
        {
            get { return _log; }
        }

        public bool StoreItem<T>(StoreItemRequest request)
        {
            Process<T>(request);
            return true;
        }

        public T GetItem<T>(GetItemRequest request)
        {
            return Process<T>(request);
        }
    }
}
