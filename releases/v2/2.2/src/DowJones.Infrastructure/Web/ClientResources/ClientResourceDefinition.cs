﻿using System;
using Newtonsoft.Json;

namespace DowJones.Web
{
    /// <summary>
    /// A DTO for transporting data that will be 
    /// transformed into a ClientResource
    /// </summary>
    public class ClientResourceDefinition
    {
        /// <summary>
        /// The unique name of this resource
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The absolute Url to the external resource.
        /// </summary>
        /// <remarks>
        /// An absolute Url takes precendence over an embedded resource
        /// declared via ResourceName.
        /// </remarks>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The Type in which the Web Resource is declared.
        /// </summary>
        /// <remarks>
        /// Defaults to the type on which this attribute is applied
        /// </remarks>
        [JsonProperty("type")]
        public Type DeclaringType { get; set; }

        /// <summary>
        /// The Embedded Resource Name
        /// </summary>
        /// <remarks>
        /// An absolute Url takes precendence over an embedded resource
        /// declared via Resource Name.
        /// </remarks>
        [JsonProperty("resourceName")]
        public string ResourceName
        {
            get
            {
                if(    string.IsNullOrWhiteSpace(_resourceName)
                    && HasRelativeResourceName 
                    && HasDeclaringType
                  )
                {
                    string relativeName = RelativeResourceName;

                    if (relativeName[0] == '.')
                        relativeName = relativeName.Substring(1);

                    _resourceName = 
                        string.Format("{0}.{1}", DeclaringType.Namespace, relativeName);
                }

                return _resourceName;
            }
            set { _resourceName = value; }
        }
        private string _resourceName;


        /// <summary>
        /// The Embedded Resource Name 
        /// (relative to the namespace of the <see cref="DeclaringType"/>)
        /// </summary>
        /// <remarks>
        /// An absolute Url takes precendence over an embedded resource
        /// declared via Resource Name.
        /// </remarks>
        public string RelativeResourceName { get; set; }

        /// <summary>
        /// The Resource Kind
        /// </summary>
        [JsonProperty("kind")]
        public ClientResourceKind? ResourceKind { get; set; }

        /// <summary>
        /// The Dependency Level
        /// </summary>
        [JsonProperty("level")]
        public ClientResourceDependencyLevel? DependencyLevel { get; set; }

        #region HasX Helpers

        public bool HasDeclaringType
        {
            get { return DeclaringType != null; }
        }

        public bool HasName
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        public bool HasResourceKind
        {
            get { return ResourceKind.HasValue; }
        }

        public bool HasResourceName
        {
            get { return !string.IsNullOrEmpty(ResourceName); }
        }

        public bool HasRelativeResourceName
        {
            get { return !string.IsNullOrEmpty(RelativeResourceName); }
        }

        public bool HasUrl
        {
            get { return !string.IsNullOrEmpty(Url); }
        }

        #endregion

        /// <summary>
        /// Partial Name (display name) of the embedded resource's assembly.
        /// </summary>
        /// <value>The display name of the assembly.</value>
        [JsonProperty("assembly")]
        public string DeclaringAssembly { get; set; }

        [JsonProperty("templateId")]
        public string TemplateId { get; set; }
    }
}