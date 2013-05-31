using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using DowJones.Extensions;

namespace DowJones.Managers.Core
{
    /// <summary>
    /// The abstract service part result.
    /// </summary>
    /// <typeparam name="TPackage">The type of the package.</typeparam>
    [DataContract(Namespace = "")]
    public abstract class AbstractServicePartResult<TPackage> : IServicePartResult<TPackage> where TPackage : class, IPackage
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string packageType;

        #region IServicePartResult<TPackage> Members

        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        [DataMember(Name = "identifier", EmitDefaultValue = true)]
        public string Identifier { get; set; }


        /// <summary>
        /// Gets or sets the package.
        /// </summary>
        /// <value>
        /// The package.
        /// </value>
        [DataMember(Name = "packageType")]
        public string PackageType
        {
            get
            {
                return packageType.IsNullOrEmpty() ? Package != null ? packageType = Package.GetType().GetName() : string.Empty : packageType;
            }

            set
            {
                packageType = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        [DataMember(Name = "returnCode", EmitDefaultValue = true)]
        [XmlElement(ElementName = "returnCode")]
        public long ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        [DataMember(Name = "statusMessage", EmitDefaultValue = true)]
        [XmlElement(ElementName = "statusMessage")]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        [DataMember(Name = "elapsedTime", EmitDefaultValue = true)]
        public long ElapsedTime { get; set; }

        /// <summary>
        /// Gets or sets the package.
        /// </summary>
        /// <value>
        /// The package.
        /// </value>
        [DataMember(Name = "package")]
        public TPackage Package { get; set; }

        #endregion
    }
}