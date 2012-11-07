using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Pages.Modules.Snapshot;

namespace DowJones.Factiva.Currents.Website.Models.PageService
{
    [DataContract(Name = "newsPagesListPackage", Namespace = "")]
    public class NewsPagesListPackage : IPackage
    {

        [DataMember(Name = "newsPages")]
        public List<NewsPage> NewsPages { get; set; }

        [DataMember(Name = "requestedNewsPage")]
        public NewsPage RequestedNewsPage { get; set; }
    }
}
