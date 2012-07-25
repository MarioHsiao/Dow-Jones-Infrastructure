using System.Collections.Generic;
using System.IO;
using System.Linq;
using DowJones.Documentation.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Documentation.Tests.Core.DataAccess
{
    [TestClass]
    public class FileBasedContentRepositoryTests
    {
        readonly string[] _expectedCategoryKeys = new[] { "Components", "Dashboards" };

        private FileBasedContentRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Yeah, this is crazy, right?  Stupid Visual Studio test runner...
            var testPathReplacer = new System.Text.RegularExpressions.Regex(@"\\(?:(TestResults\\[^\\]*\\Out)|([^\\]*\\bin\\[^\\]*))");
            var assemblyDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
            assemblyDirectory = testPathReplacer.Replace(assemblyDirectory, string.Empty);

            var baseDirectory = Path.Combine(assemblyDirectory, "Tests", "Core", "DataAccess", "TestData");
            _repository = new FileBasedContentRepository(baseDirectory);
        }

        [TestMethod]
        public void ShouldLoadCategoriesFromTopLevelFolders()
        {
            var categories = _repository.GetCategories();
            var categoryNames = categories.Select(x => x.Name.Value).ToArray();
            CollectionAssert.AreEquivalent(_expectedCategoryKeys, categoryNames);
        }

        [TestMethod]
        public void ShouldLoadPagesFromCategoryFiles()
        {
            var categories = _repository.GetCategories().ToArray();

            Assert.IsNotNull(GetPage(categories, "Components", "Overview"));
            Assert.IsNotNull(GetPage(categories, "Dashboards", "Overview"));
        }

        [TestMethod]
        public void ShouldLoadPagesFromCategoryFolders()
        {
            var categories = _repository.GetCategories().ToArray();

            Assert.IsNotNull(GetPage(categories, "Components", "DiscoveryGraph"));
            Assert.IsNotNull(GetPage(categories, "Dashboards", "CanvasModule"));
        }

        [TestMethod]
        public void ShouldLoadPageSectionsFromFiles()
        {
            var categories = _repository.GetCategories().ToArray();

            var page = GetPage(categories, "Components", "DiscoveryGraph");
            var sectionNames = page.Children.Select(x => x.Name.Key);
            CollectionAssert.AreEquivalent(new[] { "methods", "overview", "properties" }, sectionNames.ToArray());
        }

        [TestMethod]
        public void ContentSectionComparerShouldOrderNames()
        {
            var order = new[] {"1", "2", "3"};
            var names = new Name[] { "4", "3", "1", "5", "2" }.Select(x => new ContentSection(x));
            var ordered = names.OrderBy(x => x, new FileBasedContentRepository.ContentSectionComparer(order)).Select(x => x.Name.Key);

            CollectionAssert.AreEquivalent(
                new[] { "1", "2", "3", "4", "5" },
                ordered.ToArray());
        }

        private static ContentSection GetCategory(IEnumerable<ContentSection> categories, string categoryName)
        {
            return categories.First(x => x.Name.Value == categoryName);
        }

        private static ContentSection GetPage(IEnumerable<ContentSection> categories, string categoryName, string pageName)
        {
            return GetCategory(categories, categoryName).Children.First(x => x.Name.Value == pageName);
        }
    }
}
