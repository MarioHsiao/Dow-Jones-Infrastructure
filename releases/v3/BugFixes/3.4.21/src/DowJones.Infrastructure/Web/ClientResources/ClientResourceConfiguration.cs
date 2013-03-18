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
                if (_aliases != null)
                    return _aliases;

                _aliases =
                    (
                        from mapping in _config.Descendants("mapping")
                        let alias = mapping.Attribute("alias").Value
                        let name = mapping.Attribute("name").Value
                        select new ClientResourceAlias
                                   {
                                       Alias = alias,
                                       Name = name
                                   }
                    ).ToArray();

                ValidateAliases(_aliases);

                return _aliases;
            }
        }
        private IEnumerable<ClientResourceAlias> _aliases;

        public IEnumerable<ClientResourceDefinition> Resources
        {
            get
            {
                if (_resources != null)
                    return _resources;

                _resources =
                    (
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
                                   }
                    ).ToArray();

                ValidateResources(_resources);

                return _resources;
            }
        }
        private IEnumerable<ClientResourceDefinition> _resources;

        public IEnumerable<ClientResourceDirectory> Directories
        {
            get
            {
                _directories = _directories ??
                    from mapping in _config.Descendants("directory")
                    let path = mapping.GetAttributeValue("path")
                    let level = Mapper.Map<ClientResourceDependencyLevel>(mapping.GetAttributeValue("level"))
                    where path != null
                    select new ClientResourceDirectory { Path = path, Level = level };

                return _directories;
            }
        }
        private IEnumerable<ClientResourceDirectory> _directories;

        public ClientResourceConfiguration()
            : this(Config.Value)
        {
        }

        internal ClientResourceConfiguration(XDocument config)
        {
            _config = config;
        }

        static void ValidateAliases(IEnumerable<ClientResourceAlias> aliases)
        {
            var invalidAliases = aliases.Where(x => x.Alias == null || string.IsNullOrWhiteSpace(x.Alias)).ToArray();
            var invalidNames = aliases.Where(x => x.Name == null || string.IsNullOrWhiteSpace(x.Name)).ToArray();

            if (invalidAliases.Any() || invalidNames.Any())
            {
                var errorMessage = new StringBuilder("Invalid Client Resource Aliases: ");

                errorMessage.Append(string.Join("Name: {0}; ", invalidAliases.Where(x => x.Name != null).Select(x => x.Name)));
                errorMessage.Append(string.Join("Alias: {0}; ", invalidNames.Where(x => x.Alias != null).Select(x => x.Name)));

                errorMessage.AppendFormat("Invalid aliases: {0}", invalidAliases.Count());
                errorMessage.AppendFormat("Invalid names: {0}", invalidNames.Count());

                throw new DowJonesUtilitiesException(errorMessage.ToString());
            }
        }

        static void ValidateResources(IEnumerable<ClientResourceDefinition> resources)
        {
            var invalidNames = resources.Where(x => x.Name == null || string.IsNullOrWhiteSpace(x.Name)).ToArray();
            if(invalidNames.Any())
                throw new DowJonesUtilitiesException("Invalid Client Resources found (name is empty)");
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