using DowJones.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web
{
    public class ProcessedClientResource
    {
        private readonly ClientResource _resource;

        public IEnumerable<ProcessedClientResource> ClientTemplates { get; set; }

        public ClientResource ClientResource
        {
            get { return _resource; }
        }

        public string Content { get; set; }

        public bool ContentLoaded { get; set; }

        public bool HasClientTemplates
        {
            get { return ClientTemplates.Any(); }
        }

        public virtual bool HasContent
        {
            get
            {
                return ContentLoaded
                    && !string.IsNullOrWhiteSpace(Content);
            }
        }

        public string MimeType { get; set; }

        public string Name
        {
            get
            {
                return (ClientResource == null) ? null : ClientResource.Name;
            }
        }


        public ProcessedClientResource(ClientResource resource, string content = null)
        {
            Guard.IsNotNull(resource, "resource");

            _resource = resource;

            Content = content;
            ContentLoaded = content != null;
            MimeType = resource.ResourceKind.GetMimeType();
            ClientTemplates = _resource.ClientTemplates.Select(r => new ProcessedClientResource(r));
        }

    }
}