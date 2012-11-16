using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Common.Headlines.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages
{
    [DataContract(Name = "newsstandPackage", Namespace = "")]
    [KnownType(typeof(NewsstandHeadlinesPackage))]
    [KnownType(typeof(NewsstandDiscoveredEntitiesPackage))] 
    [KnownType(typeof(NewsstandHeadlineHitCountsPackage))]
    public abstract class AbstractNewsstandPackage : AbstractHeadlinePackage, IViewAllSearchContextRef
    {
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        #region Implementation of IViewAllSearchContextRef

        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }

        #endregion
    }
}

