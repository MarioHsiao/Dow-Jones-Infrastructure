using System.Text;
using System.Text.RegularExpressions;

namespace DowJones.Web.UI
{
    public class ClientTemplateParser
    {
        public const string DefaultFunctionTemplate = "function {0}(obj) {{ var __p=[],print=function(){{__p.push.apply(__p,arguments);}}; with(obj||{{}}){{__p.push('{1}');}}return __p.join(''); }}";
        public const string EvaluatePattern = @"<%([\s\S]+?)%>";
        public const string InterpolatePattern = @"<%=([\s\S]+?)%>";

        public string FunctionTemplate { get; set; }


        public ClientTemplateParser()
        {
            FunctionTemplate = DefaultFunctionTemplate;
        }

        /// <summary>
        /// Parses an client template written with the 
        /// underscore client template syntax
        /// </summary>
        /// <remarks>
        /// The <param name="functionName" /> parameter is optional.
        /// If not provided, the function will return an anonymous
        /// function that can be assigned to a variable.
        /// </remarks>
        /// <returns>
        /// A parsed client template. This is a JavaScript function in the format:
        /// function [functionName](context) { /* template */ }
        /// </returns>
        public string Parse(string html, string functionName = null)
        {
            var phase1 =
                new StringBuilder(html)
                    .Replace(@"\", @"\\")
                    .Replace("'", "\\'")
                    .ToString();

            var phase2 = Regex.Replace(phase1, InterpolatePattern, Interpolate);

            var phase3 = Regex.Replace(phase2, EvaluatePattern, Evaluate);

            var phase4 =
                 new StringBuilder(phase3)
                     .Replace("\r", "\\r")
                     .Replace("\n", "\\n")
                     .Replace("\t", "\\t")
                     .ToString();

            return string.Format(FunctionTemplate, functionName, phase4);
        }

        private static string Interpolate(Match m)
        {
            string retVal = "'," + m.Groups[1].Captures[0].Value.Replace("\\'", "'") + ",'";
            return retVal;
        }

        private static string Evaluate(Match m)
        {
            string retVal = m.Groups[1].Captures[0].Value.Replace("\\'", "'");
            retVal = "');" + Regex.Replace(retVal, "[\r\n\t]", " ") + "__p.push('";
            return retVal;
        }
    }
}
