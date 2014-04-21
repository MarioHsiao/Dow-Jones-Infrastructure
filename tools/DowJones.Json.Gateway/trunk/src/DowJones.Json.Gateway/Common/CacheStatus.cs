namespace DowJones.Json.Gateway.Common
{
    public enum CacheStatus
    {
        NotSpecified,

        Unknown,
        
        /// <summary>
        /// 'Hit' means items was found in the cache.
        /// </summary>
        Hit,
        
        /// <summary>
        /// 'Miss' means items was not found in the cache.
        /// </summary>
        Miss
    }
}