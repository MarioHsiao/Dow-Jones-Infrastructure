using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Ajax.PortalHeadlineList;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.RegionalMap.Packages
{
    /// <summary>
    /// The regional map headlines package.
    /// </summary>
    [DataContract(Name = "RegionalMapHeadlinesPackage", Namespace = "")]
    public class RegionalMapHeadlinesPackage : AbstractRegionalMapPackage
    {
        /// <summary>
        /// Gets or sets the newsstand section headline list.
        /// </summary>
        /// <value>The newsstand section headline list.</value>
        /// <remarks></remarks>
        [DataMember(Name = "regionalMapRegionHeadlines")]
        [XmlElement(Type = typeof(List<RegionalMapHeadlines>), ElementName = "regionalMapRegionHeadlines", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("regionalMapRegionHeadlines")]
        public PortalHeadlineListDataResult RegionalMapRegionHeadlineList { get; set; }
    }
}