using System.Web;
using DowJones.Infrastructure;

namespace DowJones.Web.ClientResources
{
    public class DependentResourceProcessor : IClientResourceProcessor
    {
        public HttpContextBase HttpContext { get; set; }

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

            if (!resource.HasContent || !resource.HasClientTemplates)
                return;

            foreach (var dependentResource in resource.ClientTemplates)
            {
                resource.Content += dependentResource.Content;
            }
        }
    }
}

