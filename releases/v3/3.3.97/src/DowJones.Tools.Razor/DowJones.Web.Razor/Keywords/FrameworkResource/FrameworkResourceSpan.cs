using System;
using System.Web.Razor.Text;
using DowJones.Web.Razor.ClientResources;
using DowJones.Web.Razor.Runtime;

namespace DowJones.Web.Razor.Keywords.FrameworkResource
{
    public class FrameworkResourceSpan : ClientResourceSpan
    {
        public override string AttributeTypeName
        {
            get { return "DowJones.Web.FrameworkResourceAttribute"; }
        }

        public override string DeclaringType
        {
            get { return null; }
        }

        public override string MimeType
        {
            get
            {
                if (base.MimeType != null || string.IsNullOrWhiteSpace(ResourceKind))
                    return base.MimeType;

                ClientResourceKind resourceKind;

                if(Enum.TryParse(ResourceKind, true, out resourceKind))
                    return resourceKind.GetMimeType();
                else
                    return KnownMimeTypes.Content;
            }
            set { base.MimeType = value; }
        }


        public FrameworkResourceSpan(SourceLocation location, string clientResourceLine)
            : base(location, clientResourceLine)
        {
            if (string.IsNullOrWhiteSpace(ResourceName))
                throw new RazorViewParseException("FrameworkResource declarations must include a ResourceName");
        }
    }
}