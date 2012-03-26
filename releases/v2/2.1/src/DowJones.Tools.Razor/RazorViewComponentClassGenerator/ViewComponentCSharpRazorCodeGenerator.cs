using System.Web.Mvc.Razor;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using DowJones.Web.Mvc.Razor.ClientPluginName;
using DowJones.Web.Mvc.Razor.ClientResources;
using DowJones.Web.Mvc.Razor.ClientResources.Spans;

namespace DowJones.Web.Mvc.Razor
{
    public class ViewComponentCSharpRazorCodeGenerator : MvcCSharpRazorCodeGenerator
    {
        public ViewComponentCSharpRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host) 
            : base(className, rootNamespaceName, sourceFileName, host)
        {
        }

        protected override bool TryVisitSpecialSpan(Span span)
        {
            return base.TryVisitSpecialSpan(span)
                   || TryVisit<ClientPluginSpan>(span, VisitClientPluginSpan)
                   || TryVisit<ClientResourceSpan>(span, VisitClientResourceSpan);
        }

        private void VisitClientPluginSpan(ClientPluginSpan span)
        {
            GeneratedClass.Members.Add(new ClientPluginNameProperty(span));
        }

        private void VisitClientResourceSpan(ClientResourceSpan span)
        {
            span.RootResourceNamespace = GeneratedNamespace.Name;

            var customAttributeDeclaration = new ClientResourceAttributeDeclaration(span);

            if(!customAttributeDeclaration.HasDeclaringType &&
               !customAttributeDeclaration.HasDeclaringAssembly)
            {
                    string declaringTypeName = string.Format("{0}.{1}", GeneratedNamespace.Name, GeneratedClass.Name);
                    customAttributeDeclaration.EnsureDeclaringType(declaringTypeName);

            }

            GeneratedClass.CustomAttributes.Add(customAttributeDeclaration);
        }
    }
}
