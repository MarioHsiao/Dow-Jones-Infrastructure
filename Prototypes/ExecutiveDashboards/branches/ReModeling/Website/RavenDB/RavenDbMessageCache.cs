using System.Collections.Generic;
using System.Linq;
using DowJones.Dash.Caching;
using Raven.Client;

namespace DowJones.Dash.Website.RavenDB
{
    public class RavenDbMessageCache : IDashboardMessageCache
    {
        private readonly IDocumentStore _store;

        public RavenDbMessageCache(IDocumentStore store)
        {
            _store = store;
        }

        public void Add(DashboardMessage message)
        {
            new Worker(GetSession()).Add(message);
        }

        public IEnumerable<DashboardMessage> Get(params string[] eventNames)
        {
            return new Worker(GetSession()).Get(eventNames);
        }

        protected IDocumentSession GetSession()
        {
            return _store.OpenSession();
        }


        public class Worker : IDashboardMessageCache
        {
            private readonly IDocumentSession _session;

            public Worker(IDocumentSession session)
            {
                _session = session;
            }

            public void Add(DashboardMessage message)
            {
                var documentId = GetDocumentId(message);

                var existing = _session.Load<DashboardMessage>(documentId);

                if (existing == null)
                    _session.Store(message, documentId);
                else
                    existing.Data = message.Data;

                _session.SaveChanges();
            }

            public IEnumerable<DashboardMessage> Get(params string[] eventNames)
            {
                IEnumerable<DashboardMessage> messages;

                var names = (eventNames ?? Enumerable.Empty<string>()).Select(GetDocumentId).ToArray();

                if (names.Any())
                    messages = _session.Load<DashboardMessage>(names);
                else
                    messages = _session.Query<DashboardMessage>();

                return messages.ToArray();
            }

            private static string GetDocumentId(DashboardMessage message)
            {
                return GetDocumentId(message.DataSource);
            }

            private static string GetDocumentId(string eventName)
            {
                return "DashboardMessages/" + eventName;
            }
        }
    }
}