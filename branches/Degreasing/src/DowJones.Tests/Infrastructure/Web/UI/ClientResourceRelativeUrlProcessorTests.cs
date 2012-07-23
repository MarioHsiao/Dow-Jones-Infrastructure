using DowJones.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI
{
    [TestClass]
    public class ClientResourceRelativeUrlProcessorTests : UnitTestFixture
    {
        const string VirtualPath = "/demosite";

        [TestInitialize]
        public void TestInitialize()
        {
            ClientResourceUrlProcessor.AbsoluteUrlThunk =
                (url, request) => url.Replace("~/", VirtualPath + "/");
            ClientResourceUrlProcessor.RelativeUrlThunk =
                (url) => url.Replace("~/", "http://www.mysite.com" + VirtualPath + "/");
        }

        
        [TestMethod]
        public void ShouldReplaceAbsoluteUrlInContent()
        {
            const string Url = "~/styles/site.css";
            const string Content = "Absolute URL: <%= AbsoluteUrl(\"" + Url + "\") %>";
            var resource = new ProcessedClientResource(new ClientResource("~/test.html"), Content);

            var request = new MockHttpRequest();
            new ClientResourceUrlProcessor(request).Process(resource);

            string expectedUrl = ClientResourceUrlProcessor.AbsoluteUrlThunk(Url, request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(expectedUrl));

            Assert.AreEqual(
                    "Absolute URL: " + expectedUrl,
                    resource.Content
                );
        }

        [TestMethod]
        public void ShouldReplaceRelativeUrlInContent()
        {
            const string url = "~/styles/site.css";
            const string Content = "Absolute URL: <%= RelativeUrl(\"" + url + "\") %>";
            var resource = new ProcessedClientResource(new ClientResource("~/test.html"), Content);

            new ClientResourceUrlProcessor(null).Process(resource);

            string expectedUrl = ClientResourceUrlProcessor.RelativeUrlThunk(url);

            Assert.IsFalse(string.IsNullOrWhiteSpace(expectedUrl));

            Assert.AreEqual(
                    "Absolute URL: " + expectedUrl,
                    resource.Content
                );
        }
    }
}