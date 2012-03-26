using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.UI
{
    public interface IClientTemplateParser
    {
        string EvaluatePattern { get; set; }
        string InterpolatePattern { get; set; } 

        string Parse(string html, string functionName = null);
    }
}
