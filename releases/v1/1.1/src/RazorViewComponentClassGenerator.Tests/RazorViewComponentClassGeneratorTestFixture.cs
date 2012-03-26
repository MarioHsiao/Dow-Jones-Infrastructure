using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor
{
    public class RazorViewComponentClassGeneratorTestFixture
    {
        private RazorViewComponentTemplateGenerator _generator;
        protected const string ExpectedNamespace = "TestNamespace";

        protected RazorViewComponentTemplateGenerator Generator
        {
            get { return _generator; }
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _generator = new RazorViewComponentTemplateGenerator("~/Test.cshtml", @"C:\Temp", ExpectedNamespace);
        }

        protected string GenerateCodeFromTemplateText(string template)
        {
            var generatorResults = Generator.GenerateRazorTemplate(template);
            var generatedClass = Generator.RenderCode(generatorResults, new CSharpCodeProvider());

            return generatedClass;
        }

        protected IEnumerable<CodeAttributeDeclaration> GetGeneratedCustomAttributes(string template, string attributeName = null)
        {
            CodeTypeDeclaration generatedType = GetGeneratedTypeFromTemplateText(template);

            var customAttributes = generatedType.CustomAttributes.Cast<CodeAttributeDeclaration>();

            if (attributeName != null)
                customAttributes = customAttributes.Where(x => x.Name == attributeName);

            return customAttributes;
        }

        protected IEnumerable<CodeAttributeDeclaration> GetGeneratedCustomAttributes(string template, string attributeName, string argumentName)
        {
            // return an empty collection so that the caller can check for count
            if (string.IsNullOrWhiteSpace(attributeName) || string.IsNullOrWhiteSpace(argumentName)) return Enumerable.Empty<CodeAttributeDeclaration>();

            CodeTypeDeclaration generatedType = GetGeneratedTypeFromTemplateText(template);

            var customAttributes = generatedType.CustomAttributes.Cast<CodeAttributeDeclaration>();

            var args = from attribute in customAttributes
                       from pArg in attribute.Arguments.Cast<CodeAttributeArgument>()
                       where pArg.Name == argumentName
                       select attribute;

            return args;
        }

        private CodeTypeDeclaration GetGeneratedTypeFromTemplateText(string template)
        {
            var generatorResults = Generator.GenerateRazorTemplate(template);
            // just do a test render to see if the code renders fine
            Generator.RenderCode(generatorResults, new CSharpCodeProvider());
            return generatorResults.GeneratedCode.Namespaces[0].Types[0];
        }
    }
}