using System.Xml.Linq;

namespace DowJones.Tools.ClientResourceAliasMapper
{
    public class ClientResourceAliasMapping
    {
        internal const string XName = "mapping";
        private const string AliasAttributeName = "alias";
        private const string OriginalAliasAttributeName = "originalAlias";
        private const string ResourceNameAttributeName = "name";

        public string Alias { get; set; }
        public string OriginalAlias { get; set; }
        public string ResourceName { get; set; }

        public static explicit operator ClientResourceAliasMapping(XElement element)
        {
            var mapping = new ClientResourceAliasMapping();

            var aliasAttribute = element.Attribute(AliasAttributeName);
            var originalAliasAttribute = element.Attribute(OriginalAliasAttributeName);
            var resourceNameAttribute = element.Attribute(ResourceNameAttributeName);

            if (aliasAttribute != null && !string.IsNullOrEmpty(aliasAttribute.Value))
                mapping.Alias = aliasAttribute.Value;

            if (originalAliasAttribute != null && !string.IsNullOrEmpty(originalAliasAttribute.Value))
                mapping.OriginalAlias = originalAliasAttribute.Value;

            if (resourceNameAttribute != null && !string.IsNullOrEmpty(resourceNameAttribute.Value))
                mapping.ResourceName = resourceNameAttribute.Value;

            return mapping;
        }

        public static explicit operator XElement(ClientResourceAliasMapping alias)
        {
            var element = new XElement(XName);

            element.Add(new XAttribute(AliasAttributeName, alias.Alias));

            if (!string.IsNullOrEmpty(alias.OriginalAlias))
                element.Add(new XAttribute(OriginalAliasAttributeName, alias.OriginalAlias));

            element.Add(new XAttribute(ResourceNameAttributeName, alias.ResourceName));

            return element;
        }
    }
}