using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;
using DowJones.Pages.Modules.Snapshot;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
{
	[DataContract(Name = "newsPagePackage", Namespace = "")]
	public class NewsPagePackage : IPackage
	{
		[DataMember(Name = "newsPage")]
		public NewsPage NewsPage { get; set; }
	}
}