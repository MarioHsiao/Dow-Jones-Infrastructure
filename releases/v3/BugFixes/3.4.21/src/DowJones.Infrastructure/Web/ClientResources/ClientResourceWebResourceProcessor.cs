using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.Extensions;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class ClientResourceWebResourceProcessor : IClientResourceProcessor
    {
        private static readonly Regex WebResourceExpression =
            new Regex(@"\<%\s*=\s*WebResource\s*\(\s*""(.*)""\s*\)\s*%>", 
                      RegexOptions.Compiled | RegexOptions.IgnoreCase | 
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        // Seam for replace non-test-friendly code
        internal static Func<Assembly, string, string> ResolveWebResourceUrlThunk =
            (assembly, resourceName) => assembly.GetWebResourceUrl(resourceName);


        public HttpContextBase HttpContext { get; set; }

        public ClientResourceProcessorOrder? Order
        {
            get { return null; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Preprocessor; }
        }

        public void Process(ProcessedClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");

            if (!resource.HasContent)
                return;

            resource.Content = 
                WebResourceExpression.Replace(
                    resource.Content, 
                    (match) => ReplaceWebResourceMatch(match, resource)
                );
        }

        private static string ReplaceWebResourceMatch(Match match, ProcessedClientResource resource)
        {
            var embeddedResource = resource.ClientResource as EmbeddedClientResource;
            
            if (embeddedResource == null)
                throw new NotSupportedException("WebResource replacement is only supported in Embedded Client Resources");

            var resourceName = match.Groups[1].Value.Trim();

            if (embeddedResource.TargetAssembly == null)
                throw new ApplicationException(string.Format("Embedded Client Resource target assembly is not specified: {0}", resource));

            return ResolveWebResourceUrlThunk(embeddedResource.TargetAssembly, resourceName);
        }
    }
}