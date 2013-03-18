using DowJones.Caching;

namespace DowJones.Session
{
    public class TransactionCacheControlData
    {
        public TransactionCacheControlData()
        {
            ExiprationPolicy = CacheExiprationPolicy.None;
            Scope = CacheScope.User;
        }

        public string Key { get; set; }
        public CacheScope Scope { get; set; }
        public int ExpirationTime { get; set; }
        public int RefreshInterval { get; set; }
        public CacheExiprationPolicy ExiprationPolicy { get; set; }
        public bool ForceCacheRefresh { get; set; }
        public bool Wait { get; set; }
    }
}