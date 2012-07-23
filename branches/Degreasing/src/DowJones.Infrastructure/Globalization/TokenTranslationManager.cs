using System.Text.RegularExpressions;

namespace DowJones.Globalization
{
    public class TokenTranslationManager
    {
        private static readonly Regex oReg = new Regex(@"\$\{(?<1>[a-zA-Z0-9_-]+)\}", RegexOptions.Compiled);

        private readonly IResourceTextManager resourceManager;

        public TokenTranslationManager(IResourceTextManager resourceTextManager)
        {
            resourceManager = resourceTextManager;
        }

        public string Translate(string str)
        {
            return oReg.Replace(str, ReplaceEachToken);
        }

        public string GetAssignedToken(object value)
        {
            return resourceManager.GetAssignedToken(value);
        }

        private string ReplaceEachToken(Match m)
        {
            var tokenValue = resourceManager.GetString(m.Groups[1].Value) ?? "${" + m.Groups[1].Value + "}";
            return tokenValue;
        }
    }
}
