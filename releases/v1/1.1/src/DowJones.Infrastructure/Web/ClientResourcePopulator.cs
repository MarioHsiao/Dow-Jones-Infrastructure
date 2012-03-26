using System;
using System.IO;
using System.Net;
using System.Web;
using DowJones.DependencyInjection;
using log4net;

namespace DowJones.Web
{
    public class ClientResourcePopulator : IClientResourceProcessor
    {
        private readonly HttpContextBase _httpContext;


        [Inject("Cross-cutting concern")]
        protected internal ILog Log { get; set; }


        public ClientResourceProcessorOrder? Order
        {
            get { return ClientResourceProcessorOrder.First; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Populator; }
        }


        public ClientResourcePopulator(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }


        public void Process(ProcessedClientResource resource)
        {
            var clientResource = resource.ClientResource;

            if (resource.ContentLoaded)
                return;

            Stream contentStream;

            if (clientResource is EmbeddedClientResource)
                contentStream = RetrieveInternalWebResourceStream(clientResource as EmbeddedClientResource);

            else if (clientResource.IsAbsoluteUrl())
                contentStream = RetrieveExternalWebResourceStream(clientResource);

            else
                contentStream = RetrieveLocalResourceStream(clientResource);


            if (contentStream == null || !contentStream.CanRead)
                throw new ClientResourcePopulationException(clientResource);

            string resourceContent;

            using (var reader = new StreamReader(contentStream))
            {
                resourceContent = reader.ReadToEnd();
            }

            resource.Content = resourceContent;
            resource.ContentLoaded = true;
        }

        protected internal virtual Stream RetrieveInternalWebResourceStream(EmbeddedClientResource embeddedClientResource)
        {
            Guard.IsNotNull(embeddedClientResource, "embeddedClientResource");

            Log.DebugFormat("Retrieving internal web resource {0}...", embeddedClientResource.Url);
            var stream = embeddedClientResource.TargetAssembly.GetManifestResourceStream(embeddedClientResource.ResourceName);

            return stream;
        }

        protected internal virtual Stream RetrieveExternalWebResourceStream(ClientResource clientResource)
        {
            Guard.IsNotNull(clientResource, "clientResource");

            Log.DebugFormat("Retrieving remote resource {0}...", clientResource.Url);

            WebRequest request = WebRequest.Create(clientResource.Url);

            var response = request.GetResponse();
            if (response == null || response.ContentLength > 0)
            {
                Log.Warn("Could not retrieve remote resource: " + clientResource.Url);
                return null;
            }

            var responseStream = response.GetResponseStream();

            if (responseStream == null)
                return null;

            return new BufferedStream(responseStream);
        }

        protected internal virtual Stream RetrieveLocalResourceStream(ClientResource clientResource)
        {
            Guard.IsNotNull(clientResource, "clientResource");

            Log.DebugFormat("Retrieving local resource {0}...", clientResource.Url);

            string relativePath;

            if (clientResource.IsAbsoluteUrl())
                relativePath = clientResource.Url;
            else
                relativePath = VirtualPathUtility.ToAbsolute(clientResource.Url);

            string filename = _httpContext.Server.MapPath(relativePath);

            var fileInfo = new FileInfo(filename);

            if (!fileInfo.Exists)
            {
                Log.WarnFormat("Local resource {0} does not exist", filename);
                return null;
            }

            return fileInfo.OpenRead();
        }


        public class ClientResourcePopulationException : Exception
        {
            public ClientResource ClientResource { get; private set; }

            public ClientResourcePopulationException(ClientResource resource)
                : base(GetMessage(resource))
            {
                ClientResource = resource;
            }

            private static string GetMessage(ClientResource resource)
            {
                if (resource == null)
                    return "Unknown Client Resource";

                return string.Format("Unable to retrieve content for {0} {1} ({2})",
                                     resource.GetType().Name, resource.Name, resource.Url);
            }
        }

    }
}
