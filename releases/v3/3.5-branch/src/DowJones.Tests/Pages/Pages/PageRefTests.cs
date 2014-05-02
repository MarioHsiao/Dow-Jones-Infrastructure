using DowJones.Infrastructure.Common;
using DowJones.Pages.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Pages
{
    [TestClass]
    public class PageRefTests : AbstractUnitTest
    {
        [TestMethod]
        public void ShouldGeneratedCachedPageID()
        {
            const int pageId = 1234;
            const int parentId = 456;
            var product = new Product("TEST", "TEST");

            var pageRef = new PageRef(pageId, product)
                              {
                                  ParentId = parentId.ToString(),
                              };

            string expectedCachedPageIdPrefix =
                string.Format("{0}_{1}_{2}_{3}", product.Id, PageRef.VersionID, pageId, parentId);

            Assert.IsTrue(pageRef.CachedPageId.StartsWith(expectedCachedPageIdPrefix));
        }
    }
}
