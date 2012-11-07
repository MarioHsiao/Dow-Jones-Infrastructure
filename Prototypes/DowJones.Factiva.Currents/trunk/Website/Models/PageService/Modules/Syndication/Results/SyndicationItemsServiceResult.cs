using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Syndication.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Syndication.Results
{
    [DataContract(Name = "syndicationItemsServiceResult", Namespace = "")]
    public class SyndicationItemsServiceResult : AbstractServiceResult
    {
       
        [DataMember(Name = "package")]
        public SyndicationItemsPackage Package { get; set; }

    }
}
