namespace DowJones.Web
{
    public class ScriptResourceUniquenessProcessor : IClientResourceProcessor
    {
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

            resource.Content = string.Format(
                "if(!(window['LoadedScripts']=window['LoadedScripts']||{{}})['{0}']) {{ {1} \r\n window['LoadedScripts']['{0}']=true; }}",
                    resource.ClientResource.Name, resource.Content
            );
        }
    }
}
