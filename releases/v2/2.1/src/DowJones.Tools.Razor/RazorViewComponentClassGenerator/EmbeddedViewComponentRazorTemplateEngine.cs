using System.Collections.Generic;
using System.Linq;
using System.Web.Razor;
using DowJones.Web.Mvc.Razor.ClientResources;
using DowJones.Web.Mvc.Razor.ClientResources.Spans;

namespace DowJones.Web.Mvc.Razor
{
    public class EmbeddedViewComponentRazorTemplateEngine : RazorTemplateEngine
    {
        public EmbeddedViewComponentRazorTemplateEngine(RazorEngineHost host) 
            : base(host)
        {
        }

        protected override GeneratorResults GenerateCodeCore(System.Web.Razor.Text.LookaheadTextReader input, string className, string rootNamespace, string sourceFileName, System.Threading.CancellationToken? cancelToken)
        {
            var results = base.GenerateCodeCore(input, className, rootNamespace, sourceFileName, cancelToken);

            if (results.Success)
                GenerateAssemblyLevelAttributes(results);

            return results;
        }

        private static void GenerateAssemblyLevelAttributes(GeneratorResults results)
        {
            IEnumerable<ClientResourceSpan> clientResourceSpans = 
                results.Document
                    .Flatten()
                    .OfType<ClientResourceSpan>()
                    .Where(x => !string.IsNullOrWhiteSpace(x.ResourceName))
                    .Where(x => !(x is FrameworkResourceSpan))
                    .Where(x => !string.IsNullOrWhiteSpace(x.RelativeResourceName)) ;

            IEnumerable<WebResourceAttributeDeclaration> webResourceAttributes =
                clientResourceSpans
                    .Select(x => new WebResourceAttributeDeclaration(x.ResourceName, x.MimeType, x.PerformSubstitution));

            results.GeneratedCode.AssemblyCustomAttributes.AddRange(webResourceAttributes.ToArray());
        }
    }
}
