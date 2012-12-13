using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DowJones.Dash.Caching;
using DowJones.DependencyInjection;
using DowJones.Utilities;
using log4net;

namespace DowJones.Dash.DataSourcesServer.Hub
{

    public class DataSourcesHub : SignalR.Hubs.Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataSourcesHub));
        private static IDashboardMessageCache _messageCache;
        private static readonly ConcurrentDictionary<string, IDashboardMessageQueue> _queues = new ConcurrentDictionary<string, IDashboardMessageQueue>();

        internal static IDashboardMessageCache MessageCache
        {
            get
            {
                if (_messageCache == null && ServiceLocator.Current != null)
                {
                    _messageCache = ServiceLocator.Current.Resolve<IDashboardMessageCache>();
                }

                return _messageCache ?? (_messageCache = new DashboardMessageCache());
            }
        }
        
        public Task<ICollection<DashboardMessage>> PrimeCache()
        {
            var clientId = Context.ConnectionId;
            Log.InfoFormat("clientId {0} requesting cache", clientId);
            return TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() => MessageCache.GetAll());
        }
    }
}