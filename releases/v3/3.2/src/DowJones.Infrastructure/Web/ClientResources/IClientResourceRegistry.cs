using System;
using System.Collections.Generic;

namespace DowJones.Web
{
    public interface IClientResourceRegistry
    {
        IEnumerable<ClientResource> GetResources(Func<ClientResource, bool> query);

        void Register(ClientResource clientResource);

        void Unregister(ClientResource clientResource);
    }

    public static class IClientResourceRegistryExtensions
    {

        public static IEnumerable<ClientResource> GetResources(this IClientResourceRegistry registry)
        {
            return registry.GetResources(null);
        }

    }
}