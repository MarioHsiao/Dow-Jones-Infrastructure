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
        private static IDashboardMessageQueue _messageQueue;

        private static IDashboardMessageQueue MessageQueue
        {
            get
            {
                if (_messageQueue == null && ServiceLocator.Current != null)
                {
                    _messageQueue = ServiceLocator.Current.Resolve<IDashboardMessageQueue>();
                }

                return _messageQueue ?? (_messageQueue = new DashboardMessageQueue());
            }
        }

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
            if (Log.IsInfoEnabled)
            {
                
            }
            return TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() =>
                {
                    var messages = MessageQueue.GetAll();
                    Log.DebugFormat("clientId {0} picking up items in queue[{1}]", clientId, messages.Count);
                    return messages;
                });
        }

        public Task<ICollection<DashboardMessage>> PrimeCache()
        {
            var clientId = Context.ConnectionId;
            if (Log.IsInfoEnabled)
            {
                Log.DebugFormat("clientId {0} requesting cache", clientId);
            }

            return TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() => MessageCache.GetAll());
        }


        public static void Publish(DashboardMessage message)
        {
            if (message == null)
                return;

            /* Log.DebugFormat("Publishing {0}", message.EventName);
             context.Clients.message(message);*/

            // update the queue
            Log.DebugFormat("Adding to queue {0}", message.Source);
            MessageQueue.Enqueue(message);

            // update the cache
            if (message is DashboardErrorMessage) return;
            Log.InfoFormat("Adding to cache {0}", message.Source);
            MessageCache.Add(message);
        }
    }
}