using System.CodeDom;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    [TestClass]
    public class StylesheetResourceTests : RazorViewComponentClassGeneratorTestFixture
    {
        [TestMethod]
        public void ShouldGenerateStylesheetResourceAttributesForStylesheetIncludes()
        {
            const string template = @"
                @StylesheetResource RelativeResourceName=Test.css
                Hello, World!
            ";

            int stylesheetResourcesCount =
                GetGeneratedCustomAttributes(template, "DowJones.Web.StylesheetResourceAttribute")
                    .Count();


            Assert.AreEqual(1, stylesheetResourcesCount);
        }

        [TestMethod]
        public void ShouldGenerateAssemblyLevelPerformSubstitutionAttributesForStylesheetIncludes()
        {
            const string template = @"
                @StylesheetResource RelativeResourceName=Test.css, PerformSubstitution=true
                @ScriptResource RelativeResourceName=Test.js, PerformSubstitution=true
                @StylesheetResource RelativeResourceName=Test2.css
                Hello, World!
            ";

            var generatorResults = Generator.GenerateRazorTemplate(template);

            var assemblyAttributes =
                generatorResults.GeneratedCode.AssemblyCustomAttributes.Cast<CodeAttributeDeclaration>();

            var args = from attribute in assemblyAttributes
                       from pArg in attribute.Arguments.Cast<CodeAttributeArgument>()
                       where pArg.Name == "PerformSubstitution"
                       select pArg.Name;


            Assert.AreEqual(2, args.Count());
        }

        [TestMethod]
        public void ShouldGenerateClientResourcePerformSubstitutionAttributesForStylesheetIncludes()
        {
            const string template = @"@StylesheetResource RelativeResourceName=Test.css, PerformSubstitution=true";

            var attributes = GetGeneratedCustomAttributes(template, "DowJones.Web.StylesheetResourceAttribute");

            var performSubstitutionArgument =
                attributes.Single()
                    .Arguments.OfType<CodeAttributeArgument>()
                    .SingleOrDefault(x => x.Name == "PerformSubstitution");

            Assert.IsNotNull(performSubstitutionArgument);
            Assert.AreEqual(true, ((CodePrimitiveExpression)performSubstitutionArgument.Value).Value);
        }

    }
}