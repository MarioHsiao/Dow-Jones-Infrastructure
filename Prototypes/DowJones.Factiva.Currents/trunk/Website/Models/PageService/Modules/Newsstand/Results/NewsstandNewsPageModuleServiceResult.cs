using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Results
{
	/// <summary>
	/// The newsstand news page module service result.
	/// </summary>
	[DataContract(Name = "newsstandNewsPageModuleServiceResult", Namespace = "")]
	public class NewsstandNewsPageModuleServiceResult :
		AbstractModuleServiceResult<NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>, AbstractNewsstandPackage, NewsstandNewspageModule>
	{
	}
}
