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

            _clientResource = new EmbeddedClientResource(_targetAssembly, ResourceName);
            
            var processor = new ClientResourceWebResourceProcessor();

            ClientResourceWebResourceProcessor.ResolveWebResourceUrlThunk = GetWebResourceUrl;
            
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
    }
}