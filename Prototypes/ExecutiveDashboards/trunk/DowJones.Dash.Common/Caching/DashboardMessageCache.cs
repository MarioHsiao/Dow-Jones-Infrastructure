using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DowJones.Dash.Caching
{
    public class DashboardMessageQueue : IDashboardMessageQueue
    {
        private readonly ConcurrentQueue<DashboardMessage> CacheQueue;
        protected ConcurrentQueue<DashboardMessage> Cache
        {
            get { return CacheQueue; }
        }

        public DashboardMessageQueue(ConcurrentQueue<DashboardMessage> cacheQueue)
        {
            CacheQueue = cacheQueue;
        }

        public virtual void Enqueue(DashboardMessage message)
        {
            if (message == null)
            {
                return;
            }

            Cache.Enqueue(message);
        }

        public virtual ICollection<DashboardMessage> GetAll()
        {
            var messages = new List<DashboardMessage>();
            DashboardMessage message;
            while (CacheQueue.TryDequeue(out message))
            {
               messages.Add(message); 
            }
            return messages;
        } 

    }

    public class DashboardMessageCache : IDashboardMessageCache
    {
        protected IDictionary<string, DashboardMessage> Cache
        {
            get { return CacheDictionary; }
        }

        private static readonly IDictionary<string, DashboardMessage> CacheDictionary = new ConcurrentDictionary<string, DashboardMessage>();

        public virtual void Add(DashboardMessage message)
        {
            if (message == null)
            {
                return;
            }

            if (CacheDictionary.ContainsKey(message.Source))
            {
                CacheDictionary[message.Source] = message;
            }
            else
            {
                CacheDictionary.Add(message.Source, message);
            }
        }

        public virtual ICollection<DashboardMessage> Get(params string[] dataSources)
        {
            if (dataSources == null || dataSources.Length == 0)
            {
                return CacheDictionary.Values;
            }

            var list = new List<DashboardMessage>();
            foreach (var dataSource in dataSources)
            {
                DashboardMessage message;
                if (CacheDictionary.TryGetValue(dataSource, out message))
                {
                    list.Add(message);
                }
            }
            return list;
        }

        public ICollection<DashboardMessage> GetAll()
        {
            return CacheDictionary.Values;
        }
    }
}