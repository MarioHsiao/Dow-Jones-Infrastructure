using System;
using DowJones.Json.Gateway.Common;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface ITransactionCacheData : IJsonSerializable
    {
        string CacheApplication { get; set; }

        CacheExpirationPolicy? CacheExpirationPolicy { get; set; }

        int CacheExpirationTime { get; set; }

        string CacheKey { get; set; }

        string CacheHit { get; set; }

        int CacheRefreshInterval { get; set; }

        CacheScope? CacheScope { get; set; }
        
        CacheStatus? CacheStatus { get; set; }

        string CacheWait { get; set; }

        bool ForceCacheRefresh { get; set; }
    }
}