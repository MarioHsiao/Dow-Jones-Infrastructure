using System.Reflection;
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
            get { return string.Empty; }
            set {  }
        }


        public EmbeddedClientResource()
        {
        }

        public EmbeddedClientResource(Assembly targetAssembly, string resourceName, Type declaringType = null)
        {
            TargetAssembly = targetAssembly;
            ResourceName = resourceName;
            ResourceKind = DetermineResourceKind(resourceName);
            ClientTemplates = DiscoverDependentResources(declaringType);
        }
         
        public IEnumerable<ClientResource> DiscoverDependentResources(Type declaringType)
        {
            if (declaringType == null || ResourceKind != ClientResourceKind.Script)
                return Enumerable.Empty<ClientResource>();

            var clientResourceAttributes = 
                (declaringType.GetClientResourceAttributes(false) ?? Enumerable.Empty<ClientResourceAttribute>()).ToArray();

            var firstScript = clientResourceAttributes.OfType<ScriptResourceAttribute>().FirstOrDefault();
            var isFirstScript = firstScript != null && (ResourceName == firstScript.ResourceName || (ResourceName ?? string.Empty).EndsWith(firstScript.RelativeResourceName ?? string.Empty));

            if (!isFirstScript)
                return Enumerable.Empty<ClientResource>();

            if (ClientResourcesCache.ContainsKey(declaringType))
                return ClientResourcesCache[declaringType];

            var resources = 
                clientResourceAttributes
                    .Where(r => r.ResourceKind == ClientResourceKind.ClientTemplate)
                    .Reverse() // Reverse the order so the anscestors are first
                    .Select(x => x.ToClientResource(declaringType));

            ClientResourcesCache.Add(declaringType, resources);

            return resources;
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
