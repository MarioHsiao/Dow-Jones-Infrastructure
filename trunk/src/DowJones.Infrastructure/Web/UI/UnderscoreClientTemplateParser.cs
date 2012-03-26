using System.Text;
using System.Text.RegularExpressions;

namespace DowJones.Web.UI
{
    public class UnderscoreClientTemplateParser : IClientTemplateParser
    {
        private const string DefaultFunctionTemplate = "function {0}(obj) {{ var __p=[],print=function(){{__p.push.apply(__p,arguments);}}; with(obj||{{}}){{__p.push('{1}');}}return __p.join(''); }}";
        private const string DefaultEvaluatePattern = @"<%([\s\S]+?)%>";
        private const string DefaultInterpolatePattern = @"<%=([\s\S]+?)%>";

        public string FunctionTemplate { get; set; }

        public UnderscoreClientTemplateParser()
        {
            FunctionTemplate = DefaultFunctionTemplate;
            EvaluatePattern = DefaultEvaluatePattern;
            InterpolatePattern = DefaultInterpolatePattern;
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

        #region IClientTemplateParser Members

        public string EvaluatePattern { get; set; }

        public string InterpolatePattern { get; set; }

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

        #endregion
    }
}
