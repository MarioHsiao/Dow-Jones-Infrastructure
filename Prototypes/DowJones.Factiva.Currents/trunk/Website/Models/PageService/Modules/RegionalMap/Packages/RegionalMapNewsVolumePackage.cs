using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using DowJones.Models.Common;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.RegionalMap.Packages
{
    /// <summary>
    /// The regional map news volume package.
    /// </summary>
    [DataContract(Name = "RegionalMapNewsVolumePackage", Namespace = "")]
    public class RegionalMapNewsVolumePackage : AbstractRegionalMapPackage
    {
        /// <summary>
        /// Gets or sets the region news volume.
        /// </summary>
        /// <value>The region news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "regionNewsVolume")]
        [XmlElement(Type = typeof(List<NewsEntityNewsVolumeVariation>), ElementName = "regionNewsVolume", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("regionNewsVolume")]
        public List<NewsEntityNewsVolumeVariation> RegionNewsVolume { get; set; }
    }
}