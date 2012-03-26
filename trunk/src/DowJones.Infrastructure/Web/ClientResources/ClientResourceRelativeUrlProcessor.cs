using System;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class ClientResourceRelativeUrlProcessor : IClientResourceProcessor
    {

        private static readonly Regex AbsoluteUrlExpression =
            new Regex(@"\<%\s*=\s*AbsoluteUrl\s*\(\s*""(.*)""\s*\)\s*%>",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        internal static Func<string, string> AbsoluteUrlThunk = VirtualPathUtility.ToAbsolute;


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

            resource.Content = AbsoluteUrlExpression.Replace(resource.Content, ReplaceAbsoluteUrlMatch);
        }
        private string ReplaceAbsoluteUrlMatch(Match match)
        {
            var relativeUrl = match.Groups[1].Value.Trim();

            var absoluteUrl = AbsoluteUrlThunk(relativeUrl);

            return absoluteUrl;
        }
    }
}