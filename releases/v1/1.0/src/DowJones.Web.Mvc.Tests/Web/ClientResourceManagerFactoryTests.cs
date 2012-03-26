using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DowJones.Infrastructure;
using DowJones.Utilities.Exceptions;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones.Web
{
    [TestClass]
    public class ClientResourceManagerFactoryTests : UnitTestFixtureBase<ClientResourceManagerFactory>
    {
        private const string TestScriptResourceName = "TEST_SCRIPT_RESOURCE";
        private const string TestStylesheetResourceName = "TEST_STYLESHEET_RESOURCE";

        private Mock<IAssemblyRegistry> _mockAssemblyRegistry;

        protected ClientResourceManagerFactory Factory
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ShouldLoadClientResourceAliases()
        {
            const string jqueryAlias = "jquery", jqueryName = "DowJones.Web.Mvc.Resources.js.jquery.js";
            const string commonAlias = "common", commonName = "DowJones.Web.Mvc.Resources.js.common.js";

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", jqueryAlias, jqueryName);
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", commonAlias, commonName);
            xmlBuilder.Append("</ClientResources>");

            Factory.ClientResourceConfiguration = XDocument.Parse(xmlBuilder.ToString());

            var aliases = Factory.GetClientResourceAliases();

            Assert.AreEqual(2, aliases.Count());
            Assert.AreEqual(jqueryName, aliases.Single(x => x.Alias == jqueryAlias).Name);
            Assert.AreEqual(commonName, aliases.Single(x => x.Alias == commonAlias).Name);
        }

        [TestMethod]
        public void ShouldThrowExceptionForInvalidClientResourceAliasMappingAlias()
        {
            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", string.Empty, "RESOURCE_NAME");
            xmlBuilder.Append("</ClientResources>");

            Factory.ClientResourceConfiguration = XDocument.Parse(xmlBuilder.ToString());

            try
            {
                Factory.GetClientResourceAliases();
            }
            catch(DowJonesUtilitiesException)
            {
                return;
            }

            Assert.Fail("Exception not thrown!");
        }

        [TestMethod]
        public void ShouldThrowExceptionForInvalidClientResourceAliasMappingName()
        {
            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<mapping alias='{0}' name='{1}'/>", "RESOURCE_ALIAS", string.Empty);
            xmlBuilder.Append("</ClientResources>");

            Factory.ClientResourceConfiguration = XDocument.Parse(xmlBuilder.ToString());

            try
            {
                Factory.GetClientResourceAliases();
            }
            catch(DowJonesUtilitiesException)
            {
                return;
            }

            Assert.Fail("Exception not thrown!");
        }

        [TestMethod]
        public void ShouldLoadClientResourcesFromClientResourceAttributes()
        {
            _mockAssemblyRegistry
                .Setup(x => x.Assemblies)
                .Returns(new[] { typeof(TestClientResourceDecoratedType).Assembly });

            var resources = Factory.GetClientResources();

            Assert.AreEqual(2, resources.Count());
            Assert.AreEqual(TestScriptResourceName, 
                            resources.Single(x => x.ResourceKind == ClientResourceKind.Script).Name);
            Assert.AreEqual(TestStylesheetResourceName, 
                            resources.Single(x => x.ResourceKind == ClientResourceKind.Stylesheet).Name);
        }

        [TestMethod]
        public void ShouldLoadClientResourcesFromMappings()
        {
            const string expectedClientResourceName = "DowJones.Web.Mvc.UI.DemoComponent.js";

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<resource name='{0}'/>", expectedClientResourceName);
            xmlBuilder.Append("</ClientResources>");

            Factory.ClientResourceConfiguration = XDocument.Parse(xmlBuilder.ToString());

            var resources = Factory.GetClientResources();

            Assert.AreEqual(expectedClientResourceName, 
                            resources.Single().Name);
        }

        [TestMethod]
        public void ShouldLoadClientResourcesFromDirectoryMapping()
        {
            string directory = Environment.CurrentDirectory;

            var expectedFilenames = Directory.GetFiles(directory);

            Assert.IsTrue(expectedFilenames.Count() > 0, 
                          "Expected some files in the test directory - something's wrong!");

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<directory path='{0}'/>", directory);
            xmlBuilder.Append("</ClientResources>");

            Factory.ClientResourceConfiguration = XDocument.Parse(xmlBuilder.ToString());

            var resources = Factory.GetClientResources().ToList();

            Assert.AreEqual(expectedFilenames.Count(), resources.Count());

            foreach(var filename in expectedFilenames)
            {
                CollectionAssert.Contains(
                    resources, 
                    new ClientResource(filename),
                    string.Format("Missing Client Resource for file {0}", filename));
            }
        }

        [TestMethod]
        public void ShouldLoadClientResourcesFromDirectoryMappingWithRelativePath()
        {
            var expectedFilenames = Directory.GetFiles(Environment.CurrentDirectory);

            string parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            string directory = "~/" + new DirectoryInfo(Environment.CurrentDirectory).Name;

            Assert.IsTrue(expectedFilenames.Count() > 0, 
                          "Expected some files in the test directory - something's wrong!");

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<directory path='{0}'/>", directory);
            xmlBuilder.Append("</ClientResources>");

            Factory.ClientResourceConfiguration = XDocument.Parse(xmlBuilder.ToString());
            Factory.RootDirectory = parentDirectory;

            var resources = Factory.GetClientResources().ToList();

            Assert.AreEqual(expectedFilenames.Count(), resources.Count());

            foreach(var filename in expectedFilenames)
            {
                CollectionAssert.Contains(
                    resources, 
                    new ClientResource(filename),
                    string.Format("Missing Client Resource for file {0}", filename));
            }
        }

        [TestMethod]
        public void ShouldLoadClientResourceDependencyLevelFromMapping()
        {
            const ClientResourceDependencyLevel expectedDependencyLevel = ClientResourceDependencyLevel.Component;

            var xmlBuilder = new StringBuilder("<ClientResources>");
            xmlBuilder.AppendFormat("<resource name='DowJones.Web.Mvc.UI.DemoComponent.js' level='{0}' />",
                                    Enum.GetName(typeof(ClientResourceDependencyLevel), expectedDependencyLevel));
            xmlBuilder.Append("</ClientResources>");

            Factory.ClientResourceConfiguration = XDocument.Parse(xmlBuilder.ToString());

            var resources = Factory.GetClientResources();

            Assert.AreEqual(expectedDependencyLevel, 
                            resources.Single().DependencyLevel);
        }


        protected override ClientResourceManagerFactory CreateUnitUnderTest()
        {
            _mockAssemblyRegistry = new Mock<IAssemblyRegistry>();

            ClientResourceManagerFactory unitUnderTest =
                new ClientResourceManagerFactory(_mockAssemblyRegistry.Object);

            unitUnderTest.ClientResourceConfiguration = XDocument.Parse("<ClientResources/>");

            unitUnderTest.Log = new Mock<ILog>().Object;

            return unitUnderTest;
        }


        [StylesheetResource(TestStylesheetResourceName, Url = "~/test.css")]
        [ScriptResource(TestScriptResourceName, Url="~/test.js")]
        public class TestClientResourceDecoratedType
        {
        }
    }
}