using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
	public class FieldBlock : AbstractBlock
	{
		public FieldBlock(string line) : base(line)
		{
			BlockStarter = new Starter();
		    var match = Regex.Match(line, BlockStarter.Pattern);
		    Name = match.Groups["Name"].Value;
		}

        internal class Starter : IBlockStarter
        {
            private const string PatternRegex = @"(?<Name>[^\s:]*)\s*:\s*({|"")";

            public string Pattern { get { return PatternRegex; } }

            public PatternType PatternType { get { return PatternType.Field; } }

            public bool IsMatch(string line)
            {
                return Regex.IsMatch(line, PatternRegex);
            }
        }
	}
}