﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractServicePartResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService
{
    /// <summary>
    /// The abstract service part result.
    /// </summary>
    /// <typeparam name="TPackage">The type of the package.</typeparam>
    [DataContract(Namespace = "")]
    public abstract class AbstractServicePartResult<TPackage> : IServicePartResult<TPackage> where TPackage : class, IPackage
    {
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
		public string PackageType { get; set; }

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