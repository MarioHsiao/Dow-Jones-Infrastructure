using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
	public class ClassBlock : AbstractBlock
	{
		public ClassBlock(string line) : base(line)
		{
			BlockStarter = new Starter();
			ParseLine(line);
		}

		private void ParseLine(string line)
		{
			var match = Regex.Match(line, BlockStarter.Pattern);
            Name = match.Groups["Name"].Value;
            Namespace = match.Groups["Namespace"].Value;
		}
		
        internal class Starter : IBlockStarter
        {
            const string PatternRegex = @"(\s*)?((?<Namespace>.*)\.)?(?<Name>[^\s]*)(\s*)?=(\s*)?([\w.]+).extend(\s*)?\((\s*)?{";

            public string Pattern { get { return PatternRegex; } }

            public PatternType PatternType { get { return PatternType.Class; } }

            public bool IsMatch(string line)
            {
                return Regex.IsMatch(line.Trim(), PatternRegex);
            }
        }
	}
}