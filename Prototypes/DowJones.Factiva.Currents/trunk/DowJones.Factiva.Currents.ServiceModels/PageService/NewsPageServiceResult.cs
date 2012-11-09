using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
{
    [DataContract(Name = "newsPageServiceResult", Namespace = "")]
    public class NewsPageServiceResult : AbstractServiceResult
    {
		[DataMember(Name = "package")]
		public NewsPagePackage Package { get; set; }
    }
}
