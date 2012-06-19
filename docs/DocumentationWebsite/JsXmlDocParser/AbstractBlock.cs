using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace JsXmlDocParser
{
	public abstract class AbstractBlock : IBlock
	{
		protected abstract string VsDocNameFormat { get; set; }
		public IBlockStarter BlockStarter { get; protected set; }
		public List<IBlock> Children { get; protected set; }
		public List<string> Comments { get; protected set; }

		protected AbstractBlock(string line)
		{
			Children = new List<IBlock>();
			Comments = new List<string>();
		}

		public virtual string ToVsDocXml()
		{
			var sb = new StringBuilder();
			using (var writer = XmlWriter.Create(sb))
				ToVsDocXml(writer);

			return sb.ToString();
		}

		public virtual void ToVsDocXml(XmlWriter writer)
		{
			writer.WriteStartElement("member");
			writer.WriteAttributeString("name", string.Format(VsDocNameFormat, ToString()));
			if (Comments.Any())
				writer.WriteNode(
					XmlReader.Create(new StringReader(String.Join(Environment.NewLine, Comments)), 
					                 new XmlReaderSettings{ConformanceLevel = ConformanceLevel.Fragment})
					, false);
				
			writer.WriteEndElement();
		}

		
	}
}