namespace JsXmlDocParser
{
	public class BlockFactory
	{
		public static IBlock CreateBlock(PatternType type, string line)
		{
			switch (type)
			{
                case PatternType.Class:
                    return new ClassBlock(line);
                case PatternType.Function:
					return new FunctionBlock(line);
				case PatternType.Field:
					return new FieldBlock(line);
				default:
					return new UnknownBlock(line);
			}
		}
	}
}