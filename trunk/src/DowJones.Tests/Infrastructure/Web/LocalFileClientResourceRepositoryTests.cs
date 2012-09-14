using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DowJones.Web.ClientResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web
{
    [TestClass]
    public class LocalFileClientResourceRepositoryTests : UnitTestFixture
    {
        [TestMethod]
        public void ShouldLoadClientResourcesFromDirectoryMappingWithRelativePath()
        {
            var expectedFilenames = Directory.GetFiles(CurrentDirectory);

            string parentDirectory = Directory.GetParent(CurrentDirectory).FullName;
            string directory = "~/" + new DirectoryInfo(CurrentDirectory).Name;

            Assert.IsTrue(expectedFilenames.Count() > 0,
                          "Expected some files in the test directory - something's wrong!");

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<directory path='{0}'/>", directory);
            xmlBuilder.Append("</ClientResources>");

            var config = new ClientResourceConfiguration(XDocument.Parse(xmlBuilder.ToString()));
            var repository = new LocalFileClientResourceRepository(config) { RootDirectory = parentDirectory };

            var resources = repository.GetClientResources().ToArray();

            Assert.AreEqual(expectedFilenames.Count(), resources.Count());

            foreach (var filename in expectedFilenames)
            {
                CollectionAssert.Contains(
                    resources,
                    new ClientResource(filename),
                    string.Format("Missing Client Resource for file {0}", filename));
            }
        }

        [TestMethod]
        public void ShouldLoadClientResourcesFromDirectoryMapping()
        {
            var expectedFilenames = Directory.GetFiles(CurrentDirectory);

            Assert.IsTrue(expectedFilenames.Count() > 0,
                          "Expected some files in the test directory - something's wrong!");

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<directory path='{0}'/>", CurrentDirectory);
            xmlBuilder.Append("</ClientResources>");

            var config = new ClientResourceConfiguration(XDocument.Parse(xmlBuilder.ToString()));
            var repository = new LocalFileClientResourceRepository(config);

            var resources = repository.GetClientResources().ToArray();


            Assert.AreEqual(expectedFilenames.Count(), resources.Count());

            foreach (var filename in expectedFilenames)
            {
                CollectionAssert.Contains(
                    resources,
                    new ClientResource(filename),
                    string.Format("Missing Client Resource for file {0}", filename));
            }
        }

        [TestMethod]
        public void ShouldLoadClientResourcesFromMappings()
        {
            const string expectedClientResourceName = "DowJones.Web.Mvc.UI.DemoComponent.js";

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<resource name='{0}'/>", expectedClientResourceName);
            xmlBuilder.Append("</ClientResources>");

            var config = new ClientResourceConfiguration(XDocument.Parse(xmlBuilder.ToString()));
            var repository = new LocalFileClientResourceRepository(config);
            var resource = repository.GetClientResources().Single();

            Assert.AreEqual(expectedClientResourceName,
                            resource.Name);
        }

        [TestMethod]
        public void ShouldLoadClientResourceDependencyLevelFromMapping()
        {
            const ClientResourceDependencyLevel expectedDependencyLevel = ClientResourceDependencyLevel.Component;

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<resource name='DowJones.Web.Mvc.UI.DemoComponent.js' level='{0}' />",
                                    Enum.GetName(typeof(ClientResourceDependencyLevel), expectedDependencyLevel));
            xmlBuilder.Append("</ClientResources>");

            var config = new ClientResourceConfiguration(XDocument.Parse(xmlBuilder.ToString()));
            var repository = new LocalFileClientResourceRepository(config);
            var resource = repository.GetClientResources().Single();

            Assert.AreEqual(expectedDependencyLevel,
                            resource.DependencyLevel);
        }

    }
}