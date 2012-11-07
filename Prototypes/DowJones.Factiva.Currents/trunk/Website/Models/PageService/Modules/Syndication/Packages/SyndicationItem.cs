using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Syndication.Packages
{
    [DataContract(Name = "syndicationItem", Namespace = "")]
    public class SyndicationItem
    {
        [DataMember(Name = "id")]
        public long Id;

        [DataMember(Name = "url")]
        public string Url;
    }
}