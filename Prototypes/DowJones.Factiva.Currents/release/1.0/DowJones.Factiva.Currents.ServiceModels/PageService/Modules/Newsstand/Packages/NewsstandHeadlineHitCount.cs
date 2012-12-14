using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages
{
    [DataContract(Name = "newsstandHeadlineHitCount", Namespace = "")]
    public class NewsstandHeadlineHitCount
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "sectionTitle")]
        public string SectionTitle { get; set; }

        [DataMember(Name = "sourceTitle")]
        public string SourceTitle { get; set; }

        [DataMember(Name = "sectionId")]
        public string SectionId { get; set; }

        [DataMember(Name = "sourceCode")]
        public string SourceCode { get; set; }

        [DataMember(Name = "hitCount")]
        public int HitCount { get; set; }

        [DataMember(Name = "searchContextRef")]
        public string SearchContextRef { get; set; }
    }
}