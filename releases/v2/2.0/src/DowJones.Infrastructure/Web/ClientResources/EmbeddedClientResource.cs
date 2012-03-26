using System.Reflection;
using DowJones.Extensions;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class EmbeddedClientResource : ClientResource
    {
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
                return _url = _url ?? TargetAssembly.GetWebResourceUrl(ResourceName);
            }
            set { _url = value; }
        }
        private string _url;


        public EmbeddedClientResource()
        {
        }

        public EmbeddedClientResource(Assembly targetAssembly, string resourceName)
        {
            TargetAssembly = targetAssembly;
            ResourceName = resourceName;
            ResourceKind = DetermineResourceKind(resourceName);
        }

        public EmbeddedClientResource(Assembly targetAssembly, string resourceName, ClientResourceKind resourceKind, ClientResourceDependencyLevel dependencyLevel, bool? performSubstitution, string templateId = null)
        {
            TargetAssembly = targetAssembly;
            ResourceName = resourceName;
            DependencyLevel = dependencyLevel;
            ResourceKind = resourceKind;
            PerformSubstitution = performSubstitution.GetValueOrDefault(true);
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