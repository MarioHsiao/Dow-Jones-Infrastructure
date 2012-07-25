using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using VSDocPreprocessor;

namespace VSDocSplitter
{
    public class Parser
    {
        public IEnumerable<Type> Parse(string filename)
        {
            using (var reader = new StreamReader(filename))
                return Parse(reader.BaseStream);
        }

        public IEnumerable<Type> Parse(Stream stream)
        {
            XDocument vsdocument = XDocument.Load(stream);
            return Parse(vsdocument);
        }

        public IEnumerable<Type> Parse(XDocument vsDoc)
        {
            Contract.Requires(vsDoc != null);
            Contract.Requires(vsDoc.Root != null);

            var members = vsDoc.Root.Element("members").Elements();
            var types = Parse(members.OrderBy(x => x.Attribute("name").Value).ToArray()).ToArray();

            var assemblyElement = vsDoc.Root.Element("assembly");
            if(assemblyElement != null && !string.IsNullOrWhiteSpace(assemblyElement.Value))
            {
                var assemblyName = assemblyElement.Value;

                foreach (var type in types)
                {
                    type.Assembly = assemblyName;
                }
            }

            return types;
        }

        public IEnumerable<Type> Parse(IEnumerable<XElement> elements)
        {
            var namedElements =
                (
                    from element in elements
                    let split = SplitName(element)
                    where split != null && split.Length > 0
                    select new { type = split[0], name = split[1], element }
                ).ToArray();

            var typeElements = namedElements.Where(x => x.type == "T");

            foreach (var typeElement in typeElements)
            {
                var childrenElements =
                    namedElements.Except(typeElements)
                        .Where(x => x.name.StartsWith(typeElement.name+"."))
                        .ToArray();

                var typeNameLength = typeElement.name.Length;
                foreach (var child in childrenElements)
                {
                    child.element.Attribute("name").Value = 
                        string.Format("{0}:{1}", child.type, child.name.Substring(typeNameLength+1));
                }

                var children = 
                    childrenElements.Select(x => x.element)
                        .Select(Parse)
                        .Where(x => x != null)
                        .ToArray();

                yield return Map(typeElement.element, new Type(children));
            }
        }

        private T Map<T>(XElement element, T entity) where T : DocumentEntity
        {
            if(element == null || entity == null)
                return entity;

            var splitName = SplitName(element);

            entity.Example = ElementValue(element, "example");
            entity.Name = splitName.Length == 2 ? splitName[1] : splitName[0];
            entity.Remarks = ElementValue(element, "remarks");
            entity.Summary = ElementValue(element, "summary");

            if (string.IsNullOrWhiteSpace(entity.Remarks) && !element.HasElements)
                entity.Remarks = element.Value;

            return entity;
        }

        private DocumentEntity Parse(XElement element)
        {
            if(element == null)
                return null;

            var parameters = element.Elements("param").Select(x => Map(x, new Parameter()));

            var split = SplitName(element);

            DocumentEntity entity;

            switch (split[0])
            {
                case("E"):
                    entity = new Event();
                    break;

                case("F"):
                    entity = new Field();
                    break;

                case ("M"):
                    if(split[1].Contains("#ctor"))
                        entity = new Constructor(parameters);
                    else
                        entity = new Method(parameters);
                    break;

                case ("P"):
                    entity = new Property();
                    break;

                default:
                    entity = null;
                    break;
            }

            return Map(element, entity);
        }


        private static string[] SplitName(XElement element)
        {
            var name = element.Attribute("name").Value;
            return name.Split(':');
        }

        private static string ElementValue(XElement element, string name)
        {
            return element.Elements(name).Select(x => x.Value.Trim()).FirstOrDefault();
        }
    }
}
