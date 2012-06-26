using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace JsXmlDocParser
{
	public class JsParser
	{

		public IEnumerable<IBlock> ParseResults { get; private set; }

		protected ParseState CurrentState { get; private set; }

		public void Parse(JavaScriptReaderAdapter reader)
		{
			CurrentState = new ParseState();
			var blocks = ReadBlocks(reader).ToList();
			// filter unwanted blocks
			blocks.ForEach(block => block.Children.RemoveAll(b => b.BlockStarter.PatternType == PatternType.Unknown));
			ParseResults = blocks.Where(b => b.BlockStarter.PatternType != PatternType.Unknown);
		}

		protected IEnumerable<IBlock> ReadBlocks(JavaScriptReaderAdapter reader)
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
			return !string.IsNullOrWhiteSpace(line) && line.Trim().StartsWith("///");
		}

		private bool ShouldCloseBlock(string line)
		{
			return IsNotComment(line) && line.Contains("}");
		}

		private bool IsNotComment(string line)
		{
			return !line.Trim().StartsWith("//");
		}

		private bool IsBlockStarter(string line, out PatternType type)
		{
			var starters = new IBlockStarter[] {
				new ClassBlockStarter(), 
				new FunctionBlockStarter(), 
				new OptionsBlockStarter(),
				new EventsBlockStarter(), 
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
			return IsNotComment(line) && line.Trim().Contains("{");

		}

		public void Parse(StreamReader reader)
		{
			Parse(new JavaScriptReaderAdapter(reader));
		}
	}
}