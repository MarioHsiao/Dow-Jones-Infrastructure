using System.Linq;
using DowJones.Documentation.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones.Documentation.Tests.Core.DataAccess
{
    [TestClass]
    public class CachingContentRepositoryTests
    {
        private Mock<IContentRepository> _source;
        private CachingContentRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _source = new Mock<IContentRepository>();
            _repository = new CachingContentRepository(_source.Object);
        }

        [TestMethod]
        public void ShouldGetCategoriesFromSource()
        {
            var expectedCategories = new[] {new ContentSection("Category1"), new ContentSection("Category2")};

            _source
                .Setup(x => x.GetCategories())
                .Returns(expectedCategories);

            var actualCategories = _repository.GetCategories();

            CollectionAssert.AreEquivalent(expectedCategories, actualCategories.ToArray());
        }

        [TestMethod]
        public void ShouldGetCategoriesFromCache()
        {
            var expectedCategories = new[] {new ContentSection("Category1"), new ContentSection("Category2")};

            _source
                .Setup(x => x.GetCategories())
                .Returns(expectedCategories);

            // Prime cache:
            _repository.GetCategories();

            // Get cached version:
            var actualCategories = _repository.GetCategories();

            _source.Verify(x => x.GetCategories(), Times.Once());
            CollectionAssert.AreEquivalent(expectedCategories, actualCategories.ToArray());
        }


        [TestMethod]
        public void ShouldGetCategoryFromSource()
        {
            var expectedCategory = new ContentSection("Category1");

            _source
                .Setup(x => x.GetCategory(expectedCategory.Name))
                .Returns(expectedCategory);

            var actualCategory = _repository.GetCategory(expectedCategory.Name);

            Assert.AreEqual(expectedCategory, actualCategory);
        }

        [TestMethod]
        public void ShouldGetCategoryFromCache()
        {
            var expectedCategory = new ContentSection("Category1");

            _source
                .Setup(x => x.GetCategory(expectedCategory.Name))
                .Returns(expectedCategory);

            // Prime cache:
            _repository.GetCategory(expectedCategory.Name);

            // Get cached version:
            var actualCategory = _repository.GetCategory(expectedCategory.Name);

            _source.Verify(x => x.GetCategory(expectedCategory.Name), Times.Once());
            Assert.AreEqual(expectedCategory, actualCategory);
        }


        [TestMethod]
        public void ShouldGetPageFromSource()
        {
            var expectedPage = new ContentSection("ExpectedPage");

            _source
                .Setup(x => x.GetPage(expectedPage.Name.Key, "SomeCategory"))
                .Returns(expectedPage);

            var actualPage = _repository.GetPage(expectedPage.Name.Key, "SomeCategory");

            Assert.AreEqual(expectedPage, actualPage);
        }

        [TestMethod]
        public void ShouldGetPageFromCache()
        {
            var expectedPage = new ContentSection("ExpectedPage");

            _source
                .Setup(x => x.GetPage(expectedPage.Name.Key, "SomeCategory"))
                .Returns(expectedPage);

            // Prime cache:
            _repository.GetPage(expectedPage.Name.Key, "SomeCategory");

            // Get cached version:
            var actualPage = _repository.GetPage(expectedPage.Name.Key, "SomeCategory");

            _source.Verify(x => x.GetPage(expectedPage.Name.Key, "SomeCategory"), Times.Once());
            Assert.AreEqual(expectedPage, actualPage);
        }
    }
}
