using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace JsXmlDocParser
{
	public class UnknownBlock : IBlock
	{
		public UnknownBlock(string line)
		{
			BlockStarter = new UnknownBlockStarter();
			Children = new List<IBlock>();
			Comments = new List<string>();
		}

		
		public IBlockStarter BlockStarter { get; private set; }
		public List<IBlock> Children { get; private set; }
		public List<string> Comments { get; private set; }
		public string ToVsDocXml()
		{
			throw new NotSupportedException("This method should not be called.");
		}

		public void ToVsDocXml(XmlWriter writer)
		{
			throw new NotSupportedException("This method should not be called.");
		}
	}
}