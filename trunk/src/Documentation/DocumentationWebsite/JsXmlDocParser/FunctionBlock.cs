using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
    public class FunctionBlock : AbstractBlock
	{
		public string Signature { get; private set; }
        public IEnumerable<string> Parameters { get; private set; }

		public FunctionBlock(string line) : base(line)
		{
			BlockStarter = new Starter();
			ParseDeclaration(line);
		}

		private void ParseDeclaration(string line)
		{
			var match = Regex.Match(line, BlockStarter.Pattern);
            Name = string.IsNullOrWhiteSpace(match.Groups["Name"].Value) ? match.Groups["Name1"].Value : match.Groups["Name"].Value;
            Namespace = match.Groups["Namespace"].Value;
			Parameters = ExtractParamemters(match);
			Signature = CreateSignature(Name, Parameters);
		}

        private IEnumerable<string> ExtractParamemters(Match match)
        {
            var parametersValue = match.Groups["Parameters"].Value;
            var parameters = parametersValue.Split(',');
            return parameters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim());
        }

        private string CreateSignature(string name, IEnumerable<string> parameters)
        {
            return string.Format("{0}({1})", name, string.Join(", ", parameters.Select(x => x)));
        }


        internal class Starter : IBlockStarter
        {
            private const string PatternRegex = @"(?:\s*(?<Name>[^=|]+)\s*(?::|=)\s*function|function\s*(?<Name1>[^\s{]+))\s*\((?<Parameters>[^)]*)\)\s*{";

            public string Pattern { get { return PatternRegex; } }

            public PatternType PatternType { get { return PatternType.Function; } }

            public bool IsMatch(string line)
            {
                return Regex.IsMatch(line, PatternRegex);
            }

        }
	}
}