using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using System.Linq;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using System.Collections.Generic;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages;

namespace DowJones.Factiva.Currents.Models
{
	public class NewsstandModel : CompositeComponentModel
	{
		public IEnumerable<CurrentsHeadlineModel> CurrentHeadlines { get; set; }
	}

	public class NewsstandModelMapper : TypeMapper<NewsstandNewsPageModuleServiceResult, NewsstandModel>
	{
		public override NewsstandModel Map(NewsstandNewsPageModuleServiceResult newstandSource)
		{
			return new NewsstandModel
				{
					CurrentHeadlines = newstandSource.PartResults
											.Where(pr => pr.Package is NewsstandHeadlinesPackage)
											.Select(p => new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Package.Result)))
				};
		}
	}
}