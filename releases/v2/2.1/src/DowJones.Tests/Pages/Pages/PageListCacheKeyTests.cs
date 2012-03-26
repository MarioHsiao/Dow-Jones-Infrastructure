using DowJones.Extensions;
using DowJones.Infrastructure.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Pages
{
    [TestClass]
    public class PageListCacheKeyTests : AbstractUnitTest
    {
        private const string ExpectedCacheApplication = "TEST";

        [TestMethod]
        public void ShouldSerialize()
        {
            var cacheKey = new PageListCacheKey(new Product("test", "test")) { CacheApplication = ExpectedCacheApplication };
            
            var serializedCacheKey = cacheKey.Serialize();

            Assert.IsTrue(serializedCacheKey.HasValue());
        }
    }
}