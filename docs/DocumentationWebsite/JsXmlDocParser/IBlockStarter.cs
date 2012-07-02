namespace JsXmlDocParser
{
	public interface IBlockStarter
	{
		string Pattern { get; }
        PatternType PatternType { get; }
	    bool IsMatch(string line);
	}
}