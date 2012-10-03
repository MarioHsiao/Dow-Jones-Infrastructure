using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DowJones.Dash.Caching;
using DowJones.DependencyInjection;
using Raven.Client;
using log4net;

namespace DowJones.Dash.Website.RavenDB
{
    /// <summary>
    /// A RavenDB-specific implementation of the DashboardMessageCache
    /// </summary>
    /// <remarks>
    /// Extends the in-memory message cache and persists it every n seconds
    /// </remarks>
    public class RavenDbDashboardMessageCache : DashboardMessageCache, IInitializable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RavenDbDashboardMessageCache));

        private readonly IDocumentStore _store;
        private Timer _timer;

        public RavenDbDashboardMessageCache(IDocumentStore store)
        {
            _store = store;
        }

        public void Initialize()
        {
            Load();
            _timer = new Timer(Persist, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        protected internal void Load()
        {
            Log.Info("Loading cached messages...");

            using(var session = _store.OpenSession())
            {
                var messages = session.Query<DashboardMessage>().ToArray();
                
                foreach (var message in messages)
                {
                    Add(message);
                }
                
                Log.DebugFormat("Loaded {0} cached messages.", messages.Count());
            }
        }

        protected internal void Persist(object state)
        {
            Log.Info("Persisting cached messages...");

            var cachedMessages = new Dictionary<string, DashboardMessage>(Cache);
            var documentIds = cachedMessages.Keys.Select(GetDocumentId);

            using (var session = _store.OpenSession())
            {
                var existingMessages = 
                    session
                        .Load<DashboardMessage>(documentIds)
                        .Where(x => x != null)
                        .ToArray();

                foreach (var key in cachedMessages.Keys)
                {
                    var message = cachedMessages[key];
                    var documentId = GetDocumentId(key);

                    var existing = existingMessages.FirstOrDefault(x => x.Source == message.Source);
                    
                    if (existing != null)
                    {
                        Log.DebugFormat("{0} ({1}) previously cached -- updating existing instance", key, documentId);
                        existing.Data = message.Data;
                    }
                    else
                    {
                        Log.DebugFormat("{0} ({1}) not previously cached -- adding new instance", key, documentId);
                        session.Store(message, documentId);
                    }
                }

                session.SaveChanges();
            }

            Log.DebugFormat("Persisted {0} messages.", cachedMessages.Values.Count());
        }

        private static string GetDocumentId(string eventName)
        {
            return "DashboardMessages/" + eventName;
        }
    }
}