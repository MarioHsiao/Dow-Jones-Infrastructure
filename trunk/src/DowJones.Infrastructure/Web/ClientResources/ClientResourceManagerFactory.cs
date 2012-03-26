using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Infrastructure;
using log4net;

namespace DowJones.Web
{
    public interface IClientResourceManagerFactory
    {
        IClientResourceManager Create();
    }

    public class ClientResourceManagerFactory : Factory<IClientResourceManager>, IClientResourceManagerFactory
    {
        private readonly IAssemblyRegistry _assemblyRegistry;

        public string AliasFilePath
        {
            get
            {
                return _aliasFilePath
                    ?? HttpContext.Current.Server.MapPath("~/ClientResources.xml");
            }
            set { _aliasFilePath = value; }
        }
        private string _aliasFilePath;

        protected internal XDocument ClientResourceConfiguration
        {
            get
            {
                if(_clientResourceAliasMappings == null)
                {
                    if (string.IsNullOrWhiteSpace(AliasFilePath))
                        throw new DowJonesUtilitiesException("Client Resource Alias file path not specified");
                    if (!File.Exists(AliasFilePath))
                        return new XDocument();

                    _clientResourceAliasMappings = XDocument.Load(AliasFilePath);
                }

                return _clientResourceAliasMappings;
            }
            set { _clientResourceAliasMappings = value; }
        }
        private XDocument _clientResourceAliasMappings;

        protected internal string RootDirectory
        {
            get
            {
                return _rootDirectory
                    ?? HttpContext.Current.Server.MapPath("~/");
            }
            set { _rootDirectory = value; }
        }
        private string _rootDirectory;

        [Inject("Optional dependency")]
        protected internal ILog Log { get; set; }


        public ClientResourceManagerFactory(IAssemblyRegistry assemblyRegistry)
        {
            _assemblyRegistry = assemblyRegistry;
        }


        public override IClientResourceManager Create()
        {
            var resources = GetClientResources();
            var aliases = GetClientResourceAliases();

            var manager = new ClientResourceManager(resources, aliases);

            return manager;
        }

        protected internal virtual IEnumerable<ClientResource> GetClientResources()
        {
            IEnumerable<ClientResource> discoveredResources = DiscoverClientResourcesFromAttributes();

            IEnumerable<ClientResource> specificMappedResources = GetSpecificMappedResources();

            IEnumerable<ClientResource> directoryMappedResources = GetDirectoryMappedResources();

            return 
                discoveredResources
                .Union(specificMappedResources)
                .Union(directoryMappedResources)
                .ToArray();
        }

        private IEnumerable<ClientResource> GetDirectoryMappedResources()
        {
            var directoryMappings =
                from mapping in ClientResourceConfiguration.Descendants("directory")
                let path = mapping.Attribute("path")
                let level = Mapper.Map<ClientResourceDependencyLevel>(mapping.GetAttributeValue("level"))
                where path != null
                let absolutePath =
                    (path.Value.StartsWith("~/"))
                        ? Path.Combine(RootDirectory, path.Value.Substring(2))
                        : path.Value
                select new { path = absolutePath, level };

            IEnumerable<ClientResource> resources =
                from mapping in directoryMappings
                from filename in Directory.GetFiles(mapping.path)
                select new ClientResource(filename)
                {
                    DependencyLevel = mapping.level
                };

            return resources;
        }

        private IEnumerable<ClientResource> GetSpecificMappedResources()
        {
            var resourceDefinitions =
                from mapping in ClientResourceConfiguration.Descendants("resource")
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

            var resources = resourceDefinitions.Select(Mapper.Map<ClientResource>);

            return resources;
        }

        private IEnumerable<ClientResource> DiscoverClientResourcesFromAttributes()
        {
            var resourceAttributesByType =
                from type in _assemblyRegistry.Assemblies.SelectMany(x => x.GetExportedTypes())
                let attributes = type.GetCustomAttributes(true).OfType<ClientResourceAttribute>()
                select new { type, attributes };

            IEnumerable<ClientResource> resources =
                from attributeGroup in resourceAttributesByType
                from attribute in attributeGroup.attributes
                select attribute.ToClientResource(attributeGroup.type);

            return resources;
        }

        protected internal virtual IEnumerable<ClientResourceAlias> GetClientResourceAliases()
        {
            var mappings =
                from mapping in ClientResourceConfiguration.Descendants("mapping")
                let alias = mapping.Attribute("alias")
                let name = mapping.Attribute("name")
                select new Tuple<XAttribute,XAttribute>(alias, name);

            ValidateMappings(mappings);

            IEnumerable<ClientResourceAlias> aliases =
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

        protected internal virtual void ValidateMappings(IEnumerable<Tuple<XAttribute, XAttribute>> mappingTuples)
        {
            // Convert tuple to anonymous class for better readability
            var mappings = mappingTuples.Select(x => new { alias = x.Item1, name = x.Item2 });

            var invalidAliases = mappings.Where(x => x.alias == null || string.IsNullOrWhiteSpace(x.alias.Value));
            var invalidNames =  mappings.Where(x => x.name == null || string.IsNullOrWhiteSpace(x.name.Value));

            if(invalidAliases.Any() || invalidNames.Any())
            {
                StringBuilder errorMessage = new StringBuilder("Invalid Client Resource Alias mappings: ");

                errorMessage.Append(string.Join("Name: {0}; ", invalidAliases.Where(x => x.name != null).Select(x => x.name.Value)));
                errorMessage.Append(string.Join("Alias: {0}; ", invalidNames.Where(x => x.alias != null).Select(x => x.name.Value)));

                errorMessage.AppendFormat("Invalid aliases: {0}", invalidAliases.Count());
                errorMessage.AppendFormat("Invalid names: {0}", invalidNames.Count());

                Log.Fatal(errorMessage);

                throw new DowJonesUtilitiesException("Invalid Client Resource Alias mappings");
            }
        }
    }
}