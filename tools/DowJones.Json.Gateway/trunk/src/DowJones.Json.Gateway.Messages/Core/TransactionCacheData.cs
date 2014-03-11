using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Core;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    public class TransactionCacheData : AbstractJsonSerializable, ITransactionCacheData
    {
        public string CacheApplication { get; set; }
        public CacheExpirationPolicy? CacheExpirationPolicy { get; set; }
        public int CacheExpirationTime { get; set; }
        public string CacheKey { get; set; }
        public string CacheHit { get; set; }
        public int CacheRefreshInterval { get; set; }
        public CacheScope? CacheScope { get; set; }
        public CacheStatus? CacheStatus { get; set; }
        public string CacheWait { get; set; }
        public bool ForceCacheRefresh { get; set; }
    }
}
