using System.Text.RegularExpressions;
using DowJones.Token;

namespace DowJones.Web
{
    public class ClientResourceTokenProcessor : IClientResourceProcessor
    {
        private static readonly Regex TokenExpression =
            new Regex(@"\<%\s*=\s*Token\s*\(\s*['""]([^'""]*)['""]\s*\)\s*%>",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        private readonly ITokenRegistry _tokenRegistry;

        public ClientResourceProcessorOrder? Order
        {
            get { return null; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Preprocessor; }
        }
        
        public ClientResourceTokenProcessor(ITokenRegistry tokenRegistry)
        {
            _tokenRegistry = tokenRegistry;
        }

        public void Process(ProcessedClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");

            if (!resource.HasContent)
                return;

            resource.Content = 
                TokenExpression.Replace(
                    resource.Content, 
                    ReplaceTokenMatch
                );
        }

        private string ReplaceTokenMatch(Match tokenMatch)
        {
            var tokenName = tokenMatch.Groups[1].Value.Trim();

            var tokenValue = _tokenRegistry.Get(tokenName);

            return tokenValue;
        }
    }
}