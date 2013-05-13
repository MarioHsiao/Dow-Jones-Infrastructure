using System.Collections.Generic;
using System.Linq;
using System.Web.Razor.Text;
using DowJones.Web.Razor.Common;

namespace DowJones.Web.Razor.ClientResources
{
    public abstract class ClientResourceSpan : KeyValueContentSpan
    {
        public string RootResourceNamespace { get; set; }

        protected bool IsAbsoluteResource { get { return string.IsNullOrWhiteSpace(RelativeResourceName); } }

        public virtual string DeclaringType
        {
            get { return GetContentPropertyValue("DeclaringType"); }
        }

        public virtual string DeclaringAssembly
        {
            get { return GetContentPropertyValue("DeclaringAssembly"); }
        }

        public string DependencyLevel
        {
            get { return GetContentPropertyValue("DependencyLevel"); }
        }

        public IEnumerable<string> DependsOn
        {
            get
            {
                var value = GetContentPropertyValue("DependsOn");

                if (string.IsNullOrWhiteSpace(value))
                    return Enumerable.Empty<string>();

                return value.Split(';').Select(x => x.Trim());
            }
        }

        public string Name
        {
            get { return GetContentPropertyValue("Name"); }
        }

        public bool PerformSubstitution
        {
            get { return GetContentPropertyValueBool("PerformSubstitution"); }
        }

        public virtual string ResourceKind
        {
            get { return GetContentPropertyValue("ResourceKind"); }
        }

        public string ResourceName
        {
            get
            {
                var resourceName = GetContentPropertyValue("ResourceName");

                if(!string.IsNullOrWhiteSpace(resourceName))
                    return resourceName;

                var relativeResourceName = RelativeResourceName;
                if (!string.IsNullOrWhiteSpace(relativeResourceName))
                {
                    resourceName = string.Format("{0}.{1}", RootResourceNamespace, relativeResourceName);
                }

                return resourceName;
            }
        }

        public string RelativeResourceName
        {
            get
            {
                var relativeResourceName = GetContentPropertyValue("RelativeResourceName");
                return string.IsNullOrWhiteSpace(relativeResourceName) ? SingletonValue : relativeResourceName;
            }
        }

        public virtual string Url
        {
            get
            {
                var url = GetContentPropertyValue("Url");
                return string.IsNullOrWhiteSpace(url) ? SingletonValue : url;
            }
        }

        public abstract string AttributeTypeName { get; }

        public virtual string MimeType { get; set; }

        protected internal ClientResourceSpan(SourceLocation location, string content)
            : base(location, content)
        {
        }
    }
}