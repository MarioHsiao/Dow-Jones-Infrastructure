using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using DowJones.Mapping;
using DowJones.Web.UI.Exceptions;
using log4net;

namespace DowJones.Web
{
    public class ClientResourceDefinitionToClientResourceMapper : TypeMapper<ClientResourceDefinition, ClientResource>
    {
        private static readonly ConcurrentDictionary<string, Assembly> FoundAssemblies = new ConcurrentDictionary<string, Assembly>();
        private readonly ILog logger = LogManager.GetLogger(typeof(ClientResourceDefinitionToClientResourceMapper));
  
        public override ClientResource Map(ClientResourceDefinition definition)
        {
            ClientResource resource = null;

            if (definition.HasUrl)
            {
                resource = new ClientResource(definition.Url);
            }
            else if (definition.HasResourceName)
            {
                resource = GetEmbeddedResource(definition, null);
            }

            if (resource == null)
                return null;

            resource.DependencyLevel = GetDependencyLevel(definition);
            resource.DependsOn = definition.DependsOn ?? Enumerable.Empty<string>();
            resource.Name = GetName(definition);
            resource.ResourceKind = definition.ResourceKind.GetValueOrDefault(resource.ResourceKind);
            resource.TemplateId = definition.TemplateId;

            return resource;
        }

        private static ClientResourceDependencyLevel GetDependencyLevel(ClientResourceDefinition definition)
        {
            if (definition.DependencyLevel != null)
            {
                return definition.DependencyLevel.Value;
            }

            if (definition.HasUrl && definition.Url.StartsWith("~/"))
            {
                return ClientResourceDependencyLevel.Independent;
            }

            return ClientResourceDependencyLevel.Component;
        }

        private ClientResource GetEmbeddedResource(ClientResourceDefinition definition, ClientResource resource)
        {
            if (definition.DeclaringType == null && definition.DeclaringAssembly == null)
            {
                throw new InvalidClientResourceException(resource, "Must provide a Declaring Type or Assembly with a ResourceName")
                          {
                              Name = definition.Name
                          };

            }

            var resourceAssembly = definition.DeclaringAssembly == null ? definition.DeclaringType.Assembly : GetDeclaringAssembly(definition.DeclaringAssembly);

            return new EmbeddedClientResource(resourceAssembly, definition.ResourceName, definition.DeclaringType);
        }

        private Assembly GetDeclaringAssembly(string declaringAssembly)
        {
            Assembly tempAssembly;
            if (logger.IsInfoEnabled)
            {
                logger.InfoFormat("Getting declaring assembly: {0}", declaringAssembly);
            }

            if (FoundAssemblies.TryGetValue(declaringAssembly, out tempAssembly))
            {
                return tempAssembly;
            }
            
            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("Attempting to load assembly: {0}", declaringAssembly);
            }

            tempAssembly = Assembly.Load(new AssemblyName(declaringAssembly));
            FoundAssemblies.TryAdd(declaringAssembly, tempAssembly);
            return tempAssembly;
        }

        private static string GetName(ClientResourceDefinition definition)
        {
            if (!string.IsNullOrWhiteSpace(definition.Name))
                return definition.Name;

            var name = definition.ResourceName;

            if (string.IsNullOrWhiteSpace(name))
                return null;

            if (name.StartsWith("~/") || name.StartsWith("/") || !name.EndsWith(".js"))
                return name;

            var parts = name.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return "DJ.UI." + parts[parts.Length - 2];

        }
    }
}