using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Packages
{
    [DataContract(Name = "newsstandHeadlineHitCountsPackage", Namespace = "")]
    public class NewsstandHeadlineHitCountsPackage : AbstractNewsstandPackage
    {
        [DataMember(Name = "newsstandHeadlineHitCounts")]
        public List<NewsstandHeadlineHitCount> NewsstandHeadlineHitCounts { get; set; }
    }
}