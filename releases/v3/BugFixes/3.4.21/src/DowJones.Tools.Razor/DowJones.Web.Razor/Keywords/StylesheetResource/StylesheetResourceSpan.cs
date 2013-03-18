using System.Web.Razor.Text;
using DowJones.Web.Razor.ClientResources;

namespace DowJones.Web.Razor.Keywords.StylesheetResource
{
    public class StylesheetResourceSpan : ClientResourceSpan
    {
        public override string AttributeTypeName
        {
            get { return "DowJones.Web.StylesheetResourceAttribute"; }
        }

        public override string ResourceKind
        {
            get { return ClientResourceKind.Stylesheet.ToString(); }
        }

        public override string MimeType
        {
            get { return KnownMimeTypes.Stylesheet; }
            set { }
        }

        public StylesheetResourceSpan(SourceLocation location, string clientResourceLine)
            : base(location, clientResourceLine)
        {
        }
    }
}