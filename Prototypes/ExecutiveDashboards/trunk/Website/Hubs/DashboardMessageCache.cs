using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Data;
using Raven.Json.Linq;

namespace DowJones.Dash.Website.Hubs
{
    public class DashboardMessage
    {
        public object Data { get; set; }
        public string DataSource { get; set; }

        public DashboardMessage(string dataSource = null, object data = null)
        {
            Data = data;
            DataSource = dataSource;
        }
    }

    public class DashboardErrorMessage : DashboardMessage
    {
        public DashboardErrorMessage(string dataSource, Exception exception = null)
            : base(dataSource, exception == null ? "Unknown error" : exception.Message)
        {
        }
    }

    public interface IDashboardMessageCache
    {
        void Add(DashboardMessage message);
        IEnumerable<DashboardMessage> Get(params string[] eventNames);
    }

    public class DashboardMessageCache : IDashboardMessageCache
    {
        private readonly IDictionary<string, DashboardMessage> _cache = 
            new ConcurrentDictionary<string, DashboardMessage>();

        public void Add(DashboardMessage message)
        {
            if(_cache.ContainsKey(message.DataSource))
                _cache[message.DataSource] = message;
            else
                _cache.Add(message.DataSource, message);
        }

        public IEnumerable<DashboardMessage> Get(params string[] eventNames)
        {
            if (eventNames == null || eventNames.Length == 0)
                return _cache.Values;

            return _cache.Keys.Select(key => _cache[key]);
        }
    }

    public class RavenDbMessageCache : IDashboardMessageCache
    {
        private readonly Raven.Client.IDocumentSession _session;

        public RavenDbMessageCache(Raven.Client.IDocumentStore documentStore)
        {
            _session = documentStore.OpenSession();
            _session.Advanced.MaxNumberOfRequestsPerSession = int.MaxValue;
            _session.Advanced.DocumentStore.DisableAggressiveCaching();
        }

        public void Add(DashboardMessage message)
        {
            var documentId = GetDocumentId(message);
            
            var value = message.Data is Array 
                ? RavenJArray.FromObject(message.Data)
                : RavenJObject.FromObject(message.Data);

            _session.Advanced.DocumentStore.DatabaseCommands.Patch(
                documentId, 
                new [] {
                    new PatchRequest { 
                        Type = PatchCommandType.Set,
                        Name = "Data",
                        Value = value,
                    }
                });
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
            return "DashboardMessages/" + eventName.GetHashCode();
        }
    }
}