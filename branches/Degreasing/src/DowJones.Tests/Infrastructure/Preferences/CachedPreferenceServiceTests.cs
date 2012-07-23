using System.Collections.Generic;
using System.Linq;
using DowJones.Preferences;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CachedPreferenceResponse=DowJones.Preferences.CachedPreferenceService.CachedPreferenceResponse;

namespace DowJones.Infrastructure.Preferences
{
    [TestClass]
    public class CachedPreferenceServiceTests : UnitTestFixtureBase<CachedPreferenceService>
    {
        private static readonly PreferenceItem TestPreference = 
            new PreferenceItem { ClassID = PreferenceClassID.Keywords, ItemID = "TEST_PREFERENCE" };

        private Mock<IPreferenceService> _mockPreferencesSource;

        protected CachedPreferenceService Service
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ShouldClearCachedItemsOnRequest()
        {
            Service.CachedResponse.Add(TestPreference);

            Assert.AreEqual(1, Service.CachedResponse.Preferences.Count());

            Service.ClearCache();

            Assert.AreEqual(0, Service.CachedResponse.Preferences.Count());
        }

        [TestMethod]
        public void ShouldCacheItemsFromAddItemRequest()
        {
            Assert.IsFalse(Service.CachedResponse.Preferences.Any());

            Service.AddItem(new AddItemRequest { Item = TestPreference });

            Assert.AreEqual(TestPreference, Service.CachedResponse.Preferences.Single());
        }

        [TestMethod]
        public void ShouldDeleteItemsFromCacheAsWellAsSource()
        {
            Service.CachedResponse.Add(TestPreference);

            Service.DeleteItem(TestPreference.ItemID);

            Assert.IsFalse(Service.CachedResponse.Preferences.Any(), 
                           "Cached item was not deleted");
        }

        [TestMethod]
        public void ShouldCacheItemsFromUpdateItemRequest()
        {
            Assert.IsFalse(Service.CachedResponse.Preferences.Any());

            Service.UpdateItem(new UpdateItemRequest { Item = TestPreference });

            Assert.AreEqual(TestPreference, Service.CachedResponse.Preferences.Single());
        }

        [TestMethod]
        public void ShouldRetrieveUncachedPreferencesByIdFromSource()
        {
            _mockPreferencesSource
                .Setup(x => x.GetItemsById(It.IsAny<IEnumerable<string>>()))
                .Returns(new CachedPreferenceResponse(new [] { TestPreference }))
                .Verifiable("GetItemsById not called");

            Service.GetItemById(TestPreference.ItemID);
        }

        [TestMethod]
        public void ShouldAddRetrievedUncachedPreferencesByIdToCachedPreferences()
        {
            _mockPreferencesSource
                .Setup(x => x.GetItemsById(It.IsAny<IEnumerable<string>>()))
                .Returns(new CachedPreferenceResponse(new [] { TestPreference }))
                .Verifiable("GetItemsById not called");

            Assert.IsFalse(Service.CachedResponse.Preferences.Any());

            Service.GetItemById(TestPreference.ItemID);

            Assert.AreEqual(TestPreference, Service.CachedResponse.Preferences.Single());
        }

        [TestMethod]
        public void ShouldNotRetrieveCachedPreferencesByIdFromSource()
        {
            Service.CachedResponse.Add(TestPreference);

            _mockPreferencesSource
                .Setup(x => x.GetItemsById(It.IsAny<IEnumerable<string>>()))
                .Callback(Assert.Fail);

            Service.GetItemById(TestPreference.ItemID);
        }

        [TestMethod]
        public void ShouldAddRetrievedUncachedPreferencesByClassToCachedPreferences()
        {
            _mockPreferencesSource
                .Setup(x => x.GetItemsByClassId(It.IsAny<IEnumerable<PreferenceClassID>>()))
                .Returns(new CachedPreferenceResponse(new[] { TestPreference }))
                .Verifiable("GetItemsByClassId not called");

            Assert.IsFalse(Service.CachedResponse.Preferences.Any());

            Service.GetItemByClassId(TestPreference.ClassID);

            Assert.AreEqual(TestPreference, Service.CachedResponse.Preferences.Single());
        }

        [TestMethod]
        public void ShouldNotRetrieveCachedPreferencesByClassFromSource()
        {
            Service.CachedResponse.Add(TestPreference);

            _mockPreferencesSource
                .Setup(x => x.GetItemsByClassId(It.IsAny<IEnumerable<PreferenceClassID>>()))
                .Callback(Assert.Fail);

            Service.GetItemByClassId(TestPreference.ClassID);
        }

        [TestMethod]
        public void GetItemsByClassIdNoCache_ShouldRetrieveValueFromSourceEvenIfItIsAlreadyCached()
        {
            Service.CachedResponse.Add(TestPreference);

            Service.GetItemsByClassIdNoCache(new GetItemsByClassIDNoCacheRequest { ClassID = new [] {TestPreference.ClassID} });

            _mockPreferencesSource
                .Verify(x => x.GetItemsByClassIdNoCache(It.IsAny<GetItemsByClassIDNoCacheRequest>()),
                        "Source GetItemsByClassIdNoCache() should have been called, but wasn't");
        }

        [TestCleanup]
        public override void TearDown()
        {
            _mockPreferencesSource.Verify();
            
            base.TearDown();
        }

        protected override CachedPreferenceService CreateUnitUnderTest()
        {
            _mockPreferencesSource = new Mock<IPreferenceService>();

            return new CachedPreferenceService(_mockPreferencesSource.Object)
            {
                CachedResponse = new CachedPreferenceResponse(new PreferenceItem[0])
            };
        }
    }
}