using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace VSDocPreprocessor
{
    public class Serializer
    {
        public void Serialize<T>(T entity, TextWriter writer) where T : DocumentEntity
        {
            using (var buffer = new MemoryStream())
            {
                var bufferWriter = new XmlTextWriter(new StreamWriter(buffer));

                new DataContractSerializer(typeof(T))
                    .WriteObject(bufferWriter, entity);

                bufferWriter.Flush();

                buffer.Seek(0, SeekOrigin.Begin);

                var xmlTextWriter = new XmlTextWriter(writer) { Formatting = Formatting.Indented };
                Cleanse(buffer, xmlTextWriter);
            }
        }

        private void Cleanse(Stream stream, XmlWriter writer)
        {
            XDocument doc = XDocument.Load(stream, LoadOptions.None);

            doc.Root.RemoveAttributes();
            PromoteElementToAttribute(doc.Root, "fullName");
            PromoteElementToAttribute(doc.Root, "name");
            PromoteElementToAttribute(doc.Root, "namespace");
            PromoteElementToAttribute(doc.Root, "type");

            doc.WriteTo(writer);
        }

        private static void PromoteElementToAttribute(XElement element, string xname)
        {
            var names = element.Descendants(xname).ToArray();
            foreach (var name in names)
            {
                if (name != null && name.Parent != null)
                {
                    name.Parent.SetAttributeValue(xname, name.Value);
                    name.Remove();
                }
            }
        }
    }
}
