using System;
using System.IO;
using System.Web.UI;

namespace DowJones.Web
{
    public class HtmlTextWriterRenderer
    {
        public string Render(Action<HtmlTextWriter> renderAction)
        {
            using (var output = new StringWriter())
            {
                var htmlTextWriter = new HtmlTextWriter(output);
                renderAction(htmlTextWriter);
                return output.ToString();
            }
        }
    }
}
