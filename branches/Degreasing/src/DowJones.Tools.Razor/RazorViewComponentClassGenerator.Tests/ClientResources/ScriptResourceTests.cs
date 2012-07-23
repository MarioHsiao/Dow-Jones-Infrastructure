using System.CodeDom;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    [TestClass]
    public class ScriptResourceTests : RazorViewComponentClassGeneratorTestFixture
    {

        [TestMethod]
        public void ShouldGenerateScriptResourceAttributesForScriptIncludes()
        {
            const string template = @"
                @ScriptResource RelativeResourceName=Test.js
                Hello, World!
            ";

            GenerateCodeFromTemplateText(template);

            int scriptResourcesCount =
                GetGeneratedCustomAttributes(template, "DowJones.Web.ScriptResourceAttribute")
                    .Count();

            Assert.AreEqual(1, scriptResourcesCount);
        }

        [TestMethod]
        public void ShouldGenerateWebResourceAttributesForScriptIncludes()
        {
            const string template = @"
                @ScriptResource RelativeResourceName=Test.js
                Hello, World!
            ";

            var generatorResults = Generator.GenerateRazorTemplate(template);

            var webResourceAttributesCount =
                generatorResults.GeneratedCode
                    .AssemblyCustomAttributes
                    .Cast<CodeAttributeDeclaration>()
                    .Count(x => x.Name.EndsWith("WebResourceAttribute"));

            Assert.AreEqual(1, webResourceAttributesCount);
        }

        [TestMethod]
        public void ShouldNotGenerateWebResourceAttributesForAbsoluteResources()
        {
            const string template = @"
                @ScriptResource ResourceName=DowJones.Blah.Dummy.SomeControl.AwesomeScript.js,System.Web.Razor
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

        [TestMethod]
        public void ShouldGenerateDeclaringAssemblyAttributesForAbsoluteResources()
        {
            const string template = @"
                @ScriptResource ResourceName=DowJones.Blah.Dummy.SomeControl.AwesomeScript.js, DeclaringAssembly=System.Web.Razor
                Hello, World!
            ";

            var generatorResults = Generator.GenerateRazorTemplate(template);

            var assemblyAttributes =
                generatorResults.GeneratedCode.AssemblyCustomAttributes.Cast<CodeAttributeDeclaration>();

            int declaringAssemblyCount =
                 GetGeneratedCustomAttributes(template, "DowJones.Web.ScriptResourceAttribute", "DeclaringAssembly")
                     .Count();


            Assert.AreEqual(1, declaringAssemblyCount);
        }

    }
}