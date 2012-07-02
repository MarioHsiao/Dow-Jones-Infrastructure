using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace JsXmlDocParser
{
	public class JsParser
	{
		protected ParseState CurrentState { get; private set; }

        public IEnumerable<IBlock> Parse(TextReader reader)
        {
            return Parse(new JavaScriptReaderAdapter(reader));
        }

        internal IEnumerable<IBlock> Parse(JavaScriptReaderAdapter reader)
		{
			CurrentState = new ParseState();
			var blocks = ReadBlocks(reader).ToList();
			
            // filter unwanted blocks
			blocks.ForEach(block => block.Children.RemoveAll(b => b.BlockStarter.PatternType == PatternType.Unknown));
			
            var knownBlocks = blocks.Where(b => b.BlockStarter.PatternType != PatternType.Unknown);
            return knownBlocks;
		}

		internal IEnumerable<IBlock> ReadBlocks(JavaScriptReaderAdapter reader)
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				PatternType type;

				if (IsBlockStarter(line, out type))
				{
					CurrentState.BeginBlock(BlockFactory.CreateBlock(type, line));
					CurrentState.AddComments(ReadComments(reader));
				}
				else if (ShouldCloseBlock(line))
				{
					CurrentState.EndBlock();
				}
			}

			return CurrentState.Blocks;

		}

		private IEnumerable<string> ReadComments(JavaScriptReaderAdapter reader)
		{
			var comments = new List<string>();
			while (true)
			{
				var line = reader.PeekLine();
				if (IsNonEmptyLine(line))
				{
					if (IsXmlComment(line))
						comments.Add(reader.ReadLine().TrimStart(" /".ToCharArray()));
					else
						break;
				}
				else
					reader.ReadLine();	// empty line, just advance to next
			}

			return comments;
		}

		private bool IsNonEmptyLine(string line)
		{
			return !string.IsNullOrWhiteSpace(line);
		}

		private bool IsXmlComment(string line)
		{
			return Regex.IsMatch(line, @"\s*///");
		}

		private bool ShouldCloseBlock(string line)
		{
			return IsNotComment(line) && line.Contains("}");
		}

		private bool IsNotComment(string line)
		{
            return !Regex.IsMatch(line, @"\s*//");
		}

		private bool IsBlockStarter(string line, out PatternType type)
		{
			var starters = new IBlockStarter[] {
				new ClassBlock.Starter(), 
				new FunctionBlock.Starter(), 
				new FieldBlock.Starter(),
			};

			var blockStarter = starters.FirstOrDefault(b => b.IsMatch(line));

			if (blockStarter != null)
			{
				type = blockStarter.PatternType;
				return true;
			}

			// not a known pattern
			type = PatternType.Unknown;

			// see if it starts an unrecognized block as we need to balance the braces
			return IsNotComment(line) && line.Contains("{");
		}
	}
}