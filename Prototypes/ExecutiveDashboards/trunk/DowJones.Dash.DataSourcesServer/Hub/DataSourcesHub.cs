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

        private static IDashboardMessageCache MessageCache
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
        
        public Task<ICollection<DashboardMessage>> Pickup()
        {
            var clientId = Context.ConnectionId;
            Log.InfoFormat("clientId {0} picking up items", clientId);
            IDashboardMessageQueue queue;
            if (_queues.TryGetValue(clientId, out queue))
            {
                Log.InfoFormat("have been able to get queue: {0}", clientId);
                return TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() =>
                    {
                        var messages = queue.Get();
                        Log.InfoFormat("clientId {0} picking up items in queue[{1}]", clientId, messages.Count);
                        return messages;
                    });
            }
            return null;
        }
        
        public Task<ICollection<DashboardMessage>> PrimeCache()
        {
            var clientId = Context.ConnectionId;
            Log.InfoFormat("clientId {0} requesting cache", clientId);
            
            if (!_queues.ContainsKey(clientId))
            {
               Log.InfoFormat("try add queue: {0}", clientId);
               if ( _queues.TryAdd(clientId, new DashboardMessageQueue(new ConcurrentQueue<DashboardMessage>())))
               {
                   Log.InfoFormat("Success at adding queue: {0}", clientId);
               }
            }

            return TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() => MessageCache.GetAll());
        }


        public static void Publish(DashboardMessage message)
        {
            if (message == null)
                return;

            /* Log.DebugFormat("Publishing {0}", message.EventName);
             context.Clients.message(message);*/

            var inactiveQueues = new List<string>();

            foreach (var key in _queues.Keys)
            {
                IDashboardMessageQueue queue;
                if (!_queues.TryGetValue(key, out queue)) continue;
                Log.DebugFormat("adding to queue: {0}", key );
                if (queue.IsActive())
                {
                    queue.Enqueue(message);
                }
                else
                {
                    inactiveQueues.Add(key);
                }
            }

            // update the cache
            if (message is DashboardErrorMessage) return;
            Log.DebugFormat("Adding to cache: {0}", message.Source);
            MessageCache.Add(message);

            if (inactiveQueues.Count <= 0) return;
            foreach (var key in inactiveQueues)
            {
                IDashboardMessageQueue q;
                if (_queues.TryRemove(key, out q))
                {
                    Log.DebugFormat("Removing queue: {0}", key);
                }
            }
        }
    }
}