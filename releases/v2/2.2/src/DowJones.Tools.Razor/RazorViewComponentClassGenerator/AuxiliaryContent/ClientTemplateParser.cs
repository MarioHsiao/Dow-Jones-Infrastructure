using System.Text.RegularExpressions;

namespace DowJones.Web.Mvc.Razor
{
    public class ClientTemplateParser
    {
        public class Patterns
        {
            public const string Evaluate = @"<%([\s\S]+?)%>";
            public const string Interpolate = @"<%=([\s\S]+?)%>";
        }

        const string FunctionTemplate = "function {1}(obj) {{ var __p=[],print=function(){{__p.push.apply(__p,arguments);}}; with(obj||{{}}){{__p.push('{0}');}}return __p.join(''); }}";

        public string Parse(string name, string html)
        {
            string parsedHtml = 
                html.Replace(@"\", @"\\")
                    .Replace("'", "\'")
                    .Replace("\r", "\\r")
                    .Replace("\n", "\\n")
                    .Replace("\t", "\\t")
                    .ReplaceWith(Patterns.Interpolate, new MatchEvaluator(m => "'," + m.Groups[1].Captures[0].Value.Replace("\'", "'") + ",'"))
                    .ReplaceWith(Patterns.Evaluate, new MatchEvaluator(m => "');" + m.Groups[1].Captures[0].Value.Replace("\'", "'").ReplaceWith("[\r\n\t]", " ") + "__p.push('"));

            return string.Format(FunctionTemplate, parsedHtml, name);
        }

    }



    static class StringExtensions
    {
        public static string ReplaceWith(this string value, string regexPattern, MatchEvaluator evaluator)
        {
            return Regex.Replace(value, regexPattern, evaluator, RegexOptions.None);
        }
        public static string ReplaceWith(this string value, string regexPattern, string replacement)
        {
            return Regex.Replace(value, regexPattern, replacement, RegexOptions.None);
        }
    }
}
