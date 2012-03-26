using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DowJones.DependencyInjection;

namespace DowJones.Web
{
    public class ClientResourceRegistry : IClientResourceRegistry
    {
        private readonly IList<ClientResource> _resources;

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected CultureInfo Culture { get; set; }

        protected internal IClientResourceManager GlobalResourceManager
        {
            get;
            set;
        }


        protected ClientResourceRegistry(IClientResourceManager globalResourceManager)
        {
            GlobalResourceManager = globalResourceManager;
            _resources = new List<ClientResource>();
        }


        public virtual IEnumerable<ClientResource> GetResources(Func<ClientResource, bool> query = null)
        {
            IEnumerable<ClientResource> resources = _resources;
            
            if(query != null)
                resources = resources.Where(query);

            var distinctAndOrderedResources =
                resources
                    .Distinct()
                    .OrderByDescending(x => x.DependencyLevel);

            return distinctAndOrderedResources;
        }

        public virtual void Register(ClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");
            Guard.IsNotNullOrEmpty(resource.Url, "resource.Url");

            var existingResource = GetResources().SingleOrDefault(x => x == resource);
            if (existingResource != null)
            {
                existingResource.Update(resource);
                return;
            }

            var resolvedResource = GlobalResourceManager.GetClientResource(resource);

            if (resolvedResource == null)
                resolvedResource = resource;

            _resources.Add(resolvedResource);
            OnRegistered(resolvedResource);
        }

        public virtual void Unregister(ClientResource resource)
        {
            if (_resources.Contains(resource))
                _resources.Remove(resource);
        }

        protected virtual void OnRegistered(ClientResource resource)
        {
            // Empty: for subclasses to override
        }
    }
}