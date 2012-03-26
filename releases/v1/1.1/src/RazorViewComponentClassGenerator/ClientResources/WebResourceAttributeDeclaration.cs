using System.CodeDom;
using System.Web.UI;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    public class WebResourceAttributeDeclaration : CodeAttributeDeclaration
    {
        public WebResourceAttributeDeclaration(string resourceName, string mimeType, bool performSubstitution)
            : base(new CodeTypeReference(typeof(WebResourceAttribute)))
        {
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(resourceName)));
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(mimeType)));
            
            if(performSubstitution)
                Arguments.Add(new CodeAttributeArgument("PerformSubstitution", new CodePrimitiveExpression(true)));
        }
    }
}