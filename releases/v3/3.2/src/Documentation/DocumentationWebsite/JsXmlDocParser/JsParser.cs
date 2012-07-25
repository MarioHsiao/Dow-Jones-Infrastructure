using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JsXmlDocParser
{
	public class JsParser
	{
		protected ParseState CurrentState { get; private set; }

        public IEnumerable<IBlock> Parse(TextReader reader)
        {
            return Parse(new BufferedTextReader(reader));
        }

        internal IEnumerable<IBlock> Parse(BufferedTextReader reader)
		{
			CurrentState = new ParseState();
			var blocks = ReadBlocks(reader).ToList();
			
            // filter unwanted blocks
			blocks.ForEach(block => block.Children.RemoveAll(b => b.BlockStarter.PatternType == PatternType.Unknown));
			
            var knownBlocks = blocks.Where(b => b.BlockStarter.PatternType != PatternType.Unknown);
            return knownBlocks;
		}

		internal IEnumerable<IBlock> ReadBlocks(BufferedTextReader reader)
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                PatternType type;

				if (IsBlockStarter(line, out type))
				{
					CurrentState.BeginBlock(BlockFactory.CreateBlock(type, line));
				    var comments = ReadComments(reader).ToArray();
				    CurrentState.AddComments(comments);
				}
				else if (ShouldCloseBlock(line))
				{
					CurrentState.EndBlock();
				}
			}

			return CurrentState.Blocks;
		}

		private IEnumerable<string> ReadComments(BufferedTextReader reader)
		{
            string line;
            bool keepReading;
            do
		    {
		        line = reader.PeekLine();
		        if (keepReading = (IsEmptyLine(line) || IsXmlComment(line)))
		            yield return reader.ReadLine();
		    } while (keepReading);
		}

        private bool IsEmptyLine(string line)
		{
			return string.IsNullOrWhiteSpace(line);
		}

	    private bool IsComment(string line)
	    {
            return line.TrimStart().StartsWith(@"//");
	    }

	    private bool IsXmlComment(string line)
		{
			return line.TrimStart().StartsWith(@"///");
		}

	    private bool ShouldCloseBlock(string line)
		{
			return !IsComment(line) && line.Contains("}");
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

            if (IsComment(line))
            {
                type = PatternType.Comment;
                return false;
            }

			// not a known pattern
			type = PatternType.Unknown;

			// see if it starts an unrecognized block as we need to balance the braces
			return line.Contains("{");
		}
	}
}