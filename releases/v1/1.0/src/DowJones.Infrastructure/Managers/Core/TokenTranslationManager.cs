using System.Text.RegularExpressions;
using DowJones.Utilities.Managers.Core;

namespace DowJones.Managers.Core
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

        private string ReplaceEachToken(Match m)
        {
            var tokenValue = resourceManager.GetString(m.Groups[1].Value) ?? "${" + m.Groups[1].Value + "}";
            return tokenValue;
        }
    }
}
