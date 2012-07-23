using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI
{
    [TestClass]
    public class StylesheetResourceImageUrlResolverTests : UnitTestFixtureBase<StylesheetResourceImageUrlResolver>
    {
        protected StylesheetResourceImageUrlResolver Resolver
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ShouldRebaseUrls()
        {
            var resource = CreateResource("~/test/test.css", @"url(/images/test.png)");

            Resolver.Process(resource);

            Assert.AreEqual("url('/test/images/test.png')", resource.Content);
        }

        [TestMethod]
        public void ShouldHandleUrlsWithPreviousDirectoryCharacters()
        {
            var resource = CreateResource("~/test/styles/theme/test.css", @"url(../../../test.png)");

            Resolver.Process(resource);

            Assert.AreEqual("url('/test.png')", resource.Content);
        }


        private static ProcessedClientResource CreateResource(string url, string content)
        {
            var clientResource = new ClientResource(url)
                                     {
                                         ResourceKind = ClientResourceKind.Stylesheet
                                     };

            return new ProcessedClientResource(clientResource)
                       {
                           Content = content,
                           ContentLoaded = true,
                       };
        }

        protected override StylesheetResourceImageUrlResolver CreateUnitUnderTest()
        {
            StylesheetResourceImageUrlResolver.ApplicationPath = "/";
            
            var unitUnderTest = new StylesheetResourceImageUrlResolver();

            return unitUnderTest;
        }
    }
}