using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Models.Common;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Packages
{
    [DataContract(Name = "summaryRegionalMapPackage", Namespace = "")]
    public class SummaryRegionalMapPackage : AbstractSummaryPackage
    {
        /// <summary>
        /// Gets or sets the region news volume.
        /// </summary>
        /// <value>The region news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "regionNewsVolume")]
        public List<NewsEntity> RegionNewsVolume { get; set; }
    }
}