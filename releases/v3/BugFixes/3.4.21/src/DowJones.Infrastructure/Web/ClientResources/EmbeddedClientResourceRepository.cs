using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Infrastructure;

namespace DowJones.Web.ClientResources
{
    public class EmbeddedClientResourceRepository : IClientResourceRepository
    {
        private readonly Lazy<IEnumerable<ClientResource>> _clientResources;

        public EmbeddedClientResourceRepository(IAssemblyRegistry assemblyRegistry)
        {
            _clientResources = new Lazy<IEnumerable<ClientResource>>(
                    () => DiscoverClientResources(assemblyRegistry)
                );
        }

        public IEnumerable<ClientResource> GetClientResources()
        {
            return _clientResources.Value;
        }

        private static IEnumerable<ClientResource> DiscoverClientResources(IAssemblyRegistry assemblyRegistry)
        {
            var resourceAttributesByType =
                from type in assemblyRegistry.Assemblies.SelectMany(x => x.GetExportedTypes())
                let attributes = type.GetCustomAttributes(true).OfType<ClientResourceAttribute>()
                select new { type, attributes };


            var resources =
                from attributeGroup in resourceAttributesByType
                from attribute in attributeGroup.attributes
                select attribute.ToClientResource(attributeGroup.type);

            return resources.ToArray();
        }
    }
}