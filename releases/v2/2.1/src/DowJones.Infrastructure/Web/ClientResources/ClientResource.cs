using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DowJones.Web.UI.Exceptions;
using log4net;

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

        /// <summary>
        /// Indicates whether substitution logic (e.g. web resource or token replacement)
        /// should be executed
        /// </summary>
        public bool PerformSubstitution
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
        public string TemplateId { get; set;}


        public ClientResource()
        {
        }

        public ClientResource(string url)
        {
            Url = url;
            ResourceKind = DetermineResourceKind(url);
        }


        protected static ClientResourceKind DetermineResourceKind(string url)
        {
            var extension = Path.GetExtension(url ?? string.Empty).ToLower();

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

        public static implicit operator ClientResource(string resourceUrl)
        {
            return new ClientResource(resourceUrl);
        }

        /// <summary>
        /// Creates a new <see cref="ClientResource"/> instance
        /// from a <see cref="ClientResourceDefinition"/>
        /// </summary>
        /// <remarks>
        /// Yes, this breaks SOLID by referencing 
        /// EmbeddedClientResource...
        /// Feel free to convert this to a Mapper if
        /// this bothers you.
        /// </remarks>
        public static ClientResource CreateFromDefinition(ClientResourceDefinition definition)
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

        public void Update(ClientResource resource)
        {
            // Ensure the max dependency level is used
            if (DependencyLevel < resource.DependencyLevel)
            {
                DependencyLevel = resource.DependencyLevel;
            }
        }
    }
}
