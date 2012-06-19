using System;
using System.Collections.Generic;

namespace JsXmlDocParser
{
	public class BlockFactory
	{
		public static IBlock CreateBlock(PatternType type, string line)
		{
			switch (type)
			{
				case PatternType.Function:
					return new FunctionBlock(line);
				case PatternType.Class:
					return new ClassBlock(line);
				case PatternType.Events:
					return new EventsBlock(line);
				case PatternType.Options:
					return new OptionsBlock(line);
				default:
					return new UnknownBlock(line);
			}
		}
	}
}