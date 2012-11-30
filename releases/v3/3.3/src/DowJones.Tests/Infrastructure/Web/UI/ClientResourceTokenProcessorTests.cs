using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using DowJones.Globalization;
using DowJones.Mocks;
using DowJones.Token;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones.Web.UI
{
    [TestClass]
    public class ClientResourceTokenProcessorTests : UnitTestFixtureBase<ClientResourceTokenProcessor>
    {
        private Mock<ITokenRegistry> _tokenRegistry;
        private ClientResource _clientResource;


        protected override ClientResourceTokenProcessor CreateUnitUnderTest()
        {
            _tokenRegistry = new Mock<ITokenRegistry>();
            _clientResource = new ClientResource(string.Empty);
            
            var processor = new ClientResourceTokenProcessor(_tokenRegistry.Object);
            
            return processor;
        }


        [TestMethod]
        public void ShouldReplaceTokenInContent()
        {
            const string tokenName = "TOKEN_NAME";
            const string tokenValue = "TOKEN_VALUE";
            string content = string.Format("This is a token replacement example: <div>100% <%=Token(\"{0}\")%></div>", tokenName);

            _tokenRegistry
                .Setup(x => x.Get(tokenName))
                .Returns(tokenValue);

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                Regex.Replace(content, @"\<%=.*%>", tokenValue),
                resource.Content
                );
        }

        [TestMethod]
        public void ShouldReplaceTokenWithSingleQuoteDelimiter()
        {
            const string tokenName = "TOKEN_NAME";
            const string tokenValue = "TOKEN_VALUE";
            string content = string.Format("This is a token replacement example: <div>100% <%=Token('{0}')%></div>", tokenName);

            _tokenRegistry
                .Setup(x => x.Get(tokenName))
                .Returns(tokenValue);

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                Regex.Replace(content, @"\<%=.*%>", tokenValue),
                resource.Content
                );
        }

        [TestMethod]
        public void ShouldReplaceTokenWithSpacesInContent()
        {
            const string tokenName = "TOKEN_NAME";
            const string tokenValue = "TOKEN_VALUE";
            string content = string.Format("This is a token replacement example: <div>100% <%= Token(\"{0}\") %></div>", tokenName);

            _tokenRegistry
                .Setup(x => x.Get(tokenName))
                .Returns(tokenValue);

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                    Regex.Replace(content, @"\<%=.*%>", tokenValue),
                    resource.Content
                );
        }

        [TestMethod]
        public void ShouldReplaceTokenInContentWithQuotes()
        {
            const string tokenName = "TOKEN_NAME";
            const string tokenValue = "TOKEN_VALUE";
            string content = string.Format("This is a token replacement example: <div>100% \"<%= Token(\"{0}\") %>\"</div>", tokenName);

            _tokenRegistry
                .Setup(x => x.Get(tokenName))
                .Returns(tokenValue);

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                    Regex.Replace(content, @"\<%=.*%>", tokenValue),
                    resource.Content
                );
        }

        [TestMethod]
        public void ShouldReplaceTokenInConcatenatedJavaScript()
        {
            const string content = "var myVar = \"<%= Token(\"TOKEN1\") %>\" + \"<%= Token(\"TOKEN2\") %>\";";

            _tokenRegistry.Setup(x => x.Get("TOKEN1")).Returns("VALUE1");
            _tokenRegistry.Setup(x => x.Get("TOKEN2")).Returns("VALUE2");

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                    "var myVar = \"VALUE1\" + \"VALUE2\";",
                    resource.Content
                );
        }

    }
}
