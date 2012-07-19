using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web
{
    public class ClientResource : IEquatable<ClientResource>
    {

        /// <summary>
        /// The unique name of this resource
        /// </summary>
        public virtual string Name
        {
            get { return _name ?? Url; }
            set { _name = value; }
        }
        private string _name;

        /// <summary>
        /// The Url to the external resource.
        /// </summary>
        public virtual string Url
        {
            get;
            set;
        }

        /// <summary>
        /// The Dependency Level
        /// </summary>
        public ClientResourceDependencyLevel DependencyLevel
        {
            get;
            set;
        }

        internal IEnumerable<ClientResource> ClientTemplates
        {
            get; 
            set;
        }

        /// <summary>
        /// The unique name of this resource
        /// </summary>
        public IEnumerable<string> DependsOn
        {
            get; 
            set;
        }


        /// <summary>
        /// The Resource Kind
        /// </summary>
        public ClientResourceKind ResourceKind
        {
            get;
            set;
        }


        // hp: should reside in a derived class but for now, this is simple.
        // go ahead and change it if you have time and passion for it.
        /// <summary>
        /// Gets or sets the template id for Client Template directives.
        /// </summary>
        /// <value>The template id.</value>
        public string TemplateId
        {
            get; 
            set;
        }


        public ClientResource()
        {
            ClientTemplates = Enumerable.Empty<ClientResource>();
            ResourceKind = ClientResourceKind.Content;
        }

        public ClientResource(string url):this()
        {
            Url = url;
            ResourceKind = DetermineResourceKind(url);
        }


        protected static ClientResourceKind DetermineResourceKind(string url)
        {
            var extension = Path.GetExtension(url ?? "noname.txt").ToLower();

            switch(extension)
            {
                case(".js"):
                    return ClientResourceKind.Script;
                case(".css"):
                    return ClientResourceKind.Stylesheet;
                case(".html"):
                case(".htm"):
                    return ClientResourceKind.ClientTemplate;
                default:
                    return ClientResourceKind.Content;
            }
        }

        #region Equality Methods

        public virtual bool Equals(ClientResource other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Url, other.Url, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ClientResource);
        }

        public override int GetHashCode()
        {
            return (Url != null ? Url.GetHashCode() : 0);
        }

        public static bool operator ==(ClientResource x, ClientResource y)
        {
            if (ReferenceEquals(null, x))
                return ReferenceEquals(null, y);

            return x.Equals(y);
        }
        public static bool operator !=(ClientResource x, ClientResource y)
        {
            return !(x == y);
        }

        #endregion

        public virtual bool IsAbsoluteUrl()
        {
            if (string.IsNullOrWhiteSpace(Url))
                return false;

            return Regex.IsMatch(Url, @"^\w*://");
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return Url;

            return string.Format("{0} [{1}]", Name, Url);
        }

        public void Update(ClientResource resource)
        {
            // Ensure the max dependency level is used
            if (DependencyLevel < resource.DependencyLevel)
            {
                DependencyLevel = resource.DependencyLevel;
            }

            if (resource.DependsOn != null)
            {
                DependsOn =
                    (DependsOn ?? Enumerable.Empty<string>())
                        .Union(resource.DependsOn);
            }
        }


        public static explicit operator ClientResource(string resourceUrl)
        {
            return new ClientResource(resourceUrl);
        }
    }
}
