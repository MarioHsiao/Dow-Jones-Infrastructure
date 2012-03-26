using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI
{
    [TestClass]
    public class ClientResourceWebResourceProcessorTests : UnitTestFixtureBase<ClientResourceWebResourceProcessor>
    {
        const string ResourceName = "DowJones.Web.Mvc.Tests.Resource.js";

        private ClientResource _clientResource;
        private Assembly _targetAssembly;


        protected override ClientResourceWebResourceProcessor CreateUnitUnderTest()
        {
            _targetAssembly = GetType().Assembly;

            _clientResource = new EmbeddedClientResource(_targetAssembly, ResourceName) { PerformSubstitution = true };
            
            var processor = new MockClientResourceWebResourceProcessor();
            
            return processor;
        }


        [TestMethod]
        public void ShouldReplaceWebResourceReferenceInContent()
        {
            string content = string.Format("This is a web resource replacement example: <div>100% <%=WebResource(\"{0}\")%></div>", ResourceName);
            string webResourceUrl = GetWebResourceUrl(_targetAssembly, ResourceName);

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                    Regex.Replace(content, @"\<%=.*%>", webResourceUrl),
                    resource.Content
                );
        }



        /// <summary>
        /// Class derived from ClientResourceWebResourceProcessor to override
        /// untestable ResolveWebResourceUrl method with deterministic behavior
        /// </summary>
        private class MockClientResourceWebResourceProcessor : ClientResourceWebResourceProcessor
        {
            internal override string ResolveWebResourceUrl(Assembly assembly, string resourceName)
            {
                return GetWebResourceUrl(assembly, resourceName);
            }
        }
    }
}