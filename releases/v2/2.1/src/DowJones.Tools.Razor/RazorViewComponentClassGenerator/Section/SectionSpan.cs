using System.Web;
using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;

namespace DowJones.Web.Mvc.Razor.Section
{
    public class SectionSpan : CodeSpan
    {
        public SectionSpan(ParserContext context, string sectionName, IHtmlString content)
            : base(context.CurrentLocation, content.ToHtmlString())
        {
            SectionName = sectionName;
        }

        public string SectionName { get; set; }
    }
}
