// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;
using DowJones.Formatters;
using DowJones.Models.Common;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Radar.Packages
{
    [DataContract(Name = "radarPackage", Namespace = "")]
    [KnownType(typeof(RadarPackage))]
    public class RadarPackage : IPackage
    {
        [DataMember(Name = "parentNewsEntities")]
        public ParentNewsEntities ParentNewsEntities { get; set; }

        [DataMember(Name = "minHitCount")]
        public WholeNumber MinHitCount { get; set; }

        [DataMember(Name = "maxHitCount")]
        public WholeNumber MaxHitCount { get; set; }

        /// <summary>
        /// Indicates whether there are results that have not been returned.
        /// </summary>
        /// <value>true,false</value>
        /// <remarks>This flag indicates that the radar results are incomplete</remarks>
        [DataMember(Name = "moreResults")]
        public bool MoreResults { get; set; }
    }
}