// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Formatters;

namespace DowJones.Pages
{
    /// <summary>
    /// The entity news volume variation.
    /// </summary>
    /// <remarks></remarks>
    [DataContract(Name = "entityNewsVolumeVariation", Namespace = "")]
    public class NewsEntityNewsVolumeVariation : NewsEntity
    {
        /// <summary>
        /// Gets or sets the volume for previous time frame.
        /// </summary>
        /// <value>The volume for previous time frame.</value>
        /// <remarks></remarks>
        [DataMember(Name = "previousNewsVolume")]
        public WholeNumber PreviousTimeFrameNewsVolume { get; set; }

        /// <summary>
        /// Gets or sets the percent volume change.
        /// </summary>
        /// <value>The percent volume change.</value>
        /// <remarks></remarks>
        [DataMember(Name = "percentVolumeChange")]
        public Percent PercentVolumeChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [new entrant].
        /// </summary>
        /// <value><c>true</c> if [new entrant] it will be true; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [DataMember(Name = "newEntrant")]
        public bool NewEntrant { get; set; }
    }
}
