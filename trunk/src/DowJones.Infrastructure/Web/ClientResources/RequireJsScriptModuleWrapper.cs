using System.Linq;
using DowJones.Infrastructure;
using DowJones.Web.ClientResources;

namespace DowJones.Web
{
    public class RequireJsScriptModuleWrapper : IClientResourceProcessor
    {
        private static readonly string[] GlobalDependencies = new[] { "$", "$dj", "_", "JSON" };


        public ClientResourceProcessorOrder? Order
        {
            get { return ClientResourceProcessorOrder.Last; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Postprocessor; }
        }

        public void Process(ProcessedClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");

            if (resource.ClientResource.ResourceKind != ClientResourceKind.Script)
                return;

            if (!resource.HasContent)
                return;

            if (resource.ClientResource.DependencyLevel == ClientResourceDependencyLevel.Core)
                return;

            if (resource.ClientResource.DependencyLevel == ClientResourceDependencyLevel.Independent)
                return;

            var resourceName = resource.ClientResource.Name;

            var dependencies = resource.ClientResource.DependsOn ?? Enumerable.Empty<string>();

            dependencies = GlobalDependencies.Union(dependencies);
            
            resource.Content = string.Format(
                "DJ.$dj.define('{0}', {1}, function($, $dj, _, JSON) {{\r\n {2} \r\n}});",
                    new ClientResourceModuleName(resourceName), 
                    string.Format("['{0}']", string.Join("','", dependencies)), 
                    resource.Content
            );
        }
    }
}
