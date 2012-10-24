﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowJones.Dash.Caching;
using DowJones.DependencyInjection;
using DowJones.Utilities;
using SignalR;
using SignalR.Hubs;
using log4net;

namespace DowJones.Dash.Website.Hubs
{
    public class Dashboard : Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Dashboard));
        private static  IDashboardMessageCache _cache;
        internal static IDashboardMessageCache Cache
        {
            get { return _cache ?? (_cache = ServiceLocator.Current.Resolve<IDashboardMessageCache>()); }
        }

        public Dashboard(IDashboardMessageCache cache)
        {
            if (_cache == null)
            {
                _cache = cache;
            }
        }

        public Task<ICollection<DashboardMessage>> Subscribe(IEnumerable<string> sources)
        {
            var clientId = Context.ConnectionId;
            Log.WarnFormat("Subscribe-->{0}", clientId);

            return TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() => {
                var groups = (sources ?? Enumerable.Empty<string>()).ToArray();

                foreach (var @group in groups)
                {
                    Groups.Add(clientId, @group);
                }

                var temp = Cache.Get(groups);

                if (Log.IsInfoEnabled)
                {
                    foreach (var dashboardMessage in temp)
                    {
                        Log.InfoFormat("Subscribe to Source: {0}", dashboardMessage.Source);
                    }
                }

                return temp;
            });
        }

        public Task Unsubscribe(IEnumerable<string> sources)
        {
            var clientId = Context.ConnectionId;

            return Task.Factory.StartNew(() =>
            {
                var groups = (sources ?? Enumerable.Empty<string>()).ToArray();
                foreach (var @group in groups)
                {
                    Groups.Remove(clientId, @group);
                }
            });
        }

        public static void Publish(DashboardMessage message)
        {
            if (message == null)
                return;

            Log.DebugFormat("Publishing {0}", message.EventName);

            var context = GlobalHost.ConnectionManager.GetHubContext<Dashboard>();
            var subscribers = context.Clients;

            if (!string.IsNullOrWhiteSpace(message.Source))
            {
                subscribers = subscribers[message.Source];
                subscribers.messageReceived(message);
            }

            if (!(message is DashboardErrorMessage))
            {
                Cache.Add(message);
            }
        }
    }
}