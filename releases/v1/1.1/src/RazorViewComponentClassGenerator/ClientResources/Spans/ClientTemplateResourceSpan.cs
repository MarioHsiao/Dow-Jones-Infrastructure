using System.Web.Razor.Text;

namespace DowJones.Web.Mvc.Razor.ClientResources.Spans
{
    public class ClientTemplateResourceSpan : ClientResourceSpan
    {
        public override string AttributeTypeName
        {
            get { return "DowJones.Web.ClientTemplateResourceAttribute"; }
        }

        public override string MimeType
        {
            get { return KnownMimeTypes.Html; }
            set { }
        }

        public override string ResourceKind
        {
            get { return ClientResourceKind.ClientTemplate.ToString(); }
        }

        public string SectionName
        {
            get { return GetContentPropertyValue("Section"); }
        }

        public string TemplateId
        {
            get
            {
                string templateId = GetContentPropertyValue("TemplateId");
                return !string.IsNullOrWhiteSpace(templateId) ? templateId : Url;
            }
        }

        public override string Url
        {
            // External client templates are never allowed
            get { return null; }
        }

        public ClientTemplateResourceSpan(SourceLocation location, string clientResourceLine)
            : base(location, clientResourceLine)
        {
            
        }
    }
}