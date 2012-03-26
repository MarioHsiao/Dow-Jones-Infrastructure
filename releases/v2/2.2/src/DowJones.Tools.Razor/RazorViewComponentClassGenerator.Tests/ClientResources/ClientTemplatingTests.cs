using System.CodeDom;
using System.Linq;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    [TestClass]
    public class ClientTemplatingTests : RazorViewComponentClassGeneratorTestFixture
    {
        private ClientTemplateParser _clientTemplateParser;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            _clientTemplateParser = new ClientTemplateParser();
        }

        [TestMethod]
        public void ShouldGenerateClientTemplateResourceAttributesForClientTemplateIncludes()
        {
            const string template = @"
                @ClientTemplate HeadlineListCarouselClientTemplate.htm
                Hello, World!
            ";

            int clientTemplateCount =
                GetGeneratedCustomAttributes(template, "DowJones.Web.ClientTemplateResourceAttribute")
                    .Count();


            Assert.AreEqual(1, clientTemplateCount);
        }

        [TestMethod]
        public void ShouldParseIdForClientTemplateSpan()
        {
            const string template = @"
                @ClientTemplate RelativeResourceName=HeadlineListCarouselClientTemplate.htm, templateid=headlineListCarouselItemTemplate
                Hello, World!
            ";

            var attributes = GetGeneratedCustomAttributes(template, "DowJones.Web.ClientTemplateResourceAttribute");

            var templateIdArgument =
                attributes.Single()
                    .Arguments.OfType<CodeAttributeArgument>()
                    .SingleOrDefault(x => x.Name.ToLowerInvariant() == "templateid");

            Assert.IsNotNull(templateIdArgument);
            Assert.AreEqual("headlineListCarouselItemTemplate", ((CodePrimitiveExpression)templateIdArgument.Value).Value);
        }

    }
}