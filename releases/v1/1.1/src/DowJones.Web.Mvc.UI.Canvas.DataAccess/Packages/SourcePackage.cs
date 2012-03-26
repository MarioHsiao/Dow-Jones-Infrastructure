// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourcePackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "sourcesPackage", Namespace = "")]
    [KnownType(typeof(SourcePackage))]
    public class SourcePackage : AbstractHeadlinePackage
    {
        /// <summary>
        /// Gets or sets the source code FCode.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        [DataMember(Name = "sourceCode")]
        public string SourceCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        [DataMember(Name = "sourceName")]
        public string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the source logo URL.
        /// </summary>
        /// <value>
        /// The source logo URL.
        /// </value>
        [DataMember(Name = "sourceLogoUrl")]
        public string SourceLogoUrl { get; set; }

        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }
    }
}
