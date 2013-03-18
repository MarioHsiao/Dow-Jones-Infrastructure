using System.Collections.Generic;
using System.Linq;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;

namespace DowJones.Web.Razor.Keywords.DependsOn
{
    public class DependsOnSpan : CodeSpan
    {
        public IEnumerable<string> Dependencies
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Content))
                    return Enumerable.Empty<string>();

                return Content.Split(',', ';').Select(x => x.Trim());
            }
        }

        public DependsOnSpan(SourceLocation location, string content)
            : base(location, content)
        {
        }
    }
}