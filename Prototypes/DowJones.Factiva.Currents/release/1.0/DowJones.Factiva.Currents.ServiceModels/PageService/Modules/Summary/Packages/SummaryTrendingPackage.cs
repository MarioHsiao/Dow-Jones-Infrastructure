using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Models.Common;
using DowJones.Pages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Packages
{
    [DataContract(Name = "summaryTrendingPackage", Namespace = "")]
    public class SummaryTrendingPackage : AbstractSummaryPackage
    {
        [DataMember(Name = "keywordsTagCollection")]
        public TagCollection KeywordsTagCollection { get; protected internal set; }

        [DataMember(Name = "companyNewsEntities")]
        public List<NewsEntity> CompanyNewsEntities { get; protected internal set; }

        [DataMember(Name = "newsSubjectsNewsEntities")]
        public List<NewsEntity> NewsSubjectsNewsEntities { get; protected internal set; }

        [DataMember(Name = "industriesNewsEntities")]
        public List<NewsEntity> IndustriesNewsEntities { get; protected internal set; }

        [DataMember(Name = "executivesNewsEntities")]
        public List<NewsEntity> ExecutivesNewsEntities { get; protected internal set; }
    }
}