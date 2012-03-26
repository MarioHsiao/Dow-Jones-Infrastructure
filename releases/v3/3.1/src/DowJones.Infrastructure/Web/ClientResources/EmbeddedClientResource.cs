using System.Reflection;
using DowJones.Extensions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DowJones.Web
{
    public class EmbeddedClientResource : ClientResource
    {
        internal static IDictionary<Type, IEnumerable<ClientResource>> ClientResourcesCache
        {
            get
            {
                if (clientResourcesCache == null)
                    clientResourcesCache = new Dictionary<Type, IEnumerable<ClientResource>>();

                return clientResourcesCache;
            }
            set { clientResourcesCache = value; }
        }
        private volatile static IDictionary<Type, IEnumerable<ClientResource>> clientResourcesCache;

        public override string Name
        {
            get { return _name ?? ResourceName; }
            set { _name = value; }
        }
        private string _name;

        public string ResourceName { get; set; }
         
        public Assembly TargetAssembly { get; set; }

        public override string Url
        {
            get
            {
                //return _url = _url ?? TargetAssembly.GetWebResourceUrl(ResourceName);
                return string.Empty;
            }
            set { _url = value; }
        }
        private string _url;


        public EmbeddedClientResource()
        {
        }

        public EmbeddedClientResource(Assembly targetAssembly, string resourceName, Type declaringType = null)
        {
            TargetAssembly = targetAssembly;
            ResourceName = resourceName;
            ResourceKind = DetermineResourceKind(resourceName);
            if (ResourceKind == ClientResourceKind.Script && declaringType != null)
                ClientTemplates = DiscoverDependentResources(declaringType);
        }
         
        public IEnumerable<ClientResource> DiscoverDependentResources(Type declaringType)
        {
            if (ClientResourcesCache.ContainsKey(declaringType))
                return ClientResourcesCache[declaringType];

            var clientResourceAttributes = declaringType.GetClientResourceAttributes();

            // Reverse the order so the anscestors are first
            clientResourceAttributes = clientResourceAttributes.Reverse();

            var resources = clientResourceAttributes.Select(x => x.ToClientResource(declaringType));
            var dependentResources = resources.Where(r => r.ResourceKind == ClientResourceKind.ClientTemplate);
            ClientResourcesCache.Add(declaringType, dependentResources);
            return dependentResources;
        }


        public EmbeddedClientResource(Assembly targetAssembly, string resourceName, ClientResourceKind resourceKind, ClientResourceDependencyLevel dependencyLevel, string templateId = null)
        {
            TargetAssembly = targetAssembly;
            ResourceName = resourceName;
            DependencyLevel = dependencyLevel;
            ResourceKind = resourceKind;
            TemplateId = templateId;
        }


        public override bool Equals(ClientResource other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!(other is EmbeddedClientResource)) return false;

            var embeddedOther = (EmbeddedClientResource)other;

            return embeddedOther.TargetAssembly == TargetAssembly
                   && embeddedOther.ResourceName == ResourceName;
        }
    }
}
