using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI
{
    [TestClass]
    public class ClientResourceTests : UnitTestFixture
    {
        private static readonly ClientResource Resource = new ClientResource("URL1");
        private static readonly ClientResource DifferentUrl = new ClientResource("URL2");
        private static readonly ClientResource SameUrl = new ClientResource(Resource.Url);

        private static readonly ClientResource GlobalResource = new ClientResource() { DependencyLevel = ClientResourceDependencyLevel.Global };
        private static readonly ClientResource MidLevelResource = new ClientResource() { DependencyLevel = ClientResourceDependencyLevel.MidLevel };
        private static readonly ClientResource ComponentResource = new ClientResource() { DependencyLevel = ClientResourceDependencyLevel.Component };
        private static readonly ClientResource IndependentResource = new ClientResource() { DependencyLevel = ClientResourceDependencyLevel.Independent };

        [TestMethod]
        public void ShouldEvaluateEqualityOnUrl()
        {
            Assert.AreEqual(Resource, SameUrl);
            Assert.AreNotEqual(Resource, DifferentUrl);
        }

        [TestMethod]
        public void ShouldBeAbleToProvideADistinctListBasedOnUrl()
        {
            IEnumerable<ClientResource> listWithDuplicates = new[] {
                    Resource,
                    SameUrl,
                    DifferentUrl,
                    SameUrl,
                };

            var distinctList = listWithDuplicates.Distinct();

            Assert.AreEqual(2, distinctList.Count());
        }

        [TestMethod]
        public void ShouldSortInOrderOfDependencyLevel()
        {
            IEnumerable<ClientResource> randomOrderSet = new[] {
                    MidLevelResource,
                    ComponentResource,
                    GlobalResource,
                    IndependentResource,
                };

            var orderedSet = randomOrderSet.OrderByDescending(x => x.DependencyLevel);

            Assert.AreSame(GlobalResource, orderedSet.First());
            Assert.AreSame(MidLevelResource, orderedSet.Skip(1).First());
            Assert.AreSame(ComponentResource, orderedSet.Skip(2).First());
            Assert.AreSame(IndependentResource, orderedSet.Last());
        }
    }
}