namespace DowJones.Json.Gateway.Core
{
    public enum CacheStatus
    {
        /// <summary>
        /// 'Miss' means items was not found in the cache.
        /// </summary>
        Miss,

        /// <summary>
        /// 'Hit' means items was found in the cache.
        /// </summary>
        Hit
    }
}