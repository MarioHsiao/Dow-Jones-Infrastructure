using System.CodeDom;
using System.Linq;
using DowJones.Web.Razor.ClientResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    [TestClass]
    public class DependsOnKeywordTests : RazorViewComponentClassGeneratorTestFixture
    {

        [TestMethod]
        public void ShouldPopulateTheDependsOnArgumentOfThePreviousScriptResource()
        {
            string[] expectedResources = new [] { "hope", "dreams", "luck", "jquery" };
            
            string template = @"
                @ScriptResource Name=Awesome
                @DependsOn " + string.Join(", ", expectedResources) + @"
                Hello, World!
            ";

            var scriptAttribute = GetGeneratedCustomAttributes(template).Single() as ClientResourceAttributeDeclaration;

            var attributeResourcesArray = 
                scriptAttribute.GetArgument(ClientResourceAttributeDeclaration.DependsOnAttributeName).Value as CodeArrayCreateExpression;
            
            var actualResources =
                attributeResourcesArray.Initializers
                    .Cast<CodePrimitiveExpression>()
                    .Select(x => x.Value as string)
                    .ToArray();


            CollectionAssert.AreEquivalent(expectedResources, actualResources);
        }

        [TestMethod]
        public void ShouldNotPopulateTheDependsOnArgumentOfThePreviousScriptResourceWhenTheResourceAlreadySpecifiesItsDependencies()
        {
            string[] expectedResources = new [] { "hope", "dreams", "luck", "jquery" };
            
            string template = @"
                @ScriptResource Name=Awesome, DependsOn=" + string.Join(";", expectedResources) + @"
                @DependsOn something else
                Hello, World!
            ";

            var scriptAttribute = GetGeneratedCustomAttributes(template).Single() as ClientResourceAttributeDeclaration;

            var attributeResourcesArray = 
                scriptAttribute.GetArgument(ClientResourceAttributeDeclaration.DependsOnAttributeName).Value as CodeArrayCreateExpression;
            
            var actualResources =
                attributeResourcesArray.Initializers
                    .Cast<CodePrimitiveExpression>()
                    .Select(x => x.Value as string)
                    .ToArray();


            CollectionAssert.AreEquivalent(expectedResources, actualResources);
        }

    }
}
