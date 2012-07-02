using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
	public class UnknownBlock : IBlock
	{
		public UnknownBlock(string line)
		{
			BlockStarter = new Starter();
			Children = new List<IBlock>();
			Comments = new List<string>();
		}
		
		public IBlockStarter BlockStarter { get; private set; }
	    public string Name { get; private set; }
	    public List<IBlock> Children { get; private set; }
		public List<string> Comments { get; private set; }

        internal class Starter : IBlockStarter
        {
            private const string PatternRegex = @"{";

            public string Pattern { get { return PatternRegex; } }
            
            public PatternType PatternType { get { return PatternType.Unknown; } }

            public bool IsMatch(string line)
            {
                return Regex.IsMatch(line.Trim(), PatternRegex);
            }
        }
	}
}