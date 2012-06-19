using System.Collections.Generic;
using System.Xml;

namespace JsXmlDocParser
{
	public interface IBlock
	{
		IBlockStarter BlockStarter { get; }
		List<IBlock> Children { get; }
		List<string> Comments { get; }
		string ToVsDocXml();

		void ToVsDocXml(XmlWriter writer);
	}
}