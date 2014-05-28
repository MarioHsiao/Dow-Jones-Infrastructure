using System.Web.Razor.Text;
using DowJones.Web.Razor.ClientResources;

namespace DowJones.Web.Razor.Keywords.ScriptResource
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