using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Radar.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.RegionalMap.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Results;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
{
	public class PageServiceResponse
	{
		public NewsPageServiceResult NewsPageServiceResult { get; set; }

		public SummaryNewsPageModuleServiceResult SummaryNewsPageModuleServiceResult { get; set; }
		public SourcesNewsPageModuleServiceResult SourcesNewsPageModuleServiceResult { get; set; }

        public NewsstandNewsPageModuleServiceResult NewsstandNewsPageModuleServiceResult { get; set; }

		public RadarNewsPageModuleServiceResult RadarNewsPageModuleServiceResult { get; set; }

		public CustomTopicsNewsPageModuleServiceResult CustomTopicsNewsPageModuleServiceResult { get; set; }

        public TrendingNewsPageModuleServiceResult TrendingNewsPageModuleServiceResult { get; set; }

		public RegionalMapNewsPageModuleServiceResult RegionalMapNewsPageModuleServiceResult { get; set; }
	}
}