using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;

namespace DowJones.Web.Razor.Keywords.ClientPlugin
{
    public class ClientPluginSpan : CodeSpan
    {
        public string ClientPluginName
        {
            get { return Content; }
            set { Content = value; }
        }

        public ClientPluginSpan(SourceLocation location, string content)
            : base(location, content)
        {
        }
    }
}