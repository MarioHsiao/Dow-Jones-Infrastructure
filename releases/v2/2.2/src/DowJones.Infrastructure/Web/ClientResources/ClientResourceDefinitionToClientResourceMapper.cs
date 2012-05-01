using System.Reflection;
using DowJones.Mapping;
using DowJones.Web.UI.Exceptions;
using log4net;

namespace DowJones.Web
{
    public class ClientResourceDefinitionToClientResourceMapper : TypeMapper<ClientResourceDefinition, ClientResource>
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
                if (definition.DeclaringType == null && definition.DeclaringAssembly == null)
                {
                    throw new InvalidClientResourceException(resource, "Must provide a Declaring Type or Assembly with a ResourceName")
                    {
                        Name = definition.Name
                    };

                }
                // handle external resources
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

                resource = new EmbeddedClientResource(resourceAssembly, definition.ResourceName);
            }

            if (resource == null || string.IsNullOrEmpty(resource.Url))
            {
                throw new InvalidClientResourceException(resource, "Empty resource Url")
                {
                    Name = definition.Name
                };
            }

            resource.DependencyLevel = definition.DependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Component);
            resource.Name = definition.Name ?? definition.ResourceName ?? definition.Url;
            resource.PerformSubstitution = true;
            resource.ResourceKind = definition.ResourceKind.GetValueOrDefault(ClientResourceKind.Content);
            resource.TemplateId = definition.TemplateId;

            return resource;
        }

    }
}