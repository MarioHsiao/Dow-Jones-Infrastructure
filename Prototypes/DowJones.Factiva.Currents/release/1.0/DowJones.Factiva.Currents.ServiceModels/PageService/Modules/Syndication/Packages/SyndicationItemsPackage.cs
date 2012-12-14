using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Syndication.Packages
{
    [DataContract(Name = "syndicationItemsPackage", Namespace = "")]
    public class SyndicationItemsPackage
    {
        [DataMember(Name = "syndicationItems")]
        public List<SyndicationItem> SyndicationItems { get; set; }
    }
}
