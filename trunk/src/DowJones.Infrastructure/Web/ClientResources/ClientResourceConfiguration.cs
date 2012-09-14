using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DowJones.Exceptions;
using DowJones.Extensions;

namespace DowJones.Web
{
    public class ClientResourceConfiguration
    {
        private static volatile Lazy<XDocument> Config = new Lazy<XDocument>(ParseConfigFile);

        internal static string ConfigFilePath
        {
            get { return _configFilePath ?? HttpContext.Current.Server.MapPath("~/ClientResources.xml"); }
            set { _configFilePath = value; }
        }
        private static volatile string _configFilePath;

        private readonly XDocument _config;

        public IEnumerable<ClientResourceAlias> Aliases
        {
            get
            {
                var mappings =
                    (
                        from mapping in _config.Descendants("mapping")
                        let alias = mapping.Attribute("alias")
                        let name = mapping.Attribute("name")
                        select new Tuple<XAttribute, XAttribute>(alias, name)
                    ).ToArray();

                ValidateMappings(mappings);

                var aliases =
                    from mapping in mappings
                    let alias = mapping.Item1.Value
                    let name = mapping.Item2.Value
                    select new ClientResourceAlias
                               {
                                   Alias = alias,
                                   Name = name
                               };

                return aliases.ToArray();
            }
        }

        public IEnumerable<ClientResourceDefinition> Resources()
        {
            var resourceDefinitions =
                from mapping in _config.Descendants("resource")
                let name = mapping.GetAttributeValue("name")
                where name.HasValue()
                let level = Mapper.Map<ClientResourceDependencyLevel>(mapping.GetAttributeValue("level"))
                let kind = Mapper.Map<ClientResourceKind?>(mapping.GetAttributeValue("kind"))
                let assembly = mapping.GetAttributeValue("assembly")
                let templateId = mapping.GetAttributeValue("templateId")
                let dependsOn = mapping.Elements("dependsOn").Select(x => x.Value)
                let isEmbeddedResource = assembly.HasValue()
                select new ClientResourceDefinition
                {
                    DeclaringAssembly = assembly,
                    DependencyLevel = level,
                    DependsOn = dependsOn.ToArray(),
                    Name = name,
                    ResourceKind = kind,
                    ResourceName = (isEmbeddedResource) ? name : null,
                    TemplateId = templateId,
                    Url = (isEmbeddedResource) ? null : name,
                };

            return resourceDefinitions;
        }

        public IEnumerable<ClientResourceDirectory> Directories
        {
            get
            {
                var directories =
                    from mapping in _config.Descendants("directory")
                    let path = mapping.GetAttributeValue("path")
                    let level = Mapper.Map<ClientResourceDependencyLevel>(mapping.GetAttributeValue("level"))
                    where path != null
                    select new ClientResourceDirectory { Path = path, Level = level };

                return directories;
            }
        }

        public ClientResourceConfiguration()
            : this(Config.Value)
        {
        }

        internal ClientResourceConfiguration(XDocument config)
        {
            _config = config;
        }

        static void ValidateMappings(IEnumerable<Tuple<XAttribute, XAttribute>> mappingTuples)
        {
            // Convert tuple to anonymous class for better readability
            var mappings = mappingTuples.Select(x => new { alias = x.Item1, name = x.Item2 });

            var invalidAliases = mappings.Where(x => x.alias == null || string.IsNullOrWhiteSpace(x.alias.Value));
            var invalidNames = mappings.Where(x => x.name == null || string.IsNullOrWhiteSpace(x.name.Value));

            if (invalidAliases.Any() || invalidNames.Any())
            {
                StringBuilder errorMessage = new StringBuilder("Invalid Client Resource Alias mappings: ");

                errorMessage.Append(string.Join("Name: {0}; ", invalidAliases.Where(x => x.name != null).Select(x => x.name.Value)));
                errorMessage.Append(string.Join("Alias: {0}; ", invalidNames.Where(x => x.alias != null).Select(x => x.name.Value)));

                errorMessage.AppendFormat("Invalid aliases: {0}", invalidAliases.Count());
                errorMessage.AppendFormat("Invalid names: {0}", invalidNames.Count());

                throw new DowJonesUtilitiesException("Invalid Client Resource Alias mappings");
            }
        }

        private static XDocument ParseConfigFile()
        {
            if (string.IsNullOrWhiteSpace(ConfigFilePath))
                throw new DowJonesUtilitiesException("Client Resource Alias file path not specified");

            if (!File.Exists(ConfigFilePath))
                return new XDocument();

            return XDocument.Load(ConfigFilePath);
        }

        public class ClientResourceDirectory
        {
            public string Path { get; set; }
            public ClientResourceDependencyLevel Level { get; set; }
        }
    }
}