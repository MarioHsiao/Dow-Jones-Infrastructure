using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace JsXmlDocParser
{
    public class BlockWriter
    {
        public virtual string ToVsDocXml(IBlock block)
        {
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
                ToVsDocXml(writer, block);

            return sb.ToString();
        }

        public virtual void ToVsDocXml(XmlWriter writer, IBlock block)
        {
            writer.WriteStartElement("member");
            writer.WriteAttributeString("name", string.Format(block.Name, ToString()));
            if (block.Comments.Any())
            {
                writer.WriteNode(
                    XmlReader.Create(new StringReader(String.Join(Environment.NewLine, block.Comments)),
                                     new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment })
                    , false);
            }

            writer.WriteEndElement();
        } 
    }
}