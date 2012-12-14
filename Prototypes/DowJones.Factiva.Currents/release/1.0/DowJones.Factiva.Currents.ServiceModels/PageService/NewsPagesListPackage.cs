using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;
using DowJones.Pages.Modules.Snapshot;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
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
