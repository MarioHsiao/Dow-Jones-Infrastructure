// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Utilities.Formatters;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.Common;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
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
    }
}