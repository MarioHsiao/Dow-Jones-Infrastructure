namespace DowJones.Web
{
    public class ProcessedClientResource
    {
        private readonly ClientResource _resource;


        public string Content { get; set; }

        public bool ContentLoaded { get; set; }

        public virtual bool HasContent
        {
            get
            {
                return ContentLoaded
                    && !string.IsNullOrWhiteSpace(Content);
            }
        }

        public string MimeType { get; set; }

        public ClientResource ClientResource
        {
            get { return _resource; }
        }


        public ProcessedClientResource(ClientResource resource, string content = null)
        {
            Guard.IsNotNull(resource, "resource");

            _resource = resource;

            Content = content;
            ContentLoaded = content != null;
            MimeType = resource.ResourceKind.GetMimeType();
        }
 
    }
}