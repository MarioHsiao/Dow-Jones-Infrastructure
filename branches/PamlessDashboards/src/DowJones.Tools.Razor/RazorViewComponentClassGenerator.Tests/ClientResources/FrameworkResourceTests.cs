using System.CodeDom;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    [TestClass]
    public class FrameworkResourceTests : RazorViewComponentClassGeneratorTestFixture
    {
        [TestMethod]
        public void ShouldGenerateFrameworkResourceAttributesForFrameworkIncludes()
        {
            const string template = @"
                @FrameworkResource ResourceName=Js.JQuery
                Hello, World!
            ";

            int frameworkResourcesCount =
                GetGeneratedCustomAttributes(template, "DowJones.Web.FrameworkResourceAttribute")
                    .Count();

            Assert.AreEqual(1, frameworkResourcesCount);
        }

        [TestMethod]
        public void ShouldNotGenerateWebResourceAttributesForFrameworkIncludes()
        {
            const string template = @"
                @FrameworkResource ResourceName=Js.JQuery
                Hello, World!
            ";

            var generatorResults = Generator.GenerateRazorTemplate(template);

            var webResourceAttributesCount =
                generatorResults.GeneratedCode
                    .AssemblyCustomAttributes
                    .Cast<CodeAttributeDeclaration>()
                    .Count(x => x.Name.EndsWith("WebResourceAttribute"));

            Assert.AreEqual(0, webResourceAttributesCount);
        }

    }
}