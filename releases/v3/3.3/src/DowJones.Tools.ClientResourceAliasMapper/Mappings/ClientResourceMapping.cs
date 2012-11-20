using System.Xml.Linq;

namespace DowJones.Tools.ClientResourceAliasMapper.Mappings
{
    public class ClientResourceMapping
    {
        internal const string XName = "resource";
        private const string NameAttributeName = "name";
        private const string DependencyLevelAttributeName = "level";
        private const string DirectoryAttributeName = "directory";
        private const string DependsAttributeValue = "dependsOn";

        public string Name { get; set; }
        public string DependencyLevel { get; set; }
        public string Directory { get; set; }
        public string DependsOn { get; set; }

        public static explicit operator ClientResourceMapping(XElement element)
        {
            var nameAttribute = element.Attribute(NameAttributeName);
            var dependencyLevelAttribute = element.Attribute(DependencyLevelAttributeName);
            var directoryAttribute = element.Attribute(DirectoryAttributeName);
            var dependsOnAttribute1 = element.Element(DependsAttributeValue);

            var mapping = new ClientResourceMapping();

            if (nameAttribute != null && !string.IsNullOrEmpty(nameAttribute.Value))
                mapping.Name = nameAttribute.Value;

            if (directoryAttribute != null && !string.IsNullOrEmpty(directoryAttribute.Value))
                mapping.Directory = directoryAttribute.Value;

            if (dependencyLevelAttribute != null && !string.IsNullOrEmpty(dependencyLevelAttribute.Value))
                mapping.DependencyLevel = dependencyLevelAttribute.Value;

            if (dependsOnAttribute1 != null && !string.IsNullOrEmpty(dependsOnAttribute1.Value))
                mapping.DependsOn = dependsOnAttribute1.Value;

            return mapping;
        }

        public static explicit operator XElement(ClientResourceMapping mapping)
        {
            var element = new XElement(XName);

            if (!string.IsNullOrEmpty(mapping.Name))
                element.Add(new XAttribute(NameAttributeName, mapping.Name));

            if (!string.IsNullOrEmpty(mapping.Directory))
                element.Add(new XAttribute(DirectoryAttributeName, mapping.Directory));

            if (!string.IsNullOrEmpty(mapping.DependencyLevel))
                element.Add(new XAttribute(DependencyLevelAttributeName, mapping.DependencyLevel));

            if (!string.IsNullOrEmpty(mapping.DependsOn))
                element.Add(new XElement(DependsAttributeValue, mapping.DependsOn));

            return element;
        }
    }
}