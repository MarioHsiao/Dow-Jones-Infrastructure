using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Radar.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Results;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
{
	public class PageServiceResponse
	{
		public NewsPageServiceResult NewsPageServiceResult { get; set; }

		public SummaryNewsPageModuleServiceResult SummaryNewsPageModuleServiceResult { get; set; }
		public SourcesNewsPageModuleServiceResult SourcesNewsPageModuleServiceResult { get; set; }

		public RadarNewsPageModuleServiceResult RadarNewsPageModuleServiceResult { get; set; }

		public CustomTopicsNewsPageModuleServiceResult CustomTopicsNewsPageModuleServiceResult { get; set; }
	}
}