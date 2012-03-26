using System.Linq;
using System.Reflection;
using DowJones.Mapping;
using DowJones.Web.UI.Exceptions;
using log4net;

namespace DowJones.Web
{
    public class ClientResourceDefinitionToClientResourceMapper : TypeMapper<ClientResourceDefinition,ClientResource>
    {
        public override ClientResource Map(ClientResourceDefinition definition)
        {
            ClientResource resource = null;

            if (definition.HasUrl)
            {
                resource = new ClientResource(definition.Url);
            }
            else if (definition.HasResourceName)
            {
                resource = GetEmbeddedResource(definition, resource);
            }

            //if (resource == null || string.IsNullOrEmpty(resource.Url))
            //{
            //    throw new InvalidClientResourceException(resource, "Empty resource Url")
            //              {
            //                  Name = definition.Name
            //              };
            //}

            resource.DependencyLevel = GetDependencyLevel(definition);
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

        private static ClientResource GetEmbeddedResource(ClientResourceDefinition definition, ClientResource resource)
        {
            if (definition.DeclaringType == null && definition.DeclaringAssembly == null)
            {
                throw new InvalidClientResourceException(resource, "Must provide a Declaring Type or Assembly with a ResourceName")
                          {
                              Name = definition.Name
                          };

            }

            Assembly resourceAssembly;

            if (definition.DeclaringAssembly == null)
            {
                resourceAssembly = definition.DeclaringType.Assembly;
            }
            else
            {
                var logger = LogManager.GetLogger(typeof(ClientResource));
                
                if (logger.IsDebugEnabled)
                    logger.DebugFormat("Attempting to load assembly: {0}", definition.DeclaringAssembly);

                resourceAssembly = Assembly.Load(new AssemblyName(definition.DeclaringAssembly));
            }

            return new EmbeddedClientResource(resourceAssembly, definition.ResourceName, definition.DeclaringType);
        }

        private static string GetName(ClientResourceDefinition definition)
        {
            if (!string.IsNullOrWhiteSpace(definition.Name))
                return definition.Name;

            var name = definition.ResourceName; //?? definition.Url;

            if (string.IsNullOrWhiteSpace(name))
                return null;

            if (name.StartsWith("~/") || name.StartsWith("/") || !name.EndsWith(".js"))
                return name;

            return name.Substring(0, name.Length - 3);
        }
    }
}