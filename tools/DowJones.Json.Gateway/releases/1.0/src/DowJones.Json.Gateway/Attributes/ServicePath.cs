using System;

namespace DowJones.Json.Gateway.Attributes
{

    /// <summary>
    /// Summary description for ServicePath.
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServicePathAttribute : Attribute
    {
        private readonly string _path = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicePathAttribute"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ServicePathAttribute(string path)
        {
            _path = path;
        }
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return _path; }
        }
    }
}