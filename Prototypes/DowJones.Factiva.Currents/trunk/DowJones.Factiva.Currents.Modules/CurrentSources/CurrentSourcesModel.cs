using System.Collections.Generic;
using System.Linq;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Models
{
	public class CurrentSourcesModel : Module
	{
		public IEnumerable<CurrentsHeadlineModel> CurrentsHeadlines { get; set; }
	}

	public class CurrentSourcesModelMapper : TypeMapper<SourcesNewsPageModuleServiceResult, CurrentSourcesModel>
	{
		public override CurrentSourcesModel Map(SourcesNewsPageModuleServiceResult source)
		{
			return new CurrentSourcesModel
				{
					CurrentsHeadlines = source.PartResults
											  .Select(p => 
												  new CurrentsHeadlineModel(
													new PortalHeadlineListModel(p.Package.Result))),
					
				};
		}
	}
}
