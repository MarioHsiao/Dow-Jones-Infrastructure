using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;

namespace DowJones.Web.Mvc.Razor.ClientPluginName
{
    public class ClientPluginSpan : CodeSpan
    {
        public string ClientPluginName
        {
            get { return Content; }
            set { Content = value; }
        }

        public ClientPluginSpan(ParserContext context, string content)
            : base(context.CurrentLocation, content)
        {
        }
    }
}