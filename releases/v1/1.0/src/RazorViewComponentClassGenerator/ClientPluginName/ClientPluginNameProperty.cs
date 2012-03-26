using System.CodeDom;

namespace DowJones.Web.Mvc.Razor.ClientPluginName
{
    public class ClientPluginNameProperty : CodeMemberProperty
    {
        public const string PropertyName = "ClientPluginName";

        public ClientPluginNameProperty(ClientPluginSpan span)
            : this(span.ClientPluginName)
        {
        }

        public ClientPluginNameProperty(string value)
        {
            Attributes = MemberAttributes.Public | MemberAttributes.Override;
            HasSet = false;
            Type = new CodeTypeReference(typeof(string));
            Name = PropertyName;

            var returnValue = new CodePrimitiveExpression(value);
            GetStatements.Add(new CodeMethodReturnStatement(returnValue));
        }
    }
}
