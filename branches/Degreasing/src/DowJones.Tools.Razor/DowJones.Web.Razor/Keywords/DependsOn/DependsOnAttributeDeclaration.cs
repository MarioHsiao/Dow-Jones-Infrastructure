using System.CodeDom;
using System.Linq;

namespace DowJones.Web.Razor.Keywords.DependsOn
{
    internal class DependsOnAttributeDeclaration : CodeAttributeDeclaration
    {
        protected internal const string TypeName = "DowJones.Web.ClientResources.DependsOnAttribute";

        public DependsOnAttributeDeclaration(DependsOnSpan span)
            : base(new CodeTypeReference(TypeName))
        {
            CodePrimitiveExpression[] resources = 
                span.Dependencies
                    .Select(x => new CodePrimitiveExpression(x))
                    .ToArray();

            Arguments.Add(new CodeAttributeArgument(
                "Resources", 
                new CodeArrayCreateExpression(typeof (string[]), resources)));
        }
    }
}