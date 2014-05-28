using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using DowJones.Infrastructure;
using DowJones.Token;

namespace DowJones.Web
{
    public class ClientResourceTokenProcessor : IClientResourceProcessor
    {
        private static readonly Regex TokenExpression =
            new Regex(@"\<%\s*=\s*Token\s*\(\s*['""]([^'""]*)['""]\s*\)\s*%>",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        private static readonly Regex LocaleExpression =
            new Regex(@"\<%\s*=\s*Locale\s*\(\)\s*%>",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        private readonly ITokenRegistry _tokenRegistry;

        public HttpContextBase HttpContext { get; set; }

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

            resource.Content =
                LocaleExpression.Replace(
                    resource.Content,
                    ReplaceLocaleMatch
                );
        }

        private string ReplaceTokenMatch(Match tokenMatch)
        {
            var tokenName = tokenMatch.Groups[1].Value.Trim();

            var tokenValue = _tokenRegistry.Get(tokenName);

            return tokenValue;
        }

        private string ReplaceLocaleMatch(Match localeMatch)
        {
            return MapLanguageKey(Thread.CurrentThread.CurrentCulture);
        }

        public static string MapLanguageKey(CultureInfo culture)
        {
            switch (culture.ThreeLetterWindowsLanguageName)
            {
                case "CHT":
                    return "zhtw";
                case "CHS":
                    return "zhcn";
                default:
                    return culture.TwoLetterISOLanguageName;
            }
        }
    }
}