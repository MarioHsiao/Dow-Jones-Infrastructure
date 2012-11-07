using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using DowJones.Models.Common;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Packages
{
    [DataContract(Name = "newsstandDiscoveredEntitiesPackage", Namespace = "")]
    public class NewsstandDiscoveredEntitiesPackage : AbstractNewsstandPackage
    {
        /// <summary>
        /// Gets or sets the region news volume.
        /// </summary>
        /// <value>The region news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "topNewsVolumeEntities")]
        public List<NewsEntity> TopNewsVolumeEntities { get; set; }
       

        /// <summary>
        /// Gets or sets a value indicating whether [portal headlines are available].
        /// </summary>
        /// <value>
        /// <c>true</c> if [portal headlines are available]; otherwise, <c>false</c>.
        /// </value>
        [XmlElement(Type = typeof(bool), ElementName = "portalHeadlinesAreAvailable", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("portalHeadlinesAreAvailable")]
        public bool PortalHeadlinesAreAvailable { get; set; }
    }
}