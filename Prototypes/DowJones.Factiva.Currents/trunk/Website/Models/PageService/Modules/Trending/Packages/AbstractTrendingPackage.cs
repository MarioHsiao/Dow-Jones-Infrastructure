using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Models.Common;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Trending.Packages
{
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
}