// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrendingPortalPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.Common;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "trendingUpPackage", Namespace = "")]
    public class TrendingUpPackage : AbstractTrendingPackage
    {
    }

    [DataContract(Name = "trendingDownPackage", Namespace = "")]
    public class TrendingDownPackage : AbstractTrendingPackage
    {
    }

    [DataContract(Name = "trendingTopEntitiesPackage", Namespace = "")]
    public class TrendingTopEntitiesPackage : AbstractTrendingPackage
    {
    }

    [DataContract(Name = "AbstractTrendingPackage", Namespace = "")]
    [KnownType(typeof(TrendingUpPackage))]
    [KnownType(typeof(TrendingDownPackage))]
    [KnownType(typeof(TrendingTopEntitiesPackage))]
    public abstract class AbstractTrendingPackage : IPackage
    {
        /// <summary>
        /// Gets or sets the region news volume.
        /// </summary>
        /// <value>The region news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "topNewsVolumeEntities", EmitDefaultValue = false)]
        public IList<NewsEntity> TopNewsVolumeEntities { get; set; }

        /// <summary>
        /// Gets or sets the entities trending up or down
        /// </summary>
        /// <value>The entities trending up or down.</value>
        /// <remarks></remarks>
        [DataMember(Name = "trendingEntities", EmitDefaultValue = false)]
        public IList<NewsEntityNewsVolumeVariation> TrendingEntities
        {
            get; set;
        }
    }

    [DataContract(Name = "trendType", Namespace = "")]
    public enum TrendType
    {

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        TopEntities,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        TrendingUp,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        TrendingDown
    }
}
