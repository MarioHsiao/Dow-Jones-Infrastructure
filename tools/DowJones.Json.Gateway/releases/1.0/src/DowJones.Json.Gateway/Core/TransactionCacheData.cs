using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class TransactionCacheData : AbstractJsonSerializable, ITransactionCacheData
    {
        public TransactionCacheData()
        {
            Tokens = new List<Token>();
        }

        [DataMember]
        public IEnumerable<Token> Tokens { get; set; }

        [DataMember]
        public string CacheApplication { get; set; }

        [DataMember]
        public CacheExpirationPolicy? CacheExpirationPolicy { get; set; }

        [DataMember]
        public int CacheExpirationTime { get; set; }

        [DataMember]
        public string CacheKey { get; set; }

        [DataMember]
        public string CacheHit { get; set; }

        [DataMember]
        public int CacheRefreshInterval { get; set; }

        [DataMember]
        public CacheScope? CacheScope { get; set; }

        public CacheStatus? CacheStatus { get; set; }
        
        [DataMember]
        public string CacheWait { get; set; }
        
        
        [DataMember]
        public bool ForceCacheRefresh { get; set; }
    }
}