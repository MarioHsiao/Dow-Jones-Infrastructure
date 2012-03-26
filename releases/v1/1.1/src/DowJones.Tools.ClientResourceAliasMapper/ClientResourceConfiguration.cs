using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DowJones.Tools.ClientResourceAliasMapper
{
    public class ClientResourceConfiguration
    {
        public List<ClientResourceAliasMapping> Aliases { get; private set; }

        public List<ClientResourceMapping> Mappings { get; private set; }

        public ClientResourceConfiguration(IEnumerable<ClientResourceMapping> resources = null, IEnumerable<ClientResourceAliasMapping> aliases = null)
        {
            Aliases = new List<ClientResourceAliasMapping>(aliases ?? Enumerable.Empty<ClientResourceAliasMapping>());
            Mappings = new List<ClientResourceMapping>(resources ?? Enumerable.Empty<ClientResourceMapping>());
        }

        public void Save(string fileName)
        {
            var document =
                new XElement("ClientResources",
                    new XElement("resources", Mappings.Select(x => (XElement)x)),
                    new XElement("aliases", Aliases.Select(x => (XElement)x))
                );

            document.Save(fileName);
        }

        public static ClientResourceConfiguration Load(string filename)
        {
            var document = XDocument.Load(filename);

            IEnumerable<ClientResourceAliasMapping> aliases =
                document
                    .Descendants("aliases")
                    .Descendants(ClientResourceAliasMapping.XName)
                    .Select(x => (ClientResourceAliasMapping)x);

            IEnumerable<ClientResourceMapping> resourceMappings =
                document
                    .Descendants("resources")
                    .Descendants(ClientResourceMapping.XName)
                    .Select(x => (ClientResourceMapping)x);

            return new ClientResourceConfiguration(resourceMappings.ToArray(), aliases.ToArray());
        }
    }
}
