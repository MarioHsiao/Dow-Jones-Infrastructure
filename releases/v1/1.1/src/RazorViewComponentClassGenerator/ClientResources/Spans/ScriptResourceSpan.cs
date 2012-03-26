using System.Web.Razor.Text;

namespace DowJones.Web.Mvc.Razor.ClientResources.Spans
{
    public class ScriptResourceSpan : ClientResourceSpan
    {
        public override string AttributeTypeName
        {
            get { return "DowJones.Web.ScriptResourceAttribute"; }
        }

        public override string MimeType
        {
            get { return KnownMimeTypes.JavaScript; }
            set { }
        }

        public override string ResourceKind
        {
            get { return ClientResourceKind.Script.ToString(); }
        }


        public ScriptResourceSpan(SourceLocation location, string clientResourceLine)
            : base(location, clientResourceLine)
        {
        }
    }
}