using System;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.Extensions.Web;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class ClientResourceUrlProcessor : IClientResourceProcessor
    {
        private static readonly Regex UrlPreprocessorExpression =
            new Regex(@"\<%\s*=\s*(?<Kind>Absolute|Relative)Url\s*\(\s*""(?<Url>.*)""\s*\)\s*%>",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        internal static Func<string, HttpRequestBase, string> AbsoluteUrlThunk = UrlExtensions.ToAbsoluteUrl;

        internal static Func<string, string> RelativeUrlThunk = VirtualPathUtility.ToAbsolute;
        
        public HttpContextBase HttpContext { get; set; }

        public ClientResourceProcessorOrder? Order
        {
            get { return ClientResourceProcessorOrder.First; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Preprocessor; }
        }
        
        public void Process(ProcessedClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");

            if (!resource.HasContent)
                return;

            resource.Content = UrlPreprocessorExpression.Replace(resource.Content, ReplaceAbsoluteUrlMatch);
        }
        private string ReplaceAbsoluteUrlMatch(Match match)
        {
            var kind = (UrlKind)Enum.Parse(typeof(UrlKind), match.Groups["Kind"].Value.Trim());
            var url = match.Groups["Url"].Value.Trim();

            if(string.IsNullOrWhiteSpace(url))
                return string.Empty;

            return kind == UrlKind.Relative ? RelativeUrlThunk(url) : AbsoluteUrlThunk(url, HttpContext.Request);
        }
        
        enum UrlKind
        {
            Absolute,
            Relative,
        }
    }
}