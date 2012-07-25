using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace VSDocPreprocessor
{
    public class DocumentEntityConverter
    {
        public XDocument Convert<T>(T entity) where T : DocumentEntity
        {
            using (var buffer = new MemoryStream())
            {
                var bufferWriter = new XmlTextWriter(new StreamWriter(buffer));

                new DataContractSerializer(typeof(T))
                    .WriteObject(bufferWriter, entity);

                bufferWriter.Flush();

                buffer.Seek(0, SeekOrigin.Begin);

                var doc = XDocument.Load(buffer, LoadOptions.None);
                
                Cleanse(doc);
                
                return doc;
            }
        }

        private void Cleanse(XDocument doc)
        {
            doc.Root.RemoveAttributes();
            PromoteElementToAttribute(doc.Root, "assembly");
            PromoteElementToAttribute(doc.Root, "fullName");
            PromoteElementToAttribute(doc.Root, "name");
            PromoteElementToAttribute(doc.Root, "namespace");
            PromoteElementToAttribute(doc.Root, "type");
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
