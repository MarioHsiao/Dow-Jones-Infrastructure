using System;
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

            ClientResourceRelativeUrlProcessor.AbsoluteUrlThunk =
                absoluteUrl => absoluteUrl.Replace("~/", VirtualPath + "/");
        }

        
        [TestMethod]
        public void ShouldReplaceAbsoluteUrlInContent()
        {
            const string RelativeUrl = "~/styles/site.css";
            const string Content = "Absolute URL: <%= AbsoluteUrl(\"" + RelativeUrl + "\") %>";
            var resource = new ProcessedClientResource(new ClientResource("~/test.html"), Content);


            new ClientResourceRelativeUrlProcessor().Process(resource);


            string expectedUrl = ClientResourceRelativeUrlProcessor.AbsoluteUrlThunk(RelativeUrl);

            Assert.IsFalse(string.IsNullOrWhiteSpace(expectedUrl));

            Assert.AreEqual(
                    "Absolute URL: " + expectedUrl,
                    resource.Content
                );

            Console.Write(expectedUrl);
            Console.Write(resource.Content);
        }
    }
}