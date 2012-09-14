using System.Linq;
using DowJones.Infrastructure;
using DowJones.Web.ClientResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones.Web
{
    [TestClass]
    public class EmbeddedClientResourceRepositoryTests : UnitTestFixture
    {
        [TestMethod]
        public void ShouldLoadClientResourcesFromClientResourceAttributes()
        {
            var targetAssembly = typeof(TestClientResourceDecoratedType).Assembly;

            var mockAssemblyRegistry = new Mock<IAssemblyRegistry>();
            mockAssemblyRegistry
                .Setup(x => x.Assemblies)
                .Returns(new[] { targetAssembly });

            var repository = new EmbeddedClientResourceRepository(mockAssemblyRegistry.Object);
            var resources = repository.GetClientResources();

            Assert.IsNotNull(resources.Single(x => x.Name == TestScriptResourceName));
            Assert.IsNotNull(resources.Single(x => x.Name == TestStylesheetResourceName));
        }

        private const string TestScriptResourceName = "TEST_SCRIPT_RESOURCE";
        private const string TestStylesheetResourceName = "TEST_STYLESHEET_RESOURCE";
        [StylesheetResource(TestStylesheetResourceName, Url = "~/test.css")]
        [ScriptResource(TestScriptResourceName, Url = "~/test.js")]
        public class TestClientResourceDecoratedType
        {
        }
    }
}