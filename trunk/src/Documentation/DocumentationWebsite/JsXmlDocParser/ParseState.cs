using System.Collections.Generic;
using System.Linq;

namespace JsXmlDocParser
{
	public class ParseState
	{
		readonly Stack<IBlock> _blockStack;

		public List<IBlock> Blocks { get; private set; }
		
		public void BeginBlock(IBlock block)
		{
			_blockStack.Push(block);
		}

		public void EndBlock()
		{
			var block = _blockStack.Pop();
			if (_blockStack.Any())
				_blockStack.Last().Children.Add(block);
			else
				Blocks.Add(block);
		}

		public ParseState()
		{
			Blocks = new List<IBlock>();
			_blockStack = new Stack<IBlock>();
		}

		public void AddComments(IEnumerable<string> comments)
		{
			_blockStack.Peek().Comments.AddRange(comments);
		}
	}
}