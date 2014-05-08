using DowJones.Infrastructure.Common;
using DowJones.Caching;
using Newtonsoft.Json;

namespace DowJones.Pages.DataAccess.Caching
{
    public abstract class AbstractModuleCacheKey : AbstractCacheKey
    {
        protected AbstractModuleCacheKey(string pageId, string moduleId, Product product)
            : base(product)
        {
            ModuleId = moduleId;
            PageId = pageId;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.ModuleId)]
        public string ModuleId { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.PageId)]
        public string PageId { get; set; }
    }
}
