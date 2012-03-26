using System;
using System.Web.Mvc.Razor;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using DowJones.Web.Razor.ClientResources;
using DowJones.Web.Razor.Keywords.ClientPlugin;
using DowJones.Web.Razor.Keywords.DependsOn;
using DowJones.Web.Razor.Keywords.ScriptResource;

namespace DowJones.Web.Razor.Runtime
{
    public class ViewComponentCSharpRazorCodeGenerator : MvcCSharpRazorCodeGenerator
    {
        private ClientResourceAttributeDeclaration _lastScriptResource;

        public ViewComponentCSharpRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host) 
            : base(className, rootNamespaceName, sourceFileName, host)
        {
        }

        protected override bool TryVisitSpecialSpan(Span span)
        {
            return base.TryVisitSpecialSpan(span)
                   || TryVisit<ClientPluginSpan>(span, VisitClientPluginSpan)
                   || TryVisit<ClientResourceSpan>(span, VisitClientResourceSpan)
                   || TryVisit<DependsOnSpan>(span, VisitDependsOnSpan);
        }

        private void VisitClientPluginSpan(ClientPluginSpan span)
        {
            GeneratedClass.Members.Add(new ClientPluginNameProperty(span));
        }

        private void VisitClientResourceSpan(ClientResourceSpan span)
        {
            span.RootResourceNamespace = GeneratedNamespace.Name;

            var attribute = new ClientResourceAttributeDeclaration(span);

            if(!attribute.HasDeclaringAssembly)
            {
                attribute.EnsureDeclaringType(GeneratedNamespace.Name, GeneratedClass.Name);
            }

            GeneratedClass.CustomAttributes.Add(attribute);

            if (span is ScriptResourceSpan)
                _lastScriptResource = attribute;
            else
                _lastScriptResource = null;
        }

        private void VisitDependsOnSpan(DependsOnSpan span)
        {
            if(_lastScriptResource == null)
            {
                Console.Error.WriteLine("Skipping @DependsOn without a preceeding @ScriptResource");
                return;
            }

            _lastScriptResource.EnsureDependencies(span.Dependencies);
        }
    }
}
