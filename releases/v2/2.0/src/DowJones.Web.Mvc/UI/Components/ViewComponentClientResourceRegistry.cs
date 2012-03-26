using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentClientResourceRegistry : ClientResourceRegistry
    {
        protected internal IViewComponentRegistry ComponentRegistry
        {
            get;
            set;
        }

        protected ViewComponentClientResourceRegistry(IClientResourceManager globalResourceManager, IViewComponentRegistry componentRegistry)
            : base(globalResourceManager)
        {
            ComponentRegistry = componentRegistry;
        }

        public override IEnumerable<ClientResource> GetResources(Func<ClientResource, bool> query = null)
        {
            var components = ComponentRegistry.GetRegisteredComponents();

            var componentResources = components.SelectMany(x => x.ClientResources);

            if (query != null)
                componentResources = componentResources.Where(query);
            
            var registeredResources = base.GetResources(query);

            var resources = registeredResources.Union(componentResources);

            return resources.OrderByDescending(x => x.DependencyLevel);
        }
    }
}
